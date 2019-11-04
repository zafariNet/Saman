using Infrastructure.UnitOfWork;
using Model.Customers;
using Model.Customers.Validations.Interfaces;

namespace Repository.Repositories
{
    public class QuestionAnswerRepository : Repository<QuestionAnswer>, IQuestionAnswerRepository
    {
        private readonly IUnitOfWork _uow;

        public QuestionAnswerRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
        }
    }
}