using System;

namespace WpfApp.Dialog
{
    public class DialogCloseRequestedEventArgs : EventArgs
    {
        public DialogCloseRequestedEventArgs(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }

        public bool? DialogResult { get; }
    }
}
