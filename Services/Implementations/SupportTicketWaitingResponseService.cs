using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using Services.Messaging.StoreCatalogService;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Implementations
{
    public class SupportTicketWaitingResponseService : ISupportTicketWaitingResponseService
    {

        #region Declares

        private readonly ISupportTicketWaitingResponseRepository _supportTicketWitingResponseRepository;

        private readonly ICustomerRepository _customerRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly IUnitOfWork _uow;

        private readonly ISupportRepository _supportRepository;

        private readonly ISupportStatusRepository _supportStatusRepository;

        private readonly ISupportStatusRelationRepository _supportStatusRelationRepository;

        #endregion

        #region Ctor

        public SupportTicketWaitingResponseService(ISupportTicketWaitingResponseRepository supportTicketWitingResponseRepository,
            ICustomerRepository customerRepository, IEmployeeRepository employeeRepository,ISupportStatusRepository supportStatusRepository,
            IUnitOfWork uow, ISupportRepository supportRepository, ISupportStatusRelationRepository supportStatusRelationRepository
            )
        {
            _supportTicketWitingResponseRepository = supportTicketWitingResponseRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _uow = uow;
            _supportRepository = supportRepository;
            _supportStatusRepository = supportStatusRepository;
            _supportStatusRelationRepository = supportStatusRelationRepository;
        }

        #endregion

        #region Read

        #region All

        public GetGeneralResponse<IEnumerable<SupportTicketWaitingResponseView>> GetSpportTicketWaitingsRespone()
        {
            GetGeneralResponse<IEnumerable<SupportTicketWaitingResponseView>> response=new GetGeneralResponse<IEnumerable<SupportTicketWaitingResponseView>>();

            try
            {
                IEnumerable<SupportTicketWaitingResponse> SupportTicketWaitingResponses =
                    _supportTicketWitingResponseRepository.FindAll();

                response.data = SupportTicketWaitingResponses.ConvertToSupportTicketWaitingResponseViews();
                response.totalCount = SupportTicketWaitingResponses.Count();

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

        #region One

        public GetGeneralResponse<SupportTicketWaitingResponseView> GetSpportTicketWaitingRespone(Guid SupportID)
        {
            GetGeneralResponse<SupportTicketWaitingResponseView> response=new GetGeneralResponse<SupportTicketWaitingResponseView>();

            try
            {
                Query query=new Query();
                Criterion SupportIDCriteria=new Criterion("Support.ID",SupportID,CriteriaOperator.Equal);
                query.Add(SupportIDCriteria);

                SupportTicketWaitingResponse supportTicketWaitingResponse =
                    _supportTicketWitingResponseRepository.FindBy(query).FirstOrDefault();

                response.data = supportTicketWaitingResponse.ConvertToSupportTicketWaitingResponseView();
                response.totalCount = 1;
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

        #region Add

        public GeneralResponse AddSpportTicketWaitingRespone(AddSupportTicketWaitingResponseRequest request,
            Guid CreateemployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportTicketWaitingResponse supportTicketWaitingResponse=new SupportTicketWaitingResponse();

                supportTicketWaitingResponse.ID = Guid.NewGuid();
                supportTicketWaitingResponse.Comment = request.Comment;
                supportTicketWaitingResponse.CreateDate = PersianDateTime.Now;
                supportTicketWaitingResponse.CreateEmployee = _employeeRepository.FindBy(CreateemployeeID);
                supportTicketWaitingResponse.ResponsePossibilityDate = request.ResponsePossibilityDate;
                supportTicketWaitingResponse.SendNotificationToCustomer = request.SendNotificationToCustomer;
                supportTicketWaitingResponse.SendTicketDate = request.SendTicketDate;
                supportTicketWaitingResponse.Support = _supportRepository.FindBy(request.SupportID);
                supportTicketWaitingResponse.TicketNumber = request.TicketNumber;
                supportTicketWaitingResponse.RowVersion = 1;

                

                SupportStatusRelation supportStatusRelation = _supportStatusRelationRepository.FindBy(request.SupportStatusID);
                supportTicketWaitingResponse.Support.SupportStatus = _supportStatusRepository.FindBy(supportStatusRelation.RelatedSupportStatus.ID);
                supportTicketWaitingResponse.Support.Customer.SupportStatus = supportTicketWaitingResponse.Support.SupportStatus;

                if (supportTicketWaitingResponse.Support.SupportStatus.IsLastSupportState)
                    supportTicketWaitingResponse.Support.Closed = true;

                _supportTicketWitingResponseRepository.Add(supportTicketWaitingResponse);
                _uow.Commit();

                #region Send SMS

                if (supportTicketWaitingResponse.Support.SupportStatus.SendSmsOnEnter)
                {
                    // Threading
                    SmsData smsData = new SmsData() { body = supportTicketWaitingResponse.Support.SupportStatus.SmsText, phoneNumber = supportTicketWaitingResponse.Support.Customer.Mobile1 };
                    Thread oThread = new Thread(SendSmsVoid);
                    oThread.Start(smsData);
                }

                #endregion
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

        public GeneralResponse EditSpportTicketWaitingRespone(EditSupportTicketWaitingResponseRequest request,
                Guid ModifiedEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportTicketWaitingResponse supportTicketWaitingResponse=new SupportTicketWaitingResponse();
                supportTicketWaitingResponse = _supportTicketWitingResponseRepository.FindBy(request.ID);

                supportTicketWaitingResponse.Comment = request.Comment;
                supportTicketWaitingResponse.ModifiedDate = PersianDateTime.Now;
                supportTicketWaitingResponse.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                supportTicketWaitingResponse.ResponsePossibilityDate = request.ResponsePossibilityDate;
                supportTicketWaitingResponse.SendNotificationToCustomer = request.SendNotificationToCustomer;

                #region Row Version Check

                if (supportTicketWaitingResponse.RowVersion != request.RowVersion)
                {

                    response.ErrorMessages.Add("EditConcurrencyKey");
                    return response;
                }
                else
                {
                    supportTicketWaitingResponse.RowVersion += 1;
                }

                if (supportTicketWaitingResponse.GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in supportTicketWaitingResponse.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }


                #endregion

                //supportTicketWaitingResponse.Support.SupportStatus = _supportStatusRepository.FindBy(request.SupportStatusID);
                //supportTicketWaitingResponse.Support.Customer.SupportStatus = supportTicketWaitingResponse.Support.SupportStatus;

                _supportTicketWitingResponseRepository.Save(supportTicketWaitingResponse);
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

        public GeneralResponse DeleteSupportTicketWaitingResponse(DeleteRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportTicketWaitingResponse supportTicketWaitingResponse=new SupportTicketWaitingResponse();
                supportTicketWaitingResponse = _supportTicketWitingResponseRepository.FindBy(request.ID);

                _supportTicketWitingResponseRepository.Remove(supportTicketWaitingResponse);
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
