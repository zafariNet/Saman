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
using NHibernate.Criterion;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Implementations
{
    public class SupportStatusService : ISupportStatusService
    {
        #region Declare

        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISupportStatusRepository _supportStatusRepository;
        private readonly ISupportStatusRelationRepository _supportStatusRelationRepository;
        private readonly IUnitOfWork _uow;

        #endregion

        #region Ctor

        public SupportStatusService(IEmployeeRepository employeeRepository,
            ISupportStatusRepository supportStatusRepository,
            ISupportStatusRelationRepository supportStatusRelationRepository, IUnitOfWork uow)
        {
            this._employeeRepository = employeeRepository;
            this._supportStatusRepository = supportStatusRepository;
            this._supportStatusRelationRepository = supportStatusRelationRepository;
            this._uow = uow;
        }

        #endregion

        #region Read

        #region All
        public GetGeneralResponse<IEnumerable<SupportStatusView>> GetAllSupportStatuses(int pageSize, int pageNumber)
        {
           GetGeneralResponse<IEnumerable<SupportStatusView>> response=new GetGeneralResponse<IEnumerable<SupportStatusView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<SupportStatus> supportStatuses = _supportStatusRepository.FindAll(index, count);

                response.data = supportStatuses.data.ConvertToSupportStatusViews();
                response.totalCount = supportStatuses.totalCount;

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

        public GetGeneralResponse<SupportStatusView> GetAllSupportStatus(Guid supportStatusID)
        {
            GetGeneralResponse<SupportStatusView> response=new GetGeneralResponse<SupportStatusView>();

            try
            {

                SupportStatus supportStatus = _supportStatusRepository.FindBy(supportStatusID);

                response.data = supportStatus.ConvertToSupportStatusView();
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

        #region First Support Statuses

        public GetGeneralResponse<IEnumerable<SupportStatusView>> GetFirstSupportStatuses()
        {
            GetGeneralResponse<IEnumerable<SupportStatusView>> response=new GetGeneralResponse<IEnumerable<SupportStatusView>>();

            try
            {
                Query query=new Query();
                Criterion firstStatusesCriteria = new Criterion("IsFirstSupportStatus",true,CriteriaOperator.Equal);
                query.Add(firstStatusesCriteria);

                Response<SupportStatus> supportStatusViews = _supportStatusRepository.FindByQuery(query);

                response.data = supportStatusViews.data.ConvertToSupportStatusViews();
                response.totalCount = supportStatusViews.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }


        #endregion

        #endregion

        #region Add

        public GeneralResponse AddSupportStatus(AddSupportStatusRequest request, Guid CreateEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();
            try
            {
                SupportStatus supportStatus=new SupportStatus();

                supportStatus.ID = Guid.NewGuid();
                supportStatus.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                supportStatus.CreateDate = PersianDateTime.Now;
                supportStatus.IsFirstSupportStatus = request.IsFirstSupportStatus;
                supportStatus.IsLastSupportState = request.IsLastSupportState;
                supportStatus.SendEmailOnEnter = request.SendEmailOnEnter;
                supportStatus.EmailText = request.EmailText;
                supportStatus.SendSmsOnEnter = request.SendSmsOnEnter;
                supportStatus.SmsText = request.SmsText;
                supportStatus.SupportStatusName = request.SupportStatusName;

                _supportStatusRepository.Add(supportStatus);
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

        public GeneralResponse EditSupportStatus(IEnumerable<EditSupportStatusRequest> requests, Guid ModifiedEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();
            try
            {
                foreach (var request in requests)
                {


                    SupportStatus supportStatus = _supportStatusRepository.FindBy(request.ID);
                    supportStatus.CreateEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                    supportStatus.CreateDate = PersianDateTime.Now;
                    supportStatus.IsFirstSupportStatus = request.IsFirstSupportStatus;
                    supportStatus.IsLastSupportState = request.IsLastSupportState;
                    supportStatus.SendEmailOnEnter = request.SendEmailOnEnter;
                    supportStatus.EmailText = request.EmailText;
                    supportStatus.SendSmsOnEnter = request.SendSmsOnEnter;
                    supportStatus.SmsText = request.SmsText;
                    supportStatus.SupportStatusName = request.SupportStatusName;

                    #region RowVersion - Validation

                    if (supportStatus.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        supportStatus.RowVersion += 1;
                    }

                    if (supportStatus.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in supportStatus.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _supportStatusRepository.Save(supportStatus);
                    _uow.Commit();

                    #endregion
                    _supportStatusRepository.Save(supportStatus);
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

        public GeneralResponse DeleteSupportStatuses(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (DeleteRequest deleteRequest in requests)
                {
                    SupportStatus supportStatus = _supportStatusRepository.FindBy(deleteRequest.ID);
                    _supportStatusRepository.Remove(supportStatus);
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

        #region Edit Sms

        public GeneralResponse EditSms(Guid SupportStatusID, string message, Guid ModifiedEployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                SupportStatus supportStatus = _supportStatusRepository.FindBy(SupportStatusID);

                supportStatus.SmsText = message;
                supportStatus.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEployeeID);

                _supportStatusRepository.Save(supportStatus);
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        #endregion

        #region edit Email

        public GeneralResponse EditEmail(Guid SupportStatusID, string message, Guid ModifiedEployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                SupportStatus supportStatus = _supportStatusRepository.FindBy(SupportStatusID);

                supportStatus.EmailText = message;
                supportStatus.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEployeeID);

                _supportStatusRepository.Save(supportStatus);
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        #endregion
    }
}
