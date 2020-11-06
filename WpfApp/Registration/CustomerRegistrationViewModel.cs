using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using WpfApp.Helpers;
using WpfApp.Model;
using WpfApp.Registration.Service;

namespace WpfApp.Registration
{
    public class CustomerRegistrationViewModel : BaseViewModel
    {
        private ICustomerRepository myCustomerRepo;
        private IStateRepository myStateRepository;

        public ICommand BtnSaveUpdateCommand { get; }
        public ICommand BtnDeleteCommand { get; }
        public ICommand GridUpdateCommand { get; }

        private ObservableCollection<Customer> myGridCustomers;
        private string myButtonState;
        private Customer myCustomer;
        private ObservableCollection<State> myComboStates;
        private State mySelectedState;

        public CustomerRegistrationViewModel(ICustomerRepository customerRepo, IStateRepository stateRepository)
        {
            myCustomerRepo = customerRepo;
            myStateRepository = stateRepository;
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
        public ObservableCollection<State> ComboStates
        {
            get => this.myComboStates;
            set => SetProperty(ref myComboStates, value);
        }

        public State SelectedState
        {
            get => this.mySelectedState;
            set
            {
                SetProperty(ref mySelectedState, value);
                if (value == null)
                {
                    return;
                }
                Customer.StateId = mySelectedState.StateId;
                ((Command)this.BtnSaveUpdateCommand).RaiseCanExecuteChanged();
            }
        }

        public async void Load()
        {
            Clear();
            GridCustomers = new ObservableCollection<Customer>(await myCustomerRepo.GetAllAsync());
            ComboStates = new ObservableCollection<State>(await myStateRepository.GetAllAsync());

            foreach (var customer in GridCustomers)
            {
                customer.State = ComboStates.FirstOrDefault(state => state.StateId == customer.StateId);
            }
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
                    !string.IsNullOrEmpty(Customer.Address) &&
                    Customer.StateId != 0;
        }

        private void BtnSaveUpdateClick(object obj)
        {
            Customer.State = ComboStates.FirstOrDefault(state => state.StateId == Customer.StateId);
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

        public void Clear()
        {
            Customer = new Customer();

            this.ButtonState = "SAVE";
            RaiseCanExecuteChanged();
        }

        private void GridUpdate(object customer)
        {
            this.Customer = (Customer)customer;
            this.SelectedState = Customer.State;
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
