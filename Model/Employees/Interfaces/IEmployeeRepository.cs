using System;
using Infrastructure.Domain;

namespace Model.Employees.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Employee VerifyLogin(string LoginName);
    }
}
