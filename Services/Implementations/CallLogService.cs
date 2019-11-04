using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Infrastructure.Domain;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Customers;
using Model.Customers.Interfaces;
using Model.Customers.Validations.Interfaces;
using Model.Employees;
using Model.Employees.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.ViewModels.Customers;

namespace Services.Implementations
{
    public class CallLogService : ICallLogService
    {
        #region Private Members

        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICustomerContactTemplateRepository _customerContactTemplateRepository;
        private readonly ICallLogRepository _callLogRepository;
        private readonly IUnitOfWork _uow;

        #endregion

        public CallLogService(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository,
            ICustomerContactTemplateRepository customerContactTemplateRepository,ICallLogRepository callLogRepository, IUnitOfWork uow)
        {
            _customerContactTemplateRepository = customerContactTemplateRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _callLogRepository = callLogRepository;
            _uow = uow;
        }

        #region Read All

        public GetGeneralResponse<IEnumerable<CallLogView>> GetAllCallLog(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort)
        {
            var response=new GetGeneralResponse<IEnumerable<CallLogView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "CallLog", sort);

                Response<CallLog> callLogs = _callLogRepository.FindAll(query, index, count);

                response.data = callLogs.data.ConvertToCallLogViews();
                response.totalCount = callLogs.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException!=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<CallLogView>> GetOwnCallLog(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort,Guid EmployeeID)
        {
            var response = new GetGeneralResponse<IEnumerable<CallLogView>>();
            try
            {
                IList<FilterData> Filters=new List<FilterData>();
                if(filter!=null)
                    foreach(var item in filter)
                        Filters.Add(item);
                Filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "eq",
                        type = "list",
                        value = new []{EmployeeID.ToString()}
                    },
                    field = "CreateEmployee"
                });

                int index = (pageNumber - 1)*pageSize;
                int count = pageSize;

                string query = FilterUtilityService.GenerateFilterHQLQuery(Filters, "CallLog", sort);
                Response<CallLog> callLogs = _callLogRepository.FindAll(query, index, count);

                response.data = callLogs.data.ConvertToCallLogViews();
                response.totalCount = callLogs.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<CallLogView>> GetCustomerallLog(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort, Guid CustomerID)
        {
            var response = new GetGeneralResponse<IEnumerable<CallLogView>>();
            try
            {
                IList<FilterData> Filters = new List<FilterData>();
                if (filter != null)
                    foreach (var item in filter)
                        Filters.Add(item);
                Filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "eq",
                        type = "list",
                        value = new[] { CustomerID.ToString() }
                    },
                    field = "CreateEmployee"
                });

                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                string query = FilterUtilityService.GenerateFilterHQLQuery(Filters, "CallLog", sort);
                Response<CallLog> callLogs = _callLogRepository.FindAll(query, index, count);

                response.data = callLogs.data.ConvertToCallLogViews();
                response.totalCount = callLogs.totalCount;
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

        #region Set Result

        public GeneralResponse SetResult(Guid CallLogID, Guid CustomerContactTemplateID, string Description,
            Guid EmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                Employee employee = _employeeRepository.FindBy(EmployeeID);
                CallLog callLog = _callLogRepository.FindBy(CallLogID);

                if (employee.ID != callLog.CreateEmployee.ID)
                {
                    response.ErrorMessages.Add("شما مجاز به ثبت نتیجه برای این تماس نیستید");
                    return response;
                }

                callLog.ModifiedDate = PersianDateTime.Now;
                callLog.CustomerContactTemplate = _customerContactTemplateRepository.FindBy(CustomerContactTemplateID);
                callLog.Description = Description;
                callLog.RowVersion = callLog.RowVersion + 1;
                callLog.ModifiedEmployee = _employeeRepository.FindBy(EmployeeID);

                _callLogRepository.Save(callLog);
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
    }
}
