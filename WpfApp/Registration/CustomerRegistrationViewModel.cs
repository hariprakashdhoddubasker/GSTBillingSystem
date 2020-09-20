using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WpfApp.Helpers;
using WpfApp.Model;
using WpfApp.Registration.Service;

namespace WpfApp.Registration
{
    public class CustomerRegistrationViewModel : BaseViewModel
    {
        private ICustomerRepository myCustomerRepo;
        public ICommand BtnSaveUpdateCommand { get; }
        public ICommand BtnDeleteCommand { get; }
        public ICommand GridUpdateCommand { get; }

        private ObservableCollection<Customer> myGridCustomers;
        private string myButtonState;
        private Customer myCustomer;

        public CustomerRegistrationViewModel(ICustomerRepository customerRepo)
        {
            myCustomerRepo = customerRepo;
            this.Customer = new Customer();
            this.BtnSaveUpdateCommand = new Command(this.BtnSaveUpdateClick, this.IsValidCustomer);
            this.BtnDeleteCommand = new Command(this.BtnDeleteClick, this.IsValidCustomer);
            this.GridUpdateCommand = new Command(this.GridUpdate, o => true);
            GridCustomers = new ObservableCollection<Customer>();
            Customer.PropertyChanged += Customer_PropertyChanged;
        }

        public Customer Customer
        {
            get => this.myCustomer;
            set => SetProperty(ref myCustomer, value);
        }

        public string ButtonState
        {
            get => this.myButtonState;
            set => SetProperty(ref myButtonState, value);
        }

        public ObservableCollection<Customer> GridCustomers
        {
            get => this.myGridCustomers;
            set => SetProperty(ref myGridCustomers, value);
        }

        public async void Load()
        {
            Clear();
            GridCustomers = new ObservableCollection<Customer>(await myCustomerRepo.GetAllAsync());
        }

        private void Customer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "CustomerId")
            {
                ((Command)this.BtnSaveUpdateCommand).RaiseCanExecuteChanged();
            }
        }

        private bool IsValidCustomer(object arg)
        {
            return !string.IsNullOrEmpty(Customer.Name) &&
                Customer.DOB != null &&
                Customer.DOJ != null &&
                long.TryParse(Customer.MobileNumber.ToString(), out _) &&
                Customer.MobileNumber.ToString().Length == 10 &&
                int.TryParse(Customer.EmployeeId.ToString(), out _);
        }

        private void BtnSaveUpdateClick(object obj)
        {
            if (ButtonState == "SAVE")
            {
                myCustomerRepo.AddAsync(Customer);
            }
            else
            {
                myCustomerRepo.UpdateAsync(Customer);
            }

            Load();
        }

        private void BtnDeleteClick(object obj)
        {
            myCustomerRepo.DeleteAsync(Customer.CustomerId);
            this.ButtonState = "SAVE";
            Load();
        }

        private void Clear()
        {
            Customer.CustomerId = 0;
            Customer.Name = string.Empty;
            Customer.DOB = DateTime.Now;
            Customer.DOJ = DateTime.Now;
            Customer.EmployeeId = 0;
            Customer.MobileNumber = 0;
            Customer.BalanceAmount = 0;
            Customer.TotalAmountPaid = 0;

            this.ButtonState = "SAVE";
            RaiseCanExecuteChanged();
        }

        private void GridUpdate(object customer)
        {
            this.Customer = (Customer)customer;
            this.ButtonState = "UPDATE";
            RaiseCanExecuteChanged();
        }

        private void RaiseCanExecuteChanged()
        {
            ((Command)this.BtnSaveUpdateCommand).RaiseCanExecuteChanged();
            ((Command)this.BtnDeleteCommand).RaiseCanExecuteChanged();
            Customer.PropertyChanged += Customer_PropertyChanged;
        }
    }
}
