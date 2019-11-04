using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.UnitOfWork;
using Model.Support;
using Model.Support.Interfaces;

namespace Repository.Repositories
{
    public class SupportExpertDispatchRepository : Repository<SupportExpertDispatch>,ISupportExpertDispatchRepository
    {
        public SupportExpertDispatchRepository(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
