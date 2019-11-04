using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Infrastructure.UnitOfWork;
using Model.Customers.Interfaces;

namespace Repository.Repositories
{
    public class DocumentStatusRepository : Repository<DocumentStatus>, IDocumentStatusRepository
    {
        public DocumentStatusRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
