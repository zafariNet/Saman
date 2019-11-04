using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class ProductPriceMap : ClassMapping<ProductPrice>
    {
        public ProductPriceMap()
        {
            Table("Store.ProductPrice");

            // Base Properties
            Id(x => x.ID, c => c.Column("ProductPriceID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            Property(x => x.ProductPriceCode, c => c.Column("ProductPriceCode"));

            // ProductPrice Properties
            ManyToOne(x => x.Product, c => c.Column("ProductID"));
            Property(x => x.ProductPriceTitle, c => c.Length(50));
            Property(x => x.UnitPrice);
            Property(x => x.MaxDiscount);
            Property(x => x.Imposition);
            Property(x => x.Discontinued);
            Property(x => x.Note);
            Property(x => x.Bonus);
            Property(x=>x.Comission);
            Property(x => x.SortOrder);
        }
    }
}
