using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Store.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.StoreCatalogService;
using Model.Store;
using Services.ViewModels.Store;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees.Interfaces;
using Infrastructure.Domain;
using Infrastructure.Querying;

namespace Services.Implementations
{
    public class CreditServiceService : ICreditServiceService
    {
        #region Declares
        private readonly ICreditServiceRepository _creditServiceRepository;
        private readonly INetworkRepository _networkRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Ctor
        public CreditServiceService(ICreditServiceRepository creditServiceRepository, IUnitOfWork uow)
        {
            _creditServiceRepository = creditServiceRepository;
            _uow = uow;
        }

        public CreditServiceService(ICreditServiceRepository creditServiceRepository, INetworkRepository networkRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository)
            : this(creditServiceRepository, uow)
        {
            this._networkRepository = networkRepository;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Old Methods

        #region Add
        public GeneralResponse AddCreditService(AddCreditServiceRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                CreditService creditService = new CreditService();
                creditService.ID = Guid.NewGuid();
                creditService.CreateDate = PersianDateTime.Now;
                creditService.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                creditService.CreditServiceCode = request.CreditServiceCode;
                creditService.Discontinued = request.Discontinued;
                creditService.ExpDays = request.ExpDays;
                creditService.Imposition = request.Imposition;
                creditService.MaxDiscount = request.MaxDiscount;
                creditService.Network = this._networkRepository.FindBy(request.NetworkID);
                creditService.PurchaseUnitPrice = request.PurchaseUnitPrice;
                creditService.ResellerUnitPrice = request.ResellerUnitPrice;
                creditService.ServiceName = request.ServiceName;
                creditService.UnitPrice = request.UnitPrice;
                creditService.Note = request.Note;
                creditService.Discontinued = request.Discontinued;
                creditService.SortOrder = GetSortOrder();
                creditService.RowVersion = 1;

                // Validation
                if (creditService.GetBrokenRules().Count() > 0)
                {


                    foreach (BusinessRule businessRule in creditService.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                _creditServiceRepository.Add(creditService);
                _uow.Commit();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Substring(0, 177) == @"The INSERT statement conflicted with the CHECK constraint ""CK_CreditService"". The conflict occurred in database ""Saman"", table ""Store.CreditService"", column 'CreditServiceCode'.")
                {
                    response.ErrorMessages.Add("MustBeBegin3000Key");
                }
                else
                {
                    response.ErrorMessages.Add(ex.Message);
                    if (ex.InnerException != null)
                        response.ErrorMessages.Add(ex.InnerException.Message);
                }
            }

            return response;
        }
        #endregion

        #region Edit
        public GeneralResponse EditCreditService(EditCreditServiceRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();
            CreditService creditService = new CreditService();
            creditService = _creditServiceRepository.FindBy(request.ID);

            if (creditService != null)
            {
                try
                {
                    creditService.ModifiedDate = PersianDateTime.Now;
                    creditService.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                        creditService.CreditServiceCode = request.CreditServiceCode;
                        creditService.Discontinued = request.Discontinued;
                        creditService.ExpDays = request.ExpDays;
                        creditService.Imposition = request.Imposition;
                        creditService.MaxDiscount = request.MaxDiscount;
                        creditService.Network = this._networkRepository.FindBy(request.NetworkID);
                        creditService.PurchaseUnitPrice = request.PurchaseUnitPrice;
                        creditService.ResellerUnitPrice = request.ResellerUnitPrice;
                    if (request.ServiceName != null)
                        creditService.ServiceName = request.ServiceName;
                        creditService.UnitPrice = request.UnitPrice;
                    if (request.Note != null)
                        creditService.Note = request.Note;
                        creditService.Discontinued = request.Discontinued;

                    if (creditService.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        creditService.RowVersion += 1;
                    }

                    if (creditService.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in creditService.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _creditServiceRepository.Save(creditService);
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
        public GeneralResponse DeleteCreditService(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            CreditService creditService = new CreditService();
            creditService = _creditServiceRepository.FindBy(request.ID);

            if (creditService != null)
            {
                try
                {
                    _creditServiceRepository.Remove(creditService);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);

                    if (ex.Message.Substring(0, 40) == "Cannot insert the value NULL into column"
                       || ex.InnerException.Message.Substring(0, 40) == "Cannot insert the value NULL into column")
                    {
                        response.ErrorMessages.Clear();
                        response.ErrorMessages.Add("CanNotDeleteEmployeeKey");
                    }
                    else
                    {
                        if (ex.InnerException != null)
                        {
                            response.ErrorMessages.Add("FIRST INNER EXPCEPTION: " + ex.InnerException.Message);
                            if (ex.InnerException.InnerException != null)
                                response.ErrorMessages.Add("SECOND INNER EXPCEPTION: " + ex.InnerException.InnerException.Message);
                        }
                    }
                }
            }

            return response;
        }
        #endregion

        #region Get One
        public GetCreditServiceResponse GetCreditService(GetRequest request)
        {
            GetCreditServiceResponse response = new GetCreditServiceResponse();

            try
            {
                CreditService creditService = new CreditService();
                CreditServiceView creditServiceView = creditService.ConvertToCreditServiceView();

                creditService = _creditServiceRepository.FindBy(request.ID);
                if (creditService != null)
                    creditServiceView = creditService.ConvertToCreditServiceView();

                response.CreditServiceView = creditServiceView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get All
        public GetCreditServicesResponse GetCreditServices()
        {
            GetCreditServicesResponse response = new GetCreditServicesResponse();

            try
            {
                IEnumerable<CreditServiceView> creditServices = _creditServiceRepository.FindAll()
                    .OrderBy(o => o.SortOrder)
                    .ConvertToCreditServiceViews();

                response.CreditServiceViews = creditServices;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        

        #endregion

        #region New Methods

        #region Read

        public GetGeneralResponse<IEnumerable<CreditServiceView>> GetCreditServices(int pageSize, int pageNumber, IList<Sort> sort,IList<FilterData> filter)
        {
            GetGeneralResponse<IEnumerable<CreditServiceView>> response = new GetGeneralResponse<IEnumerable<CreditServiceView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "CreditService", sort);

                Response<CreditService> creditService = new Response<CreditService>();
                creditService = _creditServiceRepository.FindAll(query, index, count);

                response.data = creditService.data.ConvertToCreditServiceViews();
                response.totalCount = creditService.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<CreditServiceView>> GetCreditServices(int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<CreditServiceView>> response = new GetGeneralResponse<IEnumerable<CreditServiceView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;


                Response<CreditService> creditService = new Response<CreditService>();
                creditService = _creditServiceRepository.FindAll(index, count);

                response.data = creditService.data.ConvertToCreditServiceViews();
                response.totalCount = creditService.totalCount;
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

        #region Insert

        public GeneralResponse AddCreditServices(AddCreditServiceRequest request, Guid EmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {

                CreditService creditService = new CreditService();

                #region Validate Uncredit Service Code

                string _errorMessag = AddValidate(request.CreditServiceCode, request.ServiceName);

                if (_errorMessag != "NoError")
                {
                    response.ErrorMessages.Add(_errorMessag);
                    return response;
                }


                #endregion

                if (request.MaxDiscount > request.UnitPrice)
                {
                    response.ErrorMessages.Add("تخفیف نمیتواند بیش از قیمت پایه باشد");
                    return response;
                }

                creditService.ID = Guid.NewGuid();
                creditService.CreateDate = PersianDateTime.Now;
                creditService.CreateEmployee = _employeeRepository.FindBy(EmployeeID);
                creditService.CreditServiceCode = request.CreditServiceCode;
                creditService.Discontinued = request.Discontinued;
                creditService.ExpDays = request.ExpDays;
                creditService.Imposition = request.Imposition;
                creditService.MaxDiscount = request.MaxDiscount;
                creditService.Network = this._networkRepository.FindBy(request.NetworkID);
                creditService.PurchaseUnitPrice = request.PurchaseUnitPrice;
                creditService.ResellerUnitPrice = request.ResellerUnitPrice;
                creditService.ServiceName = request.ServiceName;
                creditService.UnitPrice = request.UnitPrice;
                creditService.Note = request.Note;
                creditService.Discontinued = request.Discontinued;
                creditService.Bonus = request.Bonus;
                creditService.Comission = request.Comission;
                creditService.SortOrder = GetSortOrder();
                creditService.RowVersion = 1;

                #region validation

                if (creditService.GetBrokenRules().Count() > 0)
                {


                    foreach (BusinessRule businessRule in creditService.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }
                #endregion

                _creditServiceRepository.Add(creditService);
                _uow.Commit();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Substring(0, 177) == @"The INSERT statement conflicted with the CHECK constraint ""CK_CreditService"". The conflict occurred in database ""Saman"", table ""Store.CreditService"", column 'CreditServiceCode'.")
                {
                    response.ErrorMessages.Add("MustBeBegin3000Key");
                }
                else
                {
                    response.ErrorMessages.Add(ex.Message);
                    if (ex.InnerException != null)
                        response.ErrorMessages.Add(ex.InnerException.Message);
                }
            }

            return response;
        }

        #endregion

        #region Edit

        public GeneralResponse EditCreditServices(EditCreditServiceRequest request, Guid EmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                CreditService creditService = _creditServiceRepository.FindBy(request.ID);

                if (creditService != null)
                {

                    #region Validate Uncredit Service Code

                    string _errorMessag = EditValidate(request.CreditServiceCode, request.ServiceName, request.ID);

                    if (_errorMessag != "NoError")
                    {
                        response.ErrorMessages.Add(_errorMessag);
                        return response;
                    }

                    #endregion


                    if (request.MaxDiscount > request.UnitPrice)
                    {
                        response.ErrorMessages.Add("تخفیف نمیتواند بیش از قیمت پایه باشد");
                        return response;
                    }

                    creditService.ModifiedDate = PersianDateTime.Now;
                    creditService.ModifiedEmployee = _employeeRepository.FindBy(EmployeeID);
                    creditService.CreditServiceCode = request.CreditServiceCode;
                    creditService.Discontinued = request.Discontinued;
                    creditService.ExpDays = request.ExpDays;
                    creditService.Imposition = request.Imposition;
                    creditService.MaxDiscount = request.MaxDiscount;
                    if (creditService.Network.ID!=request.NetworkID)
                    creditService.Network = this._networkRepository.FindBy(request.NetworkID);
                    creditService.PurchaseUnitPrice = request.PurchaseUnitPrice;
                    creditService.ResellerUnitPrice = request.ResellerUnitPrice;
                    creditService.Bonus = request.Bonus;
                    creditService.Comission = request.Comission;
                    if (request.ServiceName != null)
                        creditService.ServiceName = request.ServiceName;
                    creditService.UnitPrice = request.UnitPrice;
                    if (request.Note != null)
                        creditService.Note = request.Note;
                    creditService.Discontinued = request.Discontinued;

                    #region Validation

                    if (creditService.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        creditService.RowVersion += 1;
                    }

                    if (creditService.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in creditService.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _creditServiceRepository.Save(creditService);
                }
                else
                {

                    response.ErrorMessages.Add("NoItemToEditKey");
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
        public GeneralResponse DeleteCreditServices(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                CreditService creditService = new CreditService();

                foreach (DeleteRequest request in requests)
                {
                    

                    if (creditService != null)
                    {
                        try
                        {
                            _creditServiceRepository.RemoveById(request.ID);

                            ////response.success = true;
                        }
                        catch (Exception ex)
                        {

                            response.ErrorMessages.Add(ex.Message);

                            if (ex.Message.Substring(0, 40) == "Cannot insert the value NULL into column"
                               || ex.InnerException.Message.Substring(0, 40) == "Cannot insert the value NULL into column")
                            {
                                response.ErrorMessages.Clear();
                                response.ErrorMessages.Add("CanNotDeleteEmployeeKey");
                            }
                            else
                            {
                                if (ex.InnerException != null)
                                {
                                    response.ErrorMessages.Add("FIRST INNER EXPCEPTION: " + ex.InnerException.Message);
                                    if (ex.InnerException.InnerException != null)
                                        response.ErrorMessages.Add("SECOND INNER EXPCEPTION: " + ex.InnerException.InnerException.Message);
                                }
                            }
                        }
                    }
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

        #region Moving

        public MoveResponse MoveUp(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            // Current Agency is the Request
            CreditService currentCreditService = new CreditService();
            currentCreditService = _creditServiceRepository.FindBy(request.ID);

            // Find the Previews Agency
            CreditService previewsCreditService = new CreditService();
            try
            {
                previewsCreditService = _creditServiceRepository.FindAll()
                                .Where(s => s.SortOrder < currentCreditService.SortOrder)
                                .OrderByDescending(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (currentCreditService != null && previewsCreditService != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = currentCreditService.SortOrder;
                    int previews = previewsCreditService.SortOrder;

                    currentCreditService.SortOrder = previews;
                    previewsCreditService.SortOrder = current;

                    _creditServiceRepository.Save(currentCreditService);
                    _creditServiceRepository.Save(previewsCreditService);
                    _uow.Commit();
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }

                ////response.success = true;
            }

            return response;
        }

        public MoveResponse MoveDown(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            // Current Agency is the Request
            CreditService currentCreditService = new CreditService();
            currentCreditService = _creditServiceRepository.FindBy(request.ID);

            // Find the Previews Agency
            CreditService nextCreditService = new CreditService();
            try
            {
                nextCreditService = _creditServiceRepository.FindAll()
                                .Where(s => s.SortOrder > currentCreditService.SortOrder)
                                .OrderBy(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (currentCreditService != null && nextCreditService != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = currentCreditService.SortOrder;
                    int previews = nextCreditService.SortOrder;

                    currentCreditService.SortOrder = previews;
                    nextCreditService.SortOrder = current;

                    _creditServiceRepository.Save(currentCreditService);
                    _creditServiceRepository.Save(nextCreditService);
                    _uow.Commit();
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }

                ////response.success = true;
            }

            return response;
        }

        private int GetSortOrder()
        {
            try
            {
                IEnumerable<CreditService> CreditServices = _creditServiceRepository.FindAll();
                return CreditServices.Max(s => s.SortOrder) + 1;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }
        #endregion

        #endregion

        #region Private Methods

        private string AddValidate(int code, string title)
        {
            string errorMssage = "NoError";

            if (code < 6001 || code > 9000)
            {
                errorMssage = "کد خدمات اعتباری باید بزرگتر از 6000 و کوچکتر از 9001 باشد";
                return errorMssage;
            }

            IEnumerable<CreditService> creditServices = _creditServiceRepository.FindAll();


            foreach (CreditService creditService in creditServices)
            {
                if (creditService.CreditServiceCode == code)
                {
                    errorMssage = " این کد خدمات اعتباری قبلا برای " + creditService.ServiceName + " ثبت شده است ";
                }
                if (creditService.ServiceName == title)
                {
                    errorMssage = " خدمات اعتباری با نام " + creditService.ServiceName + "  قبلا ثبت شده است ";
                }
            }
            return errorMssage;
        }

        private string EditValidate(int code, string title, Guid ID)
        {
            string errorMssage = "NoError";

            if (code < 6001 || code > 9000)
            {
                errorMssage = "کد خدمات اعتباری باید بزرگتر از 6000 و کوچکتر از 9001 باشد";
                return errorMssage;
            }

            IEnumerable<CreditService> creditServices = _creditServiceRepository.FindAll();


            foreach (CreditService creditService in creditServices)
            {
                if (creditService.CreditServiceCode == code)
                {
                    if (creditService.ID != ID && creditService.ServiceName != title)
                        errorMssage = " این کد خدمات اعتباری قبلا برای " + creditService.ServiceName + " ثبت شده است ";
                }
                if (creditService.ServiceName == title && creditService.ID != ID)
                {
                    errorMssage = " خدمات اعتباری با نام " + creditService.ServiceName + " قبلا ثبت شده است ";
                }
            }
            return errorMssage;
        }


        #endregion

    }
}
