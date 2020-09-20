using System.Collections.Generic;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.Registration.Service
{
    public interface ICustomerRepository
    {
        Task<Customer> AddAsync(Customer customer);
        Task DeleteAsync(int customerId);
        Task<List<Customer>> GetAllAsync();
        Task<Customer> GetCustomerAsync(int id);
        Task<Customer> UpdateAsync(Customer customer);
    }
}