using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class QuestionMap:ClassMapping<Question>
    {
        public QuestionMap()
        {
            Table("Cus.Question");

            // Base Properties
            Id(x => x.ID, c => c.Column("QuestionId"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));

            Property(x=>x.QuestionText,c=>c.Length(50));
        }
    }
}
