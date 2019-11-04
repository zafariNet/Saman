using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Leads;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class NegotiationMap:ClassMapping<Negotiation>
    {
        public NegotiationMap()
        {
            Table("Lead.Negotiation");

            // Base Properties
            Id(x => x.ID, c => c.Column("NegotiationID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            ManyToOne(x => x.ReferedEmployee, c => c.Column("ReferedEmployeeID"));
            ManyToOne(x=>x.Customer,c=>c.Column("CustomerID"));


            ManyToOne(x => x.LeadTitleTemplate, c => c.Column("LeadTitleTemplateID"));
            ManyToOne(x => x.LeadResultTemplate, c => c.Column("LeadResultTemplateID"));

            Property(x => x.NegotiationDesciption);
            Property(x => x.NegotiationDate);
            Property(x => x.NegotiationTime);
            Property(x => x.RememberTime);
            Property(x => x.SendSms);
            Property(x => x.NeqotiationResultDescription);
            Property(x => x.CloseDate);
            Property(x=>x.Closed);
            Property(x=>x.NegotiationStatus);
        }
    }
}
