using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class BonusComissionMap:ClassMapping<BonusComission>
    {
        public BonusComissionMap()
        {
            Table("Emp.BonusComission");

            // Base Properties
            Id(x => x.ID, c => c.Column("BonusComissionID"));
            Property(x => x.CreateDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            Property(x => x.RowVersion);

            ManyToOne(x => x.Courier, c => c.Column("CourierID"));
            ManyToOne(x => x.CreditSaleDetail, c => c.Column("CreditSaleDetailID"));
            ManyToOne(x => x.UnCreditSaleDetail, c => c.Column("UncreditSaleDetailID"));
            ManyToOne(x => x.ProductSaleDetail, c => c.Column("ProductSaleDetailID"));
            ManyToOne(x=>x.Customer,c=>c.Column("CustomerID"));
            Property(x=>x.Comission);
            Property(x=>x.Bonus);
            Property(x=>x.IsRollback);
            Property(x=>x.UnDeliveredComission);
            Property(x=>x.SaleDeliverDate);
            Property(x=>x.CourierDeliverDate);
        }
    }
}
