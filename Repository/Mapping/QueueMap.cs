using Model.Employees;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Mapping
{
    public class QueueMap:ClassMapping<Queue>
    {
        public QueueMap()
        {
            Table("Emp.Queue");
            Id(x => x.ID, c => c.Column("QueueID"));

            Property(x => x.AsteriskID);
            Property(x => x.QueueName);
            Property(x=>x.PersianName);
        }
    }
}
