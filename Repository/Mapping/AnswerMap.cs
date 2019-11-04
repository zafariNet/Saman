using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class AnswerMap:ClassMapping<Answer>
    {
        public AnswerMap()
        {
            Table("Cus.Answer");

            // Base Properties
            Id(x => x.ID, c => c.Column("AnswerId"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));

            Property(x => x.AnswerText, c => c.Length(50));
            ManyToOne(x=>x.Question,c=>c.Column("QuestionID"));
        }
    }
}
