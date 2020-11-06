using System.Collections.Generic;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.Registration.Service
{
    public interface IStateRepository
    {
        Task<State> AddAsync(State state);
        Task DeleteAsync(int customerId);
        Task<List<State>> GetAllAsync();
        Task<State> GetCustomerAsync(int id);
        Task<State> UpdateAsync(State state);
    }
}