using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.UnitOfWork;
using Model.Support;
using Model.Support.Interfaces;
using NHibernate.Hql.Ast.ANTLR.Tree;

namespace Repository.Repositories
{
    public class SupportTicketWaitingResponseRepository : Repository<SupportTicketWaitingResponse>,ISupportTicketWaitingResponseRepository   
    {
        public SupportTicketWaitingResponseRepository(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
