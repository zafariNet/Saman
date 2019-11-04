using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.UnitOfWork;
using Model.Support;
using Model.Support.Interfaces;
using NHibernate.Mapping.ByCode;

namespace Repository.Repositories
{
    public class SupportQcProblemRepository : Repository<SupportQcProblem>, ISupportQcProblemRepository
    {
        public SupportQcProblemRepository(IUnitOfWork uow):base(uow)
        {

        }
    }
}
