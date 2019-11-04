using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Base;
using Model.Employees.Interfaces;
using Model.Support;
using Model.Support.Interfaces;
using Services.Interfaces;
using Model.Customers.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Implementations
{
    public class SupportQcService : ISupportQcService
    {
        #region Declares

        private readonly ISupportQcRepository _supportQcRepository;

        private readonly ICustomerRepository _customerRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly IUnitOfWork _uow;

        private readonly ISupportRepository _supportRepository;

        private readonly ISupportStatusRepository _supportStatusRepository;

        private readonly ISupportStatusRelationRepository _supportStatusRelationRepository;

        #endregion

        #region Ctor

        public SupportQcService(ISupportQcRepository supportQcRepository,
            ICustomerRepository customerRepository, IEmployeeRepository employeeRepository,ISupportStatusRepository supportStatusRepository,
            IUnitOfWork uow, ISupportRepository supportRepository, ISupportStatusRelationRepository supportStatusRelationRepository
            )
        {
            _supportQcRepository = supportQcRepository;
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

        public GetGeneralResponse<IEnumerable<SupportQcView>> GetSupportQcs()
        {
            GetGeneralResponse<IEnumerable<SupportQcView>> response=new GetGeneralResponse<IEnumerable<SupportQcView>>();

            try
            {
                IEnumerable<SupportQc> supportQcViews = _supportQcRepository.FindAll();

                response.data = supportQcViews.ConvertToSupportQcViews();
                response.totalCount = supportQcViews.Count();
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

        public GetGeneralResponse<SupportQcView> GetSupportQc(Guid SupportID)
        {
            GetGeneralResponse<SupportQcView> response=new GetGeneralResponse<SupportQcView>();

            try
            {
                Query query=new Query();
                Criterion SupportIDCriteria=new Criterion("Support.ID",SupportID,CriteriaOperator.Equal);
                query.Add(SupportIDCriteria);

                SupportQc supportQc = _supportQcRepository.FindBy(query).FirstOrDefault();

                response.data = supportQc.ConvertToSupportQcView();
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

        public GeneralResponse AddSupportQc(AddSupportQcRequest request, Guid CreateEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportQc supportQc=new SupportQc();

                supportQc.ID = Guid.NewGuid();
                supportQc.CreateDate = PersianDateTime.Now;
                supportQc.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                supportQc.Comment = request.Comment;
                supportQc.ExpertBehavior =(SupportQc.State) request.ExpertBehavior;
                supportQc.ExpertCover = (SupportQc.State) request.ExpertCover;
                supportQc.InputTime = request.InputTime;
                if(string.IsNullOrEmpty(request.Comment))
                {
                    response.ErrorMessages.Add("افزودن توضیحات به فرم الزامیست.");
                    return response;
                }
                supportQc.OutputTime = request.OutputTime;
                supportQc.RecivedCost = request.RecivedCost;
                supportQc.SaleAndService = (SupportQc.State) request.SaleAndService;
                supportQc.SendNotificationToCustomer = request.SendNotificationToCustomer;
                supportQc.SendNotificationToMaster = request.SendNotificationToMaster;
                supportQc.RowVersion = 1;
                
                supportQc.Support = _supportRepository.FindBy(request.SupportID);


                #region چک کردن عدم وجود مورد ثبت شده

                if (supportQc.Support.SupportQc.Count()>0)
                {
                    response.ErrorMessages.Add("برای هر پشتیبانی بیش از یک فرم QC نمیتوانید ثبت کنید");
                    return response;
                }

                #endregion

                SupportStatusRelation supportStatusRelation = _supportStatusRelationRepository.FindBy(request.SupportStatusID);
                supportQc.Support.SupportStatus = _supportStatusRepository.FindBy(supportStatusRelation.RelatedSupportStatus.ID);
                supportQc.Support.Customer.SupportStatus = supportQc.Support.SupportStatus;

                if (supportQc.Support.SupportStatus.IsLastSupportState)
                    supportQc.Support.Closed = true;

                _supportQcRepository.Add(supportQc);
                _uow.Commit();

                #region Send SMS

                if (supportQc.Support.SupportStatus.SendSmsOnEnter)
                {
                    // Threading
                    SmsData smsData = new SmsData() { body = supportQc.Support.SupportStatus.SmsText, phoneNumber = supportQc.Support.Customer.Mobile1 };
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

        public GeneralResponse EditSupportQc(EditSupportQcRequest request, Guid ModifiedEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportQc supportQc=new SupportQc();
                supportQc = _supportQcRepository.FindBy(request.ID);

                supportQc.ModifiedDate = PersianDateTime.Now;
                supportQc.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                supportQc.Comment = request.Comment;
                supportQc.ExpertBehavior = (SupportQc.State)request.ExpertBehavior;
                supportQc.ExpertCover = (SupportQc.State)request.ExpertCover;
                supportQc.InputTime = request.InputTime;
                
                supportQc.OutputTime = request.OutputTime;
                supportQc.RecivedCost = request.RecivedCost;
                supportQc.SaleAndService = (SupportQc.State)request.SaleAndService;
                supportQc.SendNotificationToCustomer = request.SendNotificationToCustomer;
                supportQc.SendNotificationToMaster = request.SendNotificationToMaster;

                #region Row Version Check

                if (supportQc.RowVersion != request.RowVersion)
                {

                    response.ErrorMessages.Add("EditConcurrencyKey");
                    return response;
                }
                else
                {
                    supportQc.RowVersion += 1;
                }

                if (supportQc.GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in supportQc.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }


                #endregion


                //supportQc.Support.SupportStatus = _supportStatusRepository.FindBy(request.SupportStatusID);
                //supportQc.Support.Customer.SupportStatus = supportQc.Support.SupportStatus;

                _supportQcRepository.Save(supportQc);
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

        public GeneralResponse DeleteSupportQc(DeleteRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportQc supportQc=new SupportQc();
                supportQc = _supportQcRepository.FindBy(request.ID);

                _supportQcRepository.Remove(supportQc);
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
