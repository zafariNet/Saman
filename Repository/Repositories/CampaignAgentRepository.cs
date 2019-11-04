using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Infrastructure.UnitOfWork;
using Model.Employees;
using Model.Employees.Interfaces;

namespace Repository.Repositories
{
    public class CampaignAgentRepository : Repository<CampaignAgent>, ICampaignAgentRepository
    {
        public CampaignAgentRepository(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
