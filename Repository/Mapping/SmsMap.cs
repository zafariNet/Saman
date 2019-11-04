using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class SmsMap : ClassMapping<Sms>
    {
        public SmsMap()
        {
            Table("Cus.Sms");

            // Base Properties
            Id(x => x.ID, c => c.Column("SmsID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // Sms Properties
            ManyToOne(x => x.Customer, c => c.Column("CustomerID"));
            Property(x => x.Body);
            Property(x => x.Sent);
            Property(x => x.Note);
        }
    }
}
