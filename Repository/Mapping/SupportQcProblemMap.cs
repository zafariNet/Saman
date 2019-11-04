using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Support;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class SupportQcProblemMap:ClassMapping<SupportQcProblem>
    {
        public SupportQcProblemMap()
        {
            Table("Sup.SupportQcProblem");

            // Base Properties
            Id(x => x.ID, c => c.Column("SupportQcProblemID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            //OneToOne(x => x.Support, x => x.PropertyReference(typeof(Support).GetProperty("SupportQc")));
            Property(x => x.Comment);
            Property(x => x.ExpertBehavior);
            Property(x => x.ExpertCover);
            Property(x => x.InputTime);
            Property(x => x.OutputTime);
            Property(x => x.RecivedCost);
            Property(x => x.SaleAndService);
            Property(x => x.SendNotificationToCustomer);
            Property(x => x.FiscallConfillict);
            Property(x=>x.Problem);
            ManyToOne(x => x.InstallerEmployee, c => c.Column("InstallerEmployeeID"));
        }
    }
}
