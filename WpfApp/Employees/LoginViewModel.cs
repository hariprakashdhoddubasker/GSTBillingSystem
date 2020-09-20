using WpfApp.Common;
using WpfApp.Employees.Service;
using WpfApp.Helpers;
using WpfApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace WpfApp.Employees
{
    public class LoginViewModel : AbstractNotifyPropertyChanged
    {
        private IEmployeeRepository myEmployeeRepository;
        private List<Employee> allEmployees;
        public LoginViewModel(IEmployeeRepository employeeRepository)
        {
            Employee = new Employee();
            this.myEmployeeRepository = employeeRepository;
            this.BtnLoginCommand = new Command(this.BtnLoginClick, this.CanCheckCredencials);
            Employee.PropertyChanged += Employee_PropertyChanged;
        }

        public ICommand BtnLoginCommand { get; }
        public Employee Employee { get; set; }
        public event EventHandler LoginCompleted;

        private bool CanCheckCredencials(object arg)
        {
            return !string.IsNullOrEmpty(Employee.UserName);
        }

        private void BtnLoginClick(object obj)
        {
            var currentUser = allEmployees.ToList().Where(c => string.Equals(c.UserName, Employee.UserName, StringComparison.OrdinalIgnoreCase) && string.Equals(c.Password, Employee.Password));

            if (currentUser.FirstOrDefault() == null)
            {
                UIService.ShowMessage("Invalid UserName or Password");
            }
            else
            {
                UIService.CurrentUser = currentUser.FirstOrDefault();
                LoginCompleted?.Invoke(this, EventArgs.Empty);
            }
        }

        public async void Load()
        {
            allEmployees = new List<Employee>(await myEmployeeRepository.GetAllAsync());        
        }

        private void Employee_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ((Command)this.BtnLoginCommand).RaiseCanExecuteChanged();
        }
    }
}
