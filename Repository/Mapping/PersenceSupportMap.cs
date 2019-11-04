#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Support;
using NHibernate.Mapping.ByCode.Conformist;
#endregion

namespace Repository.Mapping
{
    public class PersenceSupportMap : ClassMapping<PersenceSupport>
    {
        public PersenceSupportMap()
        {
            Table("Sup.PersenceSupport");

            #region Base Properties

            Id(x => x.ID, c => c.Column("PersenceSupportID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            #endregion

            #region PersenceSupport Properties

            ManyToOne(x => x.Customer, c => c.Column("CustomerID"));
            ManyToOne(x => x.Installer, c => c.Column("InstallerID"));
            Property(x => x.SupportType);
            Property(x => x.Problem);
            Property(x => x.PlanDate);
            Property(x => x.PlanTimeFrom);
            Property(x => x.PlanTimeTo);
            Property(x => x.PlanNote, c => c.Length(10));
            Property(x => x.Delivered);
            Property(x => x.ReceivedCost);
            //Property(x => x.ReceivedCostForExtraServices);
            Property(x => x.ConnectedToInternet);
            Property(x => x.DeliverDate, c => c.Length(10));
            Property(x => x.DeliverTime);
            Property(x => x.DeliverNote);

            #endregion
            
        }
    }
}
