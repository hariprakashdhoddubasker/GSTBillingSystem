using WpfApp.Common;
using Prism.Events;
using System.Threading.Tasks;
using Unity;
using System.Windows.Input;
using WpfApp.Helpers;
using WpfApp.Registration;
using WpfApp.Common.Events;

namespace WpfApp
{

    public class MainWindowViewModel : BaseViewModel
    {
        private BaseViewModel mySelectedViewModel;
        //private ContactViewModel myContactViewModel;
        private CustomerRegistrationViewModel myCustomerRegistrationViewModel;
        //private ProductRegistrationViewModel myProductRegistrationViewModel;
        //private InvoiceViewModel myInvoiceViewModel;
        //private CustomerInvoiceReportViewModel myCustomerAttendanceViewModel;
        //private BackUpRestoreViewModel myBackUpRestoreViewModel;
        private IEventAggregator myEventAggregator;
        private string myCurrentUserName;
        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            Task.Run(() =>
            {
                myEventAggregator = eventAggregator;
                myEventAggregator.GetEvent<NavigateViewsEvent>().Subscribe(UpdateViewModel);
                //myContactViewModel = ContainerHelper.Container.Resolve<ContactViewModel>();
     
                myCustomerRegistrationViewModel = ContainerHelper.Container.Resolve<CustomerRegistrationViewModel>();
                //myProductRegistrationViewModel = ContainerHelper.Container.Resolve<ProductRegistrationViewModel>();
                //myInvoiceViewModel = ContainerHelper.Container.Resolve<InvoiceViewModel>();
                //myBackUpRestoreViewModel = ContainerHelper.Container.Resolve<BackUpRestoreViewModel>();

                this.SelectedViewModel = myCustomerRegistrationViewModel;
                this.BtnContactCommand = new Command(this.OnContactClick, o => true);
            });
        }

        public ICommand BtnContactCommand { get; private set; }

        public NavigationViewModel NavigationViewModel { get; set; }

        public BaseViewModel SelectedViewModel
        {
            get { return mySelectedViewModel; }
            set => SetProperty(ref mySelectedViewModel, value);
        }

        public string CurrentUserName
        {
            get { return myCurrentUserName; }
            set => SetProperty(ref myCurrentUserName, value);
        }

        private void UpdateViewModel(WpfAppForms natureBoxForms)
        {
            switch (natureBoxForms)
            {
                case WpfAppForms.Customer:
                    SelectedViewModel = myCustomerRegistrationViewModel;
                    break;          
                //case WpfAppForms.Product:
                //    SelectedViewModel = myProductRegistrationViewModel;
                //    break;         
                //case WpfAppForms.Invoice:
                //    SelectedViewModel = myInvoiceViewModel;
                //    break;
                //case WpfAppForms.CustomerInvoiceReport:
                //    SelectedViewModel = myCustomerAttendanceViewModel;
                //    break;  
                //case WpfAppForms.BackUp:
                //    SelectedViewModel = myBackUpRestoreViewModel;
                //    break;
                default:
                    break;
            }
        }

        private void OnContactClick(object obj)
        {
            //SelectedViewModel = myContactViewModel;
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
