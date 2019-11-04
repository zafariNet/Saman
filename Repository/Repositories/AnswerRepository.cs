using Infrastructure.UnitOfWork;
using Model.Customers;
using Model.Customers.Validations.Interfaces;

namespace Repository.Repositories
{
    public class AnswerRepository:Repository<Answer>,IAnswerRepository
    {
        private readonly IUnitOfWork _uow;

        public AnswerRepository(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
