using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using WpfApp.Helpers;
using WpfApp.Invoices.Service;
using WpfApp.Model;

namespace WpfApp.Invoices
{
    public class SignatureViewModel : BaseViewModel
    {
        private ISignatureRepository mySignatureRepository;
        private Signature mySignature;
        private List<Signature> myAllSignature;

        public SignatureViewModel(ISignatureRepository signatureRepository)
        {
            mySignatureRepository = signatureRepository;      
            this.UpdateSignatureCommand = new Command(this.OnUpdateSignature, CanExecuteUpdateSignature);
            this.BtnOpenFileDialogCommand = new Command(this.OnOpenFileDialog, this.CanExecuteOpenFileDialog);
            this.Signature = new Signature();
        }
        public ICommand BtnOpenFileDialogCommand { get; private set; }
        public ICommand UpdateSignatureCommand { get; private set; }

        public Signature Signature
        {
            get => this.mySignature;
            set
            {
                SetProperty(ref mySignature, value);
                ((Command)this.UpdateSignatureCommand).RaiseCanExecuteChanged();
            }
        }

        public async void Load()
        {
            myAllSignature = await mySignatureRepository.GetAllAsync();

            if (myAllSignature.Any())
            {
                this.Signature = myAllSignature.FirstOrDefault();
            }
            Signature.PropertyChanged += Signature_PropertyChanged;
        }

        private bool CanExecuteOpenFileDialog(object arg)
        {
            return true;
        }

        private bool CanExecuteUpdateSignature(object arg)
        {
            return !string.IsNullOrEmpty(Signature.SignatureFilePath) && File.Exists(Signature.SignatureFilePath);
        }

        private void OnOpenFileDialog(object obj)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            var result = openFileDlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                Signature.SignatureFilePath = openFileDlg.FileName;
            }
        }

        private async void OnUpdateSignature(object obj)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Signature signature;
            Signature.SignatureType = "InvoiceSignature";
            if (!myAllSignature.Any())
            {
                signature = await mySignatureRepository.AddAsync(Signature);
            }
            else
            {
                signature = await mySignatureRepository.UpdateAsync(Signature);
            }

            Mouse.OverrideCursor = null;

            if (signature != null)
            {
                UIService.ShowMessage("Signature path Updated");
            }
        }

        private void Signature_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Signature.SignatureFilePath))
            {
                ((Command)this.UpdateSignatureCommand).RaiseCanExecuteChanged();
            }
        }
    }
}
