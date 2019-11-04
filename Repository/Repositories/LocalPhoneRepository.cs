using System;
using Model.Employees;
using Infrastructure.UnitOfWork;
using Model.Employees.Interfaces;

namespace Repository.Repositories
{
    public class LocalPhoneRepository : Repository<LocalPhone>, ILocalPhoneRepository
    {
        public LocalPhoneRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
