using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class LevelTypeMap : ClassMapping<LevelType>
    {
        public LevelTypeMap()
        {
            Table("Cus.LevelType");

            // Base Properties
            Id(x => x.ID, c => c.Column("LevelTypeID"));
            Property(x => x.CreateDate, m => m.Length(19));
            
            // LevelType Properties
            Property(x => x.Title, c => c.Length(30));

            //Bags
            //Bag(x => x.Levels,
            //collectionMapping =>
            //{

            //    collectionMapping.Table("Cus.Level");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("LevelTypeID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Level))));

        }
    }
}
