#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Fiscals.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.FiscalCatalogService;
using Model.Fiscals;
using Services.ViewModels.Fiscals;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees.Interfaces;
using Infrastructure.Querying;
using Infrastructure.Domain;
using Services.ViewModels.Employees;
#endregion

namespace Services.Implementations
{
    public class MoneyAccountService : IMoneyAccountService
    {
        #region Declares

        private readonly IMoneyAccountRepository _moneyAccountRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Ctor

        public MoneyAccountService(IMoneyAccountRepository moneyAccountRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository)
        {
            _moneyAccountRepository = moneyAccountRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Old Methods

        #region Add

        public GeneralResponse AddMoneyAccount(AddMoneyAccountRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                MoneyAccount moneyAccount = new MoneyAccount();
                moneyAccount.ID = Guid.NewGuid();
                moneyAccount.CreateDate = PersianDateTime.Now;
                moneyAccount.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                moneyAccount.AccountName = request.AccountName;
                moneyAccount.BAccountInfo = request.BAccountInfo;
                moneyAccount.BAccountNumber = request.BAccountNumber;
                moneyAccount.IsBankAccount = request.IsBankAccount;
                moneyAccount.Pay = request.Pay;
                moneyAccount.Receipt = request.Receipt;
                moneyAccount.Discontinued = request.Discontinued;
                moneyAccount.RowVersion = 1;

                _moneyAccountRepository.Add(moneyAccount);
                _uow.Commit();

                ////response.success = true;

                // Validation
                if (moneyAccount.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in moneyAccount.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
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

        public GeneralResponse EditMoneyAccount(EditMoneyAccountRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();
            MoneyAccount moneyAccount = new MoneyAccount();
            moneyAccount = _moneyAccountRepository.FindBy(request.ID);

            if (moneyAccount != null)
            {
                try
                {
                    moneyAccount.ModifiedDate = PersianDateTime.Now;
                    moneyAccount.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    moneyAccount.AccountName = request.AccountName;
                    moneyAccount.BAccountInfo = request.BAccountInfo;
                    moneyAccount.BAccountNumber = request.BAccountNumber;
                    moneyAccount.IsBankAccount = request.IsBankAccount;
                    moneyAccount.Pay = request.Pay;
                    moneyAccount.Receipt = request.Receipt;
                    moneyAccount.Discontinued = request.Discontinued;

                    if (moneyAccount.RowVersion != request.RowVersion)
                    {
                        
                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        moneyAccount.RowVersion += 1;
                    }

                    if (moneyAccount.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in moneyAccount.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _moneyAccountRepository.Save(moneyAccount);
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

        public GeneralResponse DeleteMoneyAccount(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            MoneyAccount moneyAccount = new MoneyAccount();
            moneyAccount = _moneyAccountRepository.FindBy(request.ID);

            if (moneyAccount != null)
            {
                try
                {
                    _moneyAccountRepository.Remove(moneyAccount);
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
        #endregion

        #region Get One
        public GetMoneyAccountResponse GetMoneyAccount(GetRequest request)
        {
            GetMoneyAccountResponse response = new GetMoneyAccountResponse();

            try
            {
                MoneyAccount moneyAccount = new MoneyAccount();
                MoneyAccountView moneyAccountView = moneyAccount.ConvertToMoneyAccountView();

                moneyAccount = _moneyAccountRepository.FindBy(request.ID);
                if (moneyAccount != null)
                    moneyAccountView = moneyAccount.ConvertToMoneyAccountView();

                response.MoneyAccountView = moneyAccountView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get Some

        public GetMoneyAccountsResponse GetMoneyAccounts()
        {
            GetMoneyAccountsResponse response = new GetMoneyAccountsResponse();

            try
            {
                IEnumerable<MoneyAccountView> moneyAccounts = _moneyAccountRepository.FindAll()
                    .ConvertToMoneyAccountViews();

                response.MoneyAccountViews = moneyAccounts;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }



        public GetMoneyAccountsResponse GetBankAccounts()
        {
            GetMoneyAccountsResponse response = new GetMoneyAccountsResponse();

            try
            {
                IEnumerable<MoneyAccountView> moneyAccounts = _moneyAccountRepository
                    .FindAll()
                    .Where(s => s.IsBankAccount)
                    .ConvertToMoneyAccountViews();

                response.MoneyAccountViews = moneyAccounts;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
        #endregion

        #endregion

        #region new Methods

        #region Read

        public GetGeneralResponse<IEnumerable<MoneyAccountView>> GetMoneyAccounts(bool isReceipt, int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<MoneyAccountView>> response = new GetGeneralResponse<IEnumerable<MoneyAccountView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                string param = isReceipt ? "Receipt" : "Pay";
                Criterion criteria = new Criterion(param, true, CriteriaOperator.Equal);
                query.Add(criteria);

                Response<MoneyAccount> moneyAccounts = _moneyAccountRepository.FindBy(query, index, count);

                response.data = moneyAccounts.data.ConvertToMoneyAccountViews().OrderBy(x=>x.SortOrder);
                response.totalCount = moneyAccounts.totalCount;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<MoneyAccountView>> GetAllMoneyAccounts( int pageSize, int pageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<MoneyAccountView>> response = new GetGeneralResponse<IEnumerable<MoneyAccountView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<MoneyAccount> moneyAccounts = _moneyAccountRepository.FindAllWithSort(index, count,null);

                response.data = moneyAccounts.data.ConvertToMoneyAccountViews().OrderBy(x=>x.SortOrder);
                response.totalCount = moneyAccounts.totalCount;
            }
            catch (Exception ex)
            {

            }

            return response;
        }



        #endregion

        #region Insert

        public GeneralResponse AddMoneyAccounts(IEnumerable<AddMoneyAccountRequest> requests, Guid CreatEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (AddMoneyAccountRequest request in requests)
                {
                    MoneyAccount moneyAccount = new MoneyAccount();
                    moneyAccount.ID = Guid.NewGuid();
                    moneyAccount.CreateDate = PersianDateTime.Now;
                    moneyAccount.CreateEmployee = _employeeRepository.FindBy(CreatEmployeeID);
                    moneyAccount.AccountName = request.AccountName;
                    moneyAccount.BAccountInfo = request.BAccountInfo;
                    moneyAccount.BAccountNumber = request.BAccountNumber;
                    moneyAccount.IsBankAccount = request.IsBankAccount;
                    moneyAccount.Pay = request.Pay;
                    moneyAccount.Receipt = request.Receipt;
                    moneyAccount.Discontinued = request.Discontinued;
                    moneyAccount.SortOrder = GetMaxSortOrder();
                    moneyAccount.RowVersion = 1;
                    moneyAccount.HasUniqueSerialNumber = request.HasUniqueSerialNumber;
                    moneyAccount.Has9Digits = request.Has9Digits;

                    _moneyAccountRepository.Add(moneyAccount);
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

        #region Update

        public GeneralResponse EditMoneyAccounts(IEnumerable<EditMoneyAccountRequest> requests, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (EditMoneyAccountRequest request in requests)
                {
                    MoneyAccount moneyAccount = new MoneyAccount();
                    moneyAccount = _moneyAccountRepository.FindBy(request.ID);
                    moneyAccount.ModifiedDate = PersianDateTime.Now;
                    moneyAccount.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                    moneyAccount.AccountName = request.AccountName;
                    moneyAccount.BAccountInfo = request.BAccountInfo;
                    moneyAccount.BAccountNumber = request.BAccountNumber;
                    moneyAccount.IsBankAccount = request.IsBankAccount;
                    moneyAccount.Pay = request.Pay;
                    moneyAccount.Receipt = request.Receipt;
                    moneyAccount.Discontinued = request.Discontinued;
                    moneyAccount.HasUniqueSerialNumber = request.HasUniqueSerialNumber;
                    moneyAccount.Has9Digits = request.Has9Digits;

                    if (moneyAccount.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        moneyAccount.RowVersion += 1;
                    }

                    #region Validation

                    if (moneyAccount.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in moneyAccount.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion
                    _moneyAccountRepository.Save(moneyAccount);
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

        public GeneralResponse DeleteMoneyAccounts(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                foreach (DeleteRequest request in requests)
                {
                    //MoneyAccount moneyAccount = _moneyAccountRepository.FindBy(request.ID);
                    //_moneyAccountRepository.Remove(moneyAccount);
                    _moneyAccountRepository.RemoveById(request.ID);
                }
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                    if(ex.InnerException != null)
                        response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Moving
        
        public MoveResponse MoveUp(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();
            
            MoneyAccount currentMoneyAccount = _moneyAccountRepository.FindBy(request.ID);

            MoneyAccount previewsMoneyAccount = new MoneyAccount();
            try
            {
                previewsMoneyAccount = _moneyAccountRepository.FindAll()
                                .Where(s => s.SortOrder < currentMoneyAccount.SortOrder)
                                .OrderByDescending(s => s.SortOrder)
                                .FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            if (currentMoneyAccount != null && previewsMoneyAccount != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = currentMoneyAccount.SortOrder;
                    int previews = previewsMoneyAccount.SortOrder;

                    currentMoneyAccount.SortOrder = previews;
                    previewsMoneyAccount.SortOrder = current;

                    _moneyAccountRepository.Save(currentMoneyAccount);
                    _moneyAccountRepository.Save(previewsMoneyAccount);
                    _uow.Commit();
                }
                catch (Exception ex)
                {
                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }
            }
            return response;
        }

        public MoveResponse MoveDown(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            MoneyAccount currentMoneyAccount = new MoneyAccount();
            currentMoneyAccount = _moneyAccountRepository.FindBy(request.ID);

            MoneyAccount nextMoneyAccount = new MoneyAccount();
            try
            {
                nextMoneyAccount = _moneyAccountRepository.FindAll()
                                .Where(s => s.SortOrder > currentMoneyAccount.SortOrder)
                                .OrderBy(s => s.SortOrder)
                                .FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            if (currentMoneyAccount != null && nextMoneyAccount != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = currentMoneyAccount.SortOrder;
                    int previews = nextMoneyAccount.SortOrder;

                    currentMoneyAccount.SortOrder = previews;
                    nextMoneyAccount.SortOrder = current;

                    _moneyAccountRepository.Save(currentMoneyAccount);
                    _moneyAccountRepository.Save(nextMoneyAccount);
                    _uow.Commit();
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }
            }

            return response;
        }

        #endregion

        #endregion

        #region Private Methods

        private int GetMaxSortOrder()
        {
            try
            {
                IList<Sort> sortOrders = new List<Sort>();
                sortOrders.Add(new Sort("SortOrder", false));

                Response<MoneyAccount> moneyAccoun = _moneyAccountRepository.FindAllWithSort(0, 1, sortOrders);

                return moneyAccoun.data.Max(s => s.SortOrder) + 1;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        #endregion
    }
}
