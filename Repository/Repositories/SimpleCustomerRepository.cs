using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Infrastructure.UnitOfWork;
using Model.Customers;
using Model.Customers.Validations.Interfaces;
using Model.Employees;
using NHibernate;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Util;

namespace Repository.Repositories
{
    public class SimpleCustomerRepository:Repository<SimpleCustomer>,ISimpleCustomerRepository
    {
        public SimpleCustomerRepository(IUnitOfWork uow):base(uow)
        {
            
        }

        public IList<SimpleCustomer> FindSimpleCustomer(string ADSLPhone)
        {
            string queryText =string.Format(
                    "select _cus.CustomerID as CustomerID,_cus.FirstName as FirstName,_cus.LastName as LastName,_lev.LevelTitle as LevelTitle from cus.Customer _cus inner join cus.Level _lev on _cus.LevelID=_lev.LevelID where _cus.ADSLPhone='{0}'",
                    ADSLPhone);
            IList<SimpleCustomer> list=new List<SimpleCustomer>();
            IQuery query = SessionFactory.GetCurrentSession().CreateSQLQuery(queryText);
            //IQuery query = SessionFactory.GetCurrentSession().CreateSQLQuery(queryText);//.List<SimpleCustomer>();
            //list = query.List<SimpleCustomer>();
            list = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(SimpleCustomer))).List<SimpleCustomer>();
            

            return list;
        }
    }
}
