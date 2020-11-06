using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfApp.Common.Events;
using WpfApp.Helpers;
using WpfApp.Helpers.HtmlService;
using WpfApp.Invoices.Service;
using WpfApp.Model;
using WpfApp.Registration.Service;

namespace WpfApp.Invoices
{
    public class InvoiceReportViewModel : BaseViewModel
    {
        private Customer myCustomer;
        private string mySearchString;

        private List<Customer> myAllCustomers;
        private List<Product> myAllProducts;
        private List<Invoice> myAllInvoice;
        private List<GstBill> myAllGstBill;
        private List<State> myAllStates;
        private ObservableCollection<Customer> myGridCustomers;
        private DateTime myFromDate = DateTime.Now;
        private DateTime myToDate = DateTime.Now;
        private string mySignatureFilePath;
        private double myTotalMRP;
        private int myTotalAttendedDays;
        private bool myIsAllFilter;
        private bool myIsAutoCompleteTextBoxEnabled;
        private ObservableCollection<GstBill> myGridGstBills;
        private readonly IProductRepository myProductRepo;
        private readonly IInvoiceRepository myInvoiceRepo;
        private readonly IGstBillRepository myGstBillRepository;
        private readonly ICustomerRepository myCustomerRepo;
        private readonly IStateRepository myStateRepo;
        private readonly ISignatureRepository mySignatureRepository;
        readonly IEventAggregator myEventAggregator;

        public InvoiceReportViewModel(ICustomerRepository customerRepo, IProductRepository productRepo, IInvoiceRepository invoiceRepo, IGstBillRepository gstBillRepository, IEventAggregator eventAggregator, IStateRepository stateRepo, ISignatureRepository signatureRepository)
        {
            myEventAggregator = eventAggregator;
            this.myCustomerRepo = customerRepo;
            this.myProductRepo = productRepo;
            this.myInvoiceRepo = invoiceRepo;
            this.myStateRepo = stateRepo;
            this.myGstBillRepository = gstBillRepository;
            this.mySignatureRepository = signatureRepository;
            this.myAllCustomers = new List<Customer>();
            this.GridGstBills = new ObservableCollection<GstBill>();
            this.BtnSearchCommand = new Command(this.OnSearchButtonClick, this.CanExecuteSearch);

            this.GridRowDeleteCommand = new Command(this.OnGridRowDelete, o => true);
            this.GridRowPrintCommand = new Command(this.OnGridRowPrint, o => true);
            this.GridUpdateCommand = new Command(this.OnGridUpdate, o => true);
            this.DownloadCommand = new Command(this.OnDownload, this.CanExecuteDownload);


            IsAllFilter = true;
            IsAutoCompleteTextBoxEnabled = true;
        }

        public ICommand BtnSearchCommand { get; private set; }
        public ICommand GridRowDeleteCommand { get; private set; }
        public ICommand GridRowPrintCommand { get; private set; }
        public ICommand GridUpdateCommand { get; private set; }
        public ICommand DownloadCommand { get; private set; }

        public Customer Customer
        {
            get => this.myCustomer;
            set
            {
                SetProperty(ref myCustomer, value);
                ((Command)this.BtnSearchCommand).RaiseCanExecuteChanged();
            }
        }

        public string SearchString
        {
            get => this.mySearchString;

            set
            {
                if (string.Equals(this.mySearchString, value)) return;
                this.mySearchString = value;
                GridCustomers = new ObservableCollection<Customer>(from emp in myAllCustomers where emp.Name.ToLower().Contains(value.ToLower()) select emp);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Customer> GridCustomers
        {
            get => this.myGridCustomers;
            set => SetProperty(ref myGridCustomers, value);
        }

        public ObservableCollection<GstBill> GridGstBills
        {
            get => this.myGridGstBills;
            set => SetProperty(ref myGridGstBills, value);
        }

        public DateTime FromDate
        {
            get => this.myFromDate;
            set => SetProperty(ref myFromDate, value);
        }
        public DateTime ToDate
        {
            get => this.myToDate;
            set => SetProperty(ref myToDate, value);
        }

        public double TotalMRP
        {
            get => this.myTotalMRP;
            set => SetProperty(ref myTotalMRP, value);
        }

        public int TotalAttendedDays
        {
            get => this.myTotalAttendedDays;
            set => SetProperty(ref myTotalAttendedDays, value);
        }

        public bool IsAllFilter
        {
            get => this.myIsAllFilter;
            set
            {
                SetProperty(ref myIsAllFilter, value);
                IsAutoCompleteTextBoxEnabled = !value;
                ((Command)this.BtnSearchCommand).RaiseCanExecuteChanged();
            }
        }

        public bool IsAutoCompleteTextBoxEnabled
        {
            get => this.myIsAutoCompleteTextBoxEnabled;
            set => SetProperty(ref myIsAutoCompleteTextBoxEnabled, value);
        }

        public async void Load()
        {
            Clear();
            myAllCustomers = new List<Customer>(await myCustomerRepo.GetAllAsync());
            myAllProducts = new List<Product>(await myProductRepo.GetAllAsync());
            myAllInvoice = new List<Invoice>(await myInvoiceRepo.GetAllAsync());
            myAllGstBill = new List<GstBill>(await myGstBillRepository.GetAllAsync());
            myAllStates = new List<State>(await myStateRepo.GetAllAsync());

            var signature = mySignatureRepository.GetAllAsync().Result.FirstOrDefault();

            if (signature != null)
            {
                mySignatureFilePath = signature.SignatureFilePath;
            }

            foreach (var gstBill in myAllGstBill)
            {
                gstBill.Customer = myAllCustomers.FirstOrDefault(customer => customer.CustomerId == gstBill.CustomerId);
            }

            foreach (var invoice in myAllInvoice)
            {
                invoice.Product = myAllProducts.FirstOrDefault(product => product.ProductId == invoice.ProductId);
            }
            (DateTime, DateTime) financialYear = Utility.GetFinancialYear(DateTime.Now);
            this.FromDate = financialYear.Item1;
            this.ToDate = financialYear.Item2;
            UpdateGrid();
        }

        private void OnSearchButtonClick(object obj)
        {
            if (this.ToDate.Hour == 0)
            {
                this.ToDate += new TimeSpan(23, 59, 0);
            }
            UpdateGrid();
        }

        private void OnGridRowPrint(object obj)
        {
            var currentGridRowGstBill = (GstBill)obj;
            var gstBillFilePaths = currentGridRowGstBill.GstBillFilePath.Split('|');

            currentGridRowGstBill.Customer.State = myAllStates.FirstOrDefault(state => state.StateId == currentGridRowGstBill.Customer.StateId);
            var tempInvoices = myAllInvoice.Where(invoice => invoice.GstBillId == currentGridRowGstBill.GstBillId).ToList();
            var index = 1;
            tempInvoices.ToList().ForEach(invoice => invoice.DisplayNumber = index++);
            currentGridRowGstBill.Invoices = tempInvoices;
            currentGridRowGstBill.SignatureFilePath = mySignatureFilePath;
            if (gstBillFilePaths.Count() > 0 && File.Exists(gstBillFilePaths[0]))
            {
                Utility.OpenFile(gstBillFilePaths[0]);
            }
            else
            {
                if (Customer.CustomerId == 0)
                {
                    Customer = currentGridRowGstBill.Customer;
                }
                HtmlService.GenerateInvoice(Customer, currentGridRowGstBill);
            }
        }

        private async void OnGridRowDelete(object obj)
        {
            var currentGridRowGstBill = (GstBill)obj;


           var result = UIService.ShowMessage($"Do you want to delete GST Bill : {currentGridRowGstBill.BillSerialNumber} ?", Visibility.Visible);

            if (!result.Value)
            {
                return;
            }

            var isInvoiceDeleted = await myGstBillRepository.DeleteAsync(currentGridRowGstBill.GstBillId);

            GridGstBills.Remove(currentGridRowGstBill);

            foreach (var reportPath in currentGridRowGstBill.GstBillFilePath.Split("|"))
            {
                if (File.Exists(reportPath))
                {
                    File.Delete(reportPath);
                }
            }

            if (isInvoiceDeleted)
            {
                UIService.ShowMessage($"GST Invoice deleted");
            }
        }

        private void OnGridUpdate(object gstBill)
        {
            GstBill selectedGstBill = (GstBill)gstBill;
            Clear();
            myEventAggregator.GetEvent<NavigateViewsEvent>().Publish(WpfAppForms.Invoice);
            myEventAggregator.GetEvent<LoadGstBillEvent>().Publish(selectedGstBill);
        }

        private void OnDownload(object obj)
        {
            HtmlService.GenerateGstBillStatement(GridGstBills.ToList());
        }

        private bool CanExecuteDownload(object arg)
        {
            return GridGstBills.Any();
        }

        private void UpdateGrid()
        {
            var filteredGstBills = new List<GstBill>();

            if (IsAllFilter)
            {
                filteredGstBills = myAllGstBill.Where(invoice => invoice.GstDate >= this.FromDate && invoice.GstDate < this.ToDate).ToList();
            }
            else
            {
                filteredGstBills = myAllGstBill.Where(gstBill => gstBill.GstDate >= this.FromDate && gstBill.GstDate < this.ToDate && gstBill.Customer == Customer).ToList();
            }
            filteredGstBills = filteredGstBills.OrderByDescending(gstBill => Convert.ToInt32(gstBill.BillSerialNumber.Substring(6))).ToList();
            GridGstBills = new ObservableCollection<GstBill>(filteredGstBills);
            ((Command)this.DownloadCommand).RaiseCanExecuteChanged();
        }

        private bool CanExecuteSearch(object arg)
        {
            return (Customer != null && Customer.CustomerId != 0) || IsAllFilter;
        }

        public void Clear()
        {
            Customer = null;
            SearchString = string.Empty;
            GridGstBills.Clear();
            TotalAttendedDays = 0;
            TotalMRP = 0;
            FromDate = DateTime.Now.Date + new TimeSpan(00, 01, 0);
            ToDate = DateTime.Now.Date + new TimeSpan(23, 59, 0);
            IsAllFilter = true;
        }
    }
}
