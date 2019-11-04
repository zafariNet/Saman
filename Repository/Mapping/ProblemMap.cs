#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Support;
using NHibernate.Mapping.ByCode.Conformist;
#endregion

namespace Repository.Mapping
{
    public class ProblemMap : ClassMapping<Problem>
    {
        public ProblemMap()
        {
            Table("Sup.Problem");

            #region Base Properties

            Id(x => x.ID, c => c.Column("ProblemID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            #endregion

            #region Problem Properties

            ManyToOne(x => x.Customer, c => c.Column("CustomerID"));
            Property(x => x.ProblemTitle, c => c.Length(100));
            Property(x => x.ProblemDescription);
            Property(x => x.Priority);
            Property(x => x.State);

            #endregion
        }
    }
}
