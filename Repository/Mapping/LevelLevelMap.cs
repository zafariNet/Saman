using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class LevelLevelMap : ClassMapping<LevelLevel>
    {
        public LevelLevelMap()
        {
            Table("Cus.LevelLevel");

            // Base Properties
            ComposedId(cid =>
            {
                cid.ManyToOne((x) => x.Level, (c) => c.Column("LevelID"));
                cid.ManyToOne((x) => x.RelatedLevel, (c) => c.Column("RelatedLevelID"));
            });
        }
    }
}
