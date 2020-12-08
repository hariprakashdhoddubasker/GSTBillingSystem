using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpfApp.Common;
using WpfApp.DataAccess;
using WpfApp.Helpers;
using WpfApp.Model;

namespace WpfApp.Registration.Service
{
    public class StateRepository : IStateRepository
    {
        private readonly IContextResolver _context;

        public StateRepository()
        {
            _context = new ContextResolver();
        }

        public async Task<List<State>> GetAllAsync()
        {
            using (var ctx = _context.ResolveContext())
            {
                return await ctx.States.AsNoTracking().ToListAsync();
            }
        }

        public Task<State> GetCustomerAsync(int id)
        {
            using (var ctx = _context.ResolveContext())
            {
                return ctx.States.FirstOrDefaultAsync(c => c.StateId == id);
            }
        }

        public async Task<State> AddAsync(State state)
        {
            using (var ctx = _context.ResolveContext())
            {
                ctx.States.Attach(state);
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return state;
            }
        }

        public async Task<State> UpdateAsync(State state)
        {
            using (var ctx = _context.ResolveContext())
            {
                if (!ctx.States.Local.Any(c => c.StateId == state.StateId))
                {
                    ctx.States.Attach(state);
                }
                ctx.Entry(state).State = EntityState.Modified;
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return state;
            }

        }

        public async Task DeleteAsync(int customerId)
        {
            using (var ctx = _context.ResolveContext())
            {
                var state = ctx.States.FirstOrDefault(c => c.StateId == customerId);
                if (state != null)
                {
                    ctx.States.Remove(state);
                }
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
            }
        }
    }
}
