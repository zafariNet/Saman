using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Support;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class SupportStatusRelationMap:ClassMapping<SupportStatusRelation>
    {
        public SupportStatusRelationMap()
        {
            Table("sup.SupportStatusRelation");
            Id(x => x.ID, c => c.Column("SupportStatusRelationID"));
            ManyToOne(x => x.RelatedSupportStatus, c => c.Column("SupportStatusRelatedID"));
            ManyToOne(x => x.SupportStatus, c => c.Column("SupportStatusID"));
        }
    }
}
