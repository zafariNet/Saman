using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.UnitOfWork;
using Model.Leads;
using Model.Leads.Interfaces;

namespace Repository.Repositories
{
    public class LeadResultTemplateRepository:Repository<LeadResultTemplate>,ILeadResultTemplateRepository
    {
        public LeadResultTemplateRepository(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
