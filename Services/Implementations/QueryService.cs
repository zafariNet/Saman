#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Customers.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Model.Customers;
using Services.ViewModels.Customers;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees.Interfaces;
using Model.Employees;
using Infrastructure.Domain;
#endregion

namespace Services.Implementations
{
    public class QueryService : IQueryService
    {
        #region Declares
        private readonly IQueryRepository _queryRepository;
        private readonly IQueryEmployeeRepository _queryEmployeeRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Ctor
        public QueryService(IQueryRepository queryRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository, IQueryEmployeeRepository queryEmployeeRepository)
        {
            _queryRepository = queryRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
            _queryEmployeeRepository = queryEmployeeRepository;
        }
        #endregion

        #region Old Methods

        #region Add
        public GeneralResponse AddQuery(AddQueryRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Query query = new Query();
                query.ID = Guid.NewGuid();
                query.CreateDate = PersianDateTime.Now;
                query.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                query.xType = request.xType;
                query.PrmDefinition = request.PrmDefinition;
                query.PrmValues = request.PrmValues;
                query.QueryText = request.QueryText;
                query.Title = request.Title;
                query.RowVersion = 1;

                #region Validation
                if (query.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in query.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }
                #endregion

                _queryRepository.Add(query);
                _uow.Commit();

                ////response.success = true;
            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        #endregion

        #region Edit
        public GeneralResponse EditQuery(EditQueryRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();
            Query query = new Query();
            query = _queryRepository.FindBy(request.ID);

            if (query != null)
            {
                try
                {
                    query.ModifiedDate = PersianDateTime.Now;
                    query.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    query.PrmDefinition = request.PrmDefinition;
                    query.PrmValues = request.PrmValues;
                    query.QueryText = request.QueryText;
                    query.Title = request.Title;

                    if (query.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        query.RowVersion += 1;
                    }

                    if (query.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in query.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _queryRepository.Save(query);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                }

            }
            else
            {

                response.ErrorMessages.Add("NoItemToEditKey");
            }
            return response;
        }
        #endregion

        #region Delete
        public GeneralResponse DeleteQuery(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            Query query = new Query();
            query = _queryRepository.FindBy(request.ID);

            if (query != null)
            {
                try
                {
                    _queryRepository.Remove(query);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                }
            }

            return response;
        }
        #endregion

        #region Get One
        public GetQueryResponse GetQuery(GetRequest request)
        {
            GetQueryResponse response = new GetQueryResponse();

            try
            {
                Query query = new Query();
                QueryView queryView = query.ConvertToQueryView();

                query = _queryRepository.FindBy(request.ID);
                if (query != null)
                    queryView = query.ConvertToQueryView();

                response.QueryView = queryView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get All
        /// <summary>
        /// برای بدست آوردن لینک کوئریها
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetQueriesResponse GetQueries(GetQueriesRequest request)
        {
            GetQueriesResponse response = new GetQueriesResponse();
            
            try
            {
                // current Employee
                Employee currentEmployee = _employeeRepository.FindBy(request.EmployeeID);

                // All Queries
                IEnumerable<Query> queries = _queryRepository.FindAll().ToList();

                // Output List
                IList<Query> outPutQueries = new List<Query>();

                // اگر مشتری اجازه رویت ندارد از لیست حذف شود
                foreach (var query in queries)
                {
                    if (currentEmployee.CanView(query))
                    {
                        outPutQueries.Add(query);
                    }
                }

                IEnumerable<QueryView> queryViews = outPutQueries
                    .ConvertToQueryViews().OrderBy(o => o.CreateDate);

                response.QueryViews = queryViews;
                response.TotalCount = queryViews.Count();
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        /// <summary>
        /// برای نمایش جدولی نماها در تنظیمات
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetQueriesResponse GetQueries(AjaxGetRequest request)
        {
            GetQueriesResponse response = new GetQueriesResponse();

            try
            {
                int index = (request.PageNumber - 1) * request.PageSize;
                int count = request.PageSize;

                Infrastructure.Domain.Response<Query> querysResponse = _queryRepository
                    .FindAll(index, count);

                IEnumerable<QueryView> querys = querysResponse.data
                    .ConvertToQueryViews().OrderBy(o => o.CreateDate);

                response.QueryViews = querys;
                response.TotalCount = querysResponse.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
        #endregion

        #endregion

        #region New methods

        #region Read

        #region Read One

        public GetGeneralResponse<QueryView> GetQuery(Guid QueryID)
        {
            GetGeneralResponse<QueryView> response = new GetGeneralResponse<QueryView>();

            try
            {

                Query query = new Query();
                query = _queryRepository.FindBy(QueryID);
                response.data = query.ConvertToQueryView();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Read All

        public GetGeneralResponse<IEnumerable<QueryView>> GetQueries(int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<QueryView>> response = new GetGeneralResponse<IEnumerable<QueryView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<Query> queryView = _queryRepository.FindAll(index, count);

                response.data = queryView.data.ConvertToQueryViews();
                response.totalCount = queryView.totalCount;

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #endregion

        #region Insert

        public GeneralResponse AddQuery(AddQueryRequest request, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Query query = new Query();
                query.ID = Guid.NewGuid();
                query.CreateDate = PersianDateTime.Now;
                query.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                query.PrmDefinition = request.PrmDefinition;
                query.PrmValues = request.PrmValues;
                query.QueryText = request.QueryText;
                query.RowVersion = 1;
                query.Title = request.Title;
                query.xType = request.xType;
                _queryRepository.Add(query);
                _uow.Commit();
            }

            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Edit

        public GeneralResponse EditQuery(EditQueryRequest request, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Query query = new Query();
                query = _queryRepository.FindBy(request.ID);
                query.ModifiedDate = PersianDateTime.Now;
                query.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                if (request.PrmDefinition != null)
                query.PrmDefinition = request.PrmDefinition;
                if (request.PrmValues != null)
                query.PrmValues = request.PrmValues;
                if (request.QueryText != null)
                query.QueryText = request.QueryText;
                if (request.Title != null)
                query.Title = request.Title;
                if (request.xType != null)
                query.xType = request.xType;

                #region RowVersion - Validation
                if (query.RowVersion != request.RowVersion)
                {

                    response.ErrorMessages.Add("EditConcurrencyKey");
                    return response;
                }
                else
                {
                    query.RowVersion += 1;
                }

                if (query.GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in query.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                _queryRepository.Save(query);
                _uow.Commit();

            }

            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Delete

        public GeneralResponse DeleteQuery(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (DeleteRequest request in requests)
                {
                    Query query = _queryRepository.FindBy(request.ID);
                    _queryRepository.Remove(query);
                }
                _uow.Commit();
            }

            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Query Employee methods


        #endregion

        #endregion
    }
}
