using WpfApp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpfApp.Employees.Service
{
    public interface IEmployeeRepository
    {
        Task<Employee> AddAsync(Employee employee);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetEntityByIdAsync(int id);
        Task<Employee> UpdateAsync(Employee employee);
    }
}