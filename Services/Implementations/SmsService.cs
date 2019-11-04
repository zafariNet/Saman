#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Customers.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Model.Customers;
using Services.ViewModels.Customers;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees.Interfaces;
using Infrastructure.Querying;
using Infrastructure.Domain;

#endregion

namespace Services.Implementations
{
    public class SmsService : ISmsService
    {
        #region Declares
        
        private readonly ISmsRepository _smsRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        
        #endregion

        #region Ctor
        
        public SmsService(ISmsRepository smsRepository, IUnitOfWork uow)
        {
            _smsRepository = smsRepository;
            _uow = uow;
        }
        public SmsService(ISmsRepository smsRepository, ICustomerRepository customerRepository,
            IUnitOfWork uow, IEmployeeRepository employeeRepository)
            : this(smsRepository, uow)
        {
            this._customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
        }
        
        #endregion

        #region Old Methods
        
        public GeneralResponse AddSms(AddSmsRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Sms sms = new Sms();
                sms.ID = Guid.NewGuid();
                sms.CreateDate = PersianDateTime.Now;
                sms.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                sms.Body = request.Body;
                sms.Customer = this._customerRepository.FindBy(request.CustomerID);
                sms.Note = request.Note;
                sms.RowVersion = 1;

                #region Validation
                
                if (sms.GetBrokenRules().Count() > 0)
                {


                    foreach (BusinessRule businessRule in sms.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                _smsRepository.Add(sms);
                _uow.Commit();

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public GeneralResponse EditSms(EditSmsRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            Sms sms = new Sms();
            sms = _smsRepository.FindBy(request.ID);

            if (sms != null)
            {
                try
                {
                    sms.ModifiedDate = PersianDateTime.Now;
                    sms.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if (request.Body != null)
                        sms.Body = request.Body;
                    if (request.CustomerID != null)
                        sms.Customer = this._customerRepository.FindBy(request.CustomerID);
                    if (request.Note != null)
                        sms.Note = request.Note;

                    if (sms.RowVersion != request.RowVersion)
                    {
                        
                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        sms.RowVersion += 1;
                    }

                    if (sms.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in sms.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _smsRepository.Save(sms);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                }
            }
            else
            {
                
                response.ErrorMessages.Add("هیچ موردی جهت ویرایش وجود ندارد.");
            }
            return response;
        }

        public GeneralResponse DeleteSms(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            Sms sms = new Sms();
            sms = _smsRepository.FindBy(request.ID);

            if (sms != null)
            {
                try
                {
                    _smsRepository.Remove(sms);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                }
            }

            return response;
        }

        public GetSmsResponse GetSms(GetRequest request)
        {
            GetSmsResponse response = new GetSmsResponse();

            try
            {
                Sms sms = new Sms();
                SmsView smsView = sms.ConvertToSmsView();

                sms = _smsRepository.FindBy(request.ID);
                if (sms != null)
                    smsView = sms.ConvertToSmsView();

                response.SmsView = smsView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetSmssResponse GetSmss()
        {
            GetSmssResponse response = new GetSmssResponse();

            try
            {
                IEnumerable<SmsView> smss = _smsRepository.FindAll()
                    .ConvertToSmsViews();

                response.SmsViews = smss;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        
        #endregion

        public GetGeneralResponse<IEnumerable<SmsView>> GetSmss(Guid customerID, int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<SmsView>> response = new GetGeneralResponse<IEnumerable<SmsView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("Customer.ID", customerID, CriteriaOperator.Equal);

                query.Add(criteria);

                Response<Sms> smss = _smsRepository.FindBy(query, index, count);

                response.data = smss.data.ConvertToSmsViews();
                response.totalCount = smss.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }


            return response;
        }

    }
}
