using Microsoft.EntityFrameworkCore;
using WpfApp.Model;

namespace WpfApp.DataAccess
{
    public interface IWfpAppDbContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<Employee> Employees { get; set; }
        DbSet<GstBill> GstBills { get; set; }
        DbSet<Invoice> Invoices { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Signature> Signatures { get; set; }
        DbSet<State> States { get; set; }
    }
}