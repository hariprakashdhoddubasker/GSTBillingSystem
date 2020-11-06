using System.Collections.Generic;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.Invoices.Service
{
    public interface ISignatureRepository
    {
        Task<Signature> AddAsync(Signature signature);
        Task<List<Signature>> GetAllAsync();
        Task<Signature> UpdateAsync(Signature signature);
    }
}