using Model.Employees;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Mapping
{
    public class LocalPhoneStoreMap:ClassMapping<LocalPhoneStore>
    {
        public LocalPhoneStoreMap()
        {
            Table("Emp.LocalPhoneStore");

            // Base Properties
            Id(x => x.ID, c => c.Column("LocalPhoneStoreID"));
            Property(x => x.AsteriskID);
            Property(x => x.LocalPhoneStoreNumber);
            Property(x => x.Reserved);
        }
    }
}
