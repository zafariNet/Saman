using Infrastructure.UnitOfWork;
using Model.Customers;
using Model.Customers.Validations.Interfaces;

namespace Repository.Repositories
{
    public class CustomerContactTemplateRepository:Repository<CustomerContactTemplate>,ICustomerContactTemplateRepository
    {
        private readonly IUnitOfWork _uow;

        public CustomerContactTemplateRepository(IUnitOfWork uow):base(uow)
        {
            _uow = uow;
        }
    }
}
