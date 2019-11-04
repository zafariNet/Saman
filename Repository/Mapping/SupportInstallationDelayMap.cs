using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Support;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class SupportInstallationDelayMap : ClassMapping<SupportInstallationDelay>
    {
        public SupportInstallationDelayMap()
        {
            Table("Sup.SupportInstallationDelay");

            // Base Properties
            Id(x => x.ID, c => c.Column("SupportInstallationDelayID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            ManyToOne(x => x.Support, c => c.Column("SupportID"));
            Property(x => x.RowVersion);

            //OneToOne(x => x.Support, x => x.PropertyReference(typeof(Support).GetProperty("SupportInstallationDelay")));
            Property(x=>x.Comment);
            Property(x=>x.InstallDate);
            Property(x=>x.NextCallDate);
            Property(x=>x.SendNotificationToCustomer);
        }
    }
}
