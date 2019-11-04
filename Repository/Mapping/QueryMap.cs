using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class QueryMap : ClassMapping<Query>
    {
        public QueryMap()
        {
            Table("Cus.Query");

            // Base Properties
            Id(x => x.ID, c => c.Column("QueryID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // Query Properties
            Property(x => x.Title, c => c.Length(200));
            Property(x => x.QueryText);
            Property(x => x.xType);
            Property(x => x.PrmDefinition);
            Property(x => x.PrmValues);
            Property(x => x.Columns);
            Property(x => x.CustomerCount);
            Property(x => x.PreLoad);
            ManyToOne(x=>x.Level,c=>c.Column("LevelID"));
            Property(x=>x.Counting);
            Property(x=>x.AllCustomer);

            //Bags
            Bag(x => x.QueryEmployees,
            collectionMapping =>
            {

                collectionMapping.Table("Cus.QueryEmployee");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                collectionMapping.Key(k => k.Column("QueryID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(QueryEmployee))));
        }
    }
}
