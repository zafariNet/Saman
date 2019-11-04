#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.Interfaces;
using Model.Fiscals.Interfaces;
using Model.Employees.Interfaces;
using Model.Customers.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.FiscalCatalogService;
using Model.Fiscals;
using Services.ViewModels.Fiscals;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Infrastructure.Querying;
using Model.Employees;
using Infrastructure.Domain;
using Services.ViewModels.Reports;
using Query = Infrastructure.Querying.Query;

#endregion

namespace Services.Implementations
{
    public class FiscalService : IFiscalService
    {
        #region Declares

        private readonly IFiscalRepository _fiscalRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMoneyAccountRepository _moneyAccountRepository;
        private readonly IUnitOfWork _uow;
        #endregion

        #region Ctor

        public FiscalService(IFiscalRepository fiscalRepository, IUnitOfWork uow)
        {
            _fiscalRepository = fiscalRepository;
            _uow = uow;
        }

        public FiscalService(IFiscalRepository fiscalRepository, IEmployeeRepository employeeRepository, ICustomerRepository customerRepository, 
            IMoneyAccountRepository moneyAccountRepository
            , IUnitOfWork uow)
            : this(fiscalRepository, uow)
        {
            this._employeeRepository = employeeRepository;
            this._customerRepository = customerRepository;
            _moneyAccountRepository = moneyAccountRepository;
        }
        #endregion

        #region Add

        


        public GeneralResponse AddFiscal(AddFiscalRequest request)
        {
            GeneralResponse response = new GeneralResponse();


            try
            {
                Fiscal fiscal = new Fiscal();
                fiscal.ID = Guid.NewGuid();
                fiscal.CreateDate = PersianDateTime.Now;
                fiscal.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                fiscal.Cost = request.Cost;
                if (request.CustomerID != null && request.CustomerID != Guid.Empty)
                    fiscal.Customer = this._customerRepository.FindBy(request.CustomerID);
                else if (request.Phone != null)
                {
                    fiscal.Customer = _customerRepository.FindByPhoneCode(request.Phone).First();
                }

                fiscal.DocumentSerial = request.DocumentSerial;
                fiscal.DocumentType = request.DocumentType;
                fiscal.Note = request.Note;
                fiscal.MoneyAccount = _moneyAccountRepository.FindBy(request.MoneyAccountID);
                fiscal.InvestDate = request.InvestDate;
                fiscal.Confirm = ConfirmEnum.NotChecked;
                fiscal.ChargeStatus = ChargeStatus.NotChecked;
                fiscal.ForCharge = request.ForCharge;
                fiscal.FollowNumber = NewFollowNumber;
                fiscal.RowVersion = 1;

                #region Validation

                if (fiscal.GetBrokenRules().Count() > 0)
                {
                    foreach (BusinessRule businessRule in fiscal.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }
                    return response;
                }

                #endregion

                _fiscalRepository.Add(fiscal);

                _uow.Commit();

                // for retriving FollowNumber
                response.ObjectAdded = _fiscalRepository.FindBy(fiscal.ID).ConvertToFiscalView();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                {
                    response.ErrorMessages.Add(ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null)
                        response.ErrorMessages.Add(ex.InnerException.InnerException.Message);
                }
            }

            return response;
        }

        private long NewFollowNumber
        {
            get
            {
                Fiscal fiscal = _fiscalRepository.FindAll("From Fiscal f Order by f.FollowNumber Desc", 0, 1).data.FirstOrDefault();

                if (fiscal != null && fiscal.FollowNumber != 0)
                {
                    return fiscal.FollowNumber + 1;
                }

                else return 1;
            }
        }
        private long NewAccountingSerialNumber
        {
            get
            {
                Fiscal fiscal = _fiscalRepository.FindAll("From Fiscal f Order by f.AccountingSerialNumber Desc", 0, 1).data.FirstOrDefault();

                if (fiscal != null && fiscal.AccountingSerialNumber != 0)
                {
                    return fiscal.FollowNumber + 1;
                }

                else return 1;
            }
        }
        #endregion

        #region Edit

        public GeneralResponse EditFiscal(EditFiscalRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            Fiscal fiscal = new Fiscal();
            fiscal = _fiscalRepository.FindBy(request.ID);

            if (fiscal != null)
            {
                try
                {
                    // Check If Editable or not
                    if ((int)fiscal.Confirm == 2)
                    {
                        response.ErrorMessages.Add("FiscalCanNotEditKey");
                        return response;
                    }

                    fiscal.ModifiedDate = PersianDateTime.Now;
                    fiscal.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    fiscal.Cost = request.Cost;
                    fiscal.DocumentSerial = request.DocumentSerial;
                    fiscal.DocumentType = request.DocumentType;
                    fiscal.Note = request.Note;
                    fiscal.InvestDate = request.InvestDate;
                    if (fiscal.MoneyAccount.ID!=request.ModifiedEmployeeID)
                    fiscal.MoneyAccount = _moneyAccountRepository.FindBy(request.MoneyAccountID);
                    fiscal.ChargeStatus = request.ChargeStatus;
                    fiscal.SerialNumber = request.SerialNumber;
                    fiscal.ForCharge = request.ForCharge;
                    fiscal.Confirm = ConfirmEnum.NotChecked;

                    #region RowVresion Check

                    if (fiscal.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        fiscal.RowVersion += 1;
                    }
                    #endregion

                    #region Validation

                    if (fiscal.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in fiscal.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }
                    #endregion

                    _fiscalRepository.Save(fiscal);
                    _uow.Commit();

                    response.rowVersion = fiscal.RowVersion;
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

        public GeneralResponse DeleteFiscal(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            Fiscal fiscal = new Fiscal();
            fiscal = _fiscalRepository.FindBy(request.ID);

            if (fiscal != null)
            {
                // Check if Deletable or not
                if ((int)fiscal.Confirm == 2)
                {
                    
                    response.ErrorMessages.Add("تراکنش مالی مورد نظر قابل حذف نمی باشد. فقط تراکنشهای بررسی نشده قابل حذف هستند.");
                    return response;
                }
                try
                {
                    _fiscalRepository.Remove(fiscal);
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
        public GetFiscalResponse GetFiscal(GetRequest request)
        {
            GetFiscalResponse response = new GetFiscalResponse();

            try
            {
                Fiscal fiscal = new Fiscal();
                FiscalView fiscalView = fiscal.ConvertToFiscalView();

                fiscal = _fiscalRepository.FindBy(request.ID);
                if (fiscal != null)
                    fiscalView = fiscal.ConvertToFiscalView();

                response.FiscalView = fiscalView;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public GetGeneralResponse<FiscalView> GetFiscalByID(Guid FiscalID)
        {
            GetGeneralResponse<FiscalView> response = new GetGeneralResponse<FiscalView>();

            FiscalView fiscalView = _fiscalRepository.FindBy(FiscalID).ConvertToFiscalView();

            response.data = fiscalView;
            response.totalCount = 1;

            return response;
        }


        #endregion

        #region Get All

        public GetFiscalsResponse GetFiscals(AjaxGetRequest request, Guid employeeID)
        {
            GetFiscalsResponse response = new GetFiscalsResponse();

            try
            {
                int index = (request.PageNumber - 1) * request.PageSize;
                int count = request.PageSize;

                string query = "From Fiscal F Where '" + employeeID + "' In (Select M.Employee.ID From F.MoneyAccount.MoneyAccountEmployees M)";

                Response<Fiscal> fiscalsResponse = _fiscalRepository
                    .FindAll(query, index, count);

                IEnumerable<FiscalView> fiscals = fiscalsResponse.data.ConvertToFiscalViews();

                response.FiscalViews = fiscals;
                response.Count = fiscalsResponse.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public GetFiscalsResponse GetFiscalsCreatedOrConfirmedWithMe(AjaxGetRequest request, string employeeID)
        {
            GetFiscalsResponse response = new GetFiscalsResponse();

            try
            {
                int index = (request.PageNumber - 1) * request.PageSize;
                int count = request.PageSize;

                string query = "From Fiscal Where CreateEmployee.ID = '" + employeeID + "' Or ConfirmEmployee.ID = '" + employeeID + "'";

                Infrastructure.Domain.Response<Fiscal> fiscalsResponse = _fiscalRepository
                    .FindAll(query, index, count);

                IEnumerable<FiscalView> fiscals = fiscalsResponse.data
                    .ConvertToFiscalViews();

                response.FiscalViews = fiscals;
                response.Count = fiscalsResponse.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public GetFiscalsResponse GetFiscalsCanConfirm(AjaxGetRequest request, string employeeID)
        {
            GetFiscalsResponse response = new GetFiscalsResponse();

            try
            {
                int index = (request.PageNumber - 1) * request.PageSize;
                int count = request.PageSize;

                string query = "From Fiscal F Where '" + employeeID 
                    + "' In (Select M.Employee.ID From F.MoneyAccount.MoneyAccountEmployees M)"
                    + " AND Confirm <> 2 AND Confirm <> 3";

                Response<Fiscal> fiscalsResponse = _fiscalRepository
                    .FindAll(query, index, count);

                IEnumerable<FiscalView> fiscals = fiscalsResponse.data
                    .ConvertToFiscalViews();

                response.FiscalViews = fiscals;
                response.Count = fiscalsResponse.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public GetFiscalsResponse GetFiscals(AjaxGetRequest request)
        {
            GetFiscalsResponse response = new GetFiscalsResponse();

            try
            {
                int index = (request.PageNumber - 1) * request.PageSize;
                int count = request.PageSize;

                Response<Fiscal> fiscalsResponse = _fiscalRepository.FindAll(index, count);

                IEnumerable<FiscalView> fiscals = fiscalsResponse.data.ConvertToFiscalViews();

                response.FiscalViews = fiscals;
                response.Count = fiscalsResponse.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public GetFiscalsResponse GetFiscalsOfCustomer(AjaxGetRequest request, Guid customerID)
        {
            GetFiscalsResponse response = new GetFiscalsResponse();

            try
            {
                int index = (request.PageNumber - 1) * request.PageSize;
                int count = request.PageSize;

                Query query = new Query();
                Criterion criteria = new Criterion("Customer.ID", customerID, CriteriaOperator.Equal);
                query.Add(criteria);

                Response<Fiscal> fiscalsResponse = _fiscalRepository.FindBy(query, index, count);

                IEnumerable<FiscalView> fiscals = fiscalsResponse.data.ConvertToFiscalViews();

                response.FiscalViews = fiscals;
                response.Count = fiscalsResponse.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<FiscalView>> GetFiscals(Guid customerID, int pageSize, int pageNumber,IList<Sort> sort,IList<FilterData> filter)
        {
            GetGeneralResponse<IEnumerable<FiscalView>> response = new GetGeneralResponse<IEnumerable<FiscalView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;


                IList<FilterData> _filter = new List<FilterData>();

                _filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "eq",
                        type = "string",
                        value = new[] { customerID.ToString() }
                    },
                    field = "Customer.ID"
                });

                if (filter != null)
                    foreach (var item in filter)
                        _filter.Add(item);

                foreach (var item in sort)
                {
                    if (item.SortColumn == "Balance")
                        item.SortColumn = "Customer.Balance";
                }

                string newquery = FilterUtilityService.GenerateFilterHQLQuery(_filter, "Fiscal", sort);

                Response<Fiscal> fiscals = _fiscalRepository.FindAll(newquery, index, count);

                response.data = fiscals.data.ConvertToFiscalViews();
                response.totalCount = fiscals.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }


            return response;
        }


        public GetGeneralResponse<IEnumerable<FiscalView>> GetAllFiscals(int pageSize, int pageNumber, IList<Sort> sort,IList<FilterData> filter)
        {
            GetGeneralResponse<IEnumerable<FiscalView>> response = new GetGeneralResponse<IEnumerable<FiscalView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                //foreach (var item in sort)
                //{
                //    if (item.SortColumn == "Balance")
                //        item.SortColumn = "Customer.Balance";
                //}

                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "Fiscal", sort);
                Response<Fiscal> fiscals = _fiscalRepository.FindAll(query, index, count);

                response.data = fiscals.data.ConvertToFiscalViews();
                response.totalCount = fiscals.totalCount;
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

        #region Confirm

        public GeneralResponse Confirm(ConfirmRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            Fiscal fiscal = new Fiscal();
            fiscal = _fiscalRepository.FindBy(request.FiscalID);
            Customer customer = _customerRepository.FindBy(fiscal.Customer.ID);
            if (fiscal != null)
            {
                try
                {
                    if(fiscal.MoneyAccount.Has9Digits)
                    if (request.FiscalReciptNumber < 100000000 || request.FiscalReciptNumber > 999999999)
                    {
                        response.ErrorMessages.Add("شماره رسید نمیتواند کمتر از 9 رقم باشد");
                        return response;
                    }

                    fiscal.Confirm = request.Confirm;
                    fiscal.ConfirmDate = PersianDateTime.Now;

                    fiscal.ConfirmedCost = fiscal.Cost < 0 ? -request.ConfirmedCost : request.ConfirmedCost;
                    fiscal.ConfirmEmployee = _employeeRepository.FindBy(request.ConfirmEmployeeID);

                    if (request.Confirm == ConfirmEnum.Confirmed)
                        fiscal.SerialNumber = NewSerialNumber(fiscal.MoneyAccount);

                    #region Check Permission
                    // Check if the Employee can confirm or not
                    if (!fiscal.EmployeesWhoCanConfirm.Contains(fiscal.ConfirmEmployee))
                    {
                        response.ErrorMessages.Add("YouCanNotConfirmThisMoneyAccountKey");
                        return response;
                    }
                    #endregion

                    #region Check the Cost
                    // Check if (ConfirmCost > Cost) then Rais error
                    if (request.ConfirmedCost > Math.Abs(fiscal.Cost))
                    {
                        response.ErrorMessages.Add("InvalidConfirmCostKey");
                        return response;
                    }
                    #endregion

                    #region RowVresion Check

                    if (fiscal.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        fiscal.RowVersion += 1;
                    }
                    fiscal.AccountingSerialNumber = NewAccountingSerialNumber;
                    
                    #endregion

                    if (customer.Balance < request.ConfirmedCost && fiscal.ConfirmedCost < 0 && request.Confirm == ConfirmEnum.Confirmed)
                    {
                        response.ErrorMessages.Add("مبلغ پرداختی به مشتری بیش از بستانکاری مشتری میباشد");
                        return response;
                    }

                    #region Change Customer Balance
                    if ((int)request.Confirm != 1 && (int)request.Confirm != 3)
                    {
                        if (fiscal.MoneyAccount.HasUniqueSerialNumber)
                        {
                            Query query = new Query();
                            Criterion uniqueCriterion = new Criterion("FiscalReciptNumber", request.FiscalReciptNumber, CriteriaOperator.Equal);
                            query.Add(uniqueCriterion);
                            Criterion uniqueCriterion1 = new Criterion("MoneyAccount.ID", fiscal.MoneyAccount.ID, CriteriaOperator.Equal);
                            query.Add(uniqueCriterion1);
                            IEnumerable<FiscalView> fiscalView = _fiscalRepository.FindByQuery(query).data.ConvertToFiscalViews();
                            if (fiscalView.Count() > 0)
                            {
                                response.ErrorMessages.Add("  این شماره قبلا به  " + fiscalView.FirstOrDefault().CustomerName + " با شماره تلفن " + fiscalView.FirstOrDefault().ADSLPhone + " داده شده است ");
                                return response;
                            }
                        }
                        
                        fiscal.FiscalReciptNumber = request.FiscalReciptNumber;
                        customer.Balance += fiscal.ConfirmedCost;
                        long confirmedCost = fiscal.ConfirmedCost == null ? 0 : (long)fiscal.ConfirmedCost;
                        customer.CanDeliverCost += confirmedCost;
                        _customerRepository.Save(customer);
                        if (customer.CanDeliverCost < 0)
                        {
                            response.ErrorMessages.Add(" هشدار ! با انجام این عملیات معین تحویل مشتری منفی میشود. لطفا با برنامه نویس تماس بگیرید");
                            return response;
                        }
                    }

                    #endregion

                    _fiscalRepository.Save(fiscal);
                    _uow.Commit();
                }
                catch (Exception ex)
                {
                    response.ErrorMessages.Add(ex.Message);
                    if (ex.InnerException != null)
                        response.ErrorMessages.Add(ex.InnerException.Message);
                }
            }
            else
            {
                response.ErrorMessages.Add("NoItemToEditKey");
                return response;
            }

            return response;
        }

        private long NewSerialNumber(MoneyAccount moneyAccount)
        {
            Infrastructure.Querying.Query query = new Query();
            Criterion criteria = new Criterion("MoneyAccount.ID", moneyAccount.ID, CriteriaOperator.Equal);
            query.Add(criteria);

            IEnumerable<Fiscal> fiscals = _fiscalRepository.FindBy(query);
            var response = fiscals.OrderByDescending(x => x.SerialNumber).Select(s => s.SerialNumber).First();
            if (response != null)
            {
                return response == 0 ? 100001 : response + 1;
            }
            else
                return 100001;
        }

        #endregion

        #region ChangeChargedStatus

        public GeneralResponse ChangeCharedStatus(ChargeStatus chargStatus, Guid fiscalID,Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                Fiscal fiscal = new Fiscal();
                fiscal = _fiscalRepository.FindBy(fiscalID);
                fiscal.ChargeStatus = chargStatus;
                fiscal.ModifiedDate = PersianDateTime.Now;
                fiscal.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);

                _fiscalRepository.Save(fiscal);
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

        #region GetFollowUp Number

        public GetGeneralResponse<FiscalView> GetFollowUpNumber(int FollowNumber)
        {
            GetGeneralResponse<FiscalView> response = new GetGeneralResponse<FiscalView>();

            try
            {
                Infrastructure.Querying.Query query = new Query();
                Criterion criteria = new Criterion("FollowNumber", FollowNumber, CriteriaOperator.Equal);
                query.Add(criteria);

                Fiscal fiscal = _fiscalRepository.FindBy(query).First();

                response.data = fiscal.ConvertToFiscalView();
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
        
        #region Get Report

        public GetGeneralResponse<IEnumerable<GetBankCasheReportView>> GetBankCasheReport(int? TransactionType, IEnumerable<Guid> MoneyAccountID, 
            string InvestStartDate, string InvestEndDate,string ConfirmStartDate,string ConfirmEndDate,IList<Sort> sort,bool NotConfirmed)
        {
            GetGeneralResponse<IEnumerable<GetBankCasheReportView>> response = new GetGeneralResponse<IEnumerable<GetBankCasheReportView>>();

            try
            {

                #region Preparing Filters

                IList<FilterData> filters = new List<FilterData>();

                if (NotConfirmed==true)
                {
                    FilterData TransactionTypeFilter = new FilterData()
                    {
                        field = "Confirm",
                        data = new data()
                        {
                            type = "numeric",
                            comparison = "eq",
                            value = new[] { "3" }
                        }
                    };
                    filters.Add(TransactionTypeFilter);
                }
                if (NotConfirmed==false)
                {
                    FilterData TransactionTypeFilter = new FilterData()
                    {
                        field = "Confirm",
                        data = new data()
                        {
                            type = "numeric",
                            comparison = "eq",
                            value = new[] { "2" }
                        }
                    };
                    filters.Add(TransactionTypeFilter);
                }

                #region Transaction Type
                if (TransactionType == 1)
                {
                    FilterData TransactionTypeFilter = new FilterData()
                    {
                        field = "Cost",
                        data = new data()
                        {
                            type = "numeric",
                            comparison = "gt",
                            value = new[] { "0" }
                        }
                    };
                    filters.Add(TransactionTypeFilter);
                }

                if (TransactionType == 2)
                {
                    FilterData TransactionTypeFilter = new FilterData()
                    {
                        field = "Cost",
                        data = new data()
                        {
                            type = "numeric",
                            comparison = "lt",
                            value = new[] { "0" }
                        }
                    };
                    filters.Add(TransactionTypeFilter);
                }
                #endregion

                #region MoneyAccount

                if (MoneyAccountID != null)
                {
                    FilterData MoneyAccounFilter = new FilterData()
                    {
                        field = "MoneyAccountName",
                        data = new data()
                        {
                            type = "list",
                            value = MoneyAccountID.Select(x => x.ToString()).ToArray(),
                        }
                    };
                    filters.Add(MoneyAccounFilter);
                }

                #endregion

                #region Invest Start Date

                if (InvestStartDate != null && InvestEndDate==null)
                {
                    FilterData InvestStartDateFilter = new FilterData()
                    {
                        field = "InvestDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "dateOnly",
                            value = new[] { InvestStartDate }
                        }
                    };
                    filters.Add(InvestStartDateFilter);
                }

                #endregion

                #region Invest End Date

                if (InvestEndDate != null && InvestStartDate==null)
                {
                    FilterData InvestEndDateFilter = new FilterData()
                    {
                        field = "InvestDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateOnly",
                            value = new[] { InvestEndDate }
                        }
                    };
                    filters.Add(InvestEndDateFilter);
                }

                if (InvestEndDate != null & InvestStartDate != null)
                {



                        FilterData ConfirmEndDateFilter = new FilterData()
                        {
                            field = "InvestDate",
                            data = new data()
                            {
                                comparison = "lteq",
                                type = "dateOnlyBetween",
                                value = new[] {InvestStartDate, InvestEndDate}
                            }
                        };
                        filters.Add(ConfirmEndDateFilter);
                    
                }

                #endregion

                #region Confirem Start Date

                if (ConfirmStartDate != null && ConfirmEndDate==null)
                {
                    FilterData ConfirmStartDateFilter = new FilterData()
                    {
                        field = "ConfirmDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { ConfirmStartDate }
                        }
                    };
                    filters.Add(ConfirmStartDateFilter);
                }

                #endregion

                #region Confirem End Date

                if (ConfirmEndDate != null & ConfirmStartDate==null)
                {
                    FilterData ConfirmEndDateFilter = new FilterData()
                    {
                        field = "ConfirmDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { ConfirmStartDate }
                        }
                    };
                    filters.Add(ConfirmEndDateFilter);
                }

                if (ConfirmEndDate != null & ConfirmStartDate != null)
                {
                    if (ConfirmEndDate == ConfirmStartDate)
                    {
                        FilterData ConfirmEndDateFilter = new FilterData()
                        {
                            field = "ConfirmDate",
                            data = new data()
                            {
                                comparison = "eq",
                                type = "date",
                                value = new[] {ConfirmStartDate}
                            }
                        };
                        filters.Add(ConfirmEndDateFilter);
                    }
                    else
                    {
                        FilterData ConfirmEndDateFilter = new FilterData()
                        {
                            field = "ConfirmDate",
                            data = new data()
                            {
                                comparison = "lteq",
                                type = "dateBetween",
                                value = new[] {ConfirmStartDate, ConfirmEndDate}
                            }
                        
                        };
                        filters.Add(ConfirmEndDateFilter);
                    }

                }

                #region فقط فاکتور های تایید شده

                FilterData ConfirmedFilter = new FilterData()
                {
                    field = "Confirm",
                    data = new data() { 
                        type="numeric",
                        comparison="eq",
                        value=new[]{"2"}
                    }
                };

                filters.Add(ConfirmedFilter);

                #endregion

                #endregion


                #endregion

                string query = FilterUtilityService.GenerateFilterHQLQuery(filters, "Fiscal", sort);

                Response<Fiscal> fiscals = _fiscalRepository.FindAll(query);

                #region Preparing Report View

                IList<GetBankCasheReportView> report = new List<GetBankCasheReportView>();

                foreach (Fiscal fiscal in fiscals.data)
                {
                    GetBankCasheReportView _report = new GetBankCasheReportView();

                    _report.ADSLPhone = fiscal.Customer.ADSLPhone;
                    _report.Bes = fiscal.Cost > 0 ? "بستانکار" : "";
                    _report.Bed = fiscal.Cost < 0 ? "بدهکار" : "";

                    _report.ConfiremdCost = fiscal.ConfirmedCost == null ? 0 : (long)fiscal.ConfirmedCost;
                    _report.ConfirmDate = fiscal.ConfirmDate;
                    if (fiscal.ConfirmEmployee != null)
                        _report.ConfirmEmployeeName = fiscal.ConfirmEmployee.Name;
                    _report.Cost = fiscal.Cost;
                    _report.CreateEmployeeName = fiscal.CreateEmployee.Name;
                    _report.InvestDate = fiscal.InvestDate;
                    _report.Name = fiscal.Customer.Name;
                    _report.SerialNumber = fiscal.SerialNumber;
                    _report.MoneyAccountName = fiscal.MoneyAccount.AccountName;
                    _report.Note = fiscal.Note;
                    _report.AccountingSerialNumber = fiscal.DocumentSerial;
                    _report.FiscalReciptNumber = fiscal.FiscalReciptNumber;

                    #region Preparing DOC Type
                    int docType = (int)fiscal.DocumentType;
                    if (docType == 1)
                        _report.DocType = "رسید عابر بانک";
                    else if(docType==2)
                        _report.DocType = "فیش بانکی";
                    else if (docType == 3)
                        _report.DocType = "چک";
                    else if (docType == 4)
                        _report.DocType = "قبض صندوق";
                    else if (docType == 5)
                        _report.DocType = "رسید POS";
                    else if (docType == 6)
                        _report.DocType = "پرداخت اینترنتی";
                    else if (docType == 7)
                        _report.DocType = "سایت ماهان";
                    else if (docType == 8)
                        _report.DocType = "سایر موارد";

                    #endregion
                    report.Add(_report);
                }

                #endregion

                response.data = report;
                response.totalCount = report.Count();

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


    }
}
