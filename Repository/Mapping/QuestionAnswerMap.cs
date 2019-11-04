
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    class QuestionAnswerMap:ClassMapping<QuestionAnswer>
    {
        public QuestionAnswerMap()
        {
            Table("Cus.QuestionAnswer");
            // Base Properties
            Id(x => x.ID, c => c.Column("QuestionAnswerID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            ManyToOne(x=>x.Answer,c=>c.Column("AnswerID"));
            ManyToOne(x=>x.Question,c=>c.Column("QuestionID"));
            ManyToOne(x=>x.Customer,c=>c.Column("CustomerID"));
        }
    }
}
