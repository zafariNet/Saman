using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class ProductLogMap : ClassMapping<ProductLog>
    {
        public ProductLogMap()
        {
            Table("Store.ProductLog");

            // Base Properties
            Id(x => x.ID, c => c.Column("ProductLogID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            ManyToOne(x => x.Store, c => c.Column("StoreID"));

            // ProductLog Properties
            ManyToOne(x => x.Product, c => c.Column("ProductID"));
            Property(x => x.LogDate, c => c.Length(10));
            Property(x => x.UnitsIO);
            Property(x => x.PurchaseUnitPrice);
            Property(x => x.TotalLine, y => y.Generated(NHibernate.Mapping.ByCode.PropertyGeneration.Always));
            Property(x => x.PurchaseDate, c => c.Length(10));
            Property(x => x.SellerName, c => c.Length(50));
            Property(x => x.PurchaseBillNumber, c => c.Length(20));
            Property(x => x.Closed);
            Property(x => x.InputSerialNumber, c => c.Length(15));
            Property(x => x.ProductSerialFrom, c => c.Length(50));
            Property(x => x.ProductSerialTo, c => c.Length(50));
            Property(x => x.Note);
        }
    }
}
