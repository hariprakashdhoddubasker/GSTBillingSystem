﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpfApp.Common;
using WpfApp.DataAccess;
using WpfApp.Helpers;
using WpfApp.Model;

namespace WpfApp.Invoices.Service
{
    public class SignatureRepository : ISignatureRepository
    {
        private readonly IContextResolver _context;

        public SignatureRepository()
        {
            _context = new ContextResolver();
        }

        public async Task<List<Signature>> GetAllAsync()
        {
            using (var ctx = _context.ResolveContext())
            {
                return await ctx.Signatures.AsNoTracking().ToListAsync();
            }
        }

        public async Task<Signature> AddAsync(Signature signature)
        {
            using (var ctx = _context.ResolveContext())
            {
                ctx.Signatures.AddRange(signature);

                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                    return null;
                }

                return signature;
            }
        }

        public async Task<Signature> UpdateAsync(Signature signature)
        {
            using (var ctx = _context.ResolveContext())
            {
                if (!ctx.Signatures.Local.Any(gst => gst.SignatureId == signature.SignatureId))
                {
                    ctx.Signatures.Attach(signature);
                }
                ctx.Entry(signature).State = EntityState.Modified;

                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                    return null;
                }
                return signature;
            }
        }
    }
}
