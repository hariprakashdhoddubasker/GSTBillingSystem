using RishiSilvers.Common;
using Prism.Events;
using System.Threading.Tasks;
using Unity;
using System.Windows.Input;
using RishiSilvers.Helpers;

namespace RishiSilvers
{

    public class MainWindowViewModel : BaseViewModel
    {
        private BaseViewModel mySelectedViewModel;

        private IEventAggregator myEventAggregator;
        private string myCurrentUserName;

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            Task.Run(() =>
            {
                myEventAggregator = eventAggregator;

                //this.SelectedViewModel = myInvoiceViewModel;
                this.BtnContactCommand = new Command(this.OnContactClick, o => true);
            });
        }

        public ICommand BtnContactCommand { get; private set; }

        //public NavigationViewModel NavigationViewModel { get; set; }

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

        //private void UpdateViewModel(NatureBoxForms natureBoxForms)
        //{
        //    switch (natureBoxForms)
        //    {
        //        case NatureBoxForms.Customer:
        //            SelectedViewModel = myCustomerRegistrationViewModel;
        //            break;
        //        case NatureBoxForms.Partner:
        //            SelectedViewModel = myEmployeeRegistrationViewModel;
        //            break;
        //        case NatureBoxForms.Product:
        //            SelectedViewModel = myProductRegistrationViewModel;
        //            break;
        //        case NatureBoxForms.HealthRecord:
        //            SelectedViewModel = myHealthRecordViewModel;
        //            break;
        //        case NatureBoxForms.CustomerPayment:
        //            SelectedViewModel = myCustomerPaymentViewModel;
        //            break;
        //        case NatureBoxForms.ParterSettlement:
        //            SelectedViewModel = myPartnerPaymentViewModel;
        //            break;
        //        case NatureBoxForms.Invoice:
        //            SelectedViewModel = myInvoiceViewModel;
        //            break;
        //        case NatureBoxForms.CustomerInvoiceReport:
        //            SelectedViewModel = myCustomerAttendanceViewModel;
        //            break;
        //        case NatureBoxForms.PartnerInvoiceReport:
        //            SelectedViewModel = myEmployeeReportViewModel;
        //            break;
        //        case NatureBoxForms.CustomerPaymentReport:
        //            SelectedViewModel = myCustomerPaymentReportViewModel;
        //            break;
        //        case NatureBoxForms.ParnterSettlementReport:
        //            SelectedViewModel = myPartnerPaymentReportViewModel;
        //            break;
        //        case NatureBoxForms.BackUp:
        //            SelectedViewModel = myBackUpRestoreViewModel;
        //            break;
        //        default:
        //            break;
        //    }
        //}

        private void OnContactClick(object obj)
        {
            //SelectedViewModel = myContactViewModel;
        }

        public void RegisterNavigationViewModel()
        {
            //NavigationViewModel = ContainerHelper.Container.Resolve<NavigationViewModel>();
        }

        public void Load()
        {
            this.CurrentUserName = "Hi, " + UIService.CurrentUser.UserName;
        }
    }
}
