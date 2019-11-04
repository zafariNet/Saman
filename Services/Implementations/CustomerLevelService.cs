#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Support;
using Model.Support.Interfaces;
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
using System.Net;
using System.Collections.Specialized;
using System.Collections;
using Infrastructure.Querying;
using Model.Sales;
using System.Threading;
using Model.Sales.Interfaces;

#endregion

namespace Services.Implementations
{
    public class CustomerLevelService : ICustomerLevelService
    {
        #region Declares
        private readonly ICustomerLevelRepository _customerLevelRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILevelRepository _levelRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILevelConditionService _levelConditionService;
        private readonly IEmailRepository _emailRepository;
        private readonly ISmsRepository _smsRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IQueryRepository _queryRepository;
        private readonly ISupportRepository _supportRepository;
        private readonly ISupportStatusRepository _supportStatusRepository;
        #endregion

        #region Ctor
        public CustomerLevelService(ICustomerLevelRepository customerLevelRepository, ICustomerRepository customerRepository, ILevelRepository levelRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository, ILevelConditionService levelConditionService,
            IEmailRepository emailRepository, ISmsRepository smsRepository, ISaleRepository saleRepository ,IQueryRepository queryRepository,
            ISupportRepository supportRepository, ISupportStatusRepository supportStatusRepository)
        {
            _customerLevelRepository = customerLevelRepository;
            _uow = uow;
            this._customerRepository = customerRepository;
            this._levelRepository = levelRepository;
            _employeeRepository = employeeRepository;
            _levelConditionService = levelConditionService;
            _emailRepository = emailRepository;
            _smsRepository = smsRepository;
            _saleRepository = saleRepository;
            _queryRepository = queryRepository;
            _supportRepository = supportRepository;
            _supportStatusRepository = supportStatusRepository;
        }
        #endregion

        #region Add (Change Level)

        Email email = new Email();

        public GeneralResponse PrepareToAddCustomerLevel(AddCustomerLevelRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                CustomerLevel customerLevel = new CustomerLevel();
                customerLevel.ID = Guid.NewGuid();
                customerLevel.CreateDate = PersianDateTime.Now;
                customerLevel.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                customerLevel.Customer = this._customerRepository.FindBy(request.CustomerID);
                customerLevel.Level = this._levelRepository.FindBy(request.NewLevelID);
                customerLevel.Note = request.Note;


                customerLevel.RowVersion = 1;
                
                #region Validation

                if (customerLevel.GetBrokenRules().Count() > 0)
                {


                    foreach (BusinessRule businessRule in customerLevel.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                #region Check Conditions

                if (customerLevel.Customer.Center == null)
                {

                    response.ErrorMessages.Add("هیچ گونه مرکز مخابراتی برای مشتری مورد نظر تعریف نشده است. لطفاً با مراجعه به تنظیمات، مرکز مخابراتی مربوط به پیش شماره مشتری را تعریف کنید.");

                    return response;
                }

                CheckConditionResponse cres = CheckLevelCondition(customerLevel.Level, customerLevel.Customer);
                if (!cres.CanEnter)
                {

                    foreach (string error in cres.ErrorMessages)
                        response.ErrorMessages.Add(error);

                    return response;
                }

                #endregion

                #region Change Customer Query Count

                #endregion

                #region CreateSupport

                if (customerLevel.Level.CreateSupportOnEnter)
                {
                    Support support=new Support();
                    support.ID = Guid.NewGuid();
                    support.CreateDate = PersianDateTime.Now;
                    support.CreateEmployee = customerLevel.CreateEmployee;
                    support.SupportTitle = "پشتیبانی ";
                    support.SupportComment = "پشتیبانی ایجاد شده خودکار توسط سیستم";
                    support.Customer = customerLevel.Customer;
                    support.CreateBy = Support.Creator.BySystem;
                    support.SupportStatus =
                        _supportStatusRepository.FindAll().Where(x => x.Key == "NoStatus").FirstOrDefault();
                    _supportRepository.Add(support);
                }

                #endregion

                _customerLevelRepository.Add(customerLevel);

                #region Query Customer Count

                //اگر مشتری جدید بود فقط به مرحله جدید یک واحد اضافه کن
                if (request.NewCustomer == true)
                {
                    Infrastructure.Querying.Query newQuery = new Infrastructure.Querying.Query();
                    Criterion crt1 = new Criterion("Level.ID", request.NewLevelID, CriteriaOperator.Equal);
                    newQuery.Add(crt1);
                    Model.Customers.Query OldQuery = _queryRepository.FindBy(newQuery).FirstOrDefault();
                    if (OldQuery != null)
                    {
                        OldQuery.CustomerCount += 1;
                        _queryRepository.Save(OldQuery);
                    }
                }
                    // اگر مشتری قبلی بود از مرحله قبل یکی کم و به مرحله جدید یکی اضافه کنذ
                else
                {
                    Infrastructure.Querying.Query oldQuery = new Infrastructure.Querying.Query();
                    Criterion crt1 = new Criterion("Level.ID", customerLevel.Customer.Level.ID, CriteriaOperator.Equal);
                    oldQuery.Add(crt1);
                    Model.Customers.Query OldQuery = _queryRepository.FindBy(oldQuery).FirstOrDefault();
                    OldQuery.CustomerCount -= 1;
                    _queryRepository.Save(OldQuery);

                    Infrastructure.Querying.Query newQuery = new Infrastructure.Querying.Query();
                    Criterion crt2 = new Criterion("Level.ID", request.NewLevelID, CriteriaOperator.Equal);
                    newQuery.Add(crt2);
                    Model.Customers.Query NewQuery = _queryRepository.FindBy(newQuery).FirstOrDefault();
                    NewQuery.CustomerCount += 1;
                    _queryRepository.Save(NewQuery);
                }

                #endregion

                #region Change Customer Level In Customer Table

                Customer customer = _customerRepository.FindBy(request.CustomerID);
                customer.Level = _levelRepository.FindBy(request.NewLevelID);
                customer.LevelEntryDate = PersianDateTime.Now;
                _customerRepository.Save(customer);

                
                #endregion

                _uow.Commit();

                #region Sending Email

                string displayName = "ماهان نت";
                string subject;
                //List<string> recipients = new List<string>();
                GeneralResponse sendResponse = new GeneralResponse();


                // if OnEnterSendEmail is true
                if (customerLevel.Level.OnEnterSendEmail)
                {
                    email.ID = Guid.NewGuid();
                    email.CreateDate = PersianDateTime.Now;
                    email.CreateEmployee = customerLevel.CreateEmployee;
                    email.Customer = customerLevel.Customer;
                    subject = customer.Name;
                    email.Subject = subject;
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

                    #region Send Email

                    // Replacing:
                    string emailBody = ReplaceTemplate(customerLevel.Level.EmailText, customerLevel.Customer.ConvertToCustomerView());

                    email.Body = emailBody;

                    string recipient = email.Customer.Email;
                    if (recipient == null || recipient == string.Empty)
                    {
                        response.ErrorMessages.Add("برای مشتری مورد نظر هیچ ایمیلی در سیستم تعریف نشده است.");

                        return response;
                    }

                    //===============  Threading:
                    EmailData emailData = new EmailData()
                    {
                        displayName = displayName,
                        body = emailBody,
                        subject = subject,
                        recipient = recipient
                    };

                    Thread oThread = new Thread(SendEmailVoid);
                    oThread.Start(emailData);

                    #endregion

                    _emailRepository.Add(email);
                }

                #endregion

                #region Sending Sms

                if (customerLevel.Level.OnEnterSendSMS)
                {
                    Sms sms = new Sms();
                    sms.ID = Guid.NewGuid();
                    sms.CreateDate = PersianDateTime.Now;
                    sms.CreateEmployee = customerLevel.CreateEmployee;
                    //sms.Body = customerLevel.Level.SMSText;
                    sms.Customer = customerLevel.Customer;
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

                    string smsBody = ReplaceTemplate(customerLevel.Level.SMSText, customerLevel.Customer.ConvertToCustomerView());

                    // Threading
                    SmsData smsData = new SmsData() { body = smsBody, phoneNumber = customerLevel.Customer.Mobile1 };
                    Thread oThread = new Thread(SendSmsVoid);
                    oThread.Start(smsData);

                    _smsRepository.Add(sms);
                }

                #endregion
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        #region Replacing Templates
        private string ReplaceTemplate(string message, CustomerView customer)
        {
            string body = message == null ? "" : message;

            IDictionary<string, string> smsReplacements = new Dictionary<string, string>();

            smsReplacements.Add("<%Name%>", customer.Name);
            smsReplacements.Add("&lt;%Name%&gt;", customer.Name);

            smsReplacements.Add("<%Level%>", customer.LevelTitle);
            smsReplacements.Add("&lt;%Level%&gt;", customer.LevelTitle);

            smsReplacements.Add("<%SaleEmployeeName%>", customer.CreateEmployeeName);
            smsReplacements.Add("&lt;%SaleEmployeeName%&gt;", customer.CreateEmployeeName);

            smsReplacements.Add("<%ADSLPhone%>", customer.ADSLPhone);
            smsReplacements.Add("&lt;%ADSLPhone%&gt;", customer.ADSLPhone);

            smsReplacements.Add("<%Balance%>", customer.Balance.ToString());
            smsReplacements.Add("&lt;%Balance%&gt;", customer.Balance.ToString());

            smsReplacements.Add("<%Title%>", customer.Gender == "مرد" ? "جناب آقای" : "سرکار خانم");
            smsReplacements.Add("&lt;%Title%&gt;", customer.Gender == "مرد" ? "جناب آقای" : "سرکار خانم");


            foreach (KeyValuePair<string, string> replacement in smsReplacements)
            {
                body = body.Replace(replacement.Key, replacement.Value);
            }

            return body;
        }
        #endregion


        public GeneralResponse AddCustomerLevel(AddCustomerLevelRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            response = PrepareToAddCustomerLevel(request);

            if (!response.hasError)
                _uow.Commit();

            return response;
        }

        #endregion

        #region CheckLevelCondition

        private CheckConditionResponse CheckLevelCondition(Level newLevel, Customer customer)
        {
            IEnumerable<Condition> conditions = newLevel.Conditions;
            CheckConditionResponse response = new CheckConditionResponse();
            Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
            Criterion criteria = new Criterion("Customer.ID", customer.ID, CriteriaOperator.Equal);
            query.Add(criteria);
            IEnumerable<Sale> sales = _saleRepository.FindBy(query);

            response.CanEnter = true;

            foreach (Condition condition in conditions)
            {
                // اگر کوئری از نوع هایبرنیت باشد
                if (condition.nHibernate)
                {
                    bool canEnter = _customerRepository.CheckCondition(condition, newLevel, customer);
                    response.CanEnter = response.CanEnter & canEnter;

                    if (!canEnter)
                        response.ErrorMessages.Add(condition.ErrorText);
                }
                // اگر کوئری از نوع هایبرنیت نباشد
                else
                {
                    // StatusIsAdameEmkan
                    // وضعیت مرکز تحت پوشش باشد
                    if (condition.QueryText == "CenterStatusIsSupport")
                    {
                        response.CanEnter = customer.Center.StatusKey == "Support" ? true : false;
                        response.ErrorMessages.Add(condition.ErrorText);
                    }
                    // وضعیت مرکز تحت پوشش نباشد
                    if (condition.QueryText == "StatusIsNotSupport")
                    {
                        response.CanEnter = customer.Center.StatusKey == "NotSupport" ? true : false;
                        response.ErrorMessages.Add(condition.ErrorText);
                    }
                    // وضعیت مرکز عدم امکان موقت باشد
                    if (condition.QueryText == "StatusIsAdameEmkan")
                    {
                        response.CanEnter = customer.Center.StatusKey == "AdameEmkan" ? true : false;
                        response.ErrorMessages.Add(condition.ErrorText);
                    }
                    // کالا تحویل نشده نداشته باشد و یا برگشت شده باشد 
                    if (condition.QueryText == "HasNoUndeliveredProductsOrAllRollbacked")
                    {
                        
                        foreach (var sale in sales)
                        {
                            foreach (var saleDetail in sale.ProductSaleDetails)
                            {
                                // تحویل نشده ی برگشت نشده فالس است
                                if (!saleDetail.Delivered && !saleDetail.Rollbacked && !saleDetail.IsRollbackDetail)
                                {
                                    response.CanEnter = false;
                                    
                                }
                            }
                            //foreach (var saleDetail in sale.CreditSaleDetails)
                            //{
                            //    // تحویل نشده ی برگشت نشده فالس است
                            //    if (!saleDetail.Delivered && !saleDetail.Rollbacked && !saleDetail.IsRollbackDetail)
                            //    {
                            //        response.CanEnter = false;

                            //    }
                            //}
                            //foreach (var saleDetail in sale.UncreditSaleDetails)
                            //{
                            //    // تحویل نشده ی برگشت نشده فالس است
                            //    if (!saleDetail.Delivered && !saleDetail.Rollbacked && !saleDetail.IsRollbackDetail)
                            //    {
                            //        response.CanEnter = false;

                            //    }
                            //}

                            response.ErrorMessages.Add(condition.ErrorText);
                        }

                    }
                    // خدمات اعتباری تحویل نشده نداشته باشد و یا برگشت شده باشد
                    else if (condition.QueryText == "HasNoUndeliveredCreditOrAllRollbacked")
                    {
                        foreach (var sale in sales)
                        {
                            foreach (var saleDetail in sale.CreditSaleDetails)
                            {
                                // تحویل نشده ی برگشت نشده فالس است
                                if (!saleDetail.Delivered && !saleDetail.Rollbacked && !saleDetail.IsRollbackDetail)
                                {
                                    response.CanEnter = false;
                                    response.ErrorMessages.Add(condition.ErrorText);
                                }
                            }
                        }

                    }
                    // خدمات غیر اعتباری تحویل نشده نداشته باشد و یا برگشت شده باشد
                    else if (condition.QueryText == "HasNoUndeliveredUncreditOrAllRollbacked")
                    {
                        foreach (var sale in sales)
                        {
                            foreach (var saleDetail in sale.UncreditSaleDetails)
                            {
                                // تحویل نشده ی برگشت نشده فالس است
                                if (!saleDetail.Delivered && !saleDetail.Rollbacked && !saleDetail.IsRollbackDetail)
                                {
                                    response.CanEnter = false;
                                    response.ErrorMessages.Add(condition.ErrorText);
                                }
                            }
                        }
                    }
                    // آخرین خدمات اعتباری که فروخته شده منقضی شده باشد
                    else if (condition.QueryText == "LastCreditExpired")
                    {
                        CreditSaleDetail saleDetail = sales.OrderByDescending(o => o.CreateDate).FirstOrDefault().CreditSaleDetails.OrderByDescending(o => o.CreateDate).FirstOrDefault();
                        // شرط عدم انقضاء
                        if (saleDetail.CreditService.ExpDays >= PersianDateTime.DateDiff(PersianDateTime.Now, saleDetail.CreateDate))
                        {
                            response.CanEnter = false;
                            response.ErrorMessages.Add(condition.ErrorText);
                        }
                    }
                    //آخرین خدمات اعتباری که فروخته شده منقضی نشده باشد
                    else if (condition.QueryText == "LastCreditNotExpired")
                    {
                        CreditSaleDetail saleDetail = sales.OrderByDescending(o => o.CreateDate).FirstOrDefault().CreditSaleDetails.OrderByDescending(o => o.CreateDate).FirstOrDefault();
                        // شرط انقضاء
                        if (saleDetail.CreditService.ExpDays < PersianDateTime.DateDiff(PersianDateTime.Now, saleDetail.CreateDate))
                        {
                            response.CanEnter = false;
                            response.ErrorMessages.Add(condition.ErrorText);
                        }
                    }
                    // شبکه انتخاب شده برای مشتری تحت پوشش باشد
                    else if (condition.QueryText == "NetworkOfCustomerIsSupport")
                    {
                        if (customer.Network != null)
                        {
                            if (!customer.Network.NetworkCenters.Where(w => w.Status == NetworkCenterStatus.Support && w.Center == customer.Center).Any())
                            {
                                response.CanEnter = false;
                                response.ErrorMessages.Add(condition.ErrorText);
                            }
                        }
                        else
                        {
                            response.CanEnter = false;
                            response.ErrorMessages.Add("برای انتقال مشتری به این مرحله ، حتما باید شبکه مشتری مشخص باشد. لطفا در ثبت نام کامل ، شبکه مشتری را انتخاب کنید.");
                        }
                    }
                    // شبکه انتخاب شده برای مشتری عدم پوشش باشد
                    else if (condition.QueryText == "NetworkOfCustomerIsNotSupport")
                    {
                        if (!customer.Network.NetworkCenters.Where(w => w.Status == NetworkCenterStatus.NotSupport && w.Center == customer.Center).Any())
                        {
                            response.CanEnter = false;
                            response.ErrorMessages.Add(condition.ErrorText);
                        }
                    }
                    // شبکه انتخاب شده برای مشتری عدم امکان موقت باشد
                    else if (condition.QueryText == "NetworkOfCustomerIsAdameEmkan")
                    {
                        if (!customer.Network.NetworkCenters.Where(w => w.Status == NetworkCenterStatus.NotSupport && w.Center == customer.Center).Any())
                        {
                            response.CanEnter = false;
                            response.ErrorMessages.Add(condition.ErrorText);
                        }
                    }
                    // امکان تحویل حداقل یک کالا یا خدمات فروخته شده وجود داشته باشد
                    else if (condition.QueryText == "CanDeliverAtLeastOnItem")
                    {
                        foreach (Sale sale in sales)
                        {
                            if (sale.Closed)
                            {
                                foreach (var saleDetail in sale.CreditSaleDetails)
                                {
                                    // جمع سطر کمتر از مانده قابل تحویل باشد
                                    if (saleDetail.LineTotal <= customer.CanDeliverCost)
                                        response.CanEnter = true;
                                }
                                foreach (var saleDetail in sale.UncreditSaleDetails)
                                {
                                    // جمع سطر کمتر از مانده قابل تحویل باشد
                                    if (saleDetail.LineTotal <= customer.CanDeliverCost)
                                        response.CanEnter = true;
                                }
                                foreach (var saleDetail in sale.ProductSaleDetails)
                                {
                                    // جمع سطر کمتر از مانده قابل تحویل باشد
                                    if (saleDetail.LineTotal <= customer.CanDeliverCost)
                                        response.CanEnter = true;
                                }

                                response.ErrorMessages.Add(condition.ErrorText);
                            }
                        }
                    }
                    else if (condition.QueryText == "HasNoOpenCSupport")
                    {
                        if (customer.Supports.Any())
                        {
                            IEnumerable<Support> support =
                                customer.Supports.Where(x => x.SupportStatus.IsLastSupportState != true);

                            if (support.Count() > 0)
                            {
                                response.CanEnter = false;
                                response.ErrorMessages.Add(condition.ErrorText);
                            }
                        }
                    }
                }
            }

            return response;
        }

        #endregion

        #region Edit

        public GeneralResponse EditCustomerLevel(EditCustomerLevelRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            CustomerLevel customerLevel = new CustomerLevel();
            customerLevel = _customerLevelRepository.FindBy(request.ID);

            if (customerLevel != null)
            {
                try
                {
                    customerLevel.ModifiedDate = PersianDateTime.Now;
                    customerLevel.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if (request.CustomerID != customerLevel.Customer.ID)
                        customerLevel.Customer = this._customerRepository.FindBy(request.CustomerID);
                    if (request.NewLevelID != customerLevel.Level.ID)
                        customerLevel.Level = this._levelRepository.FindBy(request.NewLevelID);
                    if (request.Note != null)
                        customerLevel.Note = request.Note;

                    if (customerLevel.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        customerLevel.RowVersion += 1;
                    }

                    if (customerLevel.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in customerLevel.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _customerLevelRepository.Save(customerLevel);
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

        public GeneralResponse DeleteCustomerLevel(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

                try
                {
                    _customerLevelRepository.RemoveById(request.ID);
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

        public GetCustomerLevelResponse GetCustomerLevel(GetRequest request)
        {
            GetCustomerLevelResponse response = new GetCustomerLevelResponse();

            try
            {
                CustomerLevel customerLevel = new CustomerLevel();
                CustomerLevelView customerLevelView = customerLevel.ConvertToCustomerLevelView();

                customerLevel = _customerLevelRepository.FindBy(request.ID);
                if (customerLevel != null)
                    customerLevelView = customerLevel.ConvertToCustomerLevelView();

                response.CustomerLevelView = customerLevelView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        #endregion

        #region Get All

        public GetCustomerLevelsResponse GetCustomerLevels()
        {
            GetCustomerLevelsResponse response = new GetCustomerLevelsResponse();

            try
            {
                IEnumerable<CustomerLevelView> customerLevels = _customerLevelRepository.FindAll()
                    .ConvertToCustomerLevelViews();

                response.CustomerLevelViews = customerLevels;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<CustomerLevelView>> GetLevelHistory(Guid customerID, int pageSize, int pageNumber)
        {
            Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
            Criterion criteria = new Criterion("Customer.ID", customerID, CriteriaOperator.Equal);
            query.Add(criteria);

            int index = (pageNumber - 1) * pageSize;
            int count = pageSize;

            Infrastructure.Domain.Response<CustomerLevel> customerLevelResponse = _customerLevelRepository.FindBy(query, index, count);
            
            GetGeneralResponse<IEnumerable<CustomerLevelView>> response = new GetGeneralResponse<IEnumerable<CustomerLevelView>>();
            response.data = customerLevelResponse.data.ConvertToCustomerLevelViews();
            response.totalCount = customerLevelResponse.totalCount;

            string temp = response.data.FirstOrDefault().CreateDate;
            var list = response.data.ToArray();
            if (temp != null)
            {
                for (int i = 1; i <= list.Length - 1; i++)
                {
                    if (i == 1)
                        list[i-1].WaitingDays = PersianDateTime.DateDiff(temp, list[i].CreateDate);
                    else
                    {
                        list[i-1].WaitingDays = PersianDateTime.DateDiff(list[i - 1].CreateDate, list[i].CreateDate);
                    }
                }
            }
            list.Last().WaitingDays = PersianDateTime.DateDiff(list.Last().CreateDate, PersianDateTime.Now);
            response.data = list;

            return response;
        }


        #endregion

        #region Send Email and Sms

        public void SendEmailVoid(object data)
        {
            EmailData emailData = (EmailData)data;
            if (emailData.recipients.Count() == 0)
                SendEmail(emailData.recipient, emailData.displayName, emailData.subject, emailData.body);
            else
                SendEmail(emailData.recipients, emailData.displayName, emailData.subject, emailData.body);
        }

        public GeneralResponse SendEmail(string recipient, string displayName, string subject, string htmlBody)
        {
            IList<string> recipients = new List<string>();
            recipients.Add(recipient);

            return SendEmail(recipients, displayName, subject, htmlBody);
        }

        public  GeneralResponse SendEmail(IList<string> recipients, string displayName, string subject, string htmlBody)
        {
            GeneralResponse response = new GeneralResponse();

            #region Smtp Settings

            string Host, UserName, Password;
            bool EnableSsl;
            int Port;

            #region Read SMTP configuration from Web.config

            try
            {
                Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration("~");
                MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

                if (mailSettings != null)
                {
                    EnableSsl = mailSettings.Smtp.Network.EnableSsl;
                    Host = mailSettings.Smtp.Network.Host;
                    Password = mailSettings.Smtp.Network.Password;
                    Port = mailSettings.Smtp.Network.Port;
                    UserName = mailSettings.Smtp.Network.UserName;
                }
                else
                {
                    
                    response.ErrorMessages.Add("تنظیمات SMTP انجام نشده است. لطفاً ابتدا از منوی اطلاعات پایه => تنظیمات SMTP اقدام نمایید.");
                    return response;
                }
            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add("اشکال در خواندن تنظیمات SMTP");
                return response;
            }

            #endregion

            var smtp = new SmtpClient
            {
                Host = Host,
                Port = Port,
                EnableSsl = EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(UserName, Password)
            };

            #endregion

            MailAddress fromAddress = new MailAddress(UserName, displayName, System.Text.Encoding.UTF8);



            MailMessage msg = new MailMessage()
            {
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true,
                From = fromAddress
            };

            #region Generate recipients list

            foreach (var recipient in recipients)
            {
                MailAddress toAddress = new MailAddress(recipient);
                msg.To.Add(toAddress);
            }

            #endregion

            #region Sending

            try
            {
                smtp.Send(msg);
            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
                return response;
            }

            #endregion

            //if (!response.hasError)
            //{
            //    email.Sent = true;
            //    email.Note = "زمان ارسال ایمیل: " + PersianDateTime.Now;
            //}
            //else
            //{
            //    email.Sent = false;
            //    foreach (var err in response.ErrorMessages)
            //    {
            //        email.Note += err + "\n";
            //    }
            //}

            return response;
        }

        public void SendSmsVoid(object data)
        {
            ISmsWebService smsWebService = new ISmsWebService();
            SmsData smsData = (SmsData)data;
            smsWebService.SendSms(smsData.body, smsData.phoneNumber);
        }
        #endregion

    }

    public class EmailData
    {
        public EmailData()
        {
            recipients = new List<string>();
        }
        public string recipient { get; set; }
        public List<string> recipients { get; set; }
        public string displayName { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }

    public class SmsData
    {
        public Guid? EmployeeID { get; set; }
        public string body { get; set; }
        public string phoneNumber { get; set; }
        public string QueueName { get; set; }
    }
}
