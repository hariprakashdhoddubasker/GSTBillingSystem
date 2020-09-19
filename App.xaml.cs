using RishiSilvers.Dialog;
using RishiSilvers.Employees;
using RishiSilvers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Unity;

namespace RishiSilvers
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly List<string> macIdList = new List<string>() { "C8D9D2EE9E6E"};

        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "NatureBox";
            var _mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew)
            {
                MessageBox.Show("App is already running! Please click Ok to exit.");
                Current.Shutdown();
            }
            var result = macIdList.Select(x => x).Intersect(GetMacAddress()).Any();
            if (!result)
            {
                MessageBox.Show("MAC address mismatched, contact Admin. Please click Ok to exit.");
                Current.Shutdown();
            }

            base.OnStartup(e);
            IDialogService dialogService = new DialogService(MainWindow);
            dialogService.Register<DialogViewModel, DialogWindow>();
            UIService.DispatcherSynchronizationContext = new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher, DispatcherPriority.DataBind);
            UIService.DialogService = dialogService;
            var mainWindowViewModel = ContainerHelper.Container.Resolve<MainWindowViewModel>();
            var loginViewModel = ContainerHelper.Container.Resolve<LoginViewModel>();
            var login = new LoginView { DataContext = loginViewModel };

            loginViewModel.LoginCompleted += (sender, args) =>
            {
                mainWindowViewModel.RegisterNavigationViewModel();
                MainWindow mainWindow = new MainWindow() { DataContext = mainWindowViewModel };
                mainWindow.Show();
                login.Close();
            };
            login.ShowDialog();

            //UIService.CurrentUser = new Model.Partner { UserName = "DemoUser", Role = NatureBoxRoles.Admin.ToString() };
            //mainWindowViewModel.RegisterNavigationViewModel();
            //MainWindow mainWindow = new MainWindow() { DataContext = mainWindowViewModel };
            //mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected error occured. Please inform the admin."
              + Environment.NewLine + e.Exception.Message, "Unexpected error");
            LogService.LogException(e.Exception);
            e.Handled = true;
        }

        public static List<string> GetMacAddress()
        {
            List<string> macAddressList = new List<string>();
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                string macAddress = adapter.GetPhysicalAddress().ToString();
                if (!string.IsNullOrEmpty(macAddress) && !macAddressList.Contains(macAddress))
                {
                    macAddressList.Add(macAddress);
                }
            }
            return macAddressList;
        }
    }
}
