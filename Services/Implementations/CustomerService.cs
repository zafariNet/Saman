#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using Model.Customers.Validations;
using NHibernate.Criterion;
using Services.Interfaces;
using Model.Customers.Interfaces;
using Model.Employees.Interfaces;
using Model.Store.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Model.Customers;
using Services.Messaging.ReportCatalogService;
using Services.ViewModels.Customers;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees;
using Infrastructure.Querying;
using Model.Store;
using Infrastructure.Domain;
using Model.Sales;
using Services.ViewModels.Reports;
using Services.ViewModels.Sales;
using Model.Customers.Validations.Interfaces;
using Model.Fiscals;
using Model.Sales.Interfaces;
using Model.Fiscals.Interfaces;
using System.Collections;
#endregion

namespace Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        #region Declares

        private readonly ICustomerRepository _customerRepository;
        private readonly ICenterRepository _centerRepository;
        private readonly ICenterService _centerService;
        private readonly IAgencyRepository _agencyRepository;
        private readonly IDocumentStatusRepository _documentStatusRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISuctionModeRepository _suctionModeRepository;
        private readonly INetworkRepository _networkRepository;
        private readonly IUnitOfWork _uow;
        private readonly ICustomerLevelService _customerLevelService;
        private readonly IQueryRepository _queryRepository;
        private readonly ILevelRepository _levelRepository;
        private readonly IBuyPossibilityRepository _buyPossibilityRepository;
        private readonly IFollowStatusRepository _followStatusRepository;
        private readonly ISuctionModeDetailRepository _suctionModeDetailRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IFiscalRepository _fiscalRepository;
        private readonly ISmsRepository _smsRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly ISimpleCustomerRepository _simpleCustomerRepository;

        
        #endregion

        #region Ctor
        public CustomerService(ICustomerRepository customerRepository, IUnitOfWork uow)
        {
            _customerRepository = customerRepository;
            _uow = uow;
        }

        public CustomerService(ICustomerRepository customerRepository, ICenterRepository centerRepository, IAgencyRepository agencyRepository,
            IDocumentStatusRepository documentStatusRepository, IEmployeeRepository employeeRepository, ISuctionModeRepository suctionModeRepository,
            INetworkRepository networkRepository, IUnitOfWork uow, ICustomerLevelService customerLevelService,
            IQueryRepository queryRepository, ISuctionModeDetailRepository suctionModeDetailRepository,
            ICenterService centerService,
            ILevelRepository levelRepository,
            IBuyPossibilityRepository buyPossibilityRepository,
            IFollowStatusRepository followStatusRepository, IFiscalRepository fiscalRepository, ISaleRepository saleRepository, 
            ISmsRepository smsRepository, IEmailRepository emailRepository,ISimpleCustomerRepository simpleCustomerRepository)
            : this(customerRepository, uow)
        {
            this._agencyRepository = agencyRepository;
            this._centerRepository = centerRepository;
            this._documentStatusRepository = documentStatusRepository;
            this._employeeRepository = employeeRepository;
            this._networkRepository = networkRepository;
            this._suctionModeRepository = suctionModeRepository;
            this._suctionModeDetailRepository = suctionModeDetailRepository;
            _customerLevelService = customerLevelService;
            _centerService = centerService;
            this._queryRepository = queryRepository;
            _levelRepository = levelRepository;
            _buyPossibilityRepository = buyPossibilityRepository;
            _followStatusRepository = followStatusRepository;
            _fiscalRepository = fiscalRepository;
            _saleRepository = saleRepository;
            _smsRepository = smsRepository;
            _emailRepository = emailRepository;
            this._simpleCustomerRepository = simpleCustomerRepository;


        }
        #endregion

        #region Get Employee For PopUp

        public GetGeneralResponse<SimpleCustomerView> GetSimpleCustomer(string ADSLPhone)
        {
            GetGeneralResponse<SimpleCustomerView> response = new GetGeneralResponse<SimpleCustomerView>();

            try
            {
                IList<SimpleCustomer> simpleCustomer = _simpleCustomerRepository.FindSimpleCustomer(ADSLPhone);

                response.data = simpleCustomer.ConvertToSimpleCustomerViews().FirstOrDefault();
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

        public GeneralResponse AddCustomer(AddCustomerRequest request)
        {
            // برای اینکه بتوان شرط ورود به مرحله ابتدایی را چک کرد ابتدا باید مشتری را بدون شرط ایجاد کرد
            // سپس با استفاده از تغییر مرحله، مشتری را به مرحله مورد نظر ارسال کرد
            // در صورتی که شرط برقرار نبود، مشتری ایجاد شده را پاک کرد

            GeneralResponse response = new GeneralResponse();

            try
            {
                Customer customer = new Customer();
                customer.ID = Guid.NewGuid();
                customer.CreateDate = PersianDateTime.Now;
                customer.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                customer.ADSLPhone = request.ADSLPhone;
                customer.Agency = this._agencyRepository.FindBy(request.AgencyID);
                customer.BirthDate = request.BirthDate;
                customer.AddedToSite = false;
                if (request.CenterID != Guid.Empty)
                {
                    customer.Center = _centerService.GetCenterInfo(request.ADSLPhone, 4).Center;
                }
                customer.DocumentStatus = this._documentStatusRepository.FindBy(request.DocumentStatusID);
                customer.Email = request.Email;
                customer.FirstName = request.FirstName;
                customer.LegalType = request.LegalType;
                customer.Gender = request.Gender;
                customer.Job = request.Job;
                customer.LastName = request.LastName;
                customer.Locked = request.Locked;
                customer.LockEmployee = this._employeeRepository.FindBy(request.LockEmployeeID);
                customer.LockNote = request.LockNote;
                customer.Mobile1 = request.Mobile1;
                customer.Mobile2 = request.Mobile2;
                customer.Network = this._networkRepository.FindBy(request.NetworkID);
                customer.Phone = request.Phone;
                customer.SentToPap = request.SentToPap;
                customer.SFirstName = request.SFirstName;
                customer.SLastName = request.SLastName;
                customer.SuctionMode = this._suctionModeRepository.FindBy(request.SuctionModeID);
                customer.SuctionModeDetail = this._suctionModeDetailRepository.FindBy(request.SuctionModeDetailID);
                customer.Address = request.Address;
                customer.Note = request.Note;
                customer.Discontinued = request.Discontinued;
                customer.Balance = request.Balance;
                customer.RowVersion = 1;
                customer.LevelEntryDate = PersianDateTime.Now;

                #region Validation
                if (customer.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in customer.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }
                #endregion

                _customerRepository.Add(customer);
                _uow.Commit();

                response.ID = customer.ID;
                response.ObjectAdded = customer.ConvertToCustomerView();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Substring(0, 107) == "Violation of UNIQUE KEY constraint 'IX_UniqueSPhone'. Cannot insert duplicate key in object 'Cus.Customer'.")
                {
                    response.ErrorMessages.Add("NumberMustBeMoreThan8Key");
                }
                else
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                }

            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<CustomerView>> test(IList<FilterData> filter)
        {
            GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();

            string[] pare = { "ADSLPhone", "FirstName" };

            //string query = FilterUtilityService.CreateHQL(filter, "Customer", null, pare);


            return response;
        }
        public GeneralResponse QuickAddCustomer(AddCustomerRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            #region check AdslPhone not duplicated or not more than 8 char

            if (request.ADSLPhone.Length > 8)
            {
                // شماره تلفن وارد شده بیشتر از 8 رقم می باشد
                response.ErrorMessages.Add("CharachtersMoreThan8");
                return response;
            }

            Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
            Criterion criteria = new Criterion("ADSLPhone", request.ADSLPhone, CriteriaOperator.Equal);
            query.Add(criteria);
            IEnumerable<Customer> _customer = _customerRepository.FindBy(query);
            if (_customer.Count()>0)
            {
                // شماره تلفن وارد شده تکراری می باشد.
                Customer c = _customer.First();
                response.ErrorMessages.Add("<p style=\"text-align:center;width:400px;font-size:13px;color:red\">شماره تکراری</p> نام مشتری : " +"<b>"+ c.Name + "</b><br />"+" مرحله مشتری : <b>" + c.Level.LevelTitle + "</b><br /> کارشناس فروش : <b>" +c.CreateEmployee.Name + "</b><br /> تاریخ ثبت نام : <b>" +c.CreateDate );
                return response;
            }

            #endregion

            try
            {
                Customer customer = new Customer();
                customer.ID = Guid.NewGuid();
                customer.CreateDate = PersianDateTime.Now;
                customer.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                customer.ADSLPhone = request.ADSLPhone;
                customer.Agency = this._agencyRepository.FindBy(request.AgencyID);
                customer.BirthDate = request.BirthDate;

                #region Get Center Info and set Center to new customer

                if (request.ADSLPhone == null)
                {
                    // شماره تلفن باید وارد شود در غیر اینصورت اطلاعات ذخیره نخواهد شد.
                    response.ErrorMessages.Add("AdslPhoneIsNull");
                    return response;
                }
                else
                {
                    customer.Center = _centerService.GetCenterInfo(request.ADSLPhone, 5).Center;
                    if (customer.Center == null)
                    {
                        // برای شماره مورد نظر هیچ مرکز مخابراتی ثبت نشده است. بنابراین اطلاعات شما ذخیره نمی شود.
                        response.ErrorMessages.Add("CenterIsNull");
                        return response;
                    }
                }

                #endregion
                //Added By Zafari
                //customer.LevelID = request.LevelID;
                //
                customer.DocumentStatus = GetDefaultDocumentStatus();
                customer.Email = request.Email;
                customer.FirstName = request.FirstName;
                customer.LegalType = request.LegalType;
                customer.Gender = request.Gender;
                customer.Job = request.Job;
                customer.LastName = request.LastName;
                customer.Locked = request.Locked;
                customer.LockEmployee = this._employeeRepository.FindBy(request.LockEmployeeID);
                customer.LockNote = request.LockNote;
                customer.Mobile1 = request.Mobile1;
                customer.Mobile2 = request.Mobile2;
                customer.Network = this._networkRepository.FindBy(request.NetworkID);
                customer.Phone = request.Phone;
                customer.SentToPap = request.SentToPap;
                customer.SFirstName = request.SFirstName;
                customer.SLastName = request.SLastName;
                customer.SuctionMode = this._suctionModeRepository.FindBy(request.SuctionModeID);
                customer.SuctionModeDetail = this._suctionModeDetailRepository.FindBy(request.SuctionModeDetailID);
                customer.Address = request.Address;
                customer.Note = request.Note;
                customer.Discontinued = request.Discontinued;
                customer.RowVersion = 1;
                customer.Balance = request.Balance;
                customer.Level = _levelRepository.FindBy(request.LevelID);

                

                //Addedd By Zafari
                //مشکل در ذخیره احتمال خرید
                customer.BuyPossibility = _buyPossibilityRepository.FindBy(request.BuyPossibilityID);
                //Added By Zafari
                //مشکل در ذخیره وضعیت پیگیری
                customer.FollowStatus = _followStatusRepository.FindBy(request.FollowStatusID);

                #region Validation
                if (customer.GetBrokenRules().Count() > 0)
                {
                    foreach (BusinessRule businessRule in customer.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }
                #endregion

              
                _customerRepository.Add(customer);
                // ابتدا بدون مرحله ذخیره می کنیم
                _uow.Commit();

                // سپس تغییر مرحله را اجرا می کنیم
                
                #region Set First Level

                GeneralResponse levelResponse = new GeneralResponse();

                Model.Customers.Query oldQuery = _queryRepository.FindAll().Where(x => x.AllCustomer).FirstOrDefault();
                oldQuery.CustomerCount += 1;
                _queryRepository.Save(oldQuery);


                AddCustomerLevelRequest levelRequest = new AddCustomerLevelRequest()
                {
                    CreateEmployeeID = request.CreateEmployeeID,
                    CustomerID = customer.ID,
                    NewLevelID = request.LevelID,
                    Note = "RegisterCustomerKey",
                    OldLevelID = Guid.Empty,
                    NewCustomer = true
                };

                levelResponse = _customerLevelService.PrepareToAddCustomerLevel(levelRequest);

                #endregion

                // اگر مرحله مشتری بدون مشکل ست شده بود دوباره ذخیره می کنیم

                if (!levelResponse.hasError)
                {
                    _uow.Commit();

                    response.ID = customer.ID;
                    response.ObjectAdded = customer.ConvertToCustomerView();
                }
                else
                {
                    foreach (var err in levelResponse.ErrorMessages)
                        response.ErrorMessages.Add(err);
                    // اگر مرحله به اشکال برخورد مشتری را پاک می کنیم
                    _customerRepository.Remove(customer);
                    _uow.Commit();
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        #endregion

        #region Edit

        public GeneralResponse EditCustomer(EditCustomerRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            Customer customer = new Customer();
            customer = _customerRepository.FindBy(request.ID);
            
            if (customer != null)
            {
                try
                {
                    customer.ModifiedDate = PersianDateTime.Now;
                    customer.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if(request.AgencyID!=null)
                    customer.Agency = this._agencyRepository.FindBy(request.AgencyID);
                    customer.BirthDate = request.BirthDate;
                    customer.Email = request.Email;
                    customer.Gender = request.Gender;
                    if(request.BuyPossibilityID!=null)
                    customer.BuyPossibility = _buyPossibilityRepository.FindBy(request.BuyPossibilityID);
                    if(request.FollowStatusID!=null)
                    customer.FollowStatus = _followStatusRepository.FindBy(request.FollowStatusID);
                    if(request.NetworkID!=null)
                    customer.Network = _networkRepository.FindBy(request.NetworkID);
                    customer.FirstName = request.FirstName;
                    customer.LastName = request.LastName;
                    customer.LegalType = request.LegalType;
                    if(request.DocumentStatusID!=null)
                    customer.DocumentStatus = _documentStatusRepository.FindBy(request.DocumentStatusID);
                    customer.Job = request.Job;
                    customer.Mobile1 = request.Mobile1;
                    customer.Mobile2 = request.Mobile2;
                    

                    #region Network Change

                    //Network network = _networkRepository.FindBy(request.NetworkID);
                    //if (customer.Network.ID != request.NetworkID) قبلا این بود باید بررسی شود که با تغییر این شرط کجاهای برنامه تاثیر میپذیرد
                    if (customer.Network != null && customer.Network.ID != request.NetworkID)
                    {
                        Level level = _levelRepository.FindBy(customer.Level.ID);
                        if (level != null && level.Options.CanChangeNetwork)
                            customer.Network = this._networkRepository.FindBy(request.NetworkID);
                        else
                        {
                            // در این مرحله امکان عوض کردن شبکه وجود ندارد
                            response.ErrorMessages.Add("CannotChangeNetworkInThisLevel");
                            return response;
                        }
                    }

                    #endregion

                    customer.Phone = request.Phone;
                    customer.SentToPap = request.SentToPap;
                    customer.SFirstName = request.SFirstName;
                    customer.SLastName = request.SLastName;
                    if(request.SuctionModeID!=null)
                    customer.SuctionMode = this._suctionModeRepository.FindBy(request.SuctionModeID);
                    if(request.SuctionModeDetailID !=null)
                    customer.SuctionModeDetail = this._suctionModeDetailRepository.FindBy(request.SuctionModeDetailID);
                    customer.Address = request.Address;
                    customer.Note = request.Note;
                    customer.Discontinued = request.Discontinued;

                    #region RowVersion Check
                    if (customer.RowVersion != request.RowVersion)
                    {
                        // کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.
                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        customer.RowVersion += 1; 
                        response.rowVersion = customer.RowVersion;
                    }
                    #endregion

                    #region Validation
                    if (customer.GetBrokenRules().Count() > 0)
                    {
                        //response.hasCenter = false;
                        foreach (BusinessRule businessRule in customer.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }
                    #endregion

                    _customerRepository.Save(customer);
                    _uow.Commit();

                    //response.hasCenter = true;
                }
                catch (Exception ex)
                {
                    //response.hasCenter = false;
                    response.ErrorMessages.Add(ex.Message);
                    if (ex.InnerException != null)
                        response.ErrorMessages.Add(ex.InnerException.Message);
                }
            }
            else
            {
                //response.hasCenter = false;
                response.ErrorMessages.Add("NoItemToEditKey");
            }
            return response;
        }

        #endregion

        #region Delete

        public GeneralResponse DeleteCustomer(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            Customer customer = new Customer();
            customer = _customerRepository.FindBy(request.ID);

            Model.Customers.Query query =
                _queryRepository.FindAll().Where(x => x.Level.ID == customer.Level.ID).FirstOrDefault();

            if (query != null)
                query.CustomerCount -= 1;


            Model.Customers.Query queryAll =
                _queryRepository.FindAll().Where(x => x.AllCustomer).FirstOrDefault();

            queryAll.CustomerCount -= 1;

            _queryRepository.Save(query);
            _queryRepository.Save(queryAll);

            if (customer != null)
                if (customer.CanDelete)
                {
                    try
                    {
                        _customerRepository.Remove(customer);
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
                    // به علت وجود اطلاعات مربوط به مشتری مورد نظر در بانک اطلاعات، امکان حذف آن وجود ندارد.
                    response.ErrorMessages.Add("HasDataAndCannotDelete");
                }
            else
                // مشتری مورد نظر یافت نشد.
                response.ErrorMessages.Add("CustomerNotFound");

            return response;
        }

        #endregion

        #region Get One

        public GetCustomerResponse GetCustomer(GetRequest request)
        {
            GetCustomerResponse response = new GetCustomerResponse();

            try
            {
                Customer customer = new Customer();
                CustomerView customerView = customer.ConvertToCustomerView();

                customer = _customerRepository.FindBy(request.ID);
                if (customer != null)
                    customerView = customer.ConvertToCustomerView();

                response.CustomerView = customerView;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion

        #region Get Some

        public GetGeneralResponse<IEnumerable<CustomerView>> GetCustomers()
        {
            GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();

            try
            {
                IEnumerable<CustomerView> Customers = _customerRepository.FindAll()
                    .ConvertToCustomerViews();

                response.data = Customers;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<CustomerView>> GetCustomers(Guid queryID, Guid currentEmployeeID, QuickSearchRequest request)
        {
            // current Employee
            Employee currentEmployee = _employeeRepository.FindBy(currentEmployeeID);
            // requested query
            Model.Customers.Query query = _queryRepository.FindBy(queryID);

            GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();

            // if this employee dont have permission to view requested query, return
            if (!currentEmployee.CanView(query))
            {
                return response;
            }

            // else 
            try
            {
                int index = (request.pageNumber - 1) * request.pageSize;
                int count = request.pageSize;

                Infrastructure.Domain.Response<Customer> customersResponse = _customerRepository
                    .FindAll(queryID, index, count);

                IEnumerable<CustomerView> customers = customersResponse.data
                    .ConvertToCustomerViews();

                response.data = customers;
                response.totalCount = customersResponse.totalCount;


            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public CustomerView getCustomerbyPhone(string ADSLPhone)
        {
            Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
            Criterion criteriaADSLPhone = new Criterion("ADSLPhone", ADSLPhone, CriteriaOperator.Equal);
            query.Add(criteriaADSLPhone);
            Response<Customer> customer = _customerRepository.FindBy(query, -1, -1, null);

            CustomerView response = customer.data.ConvertToCustomerViews().FirstOrDefault();

            return response;
        }

        public GetGeneralResponse<IEnumerable<CustomerView>> GetCustomers(Guid queryID, Guid currentEmployeeID, int pageSize, 
            int pageNumber,IList<Sort> sort,IList<FilterData> filter)
        {
            // current Employee
            Employee currentEmployee = _employeeRepository.FindBy(currentEmployeeID);
            // requested query
            Model.Customers.Query query = _queryRepository.FindBy(queryID);

            GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();

            // if this employee dont have permission to view requested query, return
            if (!currentEmployee.CanView(query))
            {
                return response;
            }

            // else 
            try
            {
                int index = -1;
                int count = -1;

                if (pageNumber != -1)
                {
                    index = (pageNumber - 1) * pageSize;
                    count = pageSize;
                }
                foreach (Sort _sort in sort)
                {
                    if (_sort.SortColumn == "CenterName")
                    {
                        _sort.SortColumn = "Center.CenterName";
                    }
                    if (_sort.SortColumn == "BuyPossibilityName")
                    {
                        _sort.SortColumn = "BuyPossibility.BuyPossibilityName";
                    }

                    if (_sort.SortColumn == "SuctionModeName")
                    {
                        _sort.SortColumn = "SuctionMode.SuctionModeName";
                    }

                    if (_sort.SortColumn == "DocumentStatusName")
                    {
                        _sort.SortColumn = "DocumentStatus.DucumentStatusName";
                    }

                    if (_sort.SortColumn == "NetworkName")
                    {
                        _sort.SortColumn = "Network.NetworkName";
                    }

                    if (_sort.SortColumn == "CreateEmployeeName")
                    {
                        _sort.SortColumn = "CreateEmployee.LastName";
                    }

                    if (queryID == Guid.Parse("befc8e47-c7d6-4cbf-b067-7f6d78d18a21") && _sort.SortColumn == "CreateDate")
                        _sort.SortColumn = "c.CreateDate";
                }

                Response<Customer> customersResponse=new Response<Customer>();
                if (filter != null)
                {
                    string filterString = FilterUtilityService.GenerateFilterHQLQueryForCustomer(filter, "Customer");

                   customersResponse = _customerRepository
                        .FindAll(queryID, index, count, sort, filterString);
                }
                else
                    customersResponse = _customerRepository
                        .FindAll(queryID, index, count, sort);

                IEnumerable<CustomerView> customers = customersResponse.data.ConvertToCustomerViews();

                response.data = customers;
                response.totalCount = customersResponse.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<CustomerView>> GetCustomers(QuickSearchRequest request)
        {
            GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();

            try
            {
                int index = (request.pageNumber - 1) * request.pageSize;
                int count = request.pageSize;

                
                Infrastructure.Domain.Response<Customer> customersResponse = _customerRepository
                    .FindAll(request.queryID, index, count);

                IEnumerable<CustomerView> customers = customersResponse.data
                    .ConvertToCustomerViews();

                response.data = customers;
                response.totalCount = customersResponse.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #region Quice find

        public GetGeneralResponse<IEnumerable<CustomerView>> FindCustomers(QuickSearchRequest request, Guid currentEmployeeID)
        {
            GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();
            int pageNumber = request.pageNumber == 0 ? -1 : request.pageNumber;
            int pageSize = request.pageSize == 0 ? -1 : request.pageSize;
            if (request.queryID != Guid.Empty)
            {
                pageNumber = -1;
                pageSize = -1;
            }

            int index = -1;
            int count = -1;
            if (pageNumber != -1)
            {
                index = (pageNumber - 1) * pageSize;
                count = pageSize;
            }

            foreach (Sort _sort in request.sort)
            {
                if (_sort.SortColumn == "CenterName")
                    _sort.SortColumn = "Center.CenterName";
            }
            if (request.query != null )
            {
                char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                string[] searchPhrases = request.query.Split(delimiterChars);



                // برای نوشتن جستجو در نماها


                IList<Sort> sort = request.sort;
                if (request.customerID == null)
                {

                    #region اگر دو قسمتی بود احتمالا نام و نام خانوادگی است

                    if (searchPhrases.Length == 2)
                    {
                        Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                        query.QueryOperator = QueryOperator.And;

                        IList<Criterion> criterions = new List<Criterion>();
                        criterions.Add(new Criterion("FirstName", searchPhrases[0], CriteriaOperator.Contains));
                        criterions.Add(new Criterion("LastName", searchPhrases[1], CriteriaOperator.Contains));

                        query.Add(criterions);

                        try
                        {
                            Response<Customer> customersResponse = _customerRepository
                                .FindBy(query, index, count);

                            IEnumerable<CustomerView> customers = customersResponse.data.ConvertToCustomerViews();

                            response.data = customers;
                            response.totalCount = response.data.Count();
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }

                    #endregion

                        #region اگر پاسخی نیامد احتمالا نام خانوادگی و نام است

                        if (response.totalCount == 0)
                        {
                            Infrastructure.Querying.Query query2 = new Infrastructure.Querying.Query();

                            query2.QueryOperator = QueryOperator.And;

                            IList<Criterion> criterions2 = new List<Criterion>();

                            criterions2.Add(new Criterion("LastName", searchPhrases[0], CriteriaOperator.Contains));
                            criterions2.Add(new Criterion("FirstName", searchPhrases[1], CriteriaOperator.Contains));

                            query2.Add(criterions2);

                            try
                            {
                                Infrastructure.Domain.Response<Customer> customersResponse = _customerRepository
                                    .FindBy(query2, index, count);

                                IEnumerable<CustomerView> customers = customersResponse.data
                                    .ConvertToCustomerViews();

                                response.data = customers;
                                response.totalCount = response.data.Count();
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                        }
                    }

                        #endregion


                    #region اگر هیچ پاسخی نیامد بقیه انواع سرچ را انجام بده

                    if (response.totalCount == 0)
                    {
                        Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                        string[] searchFields = { "FirstName", "LastName", "SFirstName", "SLastName", "ADSLPhone", "Phone", "Mobile1", "Mobile2", "CreateDate", "ModifiedDate" };

                        IList<Criterion> criterions2 = new List<Criterion>();
                        foreach (string searchField in searchFields)
                        {

                            criterions2.Add(new Criterion(searchField, request.query, CriteriaOperator.Contains));
                            query.QueryOperator = QueryOperator.Or;
                        }
                        query.Add(criterions2);

                        try
                        {
                            Response<Customer> customersResponse = _customerRepository
                                .FindBy(query, index, count, sort);

                            IEnumerable<CustomerView> customers = customersResponse.data
                                .ConvertToCustomerViews();

                            response.data = customers;
                            response.totalCount = customersResponse.totalCount;
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }

                    #endregion

                }
            }
            else
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                IList<Criterion> criterions = new List<Criterion>();
                criterions.Add(new Criterion("ID", request.customerID, CriteriaOperator.Equal));
                query.Add(criterions);

                try
                {
                    Response<Customer> customersResponse = _customerRepository
                        .FindBy(query, -1, -1);

                    IEnumerable<CustomerView> customers = customersResponse.data.ConvertToCustomerViews();

                    response.data = customers;
                    response.totalCount = response.data.Count();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
                #region  جستجو در نماها

                IList<CustomerView> finalCustomerView = new List<CustomerView>();

                // اگر شناسه نما خالی نبود و جستجوی مشتری مقدار داشت
                if (response.data.Count() > 0 && request.queryID != Guid.Empty)
                {
                    GetGeneralResponse<IEnumerable<CustomerView>> queryResponse = GetCustomers(request.queryID, currentEmployeeID, -1, -1, request.sort, null);
                    if (queryResponse.data.Count() > 0)
                    {
                        // مشترک ها در یک متغیر ذخیره می شوند
                        foreach (var customerView in response.data)
                        {
                            if (queryResponse.data.Contains(customerView))
                                finalCustomerView.Add(customerView);
                        }
                    }

                    // پیجینگ مجزا انجام می شود
                    #region Paging

                    pageNumber = request.pageNumber == 0 ? -1 : request.pageNumber;
                    pageSize = request.pageSize == 0 ? -1 : request.pageSize;

                    index = -1;
                    count = -1;
                    if (pageNumber != -1)
                    {
                        index = (pageNumber - 1) * pageSize;
                        count = pageSize;
                    }

                    #endregion

                    response.data = finalCustomerView.Skip(index).Take(count);
                    response.totalCount = finalCustomerView.Count();
                }
            
                        
            #endregion

            return response;

        }

        #endregion

        public GetGeneralResponse<IEnumerable<CustomerView>> FindCustomers(AdvancedSearchRequest request)
        {
            GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();

            {

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                IList<Criterion> criterions = new List<Criterion>();
                foreach (SearchPhrase searchPhrase in request.SearchPhrases)
                {

                    criterions.Add(new Criterion(searchPhrase.FieldName, searchPhrase.Value, CriteriaOperator.Contains));
                    query.QueryOperator = QueryOperator.And;
                }
                query.Add(criterions);

                int index = (request.PageNumber - 1) * request.PageSize;
                int count = request.PageSize;

                IList<Sort> sort = request.sort;

                try
                {
                    Infrastructure.Domain.Response<Customer> customersResponse = _customerRepository
                        .FindBy(query, index, count);

                    IEnumerable<CustomerView> customers = customersResponse.data
                        .ConvertToCustomerViews();

                    response.data = customers;
                    response.totalCount = customersResponse.totalCount;
                }
                catch (Exception ex)
                {
                    throw;
                }

                return response;

            }

        }

        #endregion

        #region GetAll
        public GetGeneralResponse<IEnumerable<CustomerView>> GetAllCustomrs()
        {
            GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();
            IEnumerable<Customer> customers = _customerRepository.FindAll();
            response.data = customers.ConvertToCustomerViews();
            return response;
        }
        #endregion

        #region GetSimple

        

        #endregion

        public GetGeneralResponse<LevelOptionsView> GetLevelOptions(Guid customerID)
        {
            GetGeneralResponse<LevelOptionsView> response = new GetGeneralResponse<LevelOptionsView>();

            try
            {
                Customer customer = this._customerRepository.FindBy(customerID);
                Level level = _levelRepository.FindBy(customer.Level.ID);

                LevelOptions levelOptions = level.Options;
                LevelOptionsView levelOptionsView = new LevelOptionsView();
                if (levelOptions != null)
                {
                    levelOptionsView.CanSale = levelOptions.CanSale;
                    levelOptionsView.CanPersenceSupport = levelOptions.CanPersenceSupport;
                    levelOptionsView.CanDocumentsOperation = levelOptions.CanDocumentsOperation;
                    levelOptionsView.CanChangeNetwork = levelOptions.CanChangeNetwork;
                    levelOptionsView.CanAddProblem = levelOptions.CanAddProblem;
                }

                response.data = levelOptionsView;
                response.totalCount = 5;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }
            return response;
        }

        public string LazyTest(string hqlQuery)
        {
            //GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();

            //response.data = customer.data.ConvertToCustomerViews();

            //response.totalCount = customer.totalCount;            
            
            IEnumerable<Customer> customer = _customerRepository.FindAll();

            return customer.Where(w => w.ID == Guid.Parse("8D5395ED-9845-4E9C-87BA-CF938506950B")).First().FirstName;
        }

        #region Send SMS and Email
        
        public GeneralResponse SendEmalAndSms(IEnumerable<Guid> IDs,string subject,string Content,string Message,Guid CreateEmployeeID)
        {
            GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();
            try
            {
                
                string query = "From Customer c where c.ID in(";
                int counter = IDs.Count();
                int temp = 0;

                foreach (Guid id in IDs)
                {
                    temp++;
                    if (temp == counter)
                        query += "'" + id + "')";
                    else
                        query += "'" + id + "',";
                }

                Response<Customer> customers = _customerRepository.FindAll(query);

                response.data = customers.data.ConvertToCustomerViews();

                #region Send Email
                if(Content !=null)

                    foreach (CustomerView customer in response.data)
                    {
                        // Replacing:
                        string emailBody = ReplaceTemplate(Content, customer);

                        Email email = new Email();
                        email.ID = Guid.NewGuid();
                        email.CreateDate = PersianDateTime.Now;
                        email.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                        email.Body = emailBody;
                        email.Customer = _customerRepository.FindBy(customer.ID);
                        email.Subject = subject;
                        email.RowVersion = 1;
                        _emailRepository.Add(email);


                        _customerLevelService.SendEmail(customer.Email, customer.Name, email.Subject, emailBody);
                    }

                #endregion

                #region Send SMS
                if(Message!=null)
                foreach (CustomerView customer in response.data)
                {
                    string smsBody = ReplaceTemplate(Message, customer);

                    Sms sms = new Sms();
                    sms.ID = Guid.NewGuid();
                    sms.CreateDate = PersianDateTime.Now;
                    sms.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    sms.Body = smsBody;
                    sms.Customer = _customerRepository.FindBy(customer.ID);
                    sms.RowVersion = 1;
                    _smsRepository.Add(sms);

                    ISmsWebService smsWebService = new ISmsWebService();
                    smsWebService.SendSms(smsBody, customer.Mobile1);
                }

                #endregion
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


        #region send SMS

        public GetGeneralResponse<IEnumerable<CustomerView>> SendSMS(IEnumerable<Guid> IDs, string message,Guid CreateEmployeeID)
        {
            FilterModel m = new FilterModel();
            
            GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();
            try
            {
                string query = "From Customer c where c.ID in(";
                int counter = IDs.Count();
                int temp = 0;

                foreach (Guid id in IDs)
                {
                    temp++;
                    if (temp == counter)
                        query += "'" + id + "')";
                    else
                        query += "'" + id + "',";
                }

                Response<Customer> customers = _customerRepository.FindAll(query);

                response.data = customers.data.ConvertToCustomerViews();

                foreach (CustomerView customer in response.data)
                {
                    string smsBody = ReplaceTemplate(message, customer);

                    Sms sms = new Sms();
                    sms.ID = Guid.NewGuid();
                    sms.CreateDate = PersianDateTime.Now;
                    sms.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    sms.Body = smsBody;
                    sms.Customer = _customerRepository.FindBy(customer.ID);
                    sms.RowVersion = 1;
                    _smsRepository.Add(sms);

                    ISmsWebService smsWebService = new ISmsWebService();
                    smsWebService.SendSms(smsBody, customer.Mobile1);
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

        #region Send Email

        public GetGeneralResponse<IEnumerable<CustomerView>> SendEmail(IEnumerable<Guid> IDs, string message, string Subject, Guid CreateEmployeeID)
        {
            GetGeneralResponse<IEnumerable<CustomerView>> response = new GetGeneralResponse<IEnumerable<CustomerView>>();
            try
            {
                string query = "From Customer c where c.ID in(";
                int counter = IDs.Count();
                int temp = 0;
                foreach (Guid id in IDs)
                {
                    temp++;
                    if (temp == counter)
                        query += "'" + id + "')";
                    else
                        query += "'" + id + "',";
                }

                Response<Customer> customers = _customerRepository.FindAll(query);

                response.data = customers.data.ConvertToCustomerViews();

                IList<string> recipients = new List<string>();

                foreach (CustomerView customer in response.data)
                {
                    // Replacing:
                    string emailBody = ReplaceTemplate(message, customer);

                    Email email = new Email();
                    email.ID = Guid.NewGuid();
                    email.CreateDate = PersianDateTime.Now;
                    email.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    email.Body = emailBody;
                    email.Customer = _customerRepository.FindBy(customer.ID);
                    email.Subject = customer.Name;
                    email.RowVersion = 1;
                    _emailRepository.Add(email);


                    _customerLevelService.SendEmail(customer.Email, customer.Name, email.Subject, emailBody);
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

        #endregion

        #endregion

        #region مشتریانی که در مراحل اویه هستند و یک فاکتور تحویل نشده و یک مالی دارند

        public GetGeneralResponse<IEnumerable<CustomerView>> GetCustomerMustoGoToRanje(int pageSize, int pageNumber, IList<FilterData> filter, IList<Sort> sort)
        {
            var response=new GetGeneralResponse<IEnumerable<CustomerView>>();
            int index = (pageNumber - 1) * pageSize;
            int count = pageSize;

            IList<FilterData> _filter=new List<FilterData>();

            _filter.Add(new FilterData()
            {
                data = new data()
                {
                    type = "boolean",
                    value = new[] { bool.TrueString },
                    comparison = "eq"
                },
                field = "Level.IsFirstLevel"

            });

            _filter.Add(new FilterData()
            {
                data = new data()
                {
                    type = "numeric",
                    value = new[] { "0" },
                    comparison = "gt"
                },
                field = "CanDeliverCost"
            });

            if(filter != null)
                foreach (var item in filter)
                {
                    _filter.Add(item);
                }

            string query = FilterUtilityService.GenerateFilterHQLQuery(_filter, "Customer", sort);

            Response<Customer> customers = _customerRepository.FindAll(query, index, count);
            customers.data.Where(x => x.Fiscals.Any(s => s.Confirm == ConfirmEnum.Confirmed));
            response.data = customers.data.ConvertToCustomerViews();
            response.totalCount = customers.totalCount;


            return response;
        }
        #endregion

        #region مشتری مربوط به سامانه تلفن

        public CustomerView GetCustomerByID(Guid CustomerID)
        {
            Customer customer = new Customer();
            customer = _customerRepository.FindBy(CustomerID);

            CustomerView customerView = new CustomerView();
            customerView = customer.ConvertToCustomerView();

            return customerView;
        }

        #endregion

        #region Suction Mode Report

        public GetGeneralResponse<IEnumerable<SuctionModeReportView>> GetSuctionModeReport(GetSuctionModeRequest request)
        {
            GetGeneralResponse<IEnumerable<SuctionModeReportView>> response =
                new GetGeneralResponse<IEnumerable<SuctionModeReportView>>();

            try
            {

                Response<Customer> customers=GetCustomerForReport(request);

                #region temp
                //IList<SuctionModeReportView> tempData=new List<SuctionModeReportView>();
                //foreach (var customer in customers.data)
                //{
                //    foreach (var item in suctionModedetails)
                //    {
                //        if (customer.SuctionModeDetail == item)
                //        {
                //            tempData.Add(new SuctionModeReportView()
                //            {
                //                SuctionModeName = item.SuctionModeDetailName,
                //                Count = 1
                //            });
                //        }
                //    }
                //}
                //IEnumerable<SuctionModeReportView> finalData=tempData.GroupBy(x => x.SuctionModeName)
                //    .Select(p => new SuctionModeReportView() {Count = p.Sum(c => c.Count),SuctionModeName =""});
                //var query1 = from bs in tempData
                //            group bs by bs.SuctionModeName into g
                //             select new SuctionModeReportView
                //            {
                //                SuctionModeName = g.First().SuctionModeName,

                //                Count = g.Sum(x => x.Count)
                //            };
                #endregion

                if (request.SuctionModeDetailIDs != null)
                {
                    var query1 = from bs in customers.data
                        group bs by bs.SuctionModeDetail.SuctionModeDetailName
                        into g
                        select new SuctionModeReportView
                        {
                            SuctionModeName = g.First().SuctionModeDetail.SuctionModeDetailName,

                            Count = g.Count()
                        };
                    response.data = query1;
                    response.totalCount = query1.Count();
                }
                else
                {
                    var query1 = from bs in customers.data
                                 group bs by bs.SuctionMode.SuctionModeName
                                     into g
                                     select new SuctionModeReportView
                                     {
                                         SuctionModeName = g.First().SuctionMode.SuctionModeName,
                                         
                                         Count = g.Count()
                                     };
                    response.data = query1;
                    response.totalCount = query1.Count();
                }
            }
            catch (Exception ex)
            {

                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException !=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }
            
            return response;
        }

        #endregion

        #region Get Cstomer For Report

        public GetGeneralResponse<IEnumerable<GetNetworkReportView>> GetNetworkReport(GetSuctionModeRequest request)
        {
            GetGeneralResponse<IEnumerable<GetNetworkReportView>> response =
    new GetGeneralResponse<IEnumerable<GetNetworkReportView>>();

            try
            {

                Response<Customer> customers = GetCustomerForReport(request);

                    var query1 = from bs in customers.data where bs.Network!=null
                                 group bs by bs.Network.ID
                                     into g
                                     select new GetNetworkReportView
                                     {
                                         NetworkName = g.First().Network.NetworkName,
                                         ID = g.First().ID,
                                         Count = g.Count()
                                     };
                    response.data = query1;
                    response.totalCount = query1.Count();

            }
            catch (Exception ex)
            {

                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<CenterReportView>> GetCenterReport(GetSuctionModeRequest request)
        {
            GetGeneralResponse<IEnumerable<CenterReportView>> response =
    new GetGeneralResponse<IEnumerable<CenterReportView>>();

            try
            {

                Response<Customer> customers = GetCustomerForReport(request);
                var list = customers.data.Where(x => x.Center != null).ToList();
                var query1 = from bs in list
                             where bs.Network != null
                             group bs by bs.Center.ID
                                 into g
                                 select new CenterReportView
                                 {
                                     CenterkName = g.First().Center.CenterName,
                                     ID = g.First().ID,
                                     Count = g.Count()
                                 };
                response.data = query1.ToList();
                response.totalCount = query1.Count();

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

        #region Get Customer For Campaign

        public GetGeneralResponse<IEnumerable<GetCustomerCampaignView>> GetCustomerForCampaign(IList<FilterData> filter)
        {
             GetGeneralResponse<IEnumerable<GetCustomerCampaignView>> response=new GetGeneralResponse<IEnumerable<GetCustomerCampaignView>>();

            string query = FilterUtilityService.GenerateFilterHQLQuery(filter,"Customer", null);
            Response<Customer> customers = _customerRepository.FindAll(query);

            IList<GetCustomerCampaignView> list=new List<GetCustomerCampaignView>();
            foreach (var item in customers.data)
            {
                GetCustomerCampaignView customerCampaign=new GetCustomerCampaignView();
                customerCampaign.CenterID = item.Center.ID;
                customerCampaign.CenterName = item.Center.CenterName;
                customerCampaign.HasFiscal = item.Fiscals.Any() ? true : false;
                customerCampaign.SuctionModeDetailID = item.SuctionModeDetail.ID;
                customerCampaign.SuctionModeDetailName = item.SuctionMode.SuctionModeName;
                customerCampaign.SuctionModeDetailName = item.SuctionModeDetail.SuctionModeDetailName;
                if (item.Supports.Any())
                    customerCampaign.InputSupporttDate =
                        item.Supports.OrderByDescending(x => x.CreateDate).FirstOrDefault().CreateDate;
                list.Add(customerCampaign);
            }

            response.data = list;
            response.totalCount = list.Count();

            return response;
        }

        #endregion

        #region Private Members

        #region Get Cstomer For Report

        private Response<Customer> GetCustomerForReport(GetSuctionModeRequest request)
        {
            IList<FilterData> filter = new List<FilterData>();


            #region preparing Filters



            #region Suction Mode

            if (request.SuctionModeDetailIDs != null)
            {
                if (request.SuctionModeDetailIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in request.SuctionModeDetailIDs)
                    {
                        Ids.Add(item.ToString());
                    }


                    filter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "list",
                            value = Ids.ToArray()
                        },
                        field = "SuctionModeDetail.ID"

                    });
                }
            }
            else if (request.SuctionModeIDs != null && request.SuctionModeDetailIDs == null)
            {
                if (request.SuctionModeIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in request.SuctionModeIDs)
                    {
                        Ids.Add(item.ToString());
                    }

                    filter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "list",
                            value = Ids.ToArray()
                        },
                        field = "SuctionMode.ID"

                    });
                }
            }

            #endregion

            #region Employee

            if (request.EmployeeIds != null)
                if (request.EmployeeIds.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in request.EmployeeIds)
                    {
                        Ids.Add(item.ToString());
                    }

                    filter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "list",
                            value = Ids.ToArray()
                        },
                        field = "CreateEmployee.ID"

                    });
                }

                    if (request.GroupIDs != null && request.EmployeeIds==null)
                        if (request.GroupIDs.Count() > 0)
                        {
                            IList<string> Ids = new List<string>();
                            foreach (var item in request.GroupIDs)
                            {
                                Ids.Add(item.ToString());
                            }

                            filter.Add(new FilterData()
                            {
                                data = new data()
                                {
                                    comparison = "eq",
                                    type = "list",
                                    value = Ids.ToArray()
                                },
                                field = "CreateEmployee.Group.ID"

                            });
                        }
                

            #endregion

            #region Date

            if (request.RegisterStartDate != null && request.RegisterEndDate == null)
            {
                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "date",
                        value = new[] { request.RegisterStartDate }
                    },
                    field = "CreateDate"

                });
            }

            if (request.RegisterStartDate == null && request.RegisterEndDate != null)
            {
                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "date",
                        value = new[] { request.RegisterEndDate }
                    },
                    field = "CreateDate"

                });
            }

            if (request.RegisterStartDate != null && request.RegisterEndDate != null)
            {
                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateBetween",
                        value = new[] { request.RegisterStartDate, request.RegisterEndDate }
                    },
                    field = "CreateDate"

                });
            }

            #endregion

            #region Network

            if (request.NetworkIDs != null)
                if (request.NetworkIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in request.NetworkIDs)
                    {
                        Ids.Add(item.ToString());
                    }

                    filter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "list",
                            value = Ids.ToArray()
                        },
                        field = "Network.ID"

                    });
                }

            #endregion

            #region Center

            if (request.CenterIDs != null)
                if (request.CenterIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in request.CenterIDs)
                    {
                        Ids.Add(item.ToString());
                    }

                    filter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "list",
                            value = Ids.ToArray()
                        },
                        field = "Center.ID"

                    });
                }

            #endregion

            #region Gender

            if (request.Gender != null)
                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        type = "string",
                        value = new[] { request.Gender },
                        comparison = "eq"
                    },
                    field = "Gender"

                });

            #endregion

            #region Agency

            if (request.AgencyIDs != null)
                if (request.AgencyIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in request.AgencyIDs)
                    {
                        Ids.Add(item.ToString());
                    }

                    filter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "list",
                            value = Ids.ToArray()
                        },
                        field = "Agency.ID"

                    });
                }

            #endregion

            #region Has Fiscal
            
                if (request.HasFiscal)
                {
                    filter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            type = "size",
                            value = new[] { "0" },
                            comparison = "gt"
                        },
                        field = "Fiscals"

                    });
                }

            #endregion

            #region Active Or Deactiver Users

            if (request.ActiveUsers==0)
                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        type = "boolean",
                        value = new[] { "False" },
                        comparison = "eq"
                    },
                    field = "CreateEmployee.Discontinued"

                });
            if (request.ActiveUsers == 1)
                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        type = "boolean",
                        value = new[] { "True" },
                        comparison = "eq"
                    },
                    field = "CreateEmployee.Discontinued"

                });

            #endregion

            #endregion

            string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "Customer", null);

            Response<Customer> customers = _customerRepository.FindAll(query);

            return customers;
        }
        #endregion

        //private long DeliverableCost(Guid customerID)
        //{
        //    // calculate sum of fiscals:
        //    Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
        //    Criterion criteria = new Criterion("Customer.ID", customerID, CriteriaOperator.Equal);
        //    query.Add(criteria);

        //    Response<Fiscal> fiscalsResponse = _fiscalRepository.FindBy(query, -1, -1);
        //    long sumFiscalCost = fiscalsResponse.data.Sum(s => s.ConfirmedCost.HasValue ? (long)s.ConfirmedCost : 0);

        //    // calculate sum of delivered sales:
        //    Response<Sale> salesResponse = _saleRepository.FindBy(query, -1, -1);
        //    long sumDeliveredCost = salesResponse.data.Sum(s => (long)s.SumCostOfDeliveredItems);

        //    return sumFiscalCost - sumDeliveredCost;
        //}


        private DocumentStatus GetDefaultDocumentStatus()
        {
            Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
            Criterion criteria = new Criterion("DefaultStatus", true, CriteriaOperator.Equal);
            query.Add(criteria);

            return _documentStatusRepository.FindBy(query).FirstOrDefault();
        }

        #endregion

    }
}


 