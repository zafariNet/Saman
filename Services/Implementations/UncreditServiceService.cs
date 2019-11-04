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
using Model.Employees;
using Model.Employees.Interfaces;
using Infrastructure.Domain;
using Infrastructure.Querying;

namespace Services.Implementations
{
    public class UncreditServiceService : IUncreditServiceService
    {
        #region Declares
        private readonly IUncreditServiceRepository _uncreditServiceRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Ctor
        public UncreditServiceService(IUncreditServiceRepository uncreditServiceRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository)
        {
            _uncreditServiceRepository = uncreditServiceRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Old Methods

        #region Add
        public GeneralResponse AddUncreditService(AddUncreditServiceRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                UncreditService uncreditService = new UncreditService();
                uncreditService.ID = Guid.NewGuid();
                uncreditService.CreateDate = PersianDateTime.Now;
                uncreditService.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                uncreditService.Imposition = request.Imposition;
                uncreditService.MaxDiscount = request.MaxDiscount;
                uncreditService.UnCreditServiceCode = request.UnCreditServiceCode;
                uncreditService.UncreditServiceName = request.UncreditServiceName;
                uncreditService.UnitPrice = request.UnitPrice;
                uncreditService.Note = request.Note;
                uncreditService.Discontinued = request.Discontinued;
                uncreditService.SortOrder = GetSortOrder();
                uncreditService.RowVersion = 1;

                // Validation
                if (uncreditService.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in uncreditService.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                _uncreditServiceRepository.Add(uncreditService);
                _uow.Commit();

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Substring(0, 183) == @"The INSERT statement conflicted with the CHECK constraint ""CK_UnCreditService"". The conflict occurred in database ""Saman"", table ""Store.UncreditService"", column 'UnCreditServiceCode'.")
                {
                    response.ErrorMessages.Add("کد خدمات غیراعتباری باید از 5000 شروع شود.");
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
        public GeneralResponse EditUncreditService(EditUncreditServiceRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();
            UncreditService uncreditService = new UncreditService();
            uncreditService = _uncreditServiceRepository.FindBy(request.ID);

            if (uncreditService != null)
            {
                try
                {
                    uncreditService.ModifiedDate = PersianDateTime.Now;
                    uncreditService.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                        uncreditService.Imposition = request.Imposition;
                        uncreditService.MaxDiscount = request.MaxDiscount;
                    if (request.UnCreditServiceCode != null)
                        uncreditService.UnCreditServiceCode = request.UnCreditServiceCode;
                    if (request.UncreditServiceName != null)
                        uncreditService.UncreditServiceName = request.UncreditServiceName;
                        uncreditService.UnitPrice = request.UnitPrice;
                    if (request.Note != null)
                        uncreditService.Note = request.Note;
                        uncreditService.Discontinued = request.Discontinued;

                    if (uncreditService.RowVersion != request.RowVersion)
                    {
                        
                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        uncreditService.RowVersion += 1;
                    }

                    if (uncreditService.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in uncreditService.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _uncreditServiceRepository.Save(uncreditService);
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
        #endregion

        #region Delete
        public GeneralResponse DeleteUncreditService(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            UncreditService uncreditService = new UncreditService();
            uncreditService = _uncreditServiceRepository.FindBy(request.ID);

            if (uncreditService != null)
            {
                try
                {
                    _uncreditServiceRepository.Remove(uncreditService);
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
                        response.ErrorMessages.Add("این کاربر در سیستم اطلاعات ثبت کرده است و قادر به حذف آن نیستید. به جای حذف می توانید از گزینه ویرایش > غیر فعال استفاده کنید.");
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
        public GetUncreditServiceResponse GetUncreditService(GetRequest request)
        {
            GetUncreditServiceResponse response = new GetUncreditServiceResponse();

            try
            {
                UncreditService uncreditService = new UncreditService();
                UncreditServiceView uncreditServiceView = uncreditService.ConvertToUncreditServiceView();

                uncreditService = _uncreditServiceRepository.FindBy(request.ID);
                if (uncreditService != null)
                    uncreditServiceView = uncreditService.ConvertToUncreditServiceView();

                response.UncreditServiceView = uncreditServiceView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get All
        public GetUnCreditServicesResponse GetUncreditServices()
        {
            GetUnCreditServicesResponse response = new GetUnCreditServicesResponse();

            try
            {
                IEnumerable<UncreditServiceView> unCreditServices = _uncreditServiceRepository.FindAll()
                    .OrderBy(o => o.SortOrder)
                    .ConvertToUncreditServiceViews();

                response.UncreditServiceViews = unCreditServices;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
        #endregion

        

        #endregion

        #region New Methods

        #region Read

        public GetGeneralResponse<IEnumerable<UncreditServiceView>> GetUncreditServices(int pageSize, int pageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<UncreditServiceView>> response = new GetGeneralResponse<IEnumerable<UncreditServiceView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<UncreditService> unCreditservice=_uncreditServiceRepository.FindAllWithSort(index,count,sort);
                response.data=unCreditservice.data.ConvertToUncreditServiceViews();
                response.totalCount=unCreditservice.totalCount;

            }
            catch(Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;

        }

        public GetGeneralResponse<IEnumerable<UncreditServiceView>> GetUncreditServices(int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<UncreditServiceView>> response = new GetGeneralResponse<IEnumerable<UncreditServiceView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<UncreditService> unCreditservice = _uncreditServiceRepository.FindAll(index, count);
                response.data = unCreditservice.data.ConvertToUncreditServiceViews();
                response.totalCount = unCreditservice.totalCount;

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

        public GeneralResponse AddUncreditServices(AddUncreditServiceRequest request, Guid EmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                UncreditService uncreditService = new UncreditService();

                if (request.MaxDiscount > request.UnitPrice)
                {
                    response.ErrorMessages.Add("تخفیف نمیتواند بیش از قیمت پایه باشد");
                    return response;
                }


                #region Validate Uncredit Service Code

                string _errorMessag = AddValidate(request.UnCreditServiceCode, request.UncreditServiceName);

                if (_errorMessag != "NoError")
                {
                    response.ErrorMessages.Add(_errorMessag);
                    return response;
                }


                #endregion

                uncreditService.ID = Guid.NewGuid();
                uncreditService.CreateDate = PersianDateTime.Now;
                uncreditService.CreateEmployee = _employeeRepository.FindBy(EmployeeID);
                uncreditService.UncreditServiceName = request.UncreditServiceName;
                uncreditService.UnCreditServiceCode = request.UnCreditServiceCode;
                uncreditService.UnitPrice = request.UnitPrice;
                uncreditService.MaxDiscount = request.MaxDiscount;
                uncreditService.Imposition = request.Imposition;
                uncreditService.Discontinued = request.Discontinued;
                uncreditService.Bonus = request.Bonus;
                uncreditService.Comission = request.Comission;
                uncreditService.Note = request.Note;
                uncreditService.SortOrder = GetSortOrder();

                _uncreditServiceRepository.Add(uncreditService);
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

        public GeneralResponse EditUncreditServices(EditUncreditServiceRequest request, Guid EmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                UncreditService uncreditService = new UncreditService();

                if (request.MaxDiscount > request.UnitPrice)
                {
                    response.ErrorMessages.Add("تخفیف نمیتواند بیش از قیمت پایه باشد");
                    return response;
                }


                uncreditService = _uncreditServiceRepository.FindBy(request.ID);

                #region Validate Uncredit Service Code

                string _errorMessag = EditValidate(request.UnCreditServiceCode, request.UncreditServiceName, request.ID);

                if (_errorMessag != "NoError")
                {
                    response.ErrorMessages.Add(_errorMessag);
                    return response;
                }

                #endregion

                if (uncreditService != null)
                {
                    uncreditService.ModifiedDate = PersianDateTime.Now;
                    uncreditService.ModifiedEmployee = _employeeRepository.FindBy(EmployeeID);
                    if (request.UncreditServiceName != null)
                        uncreditService.UncreditServiceName = request.UncreditServiceName;
                    if (request.UnCreditServiceCode != null)
                        uncreditService.UnCreditServiceCode = request.UnCreditServiceCode;
                    if (request.UnitPrice != null)
                        uncreditService.UnitPrice = request.UnitPrice;
                    if (request.MaxDiscount != null)
                        uncreditService.MaxDiscount = request.MaxDiscount;
                    if (request.Imposition != null)
                        uncreditService.Imposition = request.Imposition;
                    if (request.Discontinued != null)
                        uncreditService.Discontinued = request.Discontinued;
                    if (request.Note != null)
                        uncreditService.Note = request.Note;
                    if (request.SortOrder != null)
                        uncreditService.SortOrder = request.SortOrder;
                    uncreditService.Comission = request.Comission;
                    uncreditService.Bonus = request.Bonus;

                    #region Validation

                    if (uncreditService.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        uncreditService.RowVersion += 1;
                    }

                    if (uncreditService.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in uncreditService.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _uncreditServiceRepository.Save(uncreditService);
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

        public GeneralResponse DeleteUncreditServices(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (DeleteRequest request in requests)
                {
                    UncreditService uncreditService = _uncreditServiceRepository.FindBy(request.ID);
                    _uncreditServiceRepository.Remove(uncreditService);
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
            UncreditService currentUncreditService = new UncreditService();
            currentUncreditService = _uncreditServiceRepository.FindBy(request.ID);

            // Find the Previews Agency
            UncreditService previewsUncreditService = new UncreditService();
            try
            {
                previewsUncreditService = _uncreditServiceRepository.FindAll()
                                .Where(s => s.SortOrder < currentUncreditService.SortOrder)
                                .OrderByDescending(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (currentUncreditService != null && previewsUncreditService != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = (int)currentUncreditService.SortOrder;
                    int previews = (int)previewsUncreditService.SortOrder;

                    currentUncreditService.SortOrder = previews;
                    previewsUncreditService.SortOrder = current;

                    _uncreditServiceRepository.Save(currentUncreditService);
                    _uncreditServiceRepository.Save(previewsUncreditService);
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
            UncreditService currentUncreditService = new UncreditService();
            currentUncreditService = _uncreditServiceRepository.FindBy(request.ID);

            // Find the Previews Agency
            UncreditService nextUncreditService = new UncreditService();
            try
            {
                nextUncreditService = _uncreditServiceRepository.FindAll()
                                .Where(s => s.SortOrder > currentUncreditService.SortOrder)
                                .OrderBy(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (currentUncreditService != null && nextUncreditService != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = (int)currentUncreditService.SortOrder;
                    int previews = (int)nextUncreditService.SortOrder;

                    currentUncreditService.SortOrder = previews;
                    nextUncreditService.SortOrder = current;

                    _uncreditServiceRepository.Save(currentUncreditService);
                    _uncreditServiceRepository.Save(nextUncreditService);
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
                IEnumerable<UncreditService> UncreditServices = _uncreditServiceRepository.FindAll();
                return UncreditServices.Max(s => (int)s.SortOrder) + 1;
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

            if (code < 3001 || code > 6000)
            {
                errorMssage = "کد محصول باید بزرگتر از 3001 و کوچکتر از 6000 باشد";
                return errorMssage;
            }
            //Query query = new Query();

            //Criterion criteriaCode = new Criterion("ProductPriceCode", code, CriteriaOperator.Equal);
            //query.Add(criteriaCode);
            //Criterion criteriatitle = new Criterion("ProductPriceTitle", title, CriteriaOperator.Equal);
            //query.Add(criteriatitle);

            IEnumerable<UncreditService> uncreditServices = _uncreditServiceRepository.FindAll();


            foreach (UncreditService uncreditService in uncreditServices)
            {
                if (uncreditService.UnCreditServiceCode == code)
                {
                    errorMssage = " این کد خدمات غیر اعتباری قبلا برای " + uncreditService.UncreditServiceName + " ثبت شده است ";
                }
                if (uncreditService.UncreditServiceName == title)
                {
                    errorMssage = " خدمات غیر اعتباری با نام " + uncreditService.UncreditServiceName + "  قبلا ثبت شده است  ";
                }
            }
            return errorMssage;
        }

        private string EditValidate(int code, string title, Guid ID)
        {
            string errorMssage = "NoError";

            if (code < 3001 || code > 6000)
            {
                errorMssage = "کد محصول باید بزرگتر از 100 و کوچکتر از 3001 باشد";
                return errorMssage;
            }

            IEnumerable<UncreditService> uncreditServices = _uncreditServiceRepository.FindAll();


            foreach (UncreditService uncreditService in uncreditServices)
            {
                if (uncreditService.UnCreditServiceCode == code)
                {
                    if (uncreditService.ID != ID && uncreditService.UncreditServiceName != title)
                        errorMssage = " این کد خدمات غیر اعتباری قبلا برای " + uncreditService.UncreditServiceName + " ثبت شده است ";
                }
                if (uncreditService.UncreditServiceName == title && uncreditService.ID != ID)
                {
                    errorMssage = " یک خدمات غیر اعتباری با نام " + uncreditService.UncreditServiceName + " فبلا  ثبت شده است ";
                }
            }
            return errorMssage;
        }

        #endregion
    }
}
