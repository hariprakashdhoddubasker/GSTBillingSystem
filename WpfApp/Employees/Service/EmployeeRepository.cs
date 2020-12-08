using Microsoft.EntityFrameworkCore;
using WpfApp.DataAccess;
using WpfApp.Helpers;
using WpfApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Common;

namespace WpfApp.Employees.Service
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IContextResolver _context;

        public EmployeeRepository()
        {
            _context = new ContextResolver();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using (var ctx = _context.ResolveContext())
            {
                return await ctx.Employees.AsNoTracking().ToListAsync();
            }
        }

        public Task<Employee> GetEntityByIdAsync(int id)
        {
            using (var ctx = _context.ResolveContext())
            {
                return ctx.Employees.FirstOrDefaultAsync(c => c.EmployeeId == id);
            }
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            using (var ctx = _context.ResolveContext())
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
            using (var ctx = _context.ResolveContext())
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
