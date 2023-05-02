using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Microsoft.EntityFrameworkCore;

namespace MatoProductivity.EntityFrameworkCore.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            Helper.WithDbContextHelper.WithDbContext<MatoProductivityDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(MatoProductivityDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            // Host seed
            new InitialDbBuilder(context).Create();
        }

    }
}
