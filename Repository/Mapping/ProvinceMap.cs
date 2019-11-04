using System;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;
namespace Repository.Mapping
{
    public class ProvinceMap:ClassMapping<Province>
    {
        /// <summary>
        /// این کلاس توسط محمد ظفری ایجاد شده است
        /// </summary>
        public ProvinceMap()
        {
            Table("Cus.Province");
            //Base Properties
            Id(x => x.ID, c => c.Column("ProvinceID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, m => m.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, m => m.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // Province Properties
            Property(x => x.ProvinceName, m => m.Length(50));
            
            //Bags
            Bag(x => x.Cities,
                collectionMapping =>
                {
                    collectionMapping.Table("Cus.City");

                    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                    //collectionMapping.Key(k => k.Columns("ProvinceID"));
                    collectionMapping.Key(k => k.Column("ProvinceID"));
                    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
                },
                mapping => mapping.OneToMany(cr => cr.Class(typeof(City))));
        }
    }
}
