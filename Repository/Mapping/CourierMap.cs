using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Sales;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class CourierMap:ClassMapping<Courier>
    {
        public CourierMap()
        {
            Table("sales.Courier");

            // Base Properties
            Id(x => x.ID, c => c.Column("CourierID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            
            ManyToOne(x=>x.Sale ,c=>c.Column("saleID"));
            Property(x=>x.Amount);
            Property(x=>x.BuildingUnits);
            Property(x=>x.CourierCost);
            Property(x=>x.CourierType);
            Property(x=>x.DeliverDate);
            Property(x=>x.DeliverTime);
            Property(x=>x.ExpertComment);
            Property(x=>x.SaleComment);
            Property(x=>x.CourierStatuse);
            Property(x=>x.Bonus);

            ManyToOne(x=>x.CourierEmployee,c=>c.Column("CourierEmployeeID"));

        }
    }
}
