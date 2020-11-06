using Microsoft.EntityFrameworkCore;
using WpfApp.Model;
using System.Configuration;

namespace WpfApp.DataAccess
{
    public class WfpAppDbContext : DbContext
    {
        public WfpAppDbContext()
        {
            CreateSchema();
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Product> Products { get; set; }        
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<GstBill> GstBills { get; set; }
        public DbSet<Signature> Signatures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConfigurationManager.ConnectionStrings["WpfAppDB"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        private void CreateSchema()
        {
            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
                AddAdmin();
            }
        }

        private bool AddAdmin()
        {
            Employees.Add(new Employee
            {
                UserName = "Rishi",
                Password = "54321",
                MobileNumber = 8089947074,
                Role = "Admin"
            });

            return SaveChanges() == 1;
        }
    }
}
