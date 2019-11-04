using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;

namespace Model.Customers.Interfaces
{
    public interface IQueryRepository : IRepository<Query>
    {
    }
}
