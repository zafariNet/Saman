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
using System.Configuration;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Net;
using Infrastructure.Querying;

#endregion

namespace Services.Implementations
{
    public class EmailService : IEmailService
    {
        #region Declares

        private readonly IEmailRepository _emailRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;

        #endregion

        #region ctor

        public EmailService(IEmailRepository emailRepository, IUnitOfWork uow)
        {
            _emailRepository = emailRepository;
            _uow = uow;
        }

        public EmailService(IEmailRepository emailRepository, ICustomerRepository customerRepository,
            IUnitOfWork uow, IEmployeeRepository employeeRepository)
            : this(emailRepository, uow)
        {
            this._customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
        }

        #endregion

        #region IEmailService Members

        #region Add

        public GeneralResponse AddEmail(AddEmailRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Email email = new Email();
                email.ID = Guid.NewGuid();
                email.CreateDate = PersianDateTime.Now;
                email.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                email.Body = request.Body;
                email.Customer = this._customerRepository.FindBy(request.CustomerID);
                email.Subject = request.Subject;

                email.RowVersion = 1;

                #region Validation

                if (email.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in email.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                _emailRepository.Add(email);
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

        public GeneralResponse EditEmail(EditEmailRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            Email email = new Email();
            email = _emailRepository.FindBy(request.ID);

            if (email != null)
            {
                try
                {
                    email.ModifiedDate = PersianDateTime.Now;
                    email.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if (request.Body != null)
                        email.Body = request.Body;
                    if (request.CustomerID != null)
                        email.Customer = this._customerRepository.FindBy(request.CustomerID);
                    email.Sent = request.Sent;
                    if (request.Subject != null)
                        email.Subject = request.Subject;
                    if (request.Note != null)
                        email.Note = request.Note;

                    if (email.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        email.RowVersion += 1;
                    }

                    if (email.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in email.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _emailRepository.Save(email);
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
                
                response.ErrorMessages.Add("NoItemToEditKey");
            }
            return response;
        }

        #endregion

        #region Delete

        public GeneralResponse DeleteEmail(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

 
                try
                {
                    _emailRepository.RemoveById(request.ID);
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

        public GetEmailResponse GetEmail(GetRequest request)
        {
            GetEmailResponse response = new GetEmailResponse();

            try
            {
                Email email = new Email();
                EmailView emailView = email.ConvertToEmailView();

                email = _emailRepository.FindBy(request.ID);
                if (email != null)
                    emailView = email.ConvertToEmailView();

                response.EmailView = emailView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        #endregion

        #region Get Some

        public GetEmailsResponse GetEmails()
        {
            GetEmailsResponse response = new GetEmailsResponse();

            try
            {
                IEnumerable<EmailView> emails = _emailRepository.FindAll()
                    .ConvertToEmailViews();

                response.EmailViews = emails;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetEmailsResponse GetEmails(AjaxGetRequest request,IList<Sort> sort)
        {
            GetEmailsResponse response = new GetEmailsResponse();

            int pageNumber = request.PageNumber == 0 ? 1 : request.PageNumber;
            int pageSize = request.PageSize == 0 ? 10 : request.PageSize;

            int index = (pageNumber - 1) * pageSize;
            int count = pageSize;

            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                Criterion criterion = new Criterion("Customer.ID", request.ID, CriteriaOperator.Equal);
                query.Add(criterion);

                Infrastructure.Domain.Response<Email> emailsResponse = _emailRepository.FindBy(query, index, count,sort);

                IEnumerable<EmailView> emails = emailsResponse.data.ConvertToEmailViews();

                response.EmailViews = emails;
                response.TotalCount = emailsResponse.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion

        #endregion

    }
}
