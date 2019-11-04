using Model.Customers;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class CustomerContactTemplateMap:ClassMapping<CustomerContactTemplate>
    {
        public CustomerContactTemplateMap()
        {
            Table("Cus.CustomerContactTemplate");

            // Base Properties
            Id(x => x.ID, c => c.Column("CustomerContactTemplateID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            Property(x=>x.Title,c=>c.Length(50));
            ManyToOne(x=>x.Group,c=>c.Column("GroupID"));
            Property(x=>x.Description,c=>c.Length(50));
        }
    }
}
