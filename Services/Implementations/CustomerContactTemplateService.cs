using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Domain;
using Infrastructure.Persian;
using Infrastructure.UnitOfWork;
using Model.Customers;
using Model.Customers.Validations.Interfaces;
using Model.Employees;
using Model.Employees.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Customers;

namespace Services.Implementations
{
    public class CustomerContactTemplateService : ICustomerContactTemplateService
    {
        #region Private Members

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ICustomerContactTemplateRepository _customerContactTemplateRepository;
        private readonly IUnitOfWork _uow;

        #endregion

        #region Ctor

        public CustomerContactTemplateService(IEmployeeRepository employeeRepository, IGroupRepository groupRepository,
            ICustomerContactTemplateRepository customerContactTemplateRepository, IUnitOfWork uow)
        {
            _employeeRepository = employeeRepository;
            _groupRepository = groupRepository;
            _customerContactTemplateRepository = customerContactTemplateRepository;
            _uow = uow;
        }

        #endregion

        #region Read

        /// <summary>
        /// مشاهده همه قالب ها
        /// </summary>
        /// <returns></returns>

        public GetGeneralResponse<IEnumerable<CustomerContactTemplateView>> GetAll(int pageSize,int pageNumber)
        {
            GetGeneralResponse<IEnumerable<CustomerContactTemplateView>> response =
                new GetGeneralResponse<IEnumerable<CustomerContactTemplateView>>();

            try
            {

                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<CustomerContactTemplate> customerContactTemplates =
                    _customerContactTemplateRepository.FindAll(index,count);
                response.data = customerContactTemplates.data.ConvertToCustomerContactTemplateViews();
                response.totalCount = customerContactTemplates.totalCount;
            }
            catch (Exception ex)
            {

                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        /// <summary>
        /// مشاهده قالب ها بر اساس گروه کارمند
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public GetGeneralResponse<IEnumerable<CustomerContactTemplateView>> GetAllByGroup(Guid EmployeeID)
        {
            GetGeneralResponse<IEnumerable<CustomerContactTemplateView>> response =
    new GetGeneralResponse<IEnumerable<CustomerContactTemplateView>>();
            try
            {
                IEnumerable<CustomerContactTemplate> customerContactTemplates =
                    _customerContactTemplateRepository.FindAll();
                Employee employee = _employeeRepository.FindBy(EmployeeID);
                var contactTemplates = customerContactTemplates as CustomerContactTemplate[] ??
                                       customerContactTemplates.Where(x=>x.Group.ID==employee.Group.ID).ToArray();
                response.data = contactTemplates.ConvertToCustomerContactTemplateViews();
                response.totalCount = contactTemplates.Count();
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

        #region Add

        public GeneralResponse AddCustomerContactTemplate(IEnumerable<AddCustomerContactTemplateRequest> requests,
            Guid EmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (var request in requests)
                {
                    CustomerContactTemplate customerContactTemplate=new CustomerContactTemplate();
                    customerContactTemplate.ID = Guid.NewGuid();
                    customerContactTemplate.CreateDate = PersianDateTime.Now;
                    customerContactTemplate.RowVersion = 1;
                    customerContactTemplate.CreateEmployee = _employeeRepository.FindBy(EmployeeID);
                    customerContactTemplate.Group = _groupRepository.FindBy(request.GroupId);
                    customerContactTemplate.Title = request.Title;

                    #region Validation

                    if (customerContactTemplate.GetBrokenRules().Any())
                    {
                        foreach (var item in customerContactTemplate.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(item.Rule);
                        }
                        return response;
                    }

                    #endregion

                    _customerContactTemplateRepository.Add(customerContactTemplate);


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

        #region Edit

        public GeneralResponse EditCustomerContactTemplate(IEnumerable<EditCustomerContactTemplateRequest> requests,
            Guid ModifiedEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (var request in requests)
                {
                    CustomerContactTemplate customerContactTemplate=new CustomerContactTemplate();
                    customerContactTemplate = _customerContactTemplateRepository.FindBy(request.ID);
                    customerContactTemplate.Title = request.Title;
                    customerContactTemplate.Group = _groupRepository.FindBy(request.GroupId);
                    customerContactTemplate.ModifiedDate = PersianDateTime.Now;
                    customerContactTemplate.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);

                    _customerContactTemplateRepository.Save(customerContactTemplate);
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

        #region Delete

        public GeneralResponse DeleteCustomerCOntactTemplate(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (var request in requests)
                {
                    CustomerContactTemplate customerContactTemplate=new CustomerContactTemplate();

                    customerContactTemplate = _customerContactTemplateRepository.FindBy(request.ID);
                    _customerContactTemplateRepository.Remove(customerContactTemplate);
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
