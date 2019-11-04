using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class ConditionMap : ClassMapping<Condition>
    {
        public ConditionMap()
        {
            Table("Cus.Condition");

            // Base Properties
            Id(x => x.ID, c => c.Column("ConditionID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            
            // Condition Properties
            Property(x => x.ConditionTitle, c => c.Length(250));
            Property(x => x.QueryText);
            Property(x => x.PropertyName, c => c.Length(100));
            Property(x => x.Value);
            Property(x => x.CriteriaOperator);
            Property(x => x.ErrorText);
            Property(x => x.nHibernate);
            
        }
    }
}
