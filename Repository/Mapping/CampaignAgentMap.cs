using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class CampaignAgentMap : ClassMapping<CampaignAgent>
    {
        public CampaignAgentMap()
        {
            Table("Emp.CampaignAgent");

            // Base Properties
            Id(x => x.ID, c => c.Column("CampaignAgentID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            Property(x=>x.CampaignAgentName);
            Property(x=>x.TotalPayment);
        }
    }
}
