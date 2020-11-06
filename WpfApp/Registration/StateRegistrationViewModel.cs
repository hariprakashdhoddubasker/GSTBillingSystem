using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WpfApp.Helpers;
using WpfApp.Model;
using WpfApp.Registration.Service;

namespace WpfApp.Registration
{
    public class StateRegistrationViewModel : BaseViewModel
    {
        private IStateRepository myStateRepository;
        public ICommand BtnSaveUpdateCommand { get; }
        public ICommand BtnDeleteCommand { get; }
        public ICommand GridUpdateCommand { get; }

        private ObservableCollection<State> myGridStates;
        private string myButtonState;
        private State myState;

        public StateRegistrationViewModel(IStateRepository stateRepository)
        {
            myStateRepository = stateRepository;
            this.SelectedState = new State();
            this.BtnSaveUpdateCommand = new Command(this.OnSaveUpdateClick, this.CanExecuteCrudOperation);
            this.BtnDeleteCommand = new Command(this.OnDeleteClick, this.CanExecuteCrudOperation);
            this.GridUpdateCommand = new Command(this.GridUpdate, o => true);
            GridStates = new ObservableCollection<State>();
            SelectedState.PropertyChanged += State_PropertyChanged;
        }

        public State SelectedState
        {
            get => this.myState;
            set => SetProperty(ref myState, value);
        }

        public string ButtonState
        {
            get => this.myButtonState;
            set => SetProperty(ref myButtonState, value);
        }

        public ObservableCollection<State> GridStates
        {
            get => this.myGridStates;
            set => SetProperty(ref myGridStates, value);
        }

        public async void Load()
        {
            Clear();
            GridStates = new ObservableCollection<State>(await myStateRepository.GetAllAsync());
        }

        private void State_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "StateId")
            {
                ((Command)this.BtnSaveUpdateCommand).RaiseCanExecuteChanged();
            }
        }

        private bool CanExecuteCrudOperation(object arg)
        {
            return !string.IsNullOrEmpty(SelectedState.StateName) &&
                long.TryParse(SelectedState.StateCode.ToString(), out _);
        }

        private void OnSaveUpdateClick(object obj)
        {
            if (ButtonState == "SAVE")
            {
                myStateRepository.AddAsync(SelectedState);
            }
            else
            {
                myStateRepository.UpdateAsync(SelectedState);
            }

            Load();
        }

        private void OnDeleteClick(object obj)
        {
            myStateRepository.DeleteAsync(SelectedState.StateId);
            this.ButtonState = "SAVE";
            Load();
        }

        public void Clear()
        {
            SelectedState = new State();

            this.ButtonState = "SAVE";
            RaiseCanExecuteChanged();
        }

        private void GridUpdate(object state)
        {
            this.SelectedState = (State)state;
            this.ButtonState = "UPDATE";
            RaiseCanExecuteChanged();
        }

        private void RaiseCanExecuteChanged()
        {
            ((Command)this.BtnSaveUpdateCommand).RaiseCanExecuteChanged();
            ((Command)this.BtnDeleteCommand).RaiseCanExecuteChanged();
            SelectedState.PropertyChanged += State_PropertyChanged;
        }
    }
}
