using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class CallLogMap:ClassMapping<CallLog>
    {
        public CallLogMap()
        {
            Table("Cus.CallLog");

            // Base Properties
            Id(x => x.ID, c => c.Column("CallLogID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            Property(x=>x.LocalPhone);
            Property(x=>x.CallType);
            Property(x=>x.Description);
            Property(x=>x.PhoneNumber);
            ManyToOne(x=>x.Customer,c=>c.Column("CustomerID"));
            ManyToOne(x=>x.CustomerContactTemplate,c=>c.Column("CustomerContactTemplateID"));
        }
    }
}
