using Model;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Mapping
{
    public class SystemCountersMap : ClassMapping<SystemCounters>
    {
        public SystemCountersMap()
        {
            Table("dbo.SystemCounter");

            // Base Properties
            Id(x => x.ID, c => c.Column("SystemCounterID"));
            Property(x => x.ActiveArchiveCustomerCount);
            Property(x => x.AllCustomersCount);
            Property(x => x.ChargeAndOthersCustomerCount);
            Property(x => x.CompeletingDocumentCustomerCount);
            Property(x => x.HasProblemsCustomerCount);
            Property(x => x.InActiveArchiveCustomerCount);
            Property(x => x.LastCashSerialNumber);
            Property(x => x.LastFactorSerialNumber);
            Property(x => x.NotFinalizedCallsCustomerCount);
            Property(x => x.NotSupportedCustomerCount);
            Property(x => x.RegisterCustomerCount);
            Property(x => x.RemovedUsersCustomerCount);
            Property(x => x.RemoveingRanjeCustomerCount);
            Property(x => x.SendToRanjeCustomerCount);
            Property(x => x.SupportCustomerCount);
            Property(x => x.SupportUnitCustomerCount);
            Property(x => x.TemproryNotSupportedCustomerCount);
            Property(x => x.WaitForRanjeCustomerCount);
        }
    }
}
