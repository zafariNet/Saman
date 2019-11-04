using System;
using System.Linq;
using System.Collections.Generic;
using Model.Employees;
using Infrastructure.UnitOfWork;
using NHibernate;
using Model.Employees.Interfaces;
using Infrastructure.Querying;
using NHibernate.Transform;

namespace Repository.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public Employee VerifyLogin(string LoginName)
        {
            Query query = new Query();
            Criterion nameCriteria = new Criterion("LoginName", LoginName, CriteriaOperator.Equal);
            //Criterion passwordCriteria = new Criterion("Password", Password, CriteriaOperator.Equal);
            //Criterion discontinuedCriteria = new Criterion("Discontinued", true, CriteriaOperator.Equal);

            query.Add(nameCriteria);
            //query.Add(passwordCriteria);
            //query.Add(discontinuedCriteria);

            IEnumerable<Employee> employees = FindBy(query);

            try
            {
                Employee employee = employees.First();
                return employee;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
