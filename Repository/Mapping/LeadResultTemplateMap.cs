using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Leads;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class LeadResultTemplateMap:ClassMapping<LeadResultTemplate>
    {
        public LeadResultTemplateMap()
        {
            Table("Lead.LeadResultTemplate");
            // Base Properties
            Id(x => x.ID, c => c.Column("LeadResultTemplateID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x=>x.RowVersion);

            Property(x=>x.LeadResulTitle);
            Property(x=>x.Description);
            ManyToOne(x=>x.Group,c=>c.Column("GroupID"));
        }
    }
}
