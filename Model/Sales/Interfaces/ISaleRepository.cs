using System;
using Infrastructure.Domain;
using System.Collections.Generic;

namespace Model.Sales.Interfaces
{
    public interface ISaleRepository : IRepository<Sale>
    {
        IEnumerable<Sale> FindAll(Guid CustomerID);
    }
}