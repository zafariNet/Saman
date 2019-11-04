using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Base;
using Model.Customers.Interfaces;
using Model.Employees.Interfaces;
using Model.Support;
using Model.Support.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Implementations
{
    public class SupportExpertDispatchService:ISupportExpertDispatchService
    {
        #region Declares

        private readonly ISupportExpertDispatchRepository _supportExpertDispatchRepository;

        private readonly ICustomerRepository _customerRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly IUnitOfWork _uow;

        private readonly ISupportRepository _supportRepository;

        private readonly ISupportStatusRepository _supportStatusRepository;

        private readonly ISupportStatusRelationRepository _supportStatusRelationRepository;

        #endregion

        #region Ctor

        public SupportExpertDispatchService(ISupportExpertDispatchRepository supportExpertDispatchRepository,
            ICustomerRepository customerRepository, IEmployeeRepository employeeRepository,ISupportStatusRepository supportStatusRepository,
            IUnitOfWork uow, ISupportRepository supportRepository, ISupportStatusRelationRepository supportStatusRelationRepository
            )
        {
            _supportExpertDispatchRepository = supportExpertDispatchRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _uow = uow;
            _supportRepository = supportRepository;
            _supportStatusRepository = supportStatusRepository;
            _supportStatusRelationRepository = supportStatusRelationRepository;
        }

        #endregion

        #region Read

        #region Read All

        public GetGeneralResponse<IEnumerable<SupportExpertDispatchView>> GetSupportExpertDispaches()
        {
            GetGeneralResponse<IEnumerable<SupportExpertDispatchView>> response=new GetGeneralResponse<IEnumerable<SupportExpertDispatchView>>();

            try
            {
                IEnumerable<SupportExpertDispatch> supportExpertDispatches = _supportExpertDispatchRepository.FindAll();

                response.data = supportExpertDispatches.ConvertToSupportExpertDispatchViews();
                response.totalCount = supportExpertDispatches.Count();

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

        #region Read One

        public GetGeneralResponse<SupportExpertDispatchView> GetSupportExpertDispach(Guid SupportID)
        {
            GetGeneralResponse<SupportExpertDispatchView> response=new GetGeneralResponse<SupportExpertDispatchView>();
            try
            {
                Query query = new Query();
                Criterion SupportIDCriteria=new Criterion("Support.ID",SupportID,CriteriaOperator.Equal);
                query.Add(SupportIDCriteria);

                SupportExpertDispatch supportExpertDispatch =
                    _supportExpertDispatchRepository.FindBy(query).FirstOrDefault();

                response.data = supportExpertDispatch.ConvertToSupportExpertDispatchView();
                response.totalCount = 1;

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException!=null )
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;

        }

        #endregion

        #endregion

        #region Add

        public GeneralResponse AddSupportExpertDispatch(AddSupportExpertDispatchRequest request, Guid CreateemployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                SupportExpertDispatch supportExpertDispatch = new SupportExpertDispatch();

                supportExpertDispatch.ID = Guid.NewGuid();
                supportExpertDispatch.CreateDate = PersianDateTime.Now;
                supportExpertDispatch.CreateEmployee = _employeeRepository.FindBy(CreateemployeeID);
                supportExpertDispatch.Comment = request.Comment;
                supportExpertDispatch.DispatchDate = request.DispatchDate;
                supportExpertDispatch.DispatchTime = request.DispatchTime;
                supportExpertDispatch.ExpertEmployee = _employeeRepository.FindBy(request.ExpertEmployeeID);
                supportExpertDispatch.CoordinatorName = request.CoordinatorName;
                supportExpertDispatch.Support = _supportRepository.FindBy(request.SupportID);
                supportExpertDispatch.Comment = request.Comment;
                supportExpertDispatch.IsNewInstallation = request.IsNewInstallation;
                supportExpertDispatch.RowVersion = 1;

                
                SupportStatusRelation supportStatusRelation = _supportStatusRelationRepository.FindBy(request.SupportStatusID);
                supportExpertDispatch.Support.SupportStatus = _supportStatusRepository.FindBy(supportStatusRelation.RelatedSupportStatus.ID);
                supportExpertDispatch.Support.Customer.SupportStatus = supportExpertDispatch.Support.SupportStatus;

                if (supportExpertDispatch.Support.SupportStatus.IsLastSupportState)
                    supportExpertDispatch.Support.Closed = true;

                _supportExpertDispatchRepository.Add(supportExpertDispatch);
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

        public GeneralResponse EditSupportExpertDispatch(EditSupportExpertDispatchRequest request,
            Guid ModifiedEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportExpertDispatch supportExpertDispatch=new SupportExpertDispatch();

                supportExpertDispatch = _supportExpertDispatchRepository.FindBy(request.ID);
                supportExpertDispatch.Comment = request.Comment;
                supportExpertDispatch.CoordinatorName = request.CoordinatorName;
                supportExpertDispatch.DispatchDate = request.DispatchDate;
                supportExpertDispatch.DispatchTime = request.DispatchTime;
                supportExpertDispatch.ExpertEmployee = _employeeRepository.FindBy(request.ExpertEmployeeID);
                supportExpertDispatch.ModifiedDate = PersianDateTime.Now;
                supportExpertDispatch.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                supportExpertDispatch.IsNewInstallation = request.IsNewInstallation;

                #region Row Version Check

                if (supportExpertDispatch.RowVersion != request.RowVersion)
                {

                    response.ErrorMessages.Add("EditConcurrencyKey");
                    return response;
                }
                else
                {
                    supportExpertDispatch.RowVersion += 1;
                }

                if (supportExpertDispatch.GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in supportExpertDispatch.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }


                #endregion


                //supportExpertDispatch.Support.SupportStatus = _supportStatusRepository.FindBy(request.SupportStatusID);
                supportExpertDispatch.Support.Customer.SupportStatus = supportExpertDispatch.Support.SupportStatus;

                _supportExpertDispatchRepository.Save(supportExpertDispatch);
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

        #region Delete

        public GeneralResponse DeleteSupportExpertDispatch(DeleteRequest request)
        {
            GeneralResponse response=new GeneralResponse();
            try
            {
                SupportExpertDispatch supportExpertDispatch=new SupportExpertDispatch();

                supportExpertDispatch = _supportExpertDispatchRepository.FindBy(request.ID);

                _supportExpertDispatchRepository.Remove(supportExpertDispatch);
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException !=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Send SMS

        public void SendSmsVoid(object data)
        {
            ISmsWebService smsWebService = new ISmsWebService();
            SmsData smsData = (SmsData)data;
            smsWebService.SendSms(smsData.body, smsData.phoneNumber);
        }




        #endregion
    }
}
