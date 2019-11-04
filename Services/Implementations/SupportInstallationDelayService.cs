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
using NHibernate.Properties;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Implementations
{
    public class SupportInstallationDelayService : ISupportInstallationDelayService
    {
        #region Declares

        private readonly ISupportInstallationDelayRepository _supportInstallationDelayRepository;

        private readonly ICustomerRepository _customerRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly IUnitOfWork _uow;

        private readonly ISupportRepository _supportRepository;

        private readonly ISupportStatusRepository _supportStatusRepository;

        private readonly ISupportStatusRelationRepository _supportStatusRelationRepository;

        #endregion

        #region Ctor

        public SupportInstallationDelayService(ISupportInstallationDelayRepository supportInstallationDelayRepository,
            ICustomerRepository customerRepository, IEmployeeRepository employeeRepository,ISupportStatusRepository supportStatusRepository,
            IUnitOfWork uow, ISupportRepository supportRepository, ISupportStatusRelationRepository supportStatusRelationRepository
            )
        {
            _supportInstallationDelayRepository = supportInstallationDelayRepository;
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

        public GetGeneralResponse<IEnumerable<SupportInstallationDelayView>> GetSupportInstallationDelays()
        {
            GetGeneralResponse<IEnumerable<SupportInstallationDelayView>> response=new GetGeneralResponse<IEnumerable<SupportInstallationDelayView>>();

            try
            {
                IEnumerable<SupportInstallationDelay> supportInstallationDelays =
                    _supportInstallationDelayRepository.FindAll();

                response.data = supportInstallationDelays.ConvertToSupportInstallationDelayViews();
                response.totalCount = supportInstallationDelays.Count();
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

        #region One

        public GetGeneralResponse<SupportInstallationDelayView> GetSupportInstallationDelay(Guid SupportID)
        {
            GetGeneralResponse<SupportInstallationDelayView> response=new GetGeneralResponse<SupportInstallationDelayView>();

            try
            {
                Query query=new Query();
                Criterion supportIDCriteria=new Criterion("Support.ID",SupportID,CriteriaOperator.Equal);
                query.Add(supportIDCriteria);

                SupportInstallationDelay supportInstallationDelayView =
                    _supportInstallationDelayRepository.FindBy(query).FirstOrDefault();

                response.data = supportInstallationDelayView.ConvertToSupportInstallationDelayView();
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

        #region Add

        public GeneralResponse AddSupportInstallationDelay(AddSupportInstallationDelayRequest request,
            Guid CreateEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportInstallationDelay supportInstallationDelay=new SupportInstallationDelay();

                supportInstallationDelay.ID = Guid.NewGuid();
                supportInstallationDelay.Comment = request.Comment;
                supportInstallationDelay.CreateDate = PersianDateTime.Now;
                supportInstallationDelay.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                supportInstallationDelay.InstallDate = request.InstallDate;
                supportInstallationDelay.NextCallDate = request.NextCallDate;
                supportInstallationDelay.RowVersion = 1;
                supportInstallationDelay.SendNotificationToCustomer = request.SendNotificationToCustomer;
                supportInstallationDelay.Support = _supportRepository.FindBy(request.SupportID);
                supportInstallationDelay.RowVersion = 1;



                SupportStatusRelation supportStatusRelation = _supportStatusRelationRepository.FindBy(request.SupportStatusID);
                supportInstallationDelay.Support.SupportStatus = _supportStatusRepository.FindBy(supportStatusRelation.RelatedSupportStatus.ID);
                supportInstallationDelay.Support.Customer.SupportStatus = supportInstallationDelay.Support.SupportStatus;

                _supportInstallationDelayRepository.Add(supportInstallationDelay);
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

        public GeneralResponse EditSpportInstallationDelay(EditSupportInstallationDelayRequest request,Guid ModifiedEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportInstallationDelay supportInstallationDelay=new SupportInstallationDelay();

                supportInstallationDelay = _supportInstallationDelayRepository.FindBy(request.ID);

                supportInstallationDelay.ModifiedDate = PersianDateTime.Now;
                supportInstallationDelay.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                supportInstallationDelay.Comment = request.Comment;
                supportInstallationDelay.InstallDate = request.InstallDate;
                supportInstallationDelay.NextCallDate = request.NextCallDate;
                supportInstallationDelay.SendNotificationToCustomer = request.SendNotificationToCustomer;

                #region Row Version Check

                if (supportInstallationDelay.RowVersion != request.RowVersion)
                {

                    response.ErrorMessages.Add("EditConcurrencyKey");
                    return response;
                }
                else
                {
                    supportInstallationDelay.RowVersion += 1;
                }

                if (supportInstallationDelay.GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in supportInstallationDelay.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }


                #endregion

                //SupportStatusRelation supportStatusRelation = _supportStatusRelationRepository.FindBy(request.SupportStatusID);
                //supportInstallationDelay.Support.SupportStatus = _supportStatusRepository.FindBy(supportStatusRelation.RelatedSupportStatus.ID);
                //supportInstallationDelay.Support.Customer.SupportStatus = supportInstallationDelay.Support.SupportStatus;

                _supportInstallationDelayRepository.Save(supportInstallationDelay);
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

        public GeneralResponse DeleteSupportInstallationDelay(DeleteRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportInstallationDelay supportInstallationDelay=new SupportInstallationDelay();

                supportInstallationDelay = _supportInstallationDelayRepository.FindBy(request.ID);

                _supportInstallationDelayRepository.Remove(supportInstallationDelay);
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
