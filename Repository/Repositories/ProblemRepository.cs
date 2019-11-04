using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Support;
using Infrastructure.UnitOfWork;
using Model.Support.Interfaces;

namespace Repository.Repositories
{
    public class ProblemRepository : Repository<Problem>, IProblemRepository
    {
        public ProblemRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
