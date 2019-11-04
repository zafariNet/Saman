using Model.Leads;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class LeadTitleTemplateMap:ClassMapping<LeadTitleTemplate>
    {
        public LeadTitleTemplateMap()
        {
            Table("Lead.LeadTitleTemplate");

            // Base Properties
            Id(x => x.ID, c => c.Column("LeadTitleTemplateID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            Property(x=>x.Title);
            Property(x=>x.CloseLeadConversation);
            Property(x=>x.Description);
            ManyToOne(x=>x.Group,c=>c.Column("GroupID"));
        }
    }
}
