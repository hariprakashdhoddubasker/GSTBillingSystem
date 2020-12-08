using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpfApp.Common;
using WpfApp.DataAccess;
using WpfApp.Helpers;
using WpfApp.Model;

namespace WpfApp.Registration.Service
{
    public class ProductRepository : IProductRepository
    {
        private readonly IContextResolver _context;

        public ProductRepository()
        {
            _context = new ContextResolver();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            using (var ctx = _context.ResolveContext())
            {
                return await ctx.Products.AsNoTracking().ToListAsync();
            }
        }

        public Task<Product> GetCustomerAsync(int id)
        {
            using (var ctx = _context.ResolveContext())
            {
                return ctx.Products.FirstOrDefaultAsync(c => c.ProductId == id);
            }
        }

        public async Task<Product> AddAsync(Product state)
        {
            using (var ctx = _context.ResolveContext())
            {
                ctx.Products.Attach(state);
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return state;
            }
        }

        public async Task<Product> UpdateAsync(Product state)
        {
            using (var ctx = _context.ResolveContext())
            {
                if (!ctx.Products.Local.Any(c => c.ProductId == state.ProductId))
                {
                    ctx.Products.Attach(state);
                }
                ctx.Entry(state).State = EntityState.Modified;
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return state;
            }

        }

        public async Task DeleteAsync(int customerId)
        {
            using (var ctx = _context.ResolveContext())
            {
                var state = ctx.Products.FirstOrDefault(c => c.ProductId == customerId);
                if (state != null)
                {
                    ctx.Products.Remove(state);
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
