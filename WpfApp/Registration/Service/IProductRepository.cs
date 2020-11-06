using System.Collections.Generic;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.Registration.Service
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product state);
        Task DeleteAsync(int customerId);
        Task<List<Product>> GetAllAsync();
        Task<Product> GetCustomerAsync(int id);
        Task<Product> UpdateAsync(Product state);
    }
}