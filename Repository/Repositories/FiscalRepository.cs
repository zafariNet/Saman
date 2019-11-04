using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Fiscals;
using Infrastructure.UnitOfWork;
using Model.Fiscals.Interfaces;
using Infrastructure.Domain;
using NHibernate;

namespace Repository.Repositories
{
    public class FiscalRepository : Repository<Fiscal>, IFiscalRepository
    {
        public FiscalRepository(IUnitOfWork uow)
            : base(uow)
        {
        }


    }
}
