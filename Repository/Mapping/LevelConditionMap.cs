using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class LevelConditionMap : ClassMapping<LevelCondition>
    {
        public LevelConditionMap()
        {
            Table("Cus.LevelCondition");

            ComposedId(cid =>
            {
                cid.ManyToOne((x) => x.Level, (c) => c.Column("LevelID"));
                cid.ManyToOne((x) => x.Condition, (c) => c.Column("ConditionID"));
            });
            
        }
    }
}
