using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Common;
using WpfApp.DataAccess;
using WpfApp.Helpers;
using WpfApp.Model;

namespace WpfApp.Invoices.Service
{
    public class GstBillRepository : IGstBillRepository
    {
        private readonly IContextResolver _context;

        public GstBillRepository()
        {
            _context = new ContextResolver();
        }

        public async Task<List<GstBill>> GetAllAsync()
        {
            using (var ctx = _context.ResolveContext())
            {
                return await ctx.GstBills.AsNoTracking().ToListAsync();
            }
        }

        public async Task<GstBill> AddAsync(GstBill gstBill)
        {
            using (var ctx = _context.ResolveContext())
            {
                ctx.GstBills.AddRange(gstBill);

                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                    return null;
                }

                return gstBill;
            }
        }

        public async Task<GstBill> UpdateAsync(GstBill gstBill)
        {
            using (var ctx = _context.ResolveContext())
            {
                if (!ctx.GstBills.Local.Any(gst => gst.GstBillId == gstBill.GstBillId))
                {
                    ctx.GstBills.Attach(gstBill);
                }
                ctx.Entry(gstBill).State = EntityState.Modified;

                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                    return null;
                }
                return gstBill;
            }
        }

        public async Task<bool> DeleteAsync(int gstBillId)
        {
            using (var ctx = _context.ResolveContext())
            {
                var invoice = ctx.GstBills.FirstOrDefault(c => c.GstBillId == gstBillId);

                if (invoice != null)
                {
                    ctx.GstBills.Remove(invoice);
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
