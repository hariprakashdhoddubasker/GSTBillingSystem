using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RishiSilvers.Employees
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }
        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            var ctrl = (PasswordBox)sender;
            ((LoginViewModel)this.DataContext).Employee.Password = ctrl.Password;
        }
    }
}
