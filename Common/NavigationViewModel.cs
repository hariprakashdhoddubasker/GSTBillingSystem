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
            var registorSubItems = new List<SubItemViewModel>
            {
                new SubItemViewModel(WpfAppForms.Customer.GetDescription())
            };

            var reportSubItems = new List<SubItemViewModel>
            {
                new SubItemViewModel(WpfAppForms.CustomerInvoiceReport.GetDescription())
            };

            var financialSubItems = new List<SubItemViewModel>
            {
                new SubItemViewModel(WpfAppForms.Invoice.GetDescription())
            };

            if (UIService.CurrentUser.Role == WpfAppRoles.Admin.ToString())
            {
                registorSubItems.Add(new SubItemViewModel(WpfAppForms.Product.GetDescription()));

                reportSubItems.Add(new SubItemViewModel(WpfAppForms.BackUp.GetDescription()));

            }

            var financialItemMenu = new ItemMenuViewModel("FINANCIAL", financialSubItems, PackIconKind.ScaleBalance, myEventAggregator);
            var registorItemMenu = new ItemMenuViewModel("REGISTER", registorSubItems, PackIconKind.Register, myEventAggregator);
            var reportItemMenu = new ItemMenuViewModel("REPORTS", reportSubItems, PackIconKind.FileReport, myEventAggregator);

            StackPanel stackPanel = new StackPanel();
            stackPanel.Children.Add(new MenuView(registorItemMenu));
            stackPanel.Children.Add(new MenuView(financialItemMenu));
            stackPanel.Children.Add(new MenuView(reportItemMenu));
            return stackPanel;
        }
    }
}
