using Model.Employees;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Mapping
{
    public class QueueLocalPhoneStoreMap:ClassMapping<QueueLocalPhoneStore>
    {
        public QueueLocalPhoneStoreMap()
        {
            Table("Emp.QueueLocalPhoneStore");

            // Base Properties
            Id(x => x.ID, c => c.Column("QueueLocalPhoneStoreID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            ManyToOne(x => x.OwnerEmployee, c => c.Column("OwnerEmployeeID"));
            ManyToOne(x => x.Queue, c => c.Column("QueueID"));
            Property(x => x.SmsText);
            Property(x => x.SendSmsToOffLineUserOnDangerous);
            Property(x => x.SendSmsToOnLineUserOnDangerous);
            Property(x => x.DangerousRing);
            Property(x => x.DangerousSeconds);
            Property(x=>x.CanViewQueue);
        }
    }
}
