using Infrastructure.UnitOfWork;
using Model.Customers;
using Model.Customers.Validations.Interfaces;

namespace Repository.Repositories
{
    public class CallLogRepository:Repository<CallLog>,ICallLogRepository
    {
        private readonly IUnitOfWork _uow;

        public CallLogRepository(IUnitOfWork uow):base(uow)
        {
            _uow = uow;
        }
    }
}
