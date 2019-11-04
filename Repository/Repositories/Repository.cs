#region Usings

using System;
using System.Collections.Generic;
using Infrastructure.Domain;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Employees.Validations;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using System.Linq;
#endregion

namespace Repository.Repositories
{
    public abstract class Repository<T> where T : IAggregateRoot
    {
        #region Declares

        private IUnitOfWork _uow;

        
        #endregion

        #region ctor

        public Repository(IUnitOfWork uow)
        {
            _uow = uow;
            
        }

        #endregion

        #region Add

        public void Add(T entity)
        {
            SessionFactory.GetCurrentSession().Save(entity);
            IList<T> t = new List<T>();
        }

        #endregion

        #region Remove

        public void RemoveById(Guid id)
        {
            SessionFactory.GetCurrentSession().Delete<T>(id);
        }

        public void Remove(T entity)
        {
            SessionFactory.GetCurrentSession().Delete(entity);
        }

        public void Remove(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                Remove(entity);
            }
        }

        #endregion

        #region Save

        public void Save(T entity)
        {
            SessionFactory.GetCurrentSession().SaveOrUpdate(entity);
        }

        #endregion

        #region Find By

        public T FindBy(Guid id)
        {
            return SessionFactory.GetCurrentSession().Get<T>(id);
        }

        public IEnumerable<T> FindBy(Query query)
        {
            ICriteria criteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));

            AppendCriteria(criteriaQuery);

            query.TranslateIntoNHQuery<T>(criteriaQuery);

            return criteriaQuery.List<T>();
        }

        public Response<T> FindBy(Query query, int index, int count)
        {
            return FindBy(query, index, count, sorts:null);
        }

        public Response<T> FindByQuery(Query query, IList<Sort> sort)
        {
            return FindBy(query, -1, -1, sort);
        }

        public Response<T> FindByQuery(Query query)
        {
            return FindBy(query, -1, -1, sorts:null);
        } 



        public Response<T> FindBy(Query query, int index, int count, IList<Sort> sorts)
        {
            ICriteria criteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));

            #region Sorting

            if (sorts != null)
            {
                foreach (var sort in sorts)
                {
                    Order order = new Order(sort.SortColumn, sort.Asc);
                    criteriaQuery.AddOrder(order);
                }
            }
            else
            {
                //Order order = new Order("CreateDate", false);
                //criteriaQuery.AddOrder(order);
            }

            #endregion

            AppendCriteria(criteriaQuery);

            query.TranslateIntoNHQuery<T>(criteriaQuery);

            #region Paging

            IEnumerable<T> result;
            // if index and count = -1 then no index requested
            if (count == -1)
            {
                result = (List<T>)criteriaQuery.List<T>();
            }
            else
            {
                result = (List<T>)criteriaQuery.SetFirstResult(index).SetMaxResults(count).List<T>();
            }

            #endregion

            #region Calculate TotalCount

            criteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));
            query.TranslateIntoNHQuery<T>(criteriaQuery);
            int resultCount = criteriaQuery.SetProjection(Projections.Count("ID")).UniqueResult<int>();

            #endregion

            return new Response<T>(result, resultCount);
        }

        public Response<T> FindBy(string hqlQuery)
        {
            IQuery query = SessionFactory.GetCurrentSession().CreateQuery(hqlQuery);

            IList<T> result;
            try
            {
                result = (List<T>)query.List<T>();
            }
            catch (Exception ex)
            {
                throw;
            }
            return new Response<T>(result, result.Count);
        }

        #endregion

        #region Find All

        public Response<T> FindAll(int index, int count)
        {
            IList<Sort> sortOrders = new List<Sort>();
            sortOrders.Add(new Sort("CreateDate", false));
            return FindAllWithSort(index, count, sortOrders);
        }

        public Response<T> FindAllWithSort(int index, int count, IList<Sort> Sorts)
        {
            ICriteria criteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));

            // اگر سورت درخواست شده باشد
            if (Sorts != null)
                foreach (var sort in Sorts)
                {
                    NHibernate.Criterion.Order order = new NHibernate.Criterion.Order(sort.SortColumn, sort.Asc);
                    criteriaQuery.AddOrder(order);

                }
            int resultCount = 0;
            IList<T> result = (List<T>)criteriaQuery.List<T>();
            try
            {
                //resultCount = criteriaQuery.SetProjection(Projections.Count("ID")).UniqueResult<int>();
                resultCount = result.Count;
            }
            // در صورتی که آیدی نداشته باشیم شمارش از طریق دیگر انجام می شود
            catch (Exception ex)
            {
                //criteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));
                //IList<T> res = (List<T>)criteriaQuery.List<T>();
                //resultCount = result.Count;
            }
            // اگر ایندکس مقدار 1- داشت یعنی همه دیتا لود شود
            if (count != -1)
            {
                result = result.Skip(index).Take(count).ToList();
            }
            else
            {
                //result = (List<T>)criteriaQuery.List<T>();
            }



            return new Response<T>(result, resultCount);
        }

        public Response<T> FindAll(string hqlQuery)
        {
            IQuery qry = SessionFactory.GetCurrentSession().CreateQuery(hqlQuery);

            //int resultCount = qry.List().Count;
            //IList<T> list = qry.List<T>();

            //return new Response<T>(list, resultCount);

            IList<T> result = qry.List<T>();
            

            return new Response<T>(result, result.Count);
        }

        public Response<T> FindAll(string hqlQuery, int index, int count)
        {
            IQuery qry = SessionFactory.GetCurrentSession().CreateQuery(hqlQuery);

            //int resultCount = qry.List().Count;
            //IList<T> list = qry.SetFirstResult(index).SetMaxResults(count).List<T>();
            //return new Response<T>(list, resultCount);

            
            IList<T> result = qry.List<T>();
            IList<T> list = result.Skip(index).Take(count).ToList();

            return new Response<T>(list, result.Count);
        }

        public IEnumerable<T> FindAll()
        {
            ICriteria criteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));
            
            return (List<T>)criteriaQuery.List<T>();
        }
        public IQueryable<T> Queryable()
        {

            IQueryable<T> query = SessionFactory.GetCurrentSession().Query<T>();

            return query;
        }

        #endregion

    

        public virtual void AppendCriteria(ICriteria criteria)
        { }
    }
}
