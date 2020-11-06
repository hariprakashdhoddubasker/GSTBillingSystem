using Microsoft.EntityFrameworkCore;
using WpfApp.DataAccess;
using WpfApp.Helpers;
using WpfApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Employees.Service
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly Func<WfpAppDbContext> _context;

        public EmployeeRepository(Func<WfpAppDbContext> contextCreator)
        {
            _context = contextCreator;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using (var ctx = _context())
            {
                return await ctx.Employees.AsNoTracking().ToListAsync();
            }
        }

        public Task<Employee> GetEntityByIdAsync(int id)
        {
            using (var ctx = _context())
            {
                return ctx.Employees.FirstOrDefaultAsync(c => c.EmployeeId == id);
            }
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            using (var ctx = _context())
            {
                ctx.Employees.Add(employee);
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return employee;
            }
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            using (var ctx = _context())
            {
                if (!ctx.Employees.Local.Any(c => c.EmployeeId == employee.EmployeeId))
                {
                    ctx.Employees.Attach(employee);
                }
                ctx.Entry(employee).State = EntityState.Modified;
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return employee;
            }
        }
    }
}
