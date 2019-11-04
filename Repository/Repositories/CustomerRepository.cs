#region Usings
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Domain;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Customers;
using Model.Customers.Interfaces;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
#endregion

namespace Repository.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        #region Declares
        private readonly IQueryRepository _queryRepository;
        #endregion

        #region Ctor

        public CustomerRepository(IUnitOfWork uow, IQueryRepository queryRepository)
            : base(uow)
        {
            _queryRepository = queryRepository;
        }

        #endregion

        #region Check Condition

        public bool CheckCondition(Condition condition, Level level, Customer customer)
        {
            IQuery query = SessionFactory.GetCurrentSession().CreateQuery(condition.QueryText);

            try
            {
                query.SetParameter("CustomerID", customer.ID);
            }
            catch { }

            try
            {
                query.SetParameter("LevelID", level.ID);
            }
            catch { }

            IList list = query.List();

            bool result = false;
            if (list.Count > 0) result = Convert.ToBoolean(list[0]);

            return result;
        }

        #endregion

        #region Find All (For Query)

        public Response<Customer> FindAll(Guid queryID, int index, int count)
        {
            return FindAll(queryID, index, count, null);
        }

        public Response<Customer> FindAll(Guid queryID, int index, int count, IEnumerable<Sort> sorts)
        {
            Model.Customers.Query cQuery = _queryRepository.FindBy(queryID);
            
            // Sorting
            string orderByClause = string.Empty;
            if (sorts != null)
            {
                orderByClause = " Order By ";
                bool firstFlag = true;
                foreach (var sort in sorts)
                {
                    orderByClause += String.Format("{0} {1} {2} ", firstFlag ? "" : ",", sort.SortColumn, sort.Asc ? "" : "Desc");
                    firstFlag = false;
                }
            }
            cQuery.QueryText = cQuery.QueryText.Replace("OrderByClause", orderByClause);
            cQuery.QueryText = cQuery.QueryText.Replace("WhereClause", string.Empty);
             
            IQuery query = SessionFactory.GetCurrentSession().CreateQuery(cQuery.QueryText);

            if (cQuery.PrmDefinition != null && cQuery.PrmValues != null)
            {
                string[] PrmDefinition = cQuery.PrmDefinition.Split(new char[] { ';' });
                string[] PrmValues = cQuery.PrmValues.Split(new char[] { ';' });

                int i = 0;
                foreach (string prmDef in PrmDefinition)
                {
                    query.SetParameter(prmDef, PrmValues[i]);
                    i++;
                }
            }

            ICriteria criteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(Customer));
            //criteriaQuery.SetFetchMode("Center", FetchMode.Lazy).
            //    SetFetchMode("Agency", FetchMode.Lazy).
            //    SetFetchMode("Network", FetchMode.Lazy).
            //    SetFetchMode("SuctionMode", FetchMode.Lazy).
            //    SetFetchMode("SuctionModeDetail", FetchMode.Lazy).
            //    SetFetchMode("DocumentStatus", FetchMode.Lazy).
            //    SetFetchMode("Employee", FetchMode.Lazy).
            //    SetFetchMode("BuyPossibility", FetchMode.Lazy).
            //    SetFetchMode("FollowStatus", FetchMode.Lazy);
            //int resultCount = criteriaQuery.SetProjection(Projections.Count("ID")).UniqueResult<int>();
            var count1 = SessionFactory.GetCurrentSession().QueryOver<Customer>().RowCount();
            //IList<Customer> list=new List<Customer>();
            //list = query.List<Customer>();
            //int total = list.Count();
            //IEnumerable<Customer> customerList = list.Skip(index).Take(count);
            IList<Customer> customerList = query.SetFirstResult(index).SetMaxResults(count).List<Customer>();


            //int resultCount = customerList.Count();
            int finalCount=0;
            if (cQuery.Counting)
                finalCount = cQuery.CustomerCount;
            if (!cQuery.Counting && cQuery.AllCustomer)
            {
                finalCount=SessionFactory.GetCurrentSession().QueryOver<Customer>().RowCount();
                
            }
            if (!cQuery.Counting && !cQuery.AllCustomer)
            {
                finalCount = query.List<Customer>().Count();

            }
            return new Response<Customer>(customerList, finalCount);
        }

        #endregion

        #region Find All By Sort And Filter

        public Response<Customer> FindAll(Guid queryID, int index, int count, IEnumerable<Sort> sorts, string filter)
        {
            Model.Customers.Query cQuery = _queryRepository.FindBy(queryID);

            if (filter != null)
            {
                string whereClause = string.Empty;

                if (cQuery.QueryText.Contains("=:"))
                {
                    whereClause = " and " + filter;
                    cQuery.QueryText = cQuery.QueryText.Replace("WhereClause", whereClause);
                }
                else
                {
                    whereClause = " where " + filter;
                    cQuery.QueryText = cQuery.QueryText.Replace("WhereClause", whereClause);
                }
            }
            string orderByClause = string.Empty;
            if (sorts != null)
            {
                orderByClause = " Order By ";
                bool firstFlag = true;
                foreach (var sort in sorts)
                {
                    orderByClause += String.Format("{0} {1} {2} ", firstFlag ? "" : ",", sort.SortColumn, sort.Asc ? "" : "Desc");
                    firstFlag = false;
                }
            }

            cQuery.QueryText = cQuery.QueryText.Replace("OrderByClause", orderByClause);


            IQuery query = SessionFactory.GetCurrentSession().CreateQuery(cQuery.QueryText);

            if (cQuery.PrmDefinition != null && cQuery.PrmValues != null)
            {
                string[] PrmDefinition = cQuery.PrmDefinition.Split(new char[] { ';' });
                string[] PrmValues = cQuery.PrmValues.Split(new char[] { ';' });

                int i = 0;
                foreach (string prmDef in PrmDefinition)
                {
                    query.SetParameter(prmDef, PrmValues[i]);
                    i++;
                }
            }

            //int resultCount = query.List().Count;
            ICriteria criteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(Customer));
    //        criteriaQuery.SetFetchMode("Center", FetchMode.Lazy).
    //SetFetchMode("Agency", FetchMode.Lazy).
    //SetFetchMode("Network", FetchMode.Lazy).
    //SetFetchMode("SuctionMode", FetchMode.Lazy).
    //SetFetchMode("SuctionModeDetail", FetchMode.Lazy).
    //SetFetchMode("DocumentStatus", FetchMode.Lazy).
    //SetFetchMode("Employee", FetchMode.Lazy).
    //SetFetchMode("BuyPossibility", FetchMode.Lazy).
    //SetFetchMode("FollowStatus", FetchMode.Lazy);
            int total = query.List<Customer>().Count();
            int resultCount = criteriaQuery.SetProjection(Projections.Count("ID")).UniqueResult<int>();
            IList<Customer> customerList = query.SetFirstResult(index).SetMaxResults(count).List<Customer>();

            return new Response<Customer>(customerList, total);
        }


        #endregion

        #region Find Customers By Code

        public IEnumerable<Customer> FindByPhoneCode(string phoneCode)
        {
            IQuery query = SessionFactory.GetCurrentSession().CreateQuery(String.Format("From Customer Where Substring(ADSLPhone, 1, {0}) = {1}", phoneCode.Length, phoneCode));
            IList<Customer> list = query.List<Customer>();

            return list;

        }


        #endregion



        public Customer SimplEmployee(string ADSLPhone)
        {
            var result = SessionFactory.GetCurrentSession().CreateSQLQuery("select t1.FirstName,t1.LastName,t2.LevelTitle from cus.Customer t1 inner join cus.Level t2 on t1.LevelID=t2.LevelID where t1.ADSLPhone='" + ADSLPhone + "'")
            .SetResultTransformer(Transformers.AliasToBean<Customer>())
            .List<Customer>().FirstOrDefault();
            return result;
        }
    }
}
