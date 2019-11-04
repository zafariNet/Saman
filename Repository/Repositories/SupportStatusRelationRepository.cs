using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.UnitOfWork;
using Model.Support;
using Model.Support.Interfaces;

namespace Repository.Repositories
{
    public class SupportStatusRelationRepository:Repository<SupportStatusRelation>,ISupportStatusRelationRepository
    {
        public SupportStatusRelationRepository(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
