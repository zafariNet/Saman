using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class NotificationMap:ClassMapping<Notification>
    {
        public NotificationMap()
        {
            Table("Emp.Notification");

            // Base Propetries
            Id(x => x.ID, c => c.Column("NotificationID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            //-----------

            Property(x=>x.NotificationTitle);
            Property(x=>x.NotificationComment);
            Property(x=>x.NotificationType);
            Property(x => x.Visited);
            Property(x => x.VisitedDate);
            ManyToOne(x => x.ReferedEmployee, c => c.Column("ReferedEmployeeID"));
        }
    }
}
