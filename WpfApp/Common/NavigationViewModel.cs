using MaterialDesignThemes.Wpf;
using Prism.Events;
using System.Collections.Generic;
using System.Windows.Controls;
using WpfApp.Helpers;

namespace WpfApp.Common
{
    public class NavigationViewModel : BaseViewModel
    {
        private StackPanel myMenu;
        IEventAggregator myEventAggregator;
        public NavigationViewModel(IEventAggregator eventAggregator)
        {
            myEventAggregator = eventAggregator;
            MenuItem = GetMenuItems();
        }

        public StackPanel MenuItem
        {
            get
            {
                return myMenu;
            }
            set
            {
                if (string.Equals(this.myMenu, value)) return;
                this.myMenu = value;
                OnPropertyChanged();
            }
        }

        internal StackPanel GetMenuItems()
        {
            var customerSubItems = new List<SubItemViewModel>
            {
                new SubItemViewModel(WpfAppForms.Customer.GetDescription())
            };

            var stateSubItems = new List<SubItemViewModel>
            {
                new SubItemViewModel(WpfAppForms.State.GetDescription())
            };

            var Invoice = new List<SubItemViewModel>
            {
                new SubItemViewModel(WpfAppForms.Invoice.GetDescription())
            };

            var productSubItems = new List<SubItemViewModel>
            {
                new SubItemViewModel(WpfAppForms.Product.GetDescription())
            };
            var reportSubItems = new List<SubItemViewModel>
            {
                new SubItemViewModel(WpfAppForms.CustomerInvoiceReport.GetDescription())
            };

            var backUpSubItems = new List<SubItemViewModel>
            {
                new SubItemViewModel(WpfAppForms.BackUp.GetDescription())
            };

            var letterPadSubItems = new List<SubItemViewModel>
            {
                new SubItemViewModel(WpfAppForms.LetterPad.GetDescription())
            };
            var signatureSubItems = new List<SubItemViewModel>
            {
                new SubItemViewModel(WpfAppForms.Signature.GetDescription())
            };

            var customerItemMenu = new ItemMenuViewModel("CUSTOMER", customerSubItems, PackIconKind.AccountGroup, myEventAggregator);
            var stateItemMenu = new ItemMenuViewModel("STATE", stateSubItems, PackIconKind.HomeMapMarker, myEventAggregator);
            var InvoiceItemMenu = new ItemMenuViewModel("INVOICE", Invoice, PackIconKind.PodiumSilver, myEventAggregator);
            var productItemMenu = new ItemMenuViewModel("PRODUCT", productSubItems, PackIconKind.CurrencyInr, myEventAggregator);
            var reportItemMenu = new ItemMenuViewModel("REPORT", reportSubItems, PackIconKind.PrinterWireless, myEventAggregator);
            var letterPadItemMenu = new ItemMenuViewModel("LETTER PAD", letterPadSubItems, PackIconKind.NoteMultiple, myEventAggregator);
            var backUpItemMenu = new ItemMenuViewModel("BACKUP", backUpSubItems, PackIconKind.CloudUpload, myEventAggregator);
            var signatureItemMenu = new ItemMenuViewModel("SIGNATURE", signatureSubItems, PackIconKind.SignatureImage, myEventAggregator);

            StackPanel stackPanel = new StackPanel();

            stackPanel.Children.Add(new MenuView(InvoiceItemMenu));
            stackPanel.Children.Add(new MenuView(customerItemMenu));
            stackPanel.Children.Add(new MenuView(stateItemMenu));
            stackPanel.Children.Add(new MenuView(productItemMenu));
            stackPanel.Children.Add(new MenuView(reportItemMenu));
            stackPanel.Children.Add(new MenuView(letterPadItemMenu));            
            stackPanel.Children.Add(new MenuView(backUpItemMenu));
            stackPanel.Children.Add(new MenuView(signatureItemMenu));
            return stackPanel;
        }
    }
}
