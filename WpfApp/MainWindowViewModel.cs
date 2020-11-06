using WpfApp.Common;
using Prism.Events;
using System.Threading.Tasks;
using Unity;
using System.Windows.Input;
using WpfApp.Helpers;
using WpfApp.Registration;
using WpfApp.Common.Events;
using WpfApp.Invoices;
using System;
using System.Globalization;

namespace WpfApp
{

    public class MainWindowViewModel : BaseViewModel
    {
        private BaseViewModel mySelectedViewModel;
        private ContactViewModel myContactViewModel;
        private CustomerRegistrationViewModel myCustomerRegistrationViewModel;
        private StateRegistrationViewModel myStateRegistrationViewModel;
        private ProductRegistrationViewModel myProductRegistrationViewModel;
        private InvoiceViewModel myInvoiceViewModel;
        private InvoiceReportViewModel myInvoiceReportViewModel;
        private BackUpRestoreViewModel myBackUpRestoreViewModel;
        private LetterPadViewModel myLetterPadViewModel;
        private SignatureViewModel mySignatureViewModel;
        private IEventAggregator myEventAggregator;
        private string myCurrentUserName;
        private string myWpfAppName;

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            Task.Run(() =>
            {
                myEventAggregator = eventAggregator;
                myEventAggregator.GetEvent<NavigateViewsEvent>().Subscribe(UpdateViewModel);
                myContactViewModel = ContainerHelper.Container.Resolve<ContactViewModel>();

                myCustomerRegistrationViewModel = ContainerHelper.Container.Resolve<CustomerRegistrationViewModel>();
                myStateRegistrationViewModel = ContainerHelper.Container.Resolve<StateRegistrationViewModel>();
                myProductRegistrationViewModel = ContainerHelper.Container.Resolve<ProductRegistrationViewModel>();
                myInvoiceViewModel = ContainerHelper.Container.Resolve<InvoiceViewModel>();
                myBackUpRestoreViewModel = ContainerHelper.Container.Resolve<BackUpRestoreViewModel>();
                myInvoiceReportViewModel = ContainerHelper.Container.Resolve<InvoiceReportViewModel>();
                myLetterPadViewModel = ContainerHelper.Container.Resolve<LetterPadViewModel>();
                mySignatureViewModel = ContainerHelper.Container.Resolve<SignatureViewModel>();
                this.SelectedViewModel = myInvoiceViewModel;
                this.BtnContactCommand = new Command(this.OnContactClick, o => true);
            });
            WpfAppName = Constants.WfpAppName;

        }

        public ICommand BtnContactCommand { get; private set; }

        public NavigationViewModel NavigationViewModel { get; set; }

        public BaseViewModel SelectedViewModel
        {
            get { return mySelectedViewModel; }
            set => SetProperty(ref mySelectedViewModel, value);
        }

        public string WpfAppName
        {
            get { return myWpfAppName; }
            set => SetProperty(ref myWpfAppName, value);
        }

        public string CurrentUserName
        {
            get { return myCurrentUserName; }
            set => SetProperty(ref myCurrentUserName, value);
        }

        private void UpdateViewModel(WpfAppForms natureBoxForms)
        {
            Clear();

            switch (natureBoxForms)
            {
                case WpfAppForms.Customer:
                    SelectedViewModel = myCustomerRegistrationViewModel;
                    break;
                case WpfAppForms.State:
                    SelectedViewModel = myStateRegistrationViewModel;
                    break;
                case WpfAppForms.Product:
                    SelectedViewModel = myProductRegistrationViewModel;
                    break;
                case WpfAppForms.Invoice:
                    SelectedViewModel = myInvoiceViewModel;
                    break;
                case WpfAppForms.CustomerInvoiceReport:
                    SelectedViewModel = myInvoiceReportViewModel;
                    break;
                case WpfAppForms.BackUp:
                    SelectedViewModel = myBackUpRestoreViewModel;
                    break;
                case WpfAppForms.LetterPad:
                    SelectedViewModel = myLetterPadViewModel;
                    break;
                case WpfAppForms.Signature:
                    SelectedViewModel = mySignatureViewModel;
                    break;
                default:
                    break;
            }
        }

        private void Clear()
        {
            myCustomerRegistrationViewModel.Clear();
            myStateRegistrationViewModel.Clear();
            myProductRegistrationViewModel.Clear();
            myInvoiceViewModel.Clear();
            myInvoiceReportViewModel.Clear();
        }

        private void OnContactClick(object obj)
        {
            SelectedViewModel = myContactViewModel;
        }

        public void RegisterNavigationViewModel()
        {
            NavigationViewModel = ContainerHelper.Container.Resolve<NavigationViewModel>();
        }

        public void Load()
        {
            this.CurrentUserName = "Hi, " + UIService.CurrentUser.UserName;
        }
    }
}
