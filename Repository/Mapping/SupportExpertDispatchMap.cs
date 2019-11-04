using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Support;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class SupportExpertDispatchMap : ClassMapping<SupportExpertDispatch>
    {
        public SupportExpertDispatchMap()
        {
            Table("Sup.SupportExpertDispatch");

            // Base Properties
            Id(x => x.ID, c => c.Column("SupportExpertDispatchID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            ManyToOne(x => x.Support, c => c.Column("SupportID"));

            //OneToOne(x => x.Support, x => x.PropertyReference(typeof(Support).GetProperty("SupporExpertDispatch")));
            Property(x=>x.SendNotificationToCustomer);
            Property(x=>x.Comment);
            Property(x=>x.DispatchDate);
            Property(x=>x.DispatchTime);
            ManyToOne(x => x.ExpertEmployee, c => c.Column("ExpertEmployeeID"));
            Property(x=>x.CoordinatorName);
            Property(x=>x.IsNewInstallation);

        }
    }
}
