using Prism.Events;
using RishiSilvers.Employees.Service;
using Unity;
using Unity.Lifetime;

namespace RishiSilvers.Helpers
{
    public static class ContainerHelper
    {
        static ContainerHelper()
        {
            Container = new UnityContainer();
            Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IEmployeeRepository, EmployeeRepository>(new ContainerControlledLifetimeManager());
            //Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
            //Container.RegisterType<ICustomerRepository, CustomersRepository>(new ContainerControlledLifetimeManager());
            //Container.RegisterType<IProductRepository, ProductRepository>(new ContainerControlledLifetimeManager());
            //Container.RegisterType<ICustomerPaymentRepository, PaymentRepository>(new ContainerControlledLifetimeManager());     
            //Container.RegisterType<IInvoiceRepository, InvoiceRepository>(new ContainerControlledLifetimeManager());
            //Container.RegisterType<IBackUpAndRestoreRepository, BackUpAndRestoreRepository>(new ContainerControlledLifetimeManager());
        }

        public static IUnityContainer Container { get; private set; }
    }
}
