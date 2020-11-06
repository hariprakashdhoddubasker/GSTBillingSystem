using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WpfApp.Helpers;
using WpfApp.Model;
using WpfApp.Registration.Service;

namespace WpfApp.Registration
{
    public class ProductRegistrationViewModel : BaseViewModel
    {
        private readonly IProductRepository myProductRepository;
        public ICommand BtnSaveUpdateCommand { get; }
        public ICommand BtnDeleteCommand { get; }
        public ICommand GridUpdateCommand { get; }

        private ObservableCollection<Product> myGridProducts;
        private string myButtonState;
        private Product myProduct;

        public ProductRegistrationViewModel(IProductRepository stateRepository)
        {
            myProductRepository = stateRepository;
            this.SelectedProduct = new Product();
            this.BtnSaveUpdateCommand = new Command(this.OnSaveUpdateClick, this.CanExecuteCrudOperation);
            this.BtnDeleteCommand = new Command(this.OnDeleteClick, this.CanExecuteCrudOperation);
            this.GridUpdateCommand = new Command(this.GridUpdate, o => true);
            GridProducts = new ObservableCollection<Product>();
            SelectedProduct.PropertyChanged += Product_PropertyChanged;
        }

        public Product SelectedProduct
        {
            get => this.myProduct;
            set => SetProperty(ref myProduct, value);
        }

        public string ButtonState
        {
            get => this.myButtonState;
            set => SetProperty(ref myButtonState, value);
        }

        public ObservableCollection<Product> GridProducts
        {
            get => this.myGridProducts;
            set => SetProperty(ref myGridProducts, value);
        }

        public async void Load()
        {
            Clear();
            GridProducts = new ObservableCollection<Product>(await myProductRepository.GetAllAsync());
        }

        private void Product_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "ProductId")
            {
                ((Command)this.BtnSaveUpdateCommand).RaiseCanExecuteChanged();
            }
        }

        private bool CanExecuteCrudOperation(object arg)
        {
            return !string.IsNullOrEmpty(SelectedProduct.ProductName) &&
                long.TryParse(SelectedProduct.HsnCode.ToString(), out _);
        }

        private void OnSaveUpdateClick(object obj)
        {
            if (ButtonState == "SAVE")
            {
                myProductRepository.AddAsync(SelectedProduct);
            }
            else
            {
                myProductRepository.UpdateAsync(SelectedProduct);
            }

            Load();
        }

        private void OnDeleteClick(object obj)
        {
            myProductRepository.DeleteAsync(SelectedProduct.ProductId);
            this.ButtonState = "SAVE";
            Load();
        }

        public void Clear()
        {
            SelectedProduct = new Product();

            this.ButtonState = "SAVE";
            RaiseCanExecuteChanged();
        }

        private void GridUpdate(object state)
        {
            this.SelectedProduct = (Product)state;
            this.ButtonState = "UPDATE";
            RaiseCanExecuteChanged();
        }

        private void RaiseCanExecuteChanged()
        {
            ((Command)this.BtnSaveUpdateCommand).RaiseCanExecuteChanged();
            ((Command)this.BtnDeleteCommand).RaiseCanExecuteChanged();
            SelectedProduct.PropertyChanged += Product_PropertyChanged;
        }
    }
}
