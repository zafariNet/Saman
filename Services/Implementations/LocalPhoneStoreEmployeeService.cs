using Infrastructure.Domain;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Employees;
using Model.Employees.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Implementations
{
    public class LocalPhoneStoreEmployeeService : ILocalPhoneStoreEmployeeService
    {

        #region Declare

        private readonly IEmployeeRepository _employeeRepository;

        private readonly ILocalPhoneStoreRepository _localPhoneStoreRepository;

        private readonly ILocalPhoneStoreEmployeeRepository _localPhoneStoreEployeeRepository;

        private readonly IQueueRepository _queueRepository;

        private readonly IQueueLocalPhoneStoreRepository _queueLocalPhoneRepository;

        private readonly IUnitOfWork _uow;

        #endregion

        #region Ctor

        public LocalPhoneStoreEmployeeService(IEmployeeRepository employeeRepository, ILocalPhoneStoreRepository localPhoneStoreRepository,
            IQueueRepository queueRepository, IQueueLocalPhoneStoreRepository queueLocalPhoneRepository,ILocalPhoneStoreEmployeeRepository localPhoneStoreEployeeRepository, IUnitOfWork uow)
        {
            _employeeRepository = employeeRepository;
            _localPhoneStoreRepository = localPhoneStoreRepository;
            _queueRepository = queueRepository;
            _queueLocalPhoneRepository = queueLocalPhoneRepository;
            _localPhoneStoreEployeeRepository = localPhoneStoreEployeeRepository;
            _uow = uow;
        }

        #endregion


        #region get Local Phones From Asterisk

        public GeneralResponse GetLocalPhoneStoresFromAsterisk()
        {
            GeneralResponse response = new GeneralResponse();

            try
            {

                var monitoring = new MonitoringControllerService();
                string Token = monitoring.login("saman", "102030");
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

 

        #region Read

        public GetGeneralResponse<IEnumerable<LocalPhoneStoreEmployeeView>> GetAllLocalPhoneStoreEmployee(int pageSize, int pageNumber, IList<FilterData> filter, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<LocalPhoneStoreEmployeeView>> response =
                new GetGeneralResponse<IEnumerable<LocalPhoneStoreEmployeeView>>();

            try
            {
                int index = (pageNumber - 1)*pageSize;
                int count = pageSize;
                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "LocalPhoneStoreEmployee", sort);

                Response<LocalPhoneStoreEmployee> localPhoneStoreEmployee =
                    _localPhoneStoreEployeeRepository.FindAll(query, index, count);

                response.data = localPhoneStoreEmployee.data.ConvertToLocalPhoneEmployeeViews();
                response.totalCount = localPhoneStoreEmployee.totalCount;

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

        #region Local Phone Store Read By Employee

        public GetGeneralResponse<IEnumerable<LocalPhoneStoreEmployeeView>> GetLocalPhoneStoreEmployeeByEmployee(
            Guid EmployeeID)
        {
            GetGeneralResponse<IEnumerable<LocalPhoneStoreEmployeeView>> response=new GetGeneralResponse<IEnumerable<LocalPhoneStoreEmployeeView>>();

            try
            {
                Query query=new Query();
                Criterion crt=new Criterion("OwnerEmployee.ID",EmployeeID,CriteriaOperator.Equal);
                query.Add(crt);

                Response<LocalPhoneStoreEmployee> locaPhoneStoreemployees =
                    _localPhoneStoreEployeeRepository.FindByQuery(query);

                response.data = locaPhoneStoreemployees.data.ConvertToLocalPhoneEmployeeViews();
                response.totalCount = locaPhoneStoreemployees.totalCount;
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

        public GeneralResponse AddLocalPhoneStoreEmployee(IEnumerable<AddLocalPhoneStoreEmployeeRequest> requests,
            Guid OwnerEmployeeID,
            Guid CreateEployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (var request in requests)
                {
                    Query localPhoneStoreEployeeQuery = new Query();
                    Criterion crt = new Criterion("LocalPhoneStore.ID", request.LocalPhoneStoreID,
                        CriteriaOperator.Equal);
                    localPhoneStoreEployeeQuery.Add(crt);
                    Response<LocalPhoneStoreEmployee> localPhoneStoreEmployeeSaved =
                        _localPhoneStoreEployeeRepository.FindByQuery(localPhoneStoreEployeeQuery);
                    if (localPhoneStoreEmployeeSaved.totalCount > 0)
                    {
                        response.ErrorMessages.Add("این شماره داخلی قبلا برای کارمند دیگری تعریف شده است");
                        return response;
                    }

                    LocalPhoneStoreEmployee localPhoneStoreEmployee = new LocalPhoneStoreEmployee();

                    localPhoneStoreEmployee.ID = Guid.NewGuid();
                    localPhoneStoreEmployee.CreateDate = PersianDateTime.Now;
                    localPhoneStoreEmployee.CreateEmployee = _employeeRepository.FindBy(CreateEployeeID);
                    localPhoneStoreEmployee.DangerousRing = request.DangerousRing;
                    localPhoneStoreEmployee.DangerousSeconds = request.DangerousSeconds;
                    localPhoneStoreEmployee.LocalPhoneStore =
                        _localPhoneStoreRepository.FindBy(request.LocalPhoneStoreID);
                    localPhoneStoreEmployee.OwnerEmployee = _employeeRepository.FindBy(OwnerEmployeeID);
                    localPhoneStoreEmployee.SendSmsToOffLineUserOnDangerous = request.SendSmsToOffLineUserOnDangerous;
                    localPhoneStoreEmployee.SendSmsToOnLineUserOnDangerous = request.SendSmsToOnLineUserOnDangerous;
                    localPhoneStoreEmployee.SmsText = request.SmsText;
                    localPhoneStoreEmployee.RowVersion = 1;

                    _localPhoneStoreEployeeRepository.Add(localPhoneStoreEmployee);


                    #region برچسب رزرو شده به لیست شماره های داخلی

                    LocalPhoneStore localPhoneStore = _localPhoneStoreRepository.FindBy(request.LocalPhoneStoreID);
                    localPhoneStore.Reserved = true;
                    _localPhoneStoreRepository.Save(localPhoneStore);

                    #endregion

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

        #region Remove

        public GeneralResponse DeleteLocalPhoneStoreEmployee(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (var request in requests)
                {
                    LocalPhoneStoreEmployee localPhoneStoreEmployee=new LocalPhoneStoreEmployee();

                    localPhoneStoreEmployee = _localPhoneStoreEployeeRepository.FindBy(request.ID);

                    _localPhoneStoreEployeeRepository.Remove(localPhoneStoreEmployee);


                    #region آزاد سازی شماره داخلی

                    LocalPhoneStore localPhoneStore = localPhoneStoreEmployee.LocalPhoneStore;
                    localPhoneStore.Reserved = false;
                    _localPhoneStoreRepository.Save(localPhoneStore);

                    #endregion

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
