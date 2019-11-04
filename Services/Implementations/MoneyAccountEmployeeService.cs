#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Fiscals.Interfaces;
using Model.Employees.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.FiscalCatalogService;
using Model.Fiscals;
using Services.ViewModels.Fiscals;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Infrastructure.Querying;
using Infrastructure.Domain;
#endregion

namespace Services.Implementations
{
    public class MoneyAccountEmployeeService : IMoneyAccountEmployeeService
    {
        #region Declares

        private readonly IMoneyAccountEmployeeRepository _moneyAccountEmployeeRepository;
        private readonly IMoneyAccountRepository _moneyAccountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _uow;
        #endregion

        #region Ctor

        public MoneyAccountEmployeeService(IMoneyAccountEmployeeRepository moneyAccountEmployeeRepository, IUnitOfWork uow)
        {
            _moneyAccountEmployeeRepository = moneyAccountEmployeeRepository;
            _uow = uow;
        }

        public MoneyAccountEmployeeService(IMoneyAccountEmployeeRepository moneyAccountEmployeeRepository, IMoneyAccountRepository moneyAccountRepository,
            IEmployeeRepository employeeRepository, IUnitOfWork uow)
            : this(moneyAccountEmployeeRepository, uow)
        {
            this._employeeRepository = employeeRepository;
            this._moneyAccountRepository = moneyAccountRepository;
        }
        #endregion

        #region Old Methods

        #region Add

        public GeneralResponse AddMoneyAccountEmployee(AddMoneyAccountEmployeeRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                MoneyAccountEmployee moneyAccountEmployee = new MoneyAccountEmployee();
                moneyAccountEmployee.ID = Guid.NewGuid();
                moneyAccountEmployee.CreateDate = PersianDateTime.Now;
                moneyAccountEmployee.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                moneyAccountEmployee.Employee = this._employeeRepository.FindBy(request.EmployeeID);
                moneyAccountEmployee.MoneyAccount = this._moneyAccountRepository.FindBy(request.MoneyAccountID);
                moneyAccountEmployee.RowVersion = 1;

                #region Validation

                if (moneyAccountEmployee.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in moneyAccountEmployee.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }
                    return response;
                }
                #endregion

                _moneyAccountEmployeeRepository.Add(moneyAccountEmployee);
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
        public GeneralResponse EditMoneyAccountEmployee(EditMoneyAccountEmployeeRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            MoneyAccountEmployee moneyAccountEmployee = new MoneyAccountEmployee();
            moneyAccountEmployee = _moneyAccountEmployeeRepository.FindBy(request.ID);

            if (moneyAccountEmployee != null)
            {
                try
                {
                    moneyAccountEmployee.ModifiedDate = PersianDateTime.Now;
                    moneyAccountEmployee.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if (request.EmployeeID != null)
                        moneyAccountEmployee.Employee = this._employeeRepository.FindBy(request.EmployeeID);
                    if (request.MoneyAccountID != null)
                        moneyAccountEmployee.MoneyAccount = this._moneyAccountRepository.FindBy(request.MoneyAccountID);

                    if (moneyAccountEmployee.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        moneyAccountEmployee.RowVersion += 1;
                    }

                    if (moneyAccountEmployee.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in moneyAccountEmployee.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _moneyAccountEmployeeRepository.Save(moneyAccountEmployee);
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
        public GeneralResponse DeleteMoneyAccountEmployee(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            MoneyAccountEmployee moneyAccountEmployee = new MoneyAccountEmployee();
            moneyAccountEmployee = _moneyAccountEmployeeRepository.FindBy(request.ID);

            if (moneyAccountEmployee != null)
            {
                try
                {
                    _moneyAccountEmployeeRepository.Remove(moneyAccountEmployee);
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

        public GeneralResponse DeleteMoneyAccountEmployee(Guid moneyAccountID, Guid employeeID)
        {
            GeneralResponse response = new GeneralResponse();

            Infrastructure.Querying.Query moneyAccount = new Infrastructure.Querying.Query();
            Criterion criteria1 = new Criterion("MoneyAccount.ID", moneyAccountID, CriteriaOperator.Equal);
            Criterion criteria2 = new Criterion("Employee.ID", employeeID, CriteriaOperator.Equal);
            moneyAccount.Add(criteria1); moneyAccount.Add(criteria2);

            MoneyAccountEmployee moneyAccountEmployee = _moneyAccountEmployeeRepository.FindBy(moneyAccount).FirstOrDefault();

            if (moneyAccountEmployee != null)
            {
                try
                {
                    _moneyAccountEmployeeRepository.Remove(moneyAccountEmployee);
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

        public GetMoneyAccountEmployeeResponse GetMoneyAccountEmployee(GetRequest request)
        {
            GetMoneyAccountEmployeeResponse response = new GetMoneyAccountEmployeeResponse();

            try
            {
                MoneyAccountEmployee moneyAccountEmployee = new MoneyAccountEmployee();
                MoneyAccountEmployeeView moneyAccountEmployeeView = moneyAccountEmployee.ConvertToMoneyAccountEmployeeView();

                moneyAccountEmployee = _moneyAccountEmployeeRepository.FindBy(request.ID);
                if (moneyAccountEmployee != null)
                    moneyAccountEmployeeView = moneyAccountEmployee.ConvertToMoneyAccountEmployeeView();

                response.MoneyAccountEmployeeView = moneyAccountEmployeeView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get All
        public GetMoneyAccountEmployeesResponse GetMoneyAccountEmployees()
        {
            GetMoneyAccountEmployeesResponse response = new GetMoneyAccountEmployeesResponse();

            try
            {
                IEnumerable<MoneyAccountEmployeeView> moneyAccountEmployees = _moneyAccountEmployeeRepository.FindAll()
                    .ConvertToMoneyAccountEmployeeViews();

                response.MoneyAccountEmployeeViews = moneyAccountEmployees;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetMoneyAccountEmployeesResponse GetMoneyAccountEmployees(Guid moneyAccountID)
        {
            GetMoneyAccountEmployeesResponse response = new GetMoneyAccountEmployeesResponse();

            try
            {

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("MoneyAccount.ID", moneyAccountID, CriteriaOperator.Equal);
                query.Add(criteria);

                IEnumerable<MoneyAccountEmployeeView> moneyAccountEmployees = _moneyAccountEmployeeRepository.FindBy(query)
                    .ConvertToMoneyAccountEmployeeViews();

                response.MoneyAccountEmployeeViews = moneyAccountEmployees;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        #endregion

        #endregion

        #region New Methods

        #region Read

        public GetGeneralResponse<IEnumerable<MoneyAccountEmployeeView>> GetMoneyAccountEmployees(Guid MoneyAccountID, int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<MoneyAccountEmployeeView>> response = new GetGeneralResponse<IEnumerable<MoneyAccountEmployeeView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("MoneyAccount.ID", MoneyAccountID, CriteriaOperator.Equal);
                query.Add(criteria);

                IEnumerable<MoneyAccountEmployeeView> moneyAccountEmployees = _moneyAccountEmployeeRepository.FindBy(query).ConvertToMoneyAccountEmployeeViews();


                response.data = moneyAccountEmployees;
                response.totalCount = moneyAccountEmployees.Count();
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

        public GeneralResponse AddMoneyAccountEmployee(AddMoneyAccountEmployeeRequest request , Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                MoneyAccountEmployee moneyAccountEmployee = new MoneyAccountEmployee();
                moneyAccountEmployee.CreateDate = PersianDateTime.Now;
                moneyAccountEmployee.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                moneyAccountEmployee.Employee = _employeeRepository.FindBy(request.EmployeeID);
                moneyAccountEmployee.MoneyAccount = _moneyAccountRepository.FindBy(request.MoneyAccountID);

                _moneyAccountEmployeeRepository.Add(moneyAccountEmployee);
                _uow.Commit();
            }

            catch(Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Delete

        public GeneralResponse DeleteMoneyAccountEmployee(IEnumerable<DeleteMoneyAccountEmployeeRequest> requests, Guid MoneyAccountID)
        {
            GeneralResponse response = new GeneralResponse();


            try
            {
                foreach (DeleteMoneyAccountEmployeeRequest request in requests)
                {
                    Query query = new Query();
                    Criterion criteriaEmployeeID = new Criterion("Employee.ID", request.EmployeeID, CriteriaOperator.Equal);
                    query.Add(criteriaEmployeeID);
                    Criterion criteriaMoneyAccountID = new Criterion("MoneyAccount.ID",MoneyAccountID, CriteriaOperator.Equal);
                    query.Add(criteriaMoneyAccountID);

                    MoneyAccountEmployee moneyAccounEmployee = _moneyAccountEmployeeRepository.FindBy(query).FirstOrDefault();

                    _moneyAccountEmployeeRepository.Remove(moneyAccounEmployee);
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
