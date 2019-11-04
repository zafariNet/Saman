using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using Infrastructure.UnitOfWork;
using Model.Store.Interfaces;
using NHibernate.Transform;
using Services.ViewModels.Reports;

namespace Repository.Repositories
{
    public class CreditServiceRepository : Repository<CreditService>, ICreditServiceRepository
    {
        public CreditServiceRepository(IUnitOfWork uow)
            : base(uow)
        {
        }


    }
}
