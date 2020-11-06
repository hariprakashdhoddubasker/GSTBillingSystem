using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpfApp.DataAccess;
using WpfApp.Helpers;
using WpfApp.Model;

namespace WpfApp.Invoices.Service
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly Func<WfpAppDbContext> _context;

        public InvoiceRepository(Func<WfpAppDbContext> contextCreator)
        {
            _context = contextCreator;
        }

        public async Task<List<Invoice>> GetAllAsync()
        {
            using (var ctx = _context())
            {
                return await ctx.Invoices.AsNoTracking().ToListAsync();
            }
        }

        public async Task<List<Invoice>> AddRangeAsync(List<Invoice> invoices)
        {
            using (var ctx = _context())
            {
                ctx.Invoices.AddRange(invoices);

                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                    return null;
                }

                return invoices;
            }
        }

        public async Task<List<Invoice>> UpdateRangeAsync(List<Invoice> invoices)
        {
            using (var ctx = _context())
            {
                if (!ctx.Invoices.Local.Except(invoices).Any())
                {
                    ctx.Invoices.AttachRange(invoices);
                }
                foreach (var invoice in invoices)
                {
                    ctx.Entry(invoice).State = EntityState.Modified;
                }
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                    return null;
                }
                return invoices;
            }
        }

        public async Task<bool> DeleteAsync(int invoiceId)
        {
            using (var ctx = _context())
            {
                var invoice = ctx.Invoices.FirstOrDefault(c => c.InvoiceId == invoiceId);

                if (invoice != null)
                {
                    ctx.Invoices.Remove(invoice);
                }
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                    return false;
                }
                return true;
            }
        }
    }
}
