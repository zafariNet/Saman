using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Fiscals;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class MoneyAccountEmployeeMap : ClassMapping<MoneyAccountEmployee>
    {
        public MoneyAccountEmployeeMap()
        {
            Table("Fiscal.MoneyAccountEmployee");

            // Base Properties
            ComposedId(cid =>
            {
                cid.ManyToOne((x) => x.Employee, (c) => c.Column("EmployeeID"));
                cid.ManyToOne((x) => x.MoneyAccount, (c) => c.Column("MoneyAccountID"));
            });
        }
    }
}
