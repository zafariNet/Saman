using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Infrastructure.UnitOfWork;
using Model.Employees.Interfaces;
using Services.Messaging;
using Services.ViewModels.Employees;
using Infrastructure.Querying;
using Model.Employees;
using Infrastructure.Domain;
using Services.Mapping;

namespace Services.Implementations
{
    public class SmsEmployeeService:ISmsEmployeeService
    {
        #region Declare

        private readonly IUnitOfWork _uow;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly ISmsEmployeeRepository _smsEmployeeRepository;

        #endregion

        #region ctor

        public SmsEmployeeService(IUnitOfWork uow,IEmployeeRepository employeeRepository,ISmsEmployeeRepository smsEmployeeRepository)
        {
            _uow = uow;
            _employeeRepository = employeeRepository;
            _smsEmployeeRepository = smsEmployeeRepository;
        }

        #endregion

        #region Read By Employee

        public GetGeneralResponse<IEnumerable<SmsEmployeeView>> GetSmsEmployeeByOwner(Guid EmployeeID,int pageSize,int pageNumber)
        {
            GetGeneralResponse<IEnumerable<SmsEmployeeView>> response = new GetGeneralResponse<IEnumerable<SmsEmployeeView>>();

            try
            {
                                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;


                Query query = new Query();
                Criterion ctr = new Criterion("Employee.ID", EmployeeID, CriteriaOperator.Equal);
                query.Add(ctr);

                Response<SmsEmployee> smsEmployees = _smsEmployeeRepository.FindBy(query, index, count);

                response.data = smsEmployees.data.ConvertTosmsEmployeeViews();
                response.totalCount = smsEmployees.totalCount;
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

        #region Get All

        public GetGeneralResponse<IEnumerable<SmsEmployeeView>> GetSmsEmployees(IList<FilterData> filter,IList<Sort> sort,int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<SmsEmployeeView>> response = new GetGeneralResponse<IEnumerable<SmsEmployeeView>>();

            try
            {
                                                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;
                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "SmsEmployee", sort);
                Response<SmsEmployee> smsEmployees = _smsEmployeeRepository.FindAll(query, index, count);

                response.data = smsEmployees.data.ConvertTosmsEmployeeViews();
                response.totalCount = smsEmployees.totalCount;
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
