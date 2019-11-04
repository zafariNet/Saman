using System;
using Infrastructure.Domain;
using System.Collections.Generic;
using Infrastructure.Querying;

namespace Model.Customers.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Response<Customer> FindAll(Guid queryID, int index, int count);
        Response<Customer> FindAll(Guid queryID, int index, int count, IEnumerable<Sort> sorts);
        Response<Customer> FindAll(Guid queryID, int index, int count, IEnumerable<Sort> sorts, string filter);
        bool CheckCondition(Condition condition, Level level, Customer customer);

        IEnumerable<Customer> FindByPhoneCode(string phoneCode);
    }
}
