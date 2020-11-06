using System.Collections.Generic;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.Invoices.Service
{
    public interface IInvoiceRepository
    {
        Task<List<Invoice>> AddRangeAsync(List<Invoice> invoice);
        Task<bool> DeleteAsync(int invoiceId);
        Task<List<Invoice>> GetAllAsync();
        Task<List<Invoice>> UpdateRangeAsync(List<Invoice> invoices);
    }
}