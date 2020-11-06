using Prism.Events;
using WpfApp.Employees.Service;
using Unity;
using Unity.Lifetime;
using WpfApp.Registration.Service;
using WpfApp.Invoices.Service;
using WpfApp.DataAccess;

namespace WpfApp.Helpers
{
    public static class ContainerHelper
    {
        static ContainerHelper()
        {
            Container = new UnityContainer();
            Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IEmployeeRepository, EmployeeRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ICustomerRepository, CustomerRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IStateRepository, StateRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IProductRepository, ProductRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IInvoiceRepository, InvoiceRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IGstBillRepository, GstBillRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ISignatureRepository, SignatureRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IBackUpAndRestoreRepository, BackUpAndRestoreRepository>(new ContainerControlledLifetimeManager());
        }

        public static IUnityContainer Container { get; private set; }
    }
}
