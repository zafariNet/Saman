using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class LevelMap : ClassMapping<Level>
    {
        public LevelMap()
        {
            Table("Cus.Level");

            // Base Properties
            Id(x => x.ID, c => c.Column("LevelID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // Level Properties
            ManyToOne(x => x.LevelType, c => c.Column("LevelTypeID"));
            Property(x => x.LevelTitle, c => c.Length(200));
            Property(x => x.LevelNikname, c => c.Length(100));
            Property(x => x.OnEnterSendSMS);
            Property(x => x.OnEnterSendEmail);
            Property(x => x.OnEnter);
            Property(x => x.OnExit);
            Property(x => x.EmailText);
            Property(x => x.SMSText);
            Property(x => x.IsFirstLevel);
            Property(x => x.Options);
            Property(x => x.Discontinued);
            ManyToOne(x => x.LevelStaff, c => c.Column("LevelStaffId"));
            Property(x => x.GraphicalObjectProperties);
            Property(x=>x.CreateSupportOnEnter);
            Property(x => x.HasRequireNetwork);

            //Bags
            Bag(x => x.LevelConditions,
            collectionMapping =>
            {
                collectionMapping.Table("Cus.LevelCondition");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                collectionMapping.Key(k => k.Column("LevelID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(LevelCondition))));

            Bag(x => x.RelatedLevels,
            collectionMapping =>
            {
                collectionMapping.Table("Cus.LevelLevel");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                collectionMapping.Key(k => k.Column("LevelID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(LevelLevel))));

            //Bag(x => x.CustomerLevels,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Cus.CustomerLevel");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("LevelID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(CustomerLevel))));

            //Bag(x => x.Notes,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Cus.Note");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("LevelID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Note))));

            //Bag(x => x.LevelLevels,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Cus.LevelLevel");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("LevelID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(LevelLevel))));

        }
    }
}