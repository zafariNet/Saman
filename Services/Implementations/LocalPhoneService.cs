#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Employees.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Model.Employees;
using Services.ViewModels.Employees;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Services.Messaging.CustomerCatalogService;
using Infrastructure.Domain;
using Infrastructure.Querying;

#endregion

namespace Services.Implementations
{
    public class LocalPhoneService : ILocalPhoneService
    {
        #region Declares
        private readonly ILocalPhoneRepository _localPhoneRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _uow;
        #endregion

        #region Ctor
        public LocalPhoneService(ILocalPhoneRepository localPhoneRepository, IUnitOfWork uow)
        {
            _localPhoneRepository = localPhoneRepository;
            _uow = uow;
        }

        public LocalPhoneService(ILocalPhoneRepository localPhoneRepository, IEmployeeRepository employeeRepository, IUnitOfWork uow)
            : this(localPhoneRepository, uow)
        {
            this._employeeRepository = employeeRepository;
        }
        #endregion

        #region Add

        public GeneralResponse AddLocalPhone(IEnumerable<AddLocalPhoneRequest> requests, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (var request in requests)
                {

                    Query query=new Query();
                    Criterion crt = new Criterion("LocalPhoneNumber",request.LocalPhoneNumber,CriteriaOperator.Equal);
                    query.Add(crt);
                    Response<LocalPhone> localPhones = _localPhoneRepository.FindByQuery(query);
                    if (localPhones.data.Count() > 0)
                    {
                        response.ErrorMessages.Add("این شمارهداخلی قبلا ثبت شده است");
                        return response;
                    }

                    LocalPhone localPhone = new LocalPhone();
                    localPhone.ID = Guid.NewGuid();
                    localPhone.CreateDate = PersianDateTime.Now;
                    localPhone.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    localPhone.LocalPhoneNumber = request.LocalPhoneNumber;
                    localPhone.OwnerEmployee = _employeeRepository.FindBy(request.OwnerEmployeeID);
                    localPhone.RowVersion = 1;

                    #region Validation

                    if (localPhone.GetBrokenRules().Count() > 0)
                    {


                        foreach (BusinessRule businessRule in localPhone.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _localPhoneRepository.Add(localPhone);

                }

                
                _uow.Commit();

            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        #endregion

        #region Edit
        public GeneralResponse EditLocalPhone(IEnumerable<EditLocalPhoneRequest> requests, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            
            

                try
                {
                    foreach (var request in requests)
                    {
                        LocalPhone localPhone = _localPhoneRepository.FindBy(request.ID);
                        localPhone.ModifiedDate = PersianDateTime.Now;
                        localPhone.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                        if (request.LocalPhoneNumber != null)
                            localPhone.LocalPhoneNumber = request.LocalPhoneNumber;

                        if (localPhone.RowVersion != request.RowVersion)
                        {

                            response.ErrorMessages.Add("EditConcurrencyKey");
                            return response;
                        }
                        else
                        {
                            localPhone.RowVersion += 1;
                        }

                        if (localPhone.GetBrokenRules().Count() > 0)
                        {

                            foreach (BusinessRule businessRule in localPhone.GetBrokenRules())
                            {
                                response.ErrorMessages.Add(businessRule.Rule);
                            }

                            return response;
                        }

                        _localPhoneRepository.Save(localPhone);
                    }
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

        #region Delete
        public GeneralResponse DeleteLocalPhone(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

                try
                {
                    foreach (var request in requests)
                    {
                        _localPhoneRepository.RemoveById(request.ID);
                    }

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

        #region Get One
        public GetGeneralResponse<LocalPhoneView> GetLocalPhone(GetRequest request)
        {
            GetGeneralResponse<LocalPhoneView> response = new GetGeneralResponse<LocalPhoneView>();

            try
            {
                LocalPhone localPhone = new LocalPhone();
                LocalPhoneView localPhoneView = localPhone.ConvertToLocalPhoneView();

                localPhone = _localPhoneRepository.FindBy(request.ID);
                if (localPhone != null)
                    localPhoneView = localPhone.ConvertToLocalPhoneView();

                response.data = localPhoneView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get All

        public GetGeneralResponse<IEnumerable<LocalPhoneView>> GetLocalPhones()
        {
            GetGeneralResponse<IEnumerable<LocalPhoneView>> response = new GetGeneralResponse<IEnumerable<LocalPhoneView>>();

            try
            {
                IEnumerable<LocalPhoneView> localPhones = _localPhoneRepository.FindAll()
                    .ConvertToLocalPhoneViews();

                response.data = localPhones;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<LocalPhoneView>> GetLocalPhonesByEmployee(Guid EmployeeID)
        {
            GetGeneralResponse<IEnumerable<LocalPhoneView>> response = new GetGeneralResponse<IEnumerable<LocalPhoneView>>();

            try
            {

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("OwnerEmployee.ID", EmployeeID, CriteriaOperator.Equal);
                query.Add(criteria);

                IEnumerable<LocalPhone> localPhones = _localPhoneRepository.FindAll();
                IEnumerable<LocalPhone> FinallocalPhones = localPhones.Where(x => x.OwnerEmployee.ID == EmployeeID);
                response.data = FinallocalPhones.ConvertToLocalPhoneViews();
                response.totalCount = FinallocalPhones.Count();
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public string[] GetLocalPhoneStr(Guid EmployeeID)
        {
            string[] localPhonesStr;
            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("OwnerEmployee.ID", EmployeeID, CriteriaOperator.Equal);
                query.Add(criteria);

                Response<LocalPhone> localPhones = _localPhoneRepository.FindBy(query, -1, -1);

                localPhonesStr = localPhones.data.Select(s => s.LocalPhoneNumber).ToArray();
            }
            catch (Exception ex)
            {
                throw;
            }

            return localPhonesStr;
        }

        public GetGeneralResponse<IEnumerable<LocalPhoneView>> GetLocalPhones(int pageSize, int pageNumber,
            List<FilterData> filter, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<LocalPhoneView>> response =
                new GetGeneralResponse<IEnumerable<LocalPhoneView>>();

            
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;
            Response<LocalPhone> localPhones = new Response<LocalPhone>();
            string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "LocalPhone", sort);
            if(count>0)
             localPhones= _localPhoneRepository.FindAll(query, index, count);
            else
                localPhones = _localPhoneRepository.FindAll(query);
            response.data = localPhones.data.ConvertToLocalPhoneViews();
            response.totalCount = localPhones.totalCount;

            return response;
        }

        #endregion

    }
}
