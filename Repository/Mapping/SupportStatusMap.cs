using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Support;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class SupportStatusMap : ClassMapping<SupportStatus>
    {
        public SupportStatusMap()
        {
            Table("sup.SupportStatus");

            // Base Properties
            Id(x => x.ID, c => c.Column("SupportStatusID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            Property(x=>x.Key);

            Property(x => x.SendEmailOnEnter);
            Property(x => x.EmailText);
            Property(x => x.SmsText);
            Property(x => x.SendSmsOnEnter);
            Property(x => x.SupportStatusName);
            Property(x=>x.IsFirstSupportStatus);
            Property(x=>x.IsLastSupportState);

            Bag(x => x.SuportStatusRelations,
                collectionMapping =>
                {
                    collectionMapping.Table("sup.SupportStatusRelation");
                    //collectionMapping.Access(typeof(long));
                    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                    collectionMapping.Key(k => k.Column("SupportStatusID"));
                    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
                },
                mapping => mapping.OneToMany(cr => cr.Class(typeof (SupportStatusRelation))));
        }
    }
}
