using RishiSilvers.Helpers;
using System;
using System.Windows.Input;

namespace RishiSilvers.Dialog
{
    public class DialogViewModel : IDialogRequestClose
    {
        public DialogViewModel(string message, string enableCancel)
        {
            EnableCancel = enableCancel;
            Message = message;
            OkCommand = new Command(p => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)), canExecuteMethod => true);
            CancelCommand = new Command(p => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)), canExecuteMethod => true);
        }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
        public string Message { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        public string EnableCancel { get; set; }
    }
}
