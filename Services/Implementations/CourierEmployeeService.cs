using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Persian;
using Infrastructure.UnitOfWork;
using Model.Base;
using Model.Customers.Interfaces;
using Model.Employees;
using Model.Employees.Interfaces;
using Model.Sales;
using Model.Sales.Interfaces;
using NHibernate.Cfg;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Customers;
using Services.ViewModels.Employees;

namespace Services.Implementations
{
    public class CourierEmployeeService : ICourierEmployeeService
    {

        #region Declares

        private readonly ICustomerRepository _customerRepository;

        private readonly ICourierEmployeeRepository _courierEmployeeRepository;

        private readonly ICourierRepository _courierRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly ISaleRepository _saleRepository;

        private readonly IUnitOfWork _uow;

        #endregion

        #region Ctor

        public CourierEmployeeService(ICustomerRepository customerRepository,
            ICourierEmployeeRepository courierEmployeeRepository,
            ICourierRepository courierRepository, IEmployeeRepository employeeRepository, ISaleRepository saleRepository,
            IUnitOfWork uow)
        {
            _customerRepository = customerRepository;
            _courierEmployeeRepository = courierEmployeeRepository;
            _courierRepository = courierRepository;
            _employeeRepository = employeeRepository;
            _saleRepository = saleRepository;
            _uow = uow;
        }

        #endregion

        #region Read

        #region All

        public GetGeneralResponse<IEnumerable<CourierEmployeeView>> GetCourierEmployees()
        {
            GetGeneralResponse<IEnumerable<CourierEmployeeView>> response=new GetGeneralResponse<IEnumerable<CourierEmployeeView>>();

            try
            {
                IEnumerable<CourierEmployee> courieres = _courierEmployeeRepository.FindAll();

                response.data = courieres.ConvertToCourierEmployeeViews();
                response.totalCount = courieres.Count();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException!=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }
        #endregion
        #endregion

        #region Add

        public GeneralResponse AddCourierEmployee(AddCourierEmployeeRequest request, Guid CreateEployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                CourierEmployee courierEmployee=new CourierEmployee();
                courierEmployee.ID = Guid.NewGuid();
                courierEmployee.CreateDate = PersianDateTime.Now;
                courierEmployee.CreateEmployee = _employeeRepository.FindBy(CreateEployeeID);
                courierEmployee.Address = request.Address;
                courierEmployee.FirstName = request.FirstName;
                courierEmployee.LastName = request.LastName;
                courierEmployee.Mobile = request.Mobile;
                courierEmployee.Phone = request.Phone;
                courierEmployee.RowVersion = 1;
               
                _courierEmployeeRepository.Add(courierEmployee);
                _uow.Commit();

            }
            catch (Exception ex)
            {

                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException!=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }
        #endregion

        #region Edit

        public GeneralResponse EditCourierEmployee(EditCourierEmployeeRequest request, Guid ModifiedEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                CourierEmployee courierEmployee=new CourierEmployee();
                courierEmployee = _courierEmployeeRepository.FindBy(request.ID);

                courierEmployee.ModifiedDate = PersianDateTime.Now;
                courierEmployee.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                courierEmployee.Address = request.Address;
                courierEmployee.FirstName = request.FirstName;
                courierEmployee.LastName = request.LastName;
                courierEmployee.Phone = request.Phone;
                courierEmployee.Mobile = request.Mobile;

                #region Row Version Check

                if (courierEmployee.RowVersion != request.RowVersion)
                {

                    response.ErrorMessages.Add("EditConcurrencyKey");
                    return response;
                }
                else
                {
                    courierEmployee.RowVersion += 1;
                }

                if (courierEmployee.GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in courierEmployee.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }


                #endregion

                _courierEmployeeRepository.Save(courierEmployee);
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

        public GeneralResponse DeleteCourierEmployee(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (DeleteRequest request in requests)
                {

                    _courierEmployeeRepository.RemoveById(request.ID);
                    
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
    }
}