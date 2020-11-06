using System.Linq;
using System.Windows.Input;
using WpfApp.Helpers;
using WpfApp.Helpers.HtmlService;
using WpfApp.Invoices.Service;

namespace WpfApp.Invoices
{
    public class LetterPadViewModel : BaseViewModel
    {
        private ISignatureRepository mySignatureRepository;
        private string myLetterPadRtfContent;
        private string mySignatureFilePath;

        public LetterPadViewModel(ISignatureRepository signatureRepository)
        {
            mySignatureRepository = signatureRepository;
            this.PrintCommand = new Command(this.OnPrintCommand, this.CanExecutePrintCommand);
        }

        public void Load()
        {
            var signature = mySignatureRepository.GetAllAsync().Result.FirstOrDefault();

            if (signature != null)
            {
                mySignatureFilePath = signature.SignatureFilePath;
            }
        }
        public ICommand PrintCommand { get; }

        public string LetterPadRtfContent
        {
            get => this.myLetterPadRtfContent;
            set => SetProperty(ref myLetterPadRtfContent, value);
        }

        private bool CanExecutePrintCommand(object arg)
        {
            return true;
        }

        private void OnPrintCommand(object obj)
        {
             HtmlService.GenerateLetterPad(LetterPadRtfContent, mySignatureFilePath);
        }
    }
}
