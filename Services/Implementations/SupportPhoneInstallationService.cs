using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Infrastructure.Domain;
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
    public class SupportPhoneInstallationService : ISupportPhoneInstallationService
    {
        #region Declares

        private readonly ISupportPhoneInstallationRepository _supportPhoneInstallationRepository;

        private readonly ICustomerRepository _customerRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly IUnitOfWork _uow;

        private readonly ISupportRepository _supportRepository;

        private readonly ISupportStatusRepository _supportStatusRepository;

        private readonly ISupportStatusRelationRepository _supportStatusRelationRepository;

        #endregion

        #region Ctor

        public SupportPhoneInstallationService(ISupportPhoneInstallationRepository supportPhoneInstallationRepository,
            ICustomerRepository customerRepository, IEmployeeRepository employeeRepository,ISupportStatusRepository supportStatusRepository,
            IUnitOfWork uow, ISupportRepository supportRepository, ISupportStatusRelationRepository supportStatusRelationRepository
            )
        {
            _supportPhoneInstallationRepository = supportPhoneInstallationRepository;
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

        public GetGeneralResponse<IEnumerable<SupportPhoneInstallationView>> GetSupportPhoneInstallations()
        {
            GetGeneralResponse<IEnumerable<SupportPhoneInstallationView>> response=new GetGeneralResponse<IEnumerable<SupportPhoneInstallationView>>();

            try
            {
                IEnumerable<SupportPhoneInstallation> supportPhoneInstallations=_supportPhoneInstallationRepository.FindAll();

                response.data = supportPhoneInstallations.ConvertToSupportPhoneInstallationViews();
                response.totalCount = supportPhoneInstallations.Count();

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

        public GetGeneralResponse<SupportPhoneInstallationView> GetSupportPhoneInstallation(Guid SupportID)
        {
            GetGeneralResponse<SupportPhoneInstallationView> response=new GetGeneralResponse<SupportPhoneInstallationView>();

            try
            {
                Query query=new Query();
                Criterion SupportIDCriteria=new Criterion("Support.ID",SupportID,CriteriaOperator.Equal);
                query.Add(SupportIDCriteria);
                SupportPhoneInstallation supportPhonInstallation =
                    _supportPhoneInstallationRepository.FindBy(query).FirstOrDefault();

                response.data = supportPhonInstallation.ConvertToSupportPhoneInstallationView();
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

        public GeneralResponse AddSupportPhoneInstallation(AddSupportPhoneInstallationRequst request,
            Guid CreateemployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportPhoneInstallation supportPhoneInstallation=new SupportPhoneInstallation();
                supportPhoneInstallation.ID = Guid.NewGuid();
                supportPhoneInstallation.CreateDate = PersianDateTime.Now;
                supportPhoneInstallation.CreateEmployee = _employeeRepository.FindBy(CreateemployeeID);
                supportPhoneInstallation.Comment = request.Comment;
                supportPhoneInstallation.InstallDate = request.InstallDate;
                supportPhoneInstallation.Installed = request.Installed;
                supportPhoneInstallation.SendNotificationToCustomer = request.SendNotificationToCustomer;
                supportPhoneInstallation.Support = _supportRepository.FindBy(request.SupportID);
                supportPhoneInstallation.RowVersion = 1;

                
                SupportStatusRelation supportStatusRelation = _supportStatusRelationRepository.FindBy(request.SupportStatusID);
                supportPhoneInstallation.Support.SupportStatus = _supportStatusRepository.FindBy(supportStatusRelation.RelatedSupportStatus.ID);
                supportPhoneInstallation.Support.Customer.SupportStatus = supportPhoneInstallation.Support.SupportStatus;


                if (supportPhoneInstallation.Support.SupportStatus.IsLastSupportState)
                    supportPhoneInstallation.Support.Closed = true;

                _supportPhoneInstallationRepository.Add(supportPhoneInstallation);
                _uow.Commit();

                #region Send SMS

                if (supportPhoneInstallation.Support.SupportStatus.SendSmsOnEnter)
                {
                    // Threading
                    SmsData smsData = new SmsData() { body = supportPhoneInstallation.Support.SupportStatus.SmsText, phoneNumber = supportPhoneInstallation.Support.Customer.Mobile1 };
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

        public GeneralResponse EditSupportPhoneInstalltion(EditSupportPhoneInstallationRequst request,
            Guid ModifiedEployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportPhoneInstallation supportPhoneInstallation=new SupportPhoneInstallation();
                supportPhoneInstallation = _supportPhoneInstallationRepository.FindBy(request.ID);

                supportPhoneInstallation.Comment = request.Comment;
                supportPhoneInstallation.ModifiedDate = PersianDateTime.Now;
                supportPhoneInstallation.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEployeeID);
                supportPhoneInstallation.InstallDate = request.InstallDate;
                supportPhoneInstallation.Installed = request.Installed;
                supportPhoneInstallation.SendNotificationToCustomer = request.SendNotificationToCustomer;


                #region Row Version Check

                if (supportPhoneInstallation.RowVersion != request.RowVersion)
                {

                    response.ErrorMessages.Add("EditConcurrencyKey");
                    return response;
                }
                else
                {
                    supportPhoneInstallation.RowVersion += 1;
                }

                if (supportPhoneInstallation.GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in supportPhoneInstallation.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }


                #endregion

                _supportPhoneInstallationRepository.Save(supportPhoneInstallation);
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

        public GeneralResponse DeleteSupportPhonInstallation(DeleteRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportPhoneInstallation supportPhoneInstallation=new SupportPhoneInstallation();
                supportPhoneInstallation = _supportPhoneInstallationRepository.FindBy(request.ID);

                _supportPhoneInstallationRepository.Remove(supportPhoneInstallation);
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
