using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpfApp.DataAccess;
using WpfApp.Helpers;
using WpfApp.Model;

namespace WpfApp.Registration.Service
{
    public class CustomerRepository : ICustomerRepository
    {
        private Func<WfpAppDbContext> _context;

        public CustomerRepository(Func<WfpAppDbContext> contextCreator)
        {
            _context = contextCreator;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            using (var ctx = _context())
            {
                return await ctx.Customers.AsNoTracking().ToListAsync();
            }
        }

        public Task<Customer> GetCustomerAsync(int id)
        {
            using (var ctx = _context())
            {
                return ctx.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
            }
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            using (var ctx = _context())
            {
                ctx.Customers.Attach(customer);
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return customer;
            }
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            using (var ctx = _context())
            {
                if (!ctx.Customers.Local.Any(c => c.CustomerId == customer.CustomerId))
                {
                    ctx.Customers.Attach(customer);
                }
                ctx.Entry(customer).State = (EntityState)EntityState.Modified;
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return customer;
            }

        }

        public async Task DeleteAsync(int customerId)
        {
            using (var ctx = _context())
            {
                var customer = ctx.Customers.FirstOrDefault(c => c.CustomerId == customerId);
                if (customer != null)
                {
                    ctx.Customers.Remove(customer);
                }
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
            }
        }
    }
}
