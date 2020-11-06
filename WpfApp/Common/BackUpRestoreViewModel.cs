using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using WpfApp.DataAccess;
using WpfApp.Helpers;

namespace WpfApp.Common
{
    public class BackUpRestoreViewModel : BaseViewModel
    {
        private IBackUpAndRestoreRepository myBackUpAndRestoreDal;
        private string myRestoreFilePath;
        private string myBackUpFolderPath;

        public BackUpRestoreViewModel(IBackUpAndRestoreRepository backUpAndRestoreDal)
        {
            myBackUpAndRestoreDal = backUpAndRestoreDal;
            this.BtnBackUpCommand = new Command(this.BtnBackUpClick, CanExecuteBackUp);
            this.BtnRestoreCommand = new Command(this.BtnRestoreClick, CanExecuteRestore);
            this.BtnOpenFolderDialogCommand = new Command(this.OnOpenFolderDialog, this.CanExecuteOpenFolderDialog);
            this.BtnOpenFileDialogCommand = new Command(this.OnOpenFileDialog, this.CanExecuteOpenFileDialog);
        }

        public ICommand BtnOpenFolderDialogCommand { get; private set; }
        public ICommand BtnBackUpCommand { get; private set; }
        public ICommand BtnOpenFileDialogCommand { get; private set; }
        public ICommand BtnRestoreCommand { get; private set; }

        public string BackUpFolderPath
        {
            get => this.myBackUpFolderPath;
            set
            {
                SetProperty(ref myBackUpFolderPath, value);
                ((Command)this.BtnBackUpCommand).RaiseCanExecuteChanged();
                ((Command)this.BtnOpenFileDialogCommand).RaiseCanExecuteChanged();
            }
        }

        public string RestoreFilePath
        {
            get => this.myRestoreFilePath;
            set
            {
                SetProperty(ref myRestoreFilePath, value);
                ((Command)this.BtnRestoreCommand).RaiseCanExecuteChanged();
                ((Command)this.BtnOpenFolderDialogCommand).RaiseCanExecuteChanged();
            }
        }

        private bool CanExecuteBackUp(object arg)
        {
            return !string.IsNullOrEmpty(BackUpFolderPath) && Directory.Exists(BackUpFolderPath);
        }
        private bool CanExecuteOpenFolderDialog(object arg)
        {
            return string.IsNullOrEmpty(RestoreFilePath);
        }
        private bool CanExecuteOpenFileDialog(object arg)
        {
            return string.IsNullOrEmpty(BackUpFolderPath);
        }

        private bool CanExecuteRestore(object arg)
        {
            return !string.IsNullOrEmpty(RestoreFilePath) && string.IsNullOrEmpty(BackUpFolderPath) && File.Exists(RestoreFilePath);
        }

        private void OnOpenFileDialog(object obj)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "MySQL Script File (.sql)|*.sql";
            var result = openFileDlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                RestoreFilePath = openFileDlg.FileName;
            }
        }

        private void OnOpenFolderDialog(object obj)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            this.BackUpFolderPath = dialog.SelectedPath;
        }

        private async void BtnBackUpClick(object obj)
        {
            if (await myBackUpAndRestoreDal.BackUp(BackUpFolderPath))
            {
                UIService.ShowMessage("DataBase BackUp done in the following path" + Environment.NewLine + BackUpFolderPath);
            }
            BackUpFolderPath = string.Empty;
        }

        private async void BtnRestoreClick(object obj)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            if (await myBackUpAndRestoreDal.Restore(RestoreFilePath))
            {
                Mouse.OverrideCursor = null;
                UIService.ShowMessage("DataBase Restored");
            }
            else
            {
                Mouse.OverrideCursor = null;
            }
            RestoreFilePath = string.Empty;

        }
    }
}
