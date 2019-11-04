using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Model.Support;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class SupportQcMap : ClassMapping<SupportQc>
    {
        public SupportQcMap()
        {
            Table("Sup.SupportQc");

            // Base Properties
            Id(x => x.ID, c => c.Column("SupportQcID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            ManyToOne(x => x.Support, c => c.Column("SupportID"));
            //OneToOne(x => x.Support, x => x.PropertyReference(typeof(Support).GetProperty("SupportQc")));
            Property(x=>x.Comment);
            Property(x=>x.ExpertBehavior);
            Property(x=>x.ExpertCover);
            Property(x=>x.InputTime);
            Property(x=>x.OutputTime);
            Property(x=>x.RecivedCost);
            Property(x=>x.SaleAndService);
            Property(x=>x.SendNotificationToCustomer);
            Property(x=>x.SendNotificationToMaster);
            
        }
    }
}
