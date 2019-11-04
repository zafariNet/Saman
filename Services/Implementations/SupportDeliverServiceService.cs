using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public class SupportDeliverServiceService : ISupportDeliverServiceService
    {
        #region Declares

        private readonly ISupportDeliverServiceRepository _supportDeliverServiceRepository;

        private readonly ICustomerRepository _customerRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly IUnitOfWork _uow;

        private readonly ISupportRepository _supportRepository;

        private readonly ISupportStatusRepository _supportStatusRepository;

        private readonly ISupportStatusRelationRepository _supportStatusRelationRepository;

        #endregion

        #region Ctor

        public SupportDeliverServiceService(ISupportDeliverServiceRepository supportDeliverServiceRepository,
            ICustomerRepository customerRepository, IEmployeeRepository employeeRepository,ISupportStatusRepository supportStatusRepository,
            IUnitOfWork uow, ISupportRepository supportRepository, ISupportStatusRelationRepository supportStatusRelationRepository
            )
        {
            _supportDeliverServiceRepository = supportDeliverServiceRepository;
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

        public GetGeneralResponse<IEnumerable<SupportDeliverServiceView>> GetSupportDeliverServices(int pageSize,
            int pageNumber,IList<FilterData> filter)
        {
            GetGeneralResponse<IEnumerable<SupportDeliverServiceView>> response=new GetGeneralResponse<IEnumerable<SupportDeliverServiceView>>();
            try
            {

                string Filter = FilterUtilityService.GenerateFilterHQLQuery(filter, "SupportDeliverService", null);
                Response<SupportDeliverService> supportDeliverService = _supportDeliverServiceRepository.FindAll(Filter);

                response.data = supportDeliverService.data.ConvertToSupportDeliverServiceViews();
                response.totalCount = supportDeliverService.totalCount;

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

        public GetGeneralResponse<SupportDeliverServiceView> GetSupportDeliverService(Guid SupportID)
        {
            GetGeneralResponse<SupportDeliverServiceView> response=new GetGeneralResponse<SupportDeliverServiceView>();

            try
            {
                Query query=new Query();
                Criterion supportIDCriteria=new Criterion("Support.ID",SupportID,CriteriaOperator.Equal);
                query.Add(supportIDCriteria);

                SupportDeliverService supportDeliverService = _supportDeliverServiceRepository.FindBy(query).FirstOrDefault();

                response.data = supportDeliverService.ConvertToSupportDeliverServiceView();
                response.totalCount = 1;
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

        public GeneralResponse AddSeupportDeliverService(AddSupportDeliverServiceRequest request, Guid CreateEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportDeliverService supportDeliverService=new SupportDeliverService();

                supportDeliverService.ID=Guid.NewGuid();
                supportDeliverService.DeliverDate = request.DeliverDate;
                supportDeliverService.TimeInput = request.TimeInput;
                supportDeliverService.TimeOutput = request.TimeOutput;
                supportDeliverService.AmountRecived = request.AmountRecived;
                supportDeliverService.Comment = request.Comment;
                supportDeliverService.CreateDate = PersianDateTime.Now;
                supportDeliverService.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                supportDeliverService.RowVersion = 1;
                supportDeliverService.Support = _supportRepository.FindBy(request.SupportID);

                #region چک کردن عدم وجود مورد ثبت شده

                if (supportDeliverService.Support.SupportDeliverService.Count()>0)
                {
                    response.ErrorMessages.Add("برای هر پشتیبانی بیش از یک تحویل سرویس نمیتوانید ثبت کنید");
                    return response;
                }

                #endregion

                SupportStatusRelation supportStatusRelation = _supportStatusRelationRepository.FindBy(request.SupportStatusID);
                supportDeliverService.Support.SupportStatus = _supportStatusRepository.FindBy(supportStatusRelation.RelatedSupportStatus.ID);
                supportDeliverService.Support.Customer.SupportStatus = supportDeliverService.Support.SupportStatus;

                if (supportDeliverService.Support.SupportStatus.IsLastSupportState)
                    supportDeliverService.Support.Closed = true;

                _supportDeliverServiceRepository.Add(supportDeliverService);

                #region Send SMS

                if (supportDeliverService.Support.SupportStatus.SendSmsOnEnter)
                {
                                        // Threading
                    SmsData smsData = new SmsData() { body = supportDeliverService.Support.SupportStatus.SmsText, phoneNumber = supportDeliverService.Support.Customer.Mobile1 };
                    Thread oThread = new Thread(SendSmsVoid);
                    oThread.Start(smsData);
                }

                #endregion

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

        public GeneralResponse EditSupportDeliverService(EditSupportDeliverServiceRequest request,
            Guid ModifiedEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();
            try
            {
                SupportDeliverService supportDeliverService=new SupportDeliverService();
                supportDeliverService = _supportDeliverServiceRepository.FindBy(request.ID);
                supportDeliverService.AmountRecived = request.AmountRecived;
                supportDeliverService.Comment = request.Comment;
                supportDeliverService.DeliverDate = request.DeliverDate;
                supportDeliverService.TimeInput = request.TimeInput;
                supportDeliverService.TimeOutput = request.TimeOutput;
                supportDeliverService.ModifiedDate = PersianDateTime.Now;
                supportDeliverService.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);

                #region Row Version Check

                if (supportDeliverService.RowVersion != request.RowVersion)
                {

                    response.ErrorMessages.Add("EditConcurrencyKey");
                    return response;
                }
                else
                {
                    supportDeliverService.RowVersion += 1;
                }

                if (supportDeliverService.GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in supportDeliverService.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }


                #endregion


                _supportDeliverServiceRepository.Save(supportDeliverService);
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

        #region delete

        public GeneralResponse DeleteSupportDeliverService(DeleteRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportDeliverService supportDeliverservice = new SupportDeliverService();

                supportDeliverservice = _supportDeliverServiceRepository.FindBy(request.ID);
                
                _supportDeliverServiceRepository.Remove(supportDeliverservice);
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
