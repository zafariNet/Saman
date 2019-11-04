#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Customers.Interfaces;
using Model.Employees.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Model.Customers;
using Services.ViewModels.Customers;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Infrastructure.Querying;
using Infrastructure.Domain;
#endregion

namespace Services.Implementations
{
    public class QueryEmployeeService : IQueryEmployeeService
    {
        #region Declares

        private readonly IQueryEmployeeRepository _queryEmployeeRepository;
        private readonly IQueryRepository _queryRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _uow;

        #endregion

        #region Ctor

        public QueryEmployeeService(IQueryEmployeeRepository queryEmployeeRepository, IUnitOfWork uow)
        {
            _queryEmployeeRepository = queryEmployeeRepository;
            _uow = uow;
        }

        public QueryEmployeeService(IQueryEmployeeRepository queryEmployeeRepository, IQueryRepository queryRepository, IEmployeeRepository employeeRepository, IUnitOfWork uow)
            : this(queryEmployeeRepository, uow)
        {
            this._queryRepository = queryRepository;
            this._employeeRepository = employeeRepository;
        }

        #endregion

        #region Old Methods

        #region Add
        public GeneralResponse AddQueryEmployee(AddQueryEmployeeRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                QueryEmployee queryEmployee = new QueryEmployee();
                queryEmployee.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                queryEmployee.Employee = this._employeeRepository.FindBy(request.EmployeeID);
                queryEmployee.Query = this._queryRepository.FindBy(request.QueryID);

                #region Validation

                if (queryEmployee.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in queryEmployee.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                _queryEmployeeRepository.Add(queryEmployee);
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
        public GeneralResponse EditQueryEmployee(EditQueryEmployeeRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            QueryEmployee queryEmployee = new QueryEmployee();
            queryEmployee = _queryEmployeeRepository.FindBy(request.ID);

            if (queryEmployee != null)
            {
                try
                {
                    queryEmployee.ModifiedDate = PersianDateTime.Now;
                    queryEmployee.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if (request.EmployeeID != null)
                        queryEmployee.Employee = this._employeeRepository.FindBy(request.EmployeeID);
                    if (request.QueryID != null)
                        queryEmployee.Query = this._queryRepository.FindBy(request.QueryID);

                    if (queryEmployee.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        queryEmployee.RowVersion += 1;
                    }

                    if (queryEmployee.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in queryEmployee.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _queryEmployeeRepository.Save(queryEmployee);
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

        public GeneralResponse DeleteQueryEmployee(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            QueryEmployee queryEmployee = new QueryEmployee();
            queryEmployee = _queryEmployeeRepository.FindBy(request.ID);

            if (queryEmployee != null)
            {
                try
                {
                    _queryEmployeeRepository.Remove(queryEmployee);
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

        public GeneralResponse DeleteQueryEmployee(Guid queryID, Guid employeeID)
        {
            GeneralResponse response = new GeneralResponse();

            Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
            Criterion criteria1 = new Criterion("Query.ID", queryID, CriteriaOperator.Equal);
            Criterion criteria2 = new Criterion("Employee.ID", employeeID, CriteriaOperator.Equal);
            query.Add(criteria1); query.Add(criteria2);

            QueryEmployee queryEmployee = _queryEmployeeRepository.FindBy(query).FirstOrDefault();

            if (queryEmployee != null)
            {
                try
                {
                    _queryEmployeeRepository.Remove(queryEmployee);
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
        public GetQueryEmployeeResponse GetQueryEmployee(GetRequest request)
        {
            GetQueryEmployeeResponse response = new GetQueryEmployeeResponse();

            try
            {
                QueryEmployee queryEmployee = new QueryEmployee();
                QueryEmployeeView queryEmployeeView = queryEmployee.ConvertToQueryEmployeeView();

                queryEmployee = _queryEmployeeRepository.FindBy(request.ID);
                if (queryEmployee != null)
                    queryEmployeeView = queryEmployee.ConvertToQueryEmployeeView();

                response.QueryEmployeeView = queryEmployeeView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get All

        public GetQueryEmployeesResponse GetQueryEmployees()
        {
            GetQueryEmployeesResponse response = new GetQueryEmployeesResponse();

            try
            {
                IEnumerable<QueryEmployeeView> queryEmployees = _queryEmployeeRepository.FindAll()
                    .ConvertToQueryEmployeeViews();

                response.QueryEmployeeViews = queryEmployees;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetQueryEmployeesResponse GetQueryEmployees(Guid queryID)
        {
            GetQueryEmployeesResponse response = new GetQueryEmployeesResponse();

            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("Query.ID", queryID, CriteriaOperator.Equal);
                query.Add(criteria);

                IEnumerable<QueryEmployeeView> queryEmployees = _queryEmployeeRepository.FindBy(query)
                    .ConvertToQueryEmployeeViews();

                response.QueryEmployeeViews = queryEmployees;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        #endregion

        #endregion

        #region New Methods
        
        #region Read By Query ID

        public GetGeneralResponse<IEnumerable<QueryEmployeeView>> GetQueryEmployees(Guid QueryID,int pageSize,int pageNumber)
        {
            GetGeneralResponse<IEnumerable<QueryEmployeeView>> response = new GetGeneralResponse<IEnumerable<QueryEmployeeView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("Query.ID", QueryID, CriteriaOperator.Equal);
                query.Add(criteria);

                IEnumerable<QueryEmployeeView> queryEmployees = _queryEmployeeRepository.FindBy(query)
                    .ConvertToQueryEmployeeViews();

                response.data = queryEmployees;
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

        #region Insert

        public GeneralResponse AddQueryEmployees(Guid QueryID,IEnumerable<AddQueryEmployeeRequest> requests, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (AddQueryEmployeeRequest request in requests)
                {
                    QueryEmployee queryEmployee = new QueryEmployee();

                    queryEmployee.CreateDate = PersianDateTime.Now;
                    queryEmployee.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    queryEmployee.ID = Guid.NewGuid();
                    queryEmployee.Employee = _employeeRepository.FindBy(request.EmployeeID);
                    queryEmployee.Query = _queryRepository.FindBy(QueryID);
                    _queryEmployeeRepository.Add(queryEmployee);
                }
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                {
                    response.ErrorMessages.Add(ex.InnerException.Message);
                }
            }

            return response;
        }

        #endregion

        #region Delete

        public GeneralResponse DeleteQueryEmployees(IEnumerable<QueryEmployeeDeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                foreach (QueryEmployeeDeleteRequest request in requests)
                {
                    Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                    Criterion criteriaQueryID = new Criterion("Query.ID", request.QueryID, CriteriaOperator.Equal);
                    query.Add(criteriaQueryID);
                    Criterion criteriaEmployeeID = new Criterion("Employee.ID", request.EmployeeID, CriteriaOperator.Equal);
                    query.Add(criteriaEmployeeID);

                    QueryEmployee queryemployee = new QueryEmployee();
                    queryemployee = _queryEmployeeRepository.FindBy(query).FirstOrDefault();
                    _queryEmployeeRepository.Remove(queryemployee);

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
        #endregion
    }
}
