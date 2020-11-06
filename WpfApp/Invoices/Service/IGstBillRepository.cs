using System.Collections.Generic;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.Invoices.Service
{
    public interface IGstBillRepository
    {
        Task<GstBill> AddAsync(GstBill gstBill);
        Task<bool> DeleteAsync(int gstBillId);
        Task<List<GstBill>> GetAllAsync();
        Task<GstBill> UpdateAsync(GstBill gstBills);
    }
}