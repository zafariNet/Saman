using System;
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
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Implementations
{
    public class SupportTicketWaitingService : ISupportTicketWaitingService
    {
        #region Declares

        private readonly ISupportTicketWaitingRepository _supportTicketWaitingRepository;

        private readonly ICustomerRepository _customerRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly IUnitOfWork _uow;

        private readonly ISupportRepository _supportRepository;

        private readonly ISupportStatusRepository _supportStatusRepository;

        private readonly ISupportStatusRelationRepository _supportStatusRelationRepository;

        #endregion

        #region Ctor

        public SupportTicketWaitingService(ISupportTicketWaitingRepository supportTicketWaitingRepository,
            ICustomerRepository customerRepository, IEmployeeRepository employeeRepository,ISupportStatusRepository supportStatusRepository,
            IUnitOfWork uow, ISupportRepository supportRepository, ISupportStatusRelationRepository supportStatusRelationRepository
            )
        {
            _supportTicketWaitingRepository = supportTicketWaitingRepository;
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

        public GetGeneralResponse<IEnumerable<SupportTicketWaitingView>> GetSupportTicketWaitings()
        {
            GetGeneralResponse<IEnumerable<SupportTicketWaitingView>> response = new GetGeneralResponse<IEnumerable<SupportTicketWaitingView>>();

            try
            {
                IEnumerable<SupportTicketWaiting> supportTicketwaitings = _supportTicketWaitingRepository.FindAll();

                response.data = supportTicketwaitings.ConvertToSupportTicketWaitingViews();
                response.totalCount = supportTicketwaitings.Count();
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

        public GetGeneralResponse<SupportTicketWaitingView> GetSupportTicketWaiting(Guid SupportID)
        {
             GetGeneralResponse<SupportTicketWaitingView> response=new GetGeneralResponse<SupportTicketWaitingView>();

            try
            {
                Query query=new Query();
                Criterion SupportIDCriteria=new Criterion("Support.ID",SupportID,CriteriaOperator.Equal);
                query.Add(SupportIDCriteria);

                SupportTicketWaiting supportTicketWaiting=new SupportTicketWaiting();
                supportTicketWaiting = _supportTicketWaitingRepository.FindBy(query).FirstOrDefault();

                response.data = supportTicketWaiting.ConvertToSupportTicketWaitingView();
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

        public GeneralResponse AddSupportTicketWaiting(AddSupportTicketWaitingRequest request, Guid CreateEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportTicketWaiting supportTicketwaiting=new SupportTicketWaiting();

                supportTicketwaiting.ID = Guid.NewGuid();
                supportTicketwaiting.CreateDate = PersianDateTime.Now;
                supportTicketwaiting.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                supportTicketwaiting.Comment = request.Comment;
                supportTicketwaiting.DateOfPersenceDate = request.DateOfPersenceDate;
                supportTicketwaiting.InstallExpert = _employeeRepository.FindBy(request.InstallExpertID);
                supportTicketwaiting.Selt = request.Selt;
                supportTicketwaiting.SendNotificationToCustomer = request.SendNotificationToCustomer;
                supportTicketwaiting.Snr = request.Snr;
                supportTicketwaiting.SourceWireCheck = request.SourceWireCheck;
                supportTicketwaiting.Support = _supportRepository.FindBy(request.SupportID);
                supportTicketwaiting.TicketSubject = request.TicketSubject;
                supportTicketwaiting.WireColor = request.WireColor;
                supportTicketwaiting.RowVersion = 1;

                

                SupportStatusRelation supportStatusRelation = _supportStatusRelationRepository.FindBy(request.SupportStatusID);
                supportTicketwaiting.Support.SupportStatus = _supportStatusRepository.FindBy(supportStatusRelation.RelatedSupportStatus.ID);
                supportTicketwaiting.Support.Customer.SupportStatus = supportTicketwaiting.Support.SupportStatus;

                if (supportTicketwaiting.Support.SupportStatus.IsLastSupportState)
                    supportTicketwaiting.Support.Closed = true;

                _supportTicketWaitingRepository.Add(supportTicketwaiting);
                _uow.Commit();

                #region Send SMS

                if (supportTicketwaiting.Support.SupportStatus.SendSmsOnEnter)
                {
                    // Threading
                    SmsData smsData = new SmsData() { body = supportTicketwaiting.Support.SupportStatus.SmsText, phoneNumber = supportTicketwaiting.Support.Customer.Mobile1 };
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

        public GeneralResponse EditSupportTicketWaiting(EditSupportTicketWaitingRequest request, Guid ModifiedEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();
            try
            {
                SupportTicketWaiting supportTicketWaiting=new SupportTicketWaiting();

                supportTicketWaiting = _supportTicketWaitingRepository.FindBy(request.ID);

                supportTicketWaiting.Comment = request.Comment;
                supportTicketWaiting.DateOfPersenceDate = request.DateOfPersenceDate;
                supportTicketWaiting.InstallExpert = _employeeRepository.FindBy(request.InstallExpertID);
                supportTicketWaiting.ModifiedDate = PersianDateTime.Now;
                supportTicketWaiting.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                supportTicketWaiting.Selt = request.Selt;
                supportTicketWaiting.SendNotificationToCustomer = request.SendNotificationToCustomer;
                supportTicketWaiting.Snr = request.Snr;
                supportTicketWaiting.SourceWireCheck = request.SourceWireCheck;
                supportTicketWaiting.TicketSubject = request.TicketSubject;
                supportTicketWaiting.WireColor = request.WireColor;

                #region Row Version Check

                if (supportTicketWaiting.RowVersion != request.RowVersion)
                {

                    response.ErrorMessages.Add("EditConcurrencyKey");
                    return response;
                }
                else
                {
                    supportTicketWaiting.RowVersion += 1;
                }

                if (supportTicketWaiting.GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in supportTicketWaiting.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }


                #endregion

                //SupportStatusRelation supportStatusRelation = _supportStatusRelationRepository.FindBy(request.SupportStatusID);
                //supportTicketWaiting.Support.SupportStatus = _supportStatusRepository.FindBy(supportStatusRelation.RelatedSupportStatus.ID);
                //supportTicketWaiting.Support.Customer.SupportStatus = supportTicketWaiting.Support.SupportStatus;


                _supportTicketWaitingRepository.Save(supportTicketWaiting);
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

        public GeneralResponse DeleteSupportTicketWaiting(DeleteRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportTicketWaiting supportTicketWaiting=new SupportTicketWaiting();

                supportTicketWaiting = _supportTicketWaitingRepository.FindBy(request.ID);

                _supportTicketWaitingRepository.Remove(supportTicketWaiting);
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
