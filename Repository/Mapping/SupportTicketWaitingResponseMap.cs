using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Support;
using NHibernate.Hql.Ast.ANTLR;

using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class SupportTicketWaitingResponseMap : ClassMapping<SupportTicketWaitingResponse>
    {
        public SupportTicketWaitingResponseMap()
        {
            Table("Sup.SupportTicketWaitingResponse");

            // Base Properties
            Id(x => x.ID, c => c.Column("SupportTicketWaitingResponseID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            ManyToOne(x => x.Support, c => c.Column("SupportID"));
            //OneToOne(x => x.Support, x => x.PropertyReference(typeof(Support).GetProperty("SupportTicketWaitingResponse")));
            
            Property(x=>x.Comment);
            Property(x=>x.ResponsePossibilityDate);
            Property(x=>x.SendNotificationToCustomer);
            Property(x=>x.SendTicketDate);
            Property(x=>x.TicketNumber);
        }
    }
}
