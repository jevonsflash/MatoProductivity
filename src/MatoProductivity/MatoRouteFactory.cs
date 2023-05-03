using Abp.Dependency;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MatoProductivity
{
    public class MatoRouteFactory : RouteFactory, ISingletonDependency
    {
        private readonly IocManager iocManager;
        private readonly IUnitOfWorkManager unitOfWorkManager;

        public MatoRouteFactory(IocManager iocManager, IUnitOfWorkManager unitOfWorkManager, Type T)
        {
            this.iocManager = iocManager;
            this.unitOfWorkManager = unitOfWorkManager;
            this.T = T;
        }

        public Type T { get; }

        public override Element GetOrCreate()
        {
           return unitOfWorkManager.WithUnitOfWork(() =>
            {
                using (var objectWrapper = iocManager.ResolveAsDisposable(T))
                {
                    var element = objectWrapper.Object;
                    return element as Element;
                }
            });
        }


        public override Element GetOrCreate(IServiceProvider services)
        {
            return unitOfWorkManager.WithUnitOfWork(() =>
            {
                using (var objectWrapper = iocManager.ResolveAsDisposable(T))
                {
                    var element = objectWrapper.Object;
                    return element as Element;
                }
            });    
        }
    }
}
