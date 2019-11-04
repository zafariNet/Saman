using System;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;
namespace Repository.Mapping
{
    class CityMap:ClassMapping<City>
    {
        /// <summary>
        /// این کلاس توسط محمد ظفری ایجاد شده است
        /// </summary>
        public CityMap()
        {
            Table("Cus.City");
            //Base Properties
            Id(x => x.ID, c => c.Column("CityID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, m => m.Column("EmployeeId"));
            ManyToOne(x => x.ModifiedEmployee, m => m.Column("ModifiedEmployeeId"));
            Property(x => x.RowVersion);

            //City Properties
            Property(x => x.CityName, m => m.Length(50));

            //Bags
            Bag(x => x.Customers,
                collectionMapping =>
                {
                    collectionMapping.Table("Cus.Province");
                    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                    collectionMapping.Key(k => k.Column("CityID"));
                    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
                },
                mapping => mapping.OneToMany(cr => cr.Class(typeof(Customer))));
        }
    }
}
