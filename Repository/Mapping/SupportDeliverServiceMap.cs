using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Support;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class SupportDeliverServiceMap : ClassMapping<SupportDeliverService>
    {
        public SupportDeliverServiceMap()
        {
            Table("Sup.SupportDeliverService");

            // Base Properties
            Id(x => x.ID, c => c.Column("SupportDeliverServiceID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            ManyToOne(x => x.Support, c => c.Column("SupportID"));
            //OneToOne(x => x.Support, x => x.PropertyReference(typeof(Support).GetProperty("SupportDeliverService")));
            Property(x=>x.AmountRecived);
            Property(x=>x.Comment);
            Property(x=>x.DeliverDate);
            Property(x=>x.SendNotificationToCustomer);
            Property(x=>x.TimeInput);
            Property(x=>x.TimeOutput);

        }
    }
}
