using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Common.Events;
using WpfApp.Common.Wrapper;
using WpfApp.Helpers;
using WpfApp.Helpers.HtmlService;
using WpfApp.Invoices.Service;
using WpfApp.Model;
using WpfApp.Registration.Service;
using System.Printing;
using System.Windows.Controls;
using System.Windows.Xps;
using System.Windows.Documents;
using System.IO.Packaging;
using System.Windows.Xps.Packaging;
using System.Windows.Forms;
using System.Threading;

namespace WpfApp.Invoices
{
    public class InvoiceViewModel : BaseViewModel
    {
        #region Fields

        private string mySearchString;
        private ObservableCollection<Customer> myGridCustomers;
        private Customer myCustomer;
        private ObservableCollection<Product> myProducts;
        private InvoiceWrapper myInvoice;
        private List<Customer> myAllCustomers;
        private List<GstBill> myAllGstBills;
        private List<Product> myAllProducts;
        private bool myIsTamilNadu;
        private bool myIsOtherState;
        private bool myIsFormPostBack;
        private ObservableCollection<Invoice> myGridInvoices;
        private GstBill myGstBill;
        private int myIsTaxPayableReverseIndex;
        private ObservableCollection<string> myIsTaxPayableReverseItems;
        private readonly IInvoiceRepository myInvoiceRepo;
        private readonly ICustomerRepository myCustomerRepo;
        private readonly IProductRepository myProductRepo;
        private readonly IStateRepository myStateRepo;
        private readonly IGstBillRepository myGstBillRepository;
        private readonly ISignatureRepository mysignatureRepository;
        private IEventAggregator myEventAggregator;
        private string myButtonState;
        private string mySignatureFilePath;
        private System.Windows.Forms.WebBrowser webBrowser;

        #endregion

        #region Constructor

        public InvoiceViewModel(IInvoiceRepository invoiceRepo, ICustomerRepository customersRepo, IProductRepository productRepo, IStateRepository stateRepo, IGstBillRepository gstBillRepository, ISignatureRepository signatureRepository, IEventAggregator eventAggregator)
        {
            this.myInvoiceRepo = invoiceRepo;
            this.myCustomerRepo = customersRepo;
            this.myProductRepo = productRepo;
            this.myStateRepo = stateRepo;
            this.mysignatureRepository = signatureRepository;
            this.myGstBillRepository = gstBillRepository;
            this.BtnSaveCommand = new Command(this.OnSaveClick, this.CanExceuteSave);
            this.BtnClearCommand = new Command(this.OnClearClick, o => true);
            this.BtnAddCommand = new Command(this.OnAddClick, this.CanExceuteAdd);
            this.GridLeftDoubleClickCommand = new Command(this.OnGridLeftDoubleClick, o => true);

            myEventAggregator = eventAggregator;
            myEventAggregator.GetEvent<LoadGstBillEvent>().Subscribe(LoadGstBill);

            this.GstBill = new GstBill();
            this.Customer = new Customer();
            this.Invoice = new InvoiceWrapper(new Invoice());

            myAllCustomers = new List<Customer>();
            myAllProducts = new List<Product>();
            this.myAllGstBills = new List<GstBill>();
            GridInvoices = new ObservableCollection<Invoice>();
            this.IsTaxPayableReverseItems = new ObservableCollection<string> { "Yes", "No" };

            this.ButtonState = "SAVE";
            var t = new Thread(()=> { webBrowser = new System.Windows.Forms.WebBrowser(); });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
          
        }

        #endregion

        #region Properties

        public ICommand BtnSaveCommand { get; private set; }
        public ICommand BtnAddCommand { get; private set; }
        public ICommand BtnClearCommand { get; private set; }
        public ICommand GridLeftDoubleClickCommand { get; private set; }

        public Customer Customer
        {
            get => this.myCustomer;

            set
            {
                SetProperty(ref myCustomer, value);

                if (value == null)
                {
                    return;
                }
                GstBill.CustomerId = Customer.CustomerId;
                if (Customer.State == null)
                {
                    IsTamilNadu = false;
                    IsOtherState = false;
                    IsStateSelected = false;
                    return;
                }
                IsStateSelected = true;

                if (Customer.State.StateCode == 32)
                {
                    IsTamilNadu = true;
                    IsOtherState = false;
                }
                else
                {
                    IsTamilNadu = false;
                    IsOtherState = true;
                }
            }
        }

        public InvoiceWrapper Invoice
        {
            get => this.myInvoice;
            set => SetProperty(ref myInvoice, value);
        }

        public GstBill GstBill
        {
            get => this.myGstBill;
            set => SetProperty(ref myGstBill, value);
        }

        public ObservableCollection<Product> Products
        {
            get => this.myProducts;
            set => SetProperty(ref myProducts, value);
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

        public ObservableCollection<Invoice> GridInvoices
        {
            get => this.myGridInvoices;
            set => SetProperty(ref myGridInvoices, value);
        }

        public bool IsTamilNadu
        {
            get => this.myIsTamilNadu;
            set => SetProperty(ref myIsTamilNadu, value);
        }

        public bool IsOtherState
        {
            get => this.myIsOtherState;
            set => SetProperty(ref myIsOtherState, value);
        }

        public bool IsStateSelected
        {
            get => this.myIsOtherState;
            set => SetProperty(ref myIsOtherState, value);
        }

        public int IsTaxPayableReverseIndex
        {
            get => this.myIsTaxPayableReverseIndex;
            set => SetProperty(ref myIsTaxPayableReverseIndex, value);
        }

        public ObservableCollection<string> IsTaxPayableReverseItems
        {
            get => this.myIsTaxPayableReverseItems;
            set => SetProperty(ref myIsTaxPayableReverseItems, value);
        }

        public string ButtonState
        {
            get => this.myButtonState;
            set => SetProperty(ref myButtonState, value);
        }

        #endregion

        #region Methods

        public async void Load()
        {
            if (myIsFormPostBack)
            {
                myIsFormPostBack = false;
                return;
            }
            await Task.Run(() =>
            {
                if (Customer == null)
                {
                    Customer = new Customer();
                }
                LoadCustomersWithState();

                myAllGstBills = myGstBillRepository.GetAllAsync().Result;

                foreach (var gstBill in myAllGstBills)
                {
                    gstBill.Customer = myAllCustomers.FirstOrDefault(customer => customer.CustomerId == gstBill.CustomerId);
                }

                if (string.IsNullOrEmpty(GstBill.BillSerialNumber))
                {
                    GstBill.BillSerialNumber = GetGstInvoiceSerialNumber();
                }

                myAllProducts = myProductRepo.GetAllAsync().Result;
                Products = new ObservableCollection<Product>(myAllProducts);
                SubscribeInvoicePropertyChange();

                var signature = mysignatureRepository.GetAllAsync().Result.FirstOrDefault();

                if (signature != null)
                {
                    mySignatureFilePath = signature.SignatureFilePath;
                }
                GstBill.PropertyChanged += GstBill_PropertyChanged;
                Customer.PropertyChanged += Customer_PropertyChanged;
                this.ButtonState = "SAVE";
            });
        }

        private void LoadGstBill(GstBill gstBill)
        {
            myIsFormPostBack = true;
            Clear();
            LoadCustomersWithState();

            this.Customer = myAllCustomers.FirstOrDefault(customer => customer.CustomerId == gstBill.CustomerId);
            this.SearchString = this.Customer.Name;

            var allInvoices = myInvoiceRepo.GetAllAsync().Result.Where(invoice => invoice.GstBillId == gstBill.GstBillId);

            foreach (var invoice in allInvoices)
            {
                invoice.Product = myAllProducts.FirstOrDefault(product => product.ProductId == invoice.ProductId);
            }
            var tempInvoices = allInvoices.Where(invoice => invoice.GstBillId == gstBill.GstBillId);
            var index = 1;
            GridInvoices.Clear();
            tempInvoices.ToList().ForEach((invoice) =>
                                    {
                                        invoice.DisplayNumber = index++;
                                        GridInvoices.Add(invoice);
                                    });

            SubscribeInvoicePropertyChange();
            GstBill = gstBill;
            this.ButtonState = "UPDATE";
        }

        private void LoadCustomersWithState()
        {
            myAllCustomers.Clear();
            myAllCustomers.AddRange(myCustomerRepo.GetAllAsync().Result);
            var allStates = myStateRepo.GetAllAsync().Result;

            foreach (var customer in myAllCustomers)
            {
                customer.State = allStates.FirstOrDefault(employee => employee.StateId == customer.StateId);
            }
        }

        private async void OnSaveClick(object obj)
        {
            if (string.IsNullOrEmpty(mySignatureFilePath))
            {
                UIService.ShowMessage("Register Signature before proceeding");
                return;
            }

            if (ButtonState == "SAVE")
            {
                AddInvoiceToGrid();
            }

            GstBill savedGstBill = await CreateGstBill();

            savedGstBill.Invoices = GridInvoices.ToList();
            savedGstBill.GstBillFilePath = HtmlService.GenerateInvoice(Customer, savedGstBill);

            List<Invoice> savedInvoices = await UpdateInvoiceInDB(savedGstBill);

            Clear();
            Load();
            ((Command)this.BtnSaveCommand).RaiseCanExecuteChanged();
            ((Command)this.BtnAddCommand).RaiseCanExecuteChanged();

            Print(savedGstBill.GstBillFilePath);

            if (savedInvoices != null)
            {
                if (ButtonState == "UPDATE")
                {
                    ButtonState = "SAVE";
                }
                UIService.ShowMessage("Invoice Saved");
            }
        }

        void Print(string str)
        {
            webBrowser.DocumentText = File.ReadAllText(str);
            webBrowser.DocumentCompleted += webBrowser_DocumentCompleted;
        }

        void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ((System.Windows.Forms.WebBrowser)sender).ShowPrintPreviewDialog();
        }

        private void OnAddClick(object obj)
        {
            AddInvoiceToGrid();
            SubscribeInvoicePropertyChange();
        }

        private void OnClearClick(object obj)
        {
            Clear();
            Load();
            this.ButtonState = "SAVE";
            ((Command)this.BtnSaveCommand).RaiseCanExecuteChanged();
            ((Command)this.BtnAddCommand).RaiseCanExecuteChanged();
        }

        private void OnGridLeftDoubleClick(object obj)
        {
            var selectedInvoice = (Invoice)obj;
            if (selectedInvoice == null)
            {
                return;
            }
            this.Invoice = new InvoiceWrapper(selectedInvoice);
            ((Command)this.BtnSaveCommand).RaiseCanExecuteChanged();
            ((Command)this.BtnAddCommand).RaiseCanExecuteChanged();

            SubscribeInvoicePropertyChange();
        }

        private bool CanExceuteSave(object arg)
        {
            if (GridInvoices.Any() || CanExceuteAdd(null))
            {
                return true;
            }

            return false;
        }

        private bool CanExceuteAdd(object arg)
        {
            if (Customer != null && Invoice != null && Invoice.ProductId != 0 &&
                ((Invoice.Weight > 0 || Invoice.AmountAfterTax > 0) && Invoice.Rate > 0) &&
                !string.IsNullOrEmpty(GstBill.IsTaxPayableReverse) && !Invoice.HasErrors)
            {
                return true;
            }
            return false;
        }

        private void AddInvoiceToGrid()
        {
            var index = 0;

            if (!CanExceuteAdd(null))
            {
                return;
            }

            if (GridInvoices.Any())
            {
                index = GridInvoices.Max(invoice => invoice.DisplayNumber);
            }
            var newInvoice = Invoice.Clone(++index);
            GridInvoices.Add(newInvoice);
            Invoice = new InvoiceWrapper(new Invoice());
            Invoice.ClearCalculations();
            ((Command)this.BtnAddCommand).RaiseCanExecuteChanged();
        }

        private async Task<GstBill> CreateGstBill()
        {
            if (GridInvoices.Any())
            {
                GstBill.AmountBeforeTax = Math.Round(GridInvoices.Sum(invoice => invoice.AmountBeforeTax), 2);
                GstBill.IGST = Math.Round(GridInvoices.Sum(invoice => invoice.IGST), 2);
                GstBill.SGST = Math.Round(GridInvoices.Sum(invoice => invoice.SGST), 2);
                GstBill.CGST = Math.Round(GridInvoices.Sum(invoice => invoice.CGST), 2);

                if (IsTamilNadu)
                {
                    GstBill.TotalTaxAmount = GstBill.SGST + GstBill.CGST;
                }
                else
                {
                    GstBill.TotalTaxAmount = GstBill.IGST;
                }
                var amountAfterTaxWithPrecision = Math.Round(GstBill.AmountBeforeTax + GstBill.TotalTaxAmount, 2);
                GstBill.RoundOff = NumbersHelper.GetRoundOffValue(amountAfterTaxWithPrecision);
                var amountAfterTax = amountAfterTaxWithPrecision + GstBill.RoundOff;

                GstBill.AmountAfterTax = amountAfterTax;
                GstBill.AmountAfterTaxString = NumbersHelper.ConvertAmount(amountAfterTax);
                GstBill.SignatureFilePath = mySignatureFilePath;
            }
            var gstFilePath = string.Empty;

            if (!string.IsNullOrEmpty(GstBill.GstBillFilePath))
            {
                gstFilePath = GstBill.GstBillFilePath;
            }

            var folderPath = Utility.GetAvailableDrivePath() + "InvoiceReport";

            List<string> fileCopyNames = new List<string>() { "Duplicate_Copy", "Aditor_Copy", "Original_Copy" };
            var reportPaths = GstBill.GstBillFilePath;

            if (ButtonState != "SAVE")
            {
                GstBill.GstBillFilePath = string.Empty;
            }
            foreach (var fileCopyName in fileCopyNames)
            {
                if (!string.IsNullOrEmpty(GstBill.GstBillFilePath))
                {
                    GstBill.GstBillFilePath += "|";
                }
                GstBill.GstBillFilePath += $"{ folderPath }\\{Customer.Name}_{Customer.CustomerId}_{fileCopyName}_{GstBill.GstDate:ddMMMyyyy_hh_mm_ss_tt}.html";
            }
            GstBill savedGstBill;

            if (ButtonState == "SAVE")
            {
                GstBill.GstBillId = 0;

                savedGstBill = await myGstBillRepository.AddAsync(GstBill);
            }
            else
            {
                foreach (var reportPath in reportPaths.Split("|"))
                {
                    if (File.Exists(reportPath))
                    {
                        File.Delete(reportPath);
                    }
                }
                savedGstBill = await myGstBillRepository.UpdateAsync(GstBill);
            }

            return savedGstBill;
        }

        private string GetGstInvoiceSerialNumber()
        {
            (DateTime, DateTime) financialYear = Utility.GetFinancialYear(DateTime.Now);
            var financialYearString = financialYear.Item1.Year.ToString().Substring(2) + "-" + financialYear.Item2.Year.ToString().Substring(2);
            string stringIndex = GetCurrentYearNextSerialNumber(financialYearString);
            var gstInvoiceSerialNo = financialYearString + "/" + stringIndex;
            return gstInvoiceSerialNo;
        }

        private async Task<List<Invoice>> UpdateInvoiceInDB(GstBill savedGstBill)
        {
            var newInvoiceList = new List<Invoice>();
            var existingInvoiceList = new List<Invoice>();

            foreach (var invoice in GridInvoices)
            {
                invoice.GstBillId = savedGstBill.GstBillId;
                invoice.Product = null;
                if (string.IsNullOrEmpty(invoice.IsInvoiceAdded))
                {
                    newInvoiceList.Add(invoice);
                }
                else
                {
                    existingInvoiceList.Add(invoice);
                }
            }

            List<Invoice> savedInvoices = new List<Invoice>();
            newInvoiceList.ForEach(invoice => invoice.IsInvoiceAdded = "Yes");
            savedInvoices.AddRange(await myInvoiceRepo.AddRangeAsync(newInvoiceList));
            savedInvoices.AddRange(await myInvoiceRepo.UpdateRangeAsync(existingInvoiceList));

            return savedInvoices;
        }

        private string GetCurrentYearNextSerialNumber(string financialYear)
        {
            var allInvoiceNumbers = myAllGstBills.Where(invoice => invoice.BillSerialNumber.StartsWith(financialYear)).Select(invoice => Convert.ToInt32(invoice.BillSerialNumber.Substring(6))).ToList();
            allInvoiceNumbers.Sort();
            var index = 1;

            foreach (var invoiceNumber in allInvoiceNumbers)
            {
                if (invoiceNumber != index)
                {
                    //If any invoice is deleted the deleted id should be filled
                    break;
                }
                index++;
            }
            var stringIndex = index.ToString().Length == 1 ? "0" + index : index.ToString();
            return stringIndex;
        }

        private void Invoice_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //To reflect invoice product id change Main Invoice Object
            if (e.PropertyName == nameof(Invoice.Product))
            {
                UnSubscribeInvoicePropertyChange();
                Invoice.ProductId = Invoice.Product.ProductId;
                SubscribeInvoicePropertyChange();
            }
            if (e.PropertyName == nameof(Invoice.Rate) ||
                e.PropertyName == nameof(Invoice.Weight) ||
                e.PropertyName == nameof(Invoice.AmountAfterTax))
            {
                if (Invoice.Product == null || Invoice == null || Customer.State == null)
                {
                    return;
                }
                if (e.PropertyName == nameof(Invoice.Weight))
                {
                    UnSubscribeInvoicePropertyChange();
                    Invoice.AmountAfterTax = 0;
                    SubscribeInvoicePropertyChange();
                }
                if (e.PropertyName == nameof(Invoice.AmountAfterTax))
                {
                    UnSubscribeInvoicePropertyChange();
                    Invoice.Weight = 0;
                    SubscribeInvoicePropertyChange();
                }

                if (Invoice.Rate > 0)
                {
                    if (e.PropertyName == nameof(Invoice.Weight) && Invoice.Weight > 0)
                    {
                        CalculateStraightGST();
                        return;
                    }

                    if (e.PropertyName == nameof(Invoice.AmountAfterTax) && Invoice.AmountAfterTax > 0)
                    {
                        CalculateReverseGST();
                        return;
                    }
                    if (e.PropertyName == nameof(Invoice.Rate) && Invoice.Weight > 0)
                    {
                        CalculateStraightGST();
                        return;
                    }
                    if (e.PropertyName == nameof(Invoice.Rate) && Invoice.AmountAfterTax > 0)
                    {
                        CalculateReverseGST();
                        return;
                    }
                }
            }
        }

        private void SubscribeInvoicePropertyChange()
        {
            if (!Invoice.IsHandlerSubscribed)
            {
                Invoice.PropertyChanged += Invoice_PropertyChanged;
                Invoice.IsHandlerSubscribed = true;
            }
        }

        private void UnSubscribeInvoicePropertyChange()
        {
            if (Invoice.IsHandlerSubscribed)
            {
                Invoice.PropertyChanged -= Invoice_PropertyChanged;
                Invoice.IsHandlerSubscribed = false;
            }
        }

        private void GstBill_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GstBill.IsTaxPayableReverse) && Invoice.AmountAfterTax > 0)
            {
                ((Command)this.BtnSaveCommand).RaiseCanExecuteChanged();
                ((Command)this.BtnAddCommand).RaiseCanExecuteChanged();
            }
        }

        private void Customer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "CustomerId")
            {
                ((Command)this.BtnSaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void CalculateReverseGST()
        {
            var gst = Math.Round(Invoice.AmountAfterTax - Invoice.AmountAfterTax * (100f / (100f + 3f)), 2);
            Invoice.AmountBeforeTax = Math.Round(Invoice.AmountAfterTax - gst, 2);

            UnSubscribeInvoicePropertyChange();
            Invoice.Weight = Math.Round(Invoice.AmountBeforeTax / Invoice.Rate, 3);

            SubscribeInvoicePropertyChange();

            UpdateGstBasedOnState(gst);
        }

        private void CalculateStraightGST()
        {
            Invoice.AmountBeforeTax = Math.Round(Invoice.Weight * Invoice.Rate, 2);
            var gst = Math.Round(Invoice.AmountBeforeTax * 3 / 100, 2);

            UnSubscribeInvoicePropertyChange();
            Invoice.AmountAfterTax = Math.Round(Invoice.AmountBeforeTax + gst, 2);

            SubscribeInvoicePropertyChange();

            UpdateGstBasedOnState(gst);
        }

        private void UpdateGstBasedOnState(double gst)
        {
            if (IsTamilNadu)
            {
                Invoice.SGST = Math.Round(gst / 2, 2);
                Invoice.CGST = Math.Round(gst / 2, 2);
            }
            else if (IsOtherState)
            {
                Invoice.IGST = gst;
            }

            ((Command)this.BtnSaveCommand).RaiseCanExecuteChanged();
            ((Command)this.BtnAddCommand).RaiseCanExecuteChanged();
        }

        public void Clear()
        {
            this.SearchString = string.Empty;
            this.Invoice = new InvoiceWrapper(new Invoice());
            //this.Customer.Clear();
            this.myAllCustomers.Clear();
            this.GridCustomers.Clear();
            this.IsStateSelected = false;
            this.GridInvoices.Clear();
            this.IsTaxPayableReverseIndex = -1;
            this.GstBill = new GstBill();

            UnSubscribeInvoicePropertyChange();
        }

        #endregion
    }
}
