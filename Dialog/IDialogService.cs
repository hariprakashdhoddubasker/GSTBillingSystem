using System;
using System.Collections.Generic;
using System.Text;

namespace RishiSilvers.Dialog
{
    public interface IDialogService
    {
        void Register<TViewModel, TView>() where TViewModel : IDialogRequestClose
                                           where TView : IDialog;

        bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose;
    }
}
