#region Usings

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Sales.Interfaces;
using Model.Store.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.SaleCatalogService;
using Model.Sales;
using Services.ViewModels.Sales;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees.Interfaces;
using Services.ViewModels.Reports;
using Model.Store;
using Infrastructure.Querying;
using Infrastructure.Domain;
#endregion

namespace Services.Implementations
{
    public class CreditSaleDetailService : ICreditSaleDetailService
    {
        #region Declares

        private readonly ICreditSaleDetailRepository _creditSaleDetailRepository;
        private readonly ICreditServiceRepository _creditServiceRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;

        #endregion

        #region Ctor

        public CreditSaleDetailService(ICreditSaleDetailRepository creditSaleDetailRepository, IUnitOfWork uow)
        {
            _creditSaleDetailRepository = creditSaleDetailRepository;
            _uow = uow;
        }

        public CreditSaleDetailService(ICreditSaleDetailRepository creditSaleDetailRepository,
            ICreditServiceRepository creditServiceRepository, ISaleRepository saleRepository,
            IUnitOfWork uow, IEmployeeRepository employeeRepository)
            : this(creditSaleDetailRepository, uow)
        {
            this._creditServiceRepository = creditServiceRepository;
            this._saleRepository = saleRepository;
            _employeeRepository = employeeRepository;
        }

        #endregion

        #region PrepareCreditSaleDetail

        private CreditSaleDetail PrepareCreditSaleDetail(AddCreditSaleDetailRequest request)
        {
            CreditSaleDetail response = new CreditSaleDetail();

            CreditSaleDetail creditSaleDetail = new CreditSaleDetail();
            creditSaleDetail.ID = Guid.NewGuid();
            creditSaleDetail.CreateDate = PersianDateTime.Now;
            creditSaleDetail.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
            creditSaleDetail.CreditService = this._creditServiceRepository.FindBy(request.CreditServiceID);
            creditSaleDetail.Discount = request.Discount;
            creditSaleDetail.Imposition = request.Imposition;
            creditSaleDetail.UnitPrice = request.UnitPrice;
            creditSaleDetail.Units = request.Units;
            creditSaleDetail.RowVersion = 1;

            return response;
        }

        public IEnumerable<CreditSaleDetail> PrepareCreditSaleDetails(IEnumerable<AddCreditSaleDetailRequest> requests)
        {
            IList<CreditSaleDetail> response = new List<CreditSaleDetail>();

            if (requests != null && requests.Count() > 0)
                foreach (AddCreditSaleDetailRequest request in requests)
                {
                    response.Add(PrepareCreditSaleDetail(request));
                }

            return response;
        }

        #endregion

        #region Edit
        public GeneralResponse EditCreditSaleDetail(EditCreditSaleDetailRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            CreditSaleDetail creditSaleDetail = new CreditSaleDetail();
            creditSaleDetail = _creditSaleDetailRepository.FindBy(request.ID);

            if (creditSaleDetail != null)
            {
                try
                {
                    creditSaleDetail.ModifiedDate = PersianDateTime.Now;
                    creditSaleDetail.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if(creditSaleDetail.CreditService.ID!=request.CreditServiceID)
                    creditSaleDetail.CreditService = this._creditServiceRepository.FindBy(request.CreditServiceID);
                    creditSaleDetail.Discount = request.Discount;
                    creditSaleDetail.Imposition = request.Imposition;
                    creditSaleDetail.UnitPrice = request.UnitPrice;

                    if (creditSaleDetail.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        creditSaleDetail.RowVersion += 1;
                    }

                    if (creditSaleDetail.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in creditSaleDetail.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _creditSaleDetailRepository.Save(creditSaleDetail);
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
        public GeneralResponse DeleteCreditSaleDetail(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            CreditSaleDetail creditSaleDetail = new CreditSaleDetail();
            creditSaleDetail = _creditSaleDetailRepository.FindBy(request.ID);

            if (creditSaleDetail != null)
            {
                try
                {
                    _creditSaleDetailRepository.Remove(creditSaleDetail);
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
        public GetCreditSaleDetailResponse GetCreditSaleDetail(GetRequest request)
        {
            GetCreditSaleDetailResponse response = new GetCreditSaleDetailResponse();

            try
            {
                CreditSaleDetail creditSaleDetail = new CreditSaleDetail();
                //CreditSaleDetailView creditSaleDetailView = creditSaleDetail.ConvertToCreditSaleDetailView();

                creditSaleDetail = _creditSaleDetailRepository.FindBy(request.ID);
                if (creditSaleDetail != null)
                {
                    CreditSaleDetailView creditSaleDetailView = creditSaleDetail.ConvertToCreditSaleDetailView();

                    response.CreditSaleDetailView = creditSaleDetailView;
                }
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get All

        public GetCreditSaleDetailsResponse GetCreditSaleDetails()
        {
            GetCreditSaleDetailsResponse response = new GetCreditSaleDetailsResponse();

            try
            {
                IEnumerable<BaseSaleDetailView> creditSaleDetails = _creditSaleDetailRepository.FindAll()
                    .ConvertToCreditSaleDetailViews();

                response.CreditSaleDetailViews = creditSaleDetails;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Sale Detail Report Old

        //public GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> GetSaleReport(SaleReportRequest request)
        //{
        //    GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> response = new GetGeneralResponse<IEnumerable<GetSaleDetailReportView>>();

        //    IQueryable<CreditSaleDetail> query = _creditSaleDetailRepository.Queryable();


        //    if (request.SaleEmployeeID != null )
        //        query = query.Where(x => request.SaleEmployeeID.Contains(x.Sale.CreateEmployee.ID));

        //    if (request.DeliverEmployeeID != null )
        //        query = query.Where(x => request.DeliverEmployeeID.Contains(x.DeliverEmployee.ID));

        //    if (request.RollBackEmployeeID != null )
        //        query = query.Where(x => request.RollBackEmployeeID.Contains(x.CreateEmployee.ID)).Where(x=>x.IsRollbackDetail);

        //    if (request.Networks != null)
        //    {
        //        query = query.Where(x => request.Networks.Contains(x.CreditService.Network.ID));
        //    }

        //    if (request.CreditService != null)
        //    {
        //        query = query.Where(x => request.Products.Contains(x.CreditService.ID));
        //    }

        //    //if (request.SaleStartDate != null)
        //    //{

        //    //    query = query.Where(w => string.Compare(w.Sale.CloseDate.Substring(0, 10), request.SaleStartDate) >= 0);
        //    //}
        //    //if (request.SaleEndDate != null)
        //    //{
        //    //    query = query.Where(w => (string.Compare(w.Sale.CloseDate.Substring(0, 10), request.SaleEndDate) < 0));
        //    //}
        //    if (request.RollBackStartDate != null)
        //    {

        //    }
        //    if (request.RollBackEndDate != null)
        //    {

        //    }
        //    if (request.Deliverd != null)
        //    {
        //        query = query.Where(x => x.Delivered == false);
        //    }
        //    if (request.Confirmed != null)
        //    {
        //        query = query.Where(x => x.Sale.Closed == request.Confirmed);
        //    }

        //    if (request.RollBacked != null)
        //    {
        //        query = query.Where(x => x.Rollbacked == request.RollBacked);
        //    }

        //    IEnumerable<CreditSaleDetail> creditSaleDetail = query.ToList();

        //    IList<GetSaleDetailReportView> Report = new List<GetSaleDetailReportView>();
        //    foreach (CreditSaleDetail _creditSaleDetail in creditSaleDetail)
        //    {
        //        GetSaleDetailReportView item = new GetSaleDetailReportView();

        //        item.ADSLPhone = _creditSaleDetail.Sale.Customer.ADSLPhone;
        //        item.CenterName = _creditSaleDetail.Sale.Customer.Center == null ? "" : _creditSaleDetail.Sale.Customer.Center.CenterName;
        //        item.DeliverDate = _creditSaleDetail.DeliverDate;
        //        item.DeliverEmployeeName = _creditSaleDetail.DeliverEmployee == null ? "" : (string)_creditSaleDetail.DeliverEmployee.Name;
        //        item.Discount = _creditSaleDetail.LineDiscount;
        //        item.Imposition = _creditSaleDetail.LineImposition;
        //        item.Name = _creditSaleDetail.Sale.Customer.Name;
        //        item.Price = _creditSaleDetail.UnitPrice;
        //        item.Count = _creditSaleDetail.Units;
        //        item.CreditServiceName = _creditSaleDetail.CreditService.ServiceName;

        //        #region Get Network
        //        CreditService creditService = _creditServiceRepository.FindBy(_creditSaleDetail.CreditService.ID);
        //        item.NetworkName = creditService.Network.NetworkName;
        //        #endregion

        //        item.RollBackEmployeeName = _creditSaleDetail.ModifiedEmployee == null ? "" : (string)_creditSaleDetail.ModifiedEmployee.Name;
        //        item.RollBackPrice = _creditSaleDetail.RollbackPrice;
        //        item.RoolBackDate = _creditSaleDetail.ModifiedDate;
        //        item.SaleDate = _creditSaleDetail.Sale.CloseDate;
        //        item.Total = _creditSaleDetail.LineTotal;
        //        item.TotalRollBack = _creditSaleDetail.Units;
        //        item.SaleDate = _creditSaleDetail.Sale.CreateDate;
        //        item.SaleEmployeeName = _creditSaleDetail.CreateEmployee.Name;



        //        Report.Add(item);
        //    }

        //    response.data = Report;
        //    response.totalCount = Report.Count();

        //    return response;
        
        //}

        #endregion

        #region sale Report New

        public GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> GetSaleReport(IList<FilterData> filters)
        {
            GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> response = new GetGeneralResponse<IEnumerable<GetSaleDetailReportView>>();

            string query = FilterUtilityService.GenerateFilterHQLQuery(filters, "CreditSaleDetail", null);

            Response<CreditSaleDetail> creditSaleDetail = _creditSaleDetailRepository.FindAll(query);

            IList<GetSaleDetailReportView> Report = new List<GetSaleDetailReportView>();

            foreach (CreditSaleDetail _creditSaleDetail in creditSaleDetail.data)
            {

                GetSaleDetailReportView item = new GetSaleDetailReportView();
                item.ADSLPhone = _creditSaleDetail.Sale.Customer.ADSLPhone;
                item.NetworkName = _creditSaleDetail.CreditService.Network.NetworkName;
                item.CenterName = _creditSaleDetail.Sale.Customer.Center == null
                    ? ""
                    : _creditSaleDetail.Sale.Customer.Center.CenterName;
                item.Name = _creditSaleDetail.Sale.Customer.Name;
                item.CreditServiceName = _creditSaleDetail.CreditService.ServiceName;
                item.BonusDate = _creditSaleDetail.BonusDate;
                item.ComissionDate = _creditSaleDetail.ComissionDate;
                if (_creditSaleDetail.IsRollbackDetail)
                {


                    item.Bonus = _creditSaleDetail.Bonus;
                    item.Comission = _creditSaleDetail.Comission;

                    item.DeliverDate = _creditSaleDetail.MainSaleDetail.DeliverDate;
                    item.DeliverEmployeeName = _creditSaleDetail.MainSaleDetail.DeliverEmployee == null
                        ? ""
                        : (string)_creditSaleDetail.MainSaleDetail.DeliverEmployee.Name;
                    item.Discount = _creditSaleDetail.MainSaleDetail.LineDiscount;
                    item.Imposition = _creditSaleDetail.MainSaleDetail.LineImposition;
                    item.Price = _creditSaleDetail.MainSaleDetail.UnitPrice;
                    item.Count = _creditSaleDetail.MainSaleDetail.Units;
                    item.RollBackEmployeeName = _creditSaleDetail.CreateEmployee.Name;
                    item.RollBackPrice = _creditSaleDetail.RollbackPrice;
                    item.RoolBackDate = _creditSaleDetail.CreateDate;
                    item.SaleDate = _creditSaleDetail.MainSaleDetail.CreateDate;
                    item.Total = _creditSaleDetail.LineTotal;
                    item.TotalRollBack = _creditSaleDetail.Units;
                    item.SaleEmployeeName = _creditSaleDetail.MainSaleDetail.CreateEmployee.Name;
                    

                }
                else
                {


                    item.Bonus = _creditSaleDetail.Bonus;
                    item.Comission = _creditSaleDetail.Comission;

                    item.DeliverDate = _creditSaleDetail.DeliverDate;
                    item.DeliverEmployeeName = _creditSaleDetail.DeliverEmployee == null
                        ? ""
                        : (string)_creditSaleDetail.DeliverEmployee.Name;
                    item.Discount = _creditSaleDetail.LineDiscount;
                    item.Imposition = _creditSaleDetail.LineImposition;
                    item.Price = _creditSaleDetail.UnitPrice;
                    item.Count = _creditSaleDetail.Units;
                    item.SaleDate = _creditSaleDetail.CreateDate;
                    item.Total = _creditSaleDetail.LineTotal;
                    item.SaleEmployeeName = _creditSaleDetail.CreateEmployee.Name;
                    item.CustomerID = _creditSaleDetail.Sale.Customer.ID;


                    if (_creditSaleDetail.Rollbacked)
                    {
                        Infrastructure.Querying.Query q = new Query();
                        Criterion crt = new Criterion("MainSaleDetail.ID", _creditSaleDetail.ID, CriteriaOperator.Equal);
                        q.Add(crt);
                        CreditSaleDetail RollbakedCreditSaleDetail = _creditSaleDetailRepository.FindBy(q).FirstOrDefault();
                        if (RollbakedCreditSaleDetail != null)
                        {
                            item.RollBackEmployeeName = RollbakedCreditSaleDetail.CreateEmployee.Name;
                            item.RollBackPrice = RollbakedCreditSaleDetail.LineTotal;
                            item.RoolBackDate = RollbakedCreditSaleDetail.CreateDate;
                            item.TotalRollBack = RollbakedCreditSaleDetail.Units;

                        }
                    }
                }


                Report.Add(item);
            }

            var temp = Report.Sum(x => x.Total);
            response.data = Report;
            response.totalCount = Report.Count();

            return response;
        }
        #endregion





    }
}
