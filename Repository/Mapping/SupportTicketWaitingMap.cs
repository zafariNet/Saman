using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Support;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class SupportTicketWaitingMap:ClassMapping<SupportTicketWaiting>
    {
        public SupportTicketWaitingMap()
        {
            Table("Sup.SupportTicketWaiting");

            // Base Properties
            Id(x => x.ID, c => c.Column("SupportTicketWaitingID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            ManyToOne(x => x.Support, c => c.Column("SupportID"));
            ManyToOne(x => x.InstallExpert, c => c.Column("InstallExpertID"));
            //OneToOne(x => x.Support, x => x.PropertyReference(typeof(Support).GetProperty("SupportTicketWaiting")));
            Property(x=>x.Comment);
            Property(x=>x.DateOfPersenceDate);
            Property(x=>x.Selt);
            Property(x=>x.SendNotificationToCustomer);
            Property(x=>x.Snr);
            Property(x=>x.SourceWireCheck);
            Property(x=>x.TicketSubject);
            Property(x=>x.WireColor);
        }
    }
}
