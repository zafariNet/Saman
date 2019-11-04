using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.UnitOfWork;
using Model.Lead;
using Model.Lead.Interfaces;
using Model.Leads;

namespace Repository.Repositories
{
    public class LeadTitleTemplateRepository : Repository<LeadTitleTemplate>, ILeadTitleTemplateRepository
    {
        public LeadTitleTemplateRepository(IUnitOfWork uow) : base(uow)
        {

        }
    }
}
