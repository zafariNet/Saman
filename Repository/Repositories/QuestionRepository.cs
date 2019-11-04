using Infrastructure.UnitOfWork;
using Model.Customers;
using Model.Customers.Validations.Interfaces;

namespace Repository.Repositories
{
    public class QuestionRepository:Repository<Question>,IQuestionRepository
    {
        private readonly IUnitOfWork _uow;
        public QuestionRepository(IUnitOfWork uow)
            : base(uow)
        {
            
        }
    }
}
