#region Usings
using System;
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
using System.Linq.Expressions;
using Infrastructure.Querying;
using Infrastructure.Domain;
#endregion

namespace Services.Implementations
{
    public class UncreditSaleDetailService : IUncreditSaleDetailService
    {
        #region Declares
        private readonly IUncreditSaleDetailRepository _uncreditSaleDetailRepository;
        private readonly IUncreditServiceRepository _uncreditServiceRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Ctor
        public UncreditSaleDetailService(IUncreditSaleDetailRepository uncreditSaleDetailRepository, IUnitOfWork uow)
        {
            _uncreditSaleDetailRepository = uncreditSaleDetailRepository;
            _uow = uow;
        }

        public UncreditSaleDetailService(IUncreditSaleDetailRepository uncreditSaleDetailRepository, IUncreditServiceRepository uncreditServiceRepository,
            ISaleRepository saleRepository, IUnitOfWork uow, IEmployeeRepository employeeRepository)
            : this(uncreditSaleDetailRepository, uow)
        {
            this._saleRepository = saleRepository;
            this._uncreditServiceRepository = uncreditServiceRepository;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Add
        public GeneralResponse AddUncreditSaleDetail(AddSaleDetailBaseRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                UncreditSaleDetail uncreditSaleDetail = new UncreditSaleDetail();
                uncreditSaleDetail.ID = Guid.NewGuid();
                uncreditSaleDetail.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                uncreditSaleDetail.CreateDate = PersianDateTime.Now;
                uncreditSaleDetail.Discount = request.Discount;
                uncreditSaleDetail.Imposition = request.Imposition;
                uncreditSaleDetail.RollbackNote = request.RollbackNote;
                //uncreditSaleDetail.Sale = this._saleRepository.FindBy(request.SaleID);
                //uncreditSaleDetail.UncreditService = this._uncreditServiceRepository.FindBy(request.UncreditServiceID);
                uncreditSaleDetail.UnitPrice = request.UnitPrice;
                uncreditSaleDetail.Units = request.Units;
                uncreditSaleDetail.RowVersion = 1;

                _uncreditSaleDetailRepository.Add(uncreditSaleDetail);
                _uow.Commit();

                ////response.success = true;

                // Validation
                if (uncreditSaleDetail.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in uncreditSaleDetail.GetBrokenRules())
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
        public GeneralResponse EditUncreditSaleDetail(EditUncreditSaleDetailRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            UncreditSaleDetail uncreditSaleDetail = new UncreditSaleDetail();
            uncreditSaleDetail = _uncreditSaleDetailRepository.FindBy(request.ID);

            if (uncreditSaleDetail != null)
            {
                try
                {
                    uncreditSaleDetail.ModifiedDate = PersianDateTime.Now;
                    uncreditSaleDetail.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    uncreditSaleDetail.Discount = request.Discount;
                    uncreditSaleDetail.Imposition = request.Imposition;
                    uncreditSaleDetail.RollbackNote = request.RollbackNote;
                    //uncreditSaleDetail.Sale = this._saleRepository.FindBy(request.SaleID);
                    //uncreditSaleDetail.UncreditService = this._uncreditServiceRepository.FindBy(request.UncreditServiceID);
                    uncreditSaleDetail.UnitPrice = request.UnitPrice;
                    uncreditSaleDetail.Units = request.Units;

                    if (uncreditSaleDetail.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        uncreditSaleDetail.RowVersion += 1;
                    }

                    if (uncreditSaleDetail.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in uncreditSaleDetail.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _uncreditSaleDetailRepository.Save(uncreditSaleDetail);
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
        public GeneralResponse DeleteUncreditSaleDetail(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            UncreditSaleDetail uncreditSaleDetail = new UncreditSaleDetail();
            uncreditSaleDetail = _uncreditSaleDetailRepository.FindBy(request.ID);

            if (uncreditSaleDetail != null)
            {
                try
                {
                    _uncreditSaleDetailRepository.Remove(uncreditSaleDetail);
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

        public GetUncreditSaleDetailResponse GetUncreditSaleDetail(GetRequest request)
        {
            GetUncreditSaleDetailResponse response = new GetUncreditSaleDetailResponse();

            try
            {
                UncreditSaleDetail uncreditSaleDetail = new UncreditSaleDetail();
                //UncreditSaleDetailView uncreditSaleDetailView = uncreditSaleDetail.ConvertToUncreditSaleDetailView();

                uncreditSaleDetail = _uncreditSaleDetailRepository.FindBy(request.ID);
                if (uncreditSaleDetail != null)
                {
                    UncreditSaleDetailView uncreditSaleDetailView = uncreditSaleDetail.ConvertToUncreditSaleDetailView();

                    response.UncreditSaleDetailView = uncreditSaleDetailView;
                }
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        #endregion

        #region Get All
        public GetUncreditSaleDetailsResponse GetUncreditSaleDetails()
        {
            GetUncreditSaleDetailsResponse response = new GetUncreditSaleDetailsResponse();

            try
            {
                IEnumerable<UncreditSaleDetailView> uncreditSaleDetails = _uncreditSaleDetailRepository.FindAll()
                    .ConvertToUncreditSaleDetailViews();
                
                response.UncreditSaleDetailViews = uncreditSaleDetails;
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



        //    IQueryable<UncreditSaleDetail> query = _uncreditSaleDetailRepository.Queryable();


        //    if (request.SaleEmployeeID != null)
        //        query = query.Where(x => request.SaleEmployeeID.Contains(x.Sale.CreateEmployee.ID));

        //    if (request.DeliverEmployeeID != null)
        //        query = query.Where(x => request.DeliverEmployeeID.Contains(x.DeliverEmployee.ID));

        //    if (request.RollBackEmployeeID != null)
        //        query = query.Where(x => request.RollBackEmployeeID.Contains(x.CreateEmployee.ID)).Where(x=>x.IsRollbackDetail);

        //    if (request.UncreditServices != null)
        //    {
        //        query = query.Where(x => request.UncreditServices.Contains(x.UncreditService.ID));
        //    }

        //    if (request.RollBackStartDate != null)
        //    {

        //    }
        //    if (request.RollBackEndDate != null)
        //    {

        //    }
        //    if (request.Deliverd != null)
        //    {
        //        query = query.Where(x => x.Delivered == request.Deliverd);
        //    }
        //    if (request.Confirmed != null)
        //    {
        //        query = query.Where(x => x.Sale.Closed == request.Confirmed);
        //    }
        //    if (request.RollBacked != null)
        //    {
        //        query = query.Where(x => x.Rollbacked == request.RollBacked);
        //    }

        //    IEnumerable<UncreditSaleDetail> unCreditSaleDetail = query.ToList();


        //    IList<GetSaleDetailReportView> Report = new List<GetSaleDetailReportView>();

        //    foreach (UncreditSaleDetail _unCreditSaleDetail in unCreditSaleDetail)
        //    {
                
        //        GetSaleDetailReportView item = new GetSaleDetailReportView();

        //        item.ADSLPhone = _unCreditSaleDetail.Sale.Customer.ADSLPhone;
        //        item.CenterName = _unCreditSaleDetail.Sale.Customer.Center == null ? "" : _unCreditSaleDetail.Sale.Customer.Center.CenterName;
        //        item.DeliverDate = _unCreditSaleDetail.DeliverDate;
        //        item.DeliverEmployeeName = _unCreditSaleDetail.DeliverEmployee == null ? "" : (string)_unCreditSaleDetail.DeliverEmployee.Name;
        //        item.Discount = _unCreditSaleDetail.LineDiscount;
        //        item.Imposition = _unCreditSaleDetail.LineImposition;
        //        item.Name = _unCreditSaleDetail.Sale.Customer.Name;
        //        item.Price = _unCreditSaleDetail.UnitPrice;
        //        item.Count = _unCreditSaleDetail.Units;
        //        item.UncreditServiceName = _unCreditSaleDetail.UncreditService.UncreditServiceName;

        //        item.RollBackEmployeeName =_unCreditSaleDetail.CreateEmployee.Name;
        //        item.RollBackPrice = _unCreditSaleDetail.RollbackPrice;
        //        item.RoolBackDate = _unCreditSaleDetail.ModifiedDate;
        //        item.SaleDate = _unCreditSaleDetail.Sale.CloseDate;
        //        item.Total = _unCreditSaleDetail.LineTotal;
        //        item.TotalRollBack =item.RoolBackDate==null?0: _unCreditSaleDetail.Units;
        //        item.SaleDate = _unCreditSaleDetail.Sale.CreateDate;
        //        item.SaleEmployeeName = _unCreditSaleDetail.CreateEmployee.Name;




        //        Report.Add(item);

        //    }

        //    response.data = Report;
        //    response.totalCount = Report.Count();

        //    return response;
        //}
        #endregion

        #region Sale Report New
        public GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> GetSaleReport(IList<FilterData> filters) 
        {
            GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> response = new GetGeneralResponse<IEnumerable<GetSaleDetailReportView>>();

            string query = FilterUtilityService.GenerateFilterHQLQuery(filters, "UncreditSaleDetail", null);

            Response<UncreditSaleDetail> uncreditSaleDetail = _uncreditSaleDetailRepository.FindAll(query);

            IList<GetSaleDetailReportView> Report = new List<GetSaleDetailReportView>();

            foreach (UncreditSaleDetail _unCreditSaleDetail in uncreditSaleDetail.data)
            {

                GetSaleDetailReportView item = new GetSaleDetailReportView();
                item.ADSLPhone = _unCreditSaleDetail.Sale.Customer.ADSLPhone;
                item.CenterName = _unCreditSaleDetail.Sale.Customer.Center == null
                    ? ""
                    : _unCreditSaleDetail.Sale.Customer.Center.CenterName;
                item.Name = _unCreditSaleDetail.Sale.Customer.Name;
                item.UncreditServiceName = _unCreditSaleDetail.UncreditService.UncreditServiceName;
                item.BonusDate = _unCreditSaleDetail.BonusDate;
                item.ComissionDate = _unCreditSaleDetail.ComissionDate;

                if (_unCreditSaleDetail.IsRollbackDetail)
                {


                    item.Bonus = _unCreditSaleDetail.Bonus;
                    item.Comission = _unCreditSaleDetail.Comission;

                    item.DeliverDate = _unCreditSaleDetail.MainSaleDetail.DeliverDate;
                    item.DeliverEmployeeName = _unCreditSaleDetail.MainSaleDetail.DeliverEmployee == null
                        ? ""
                        : (string) _unCreditSaleDetail.MainSaleDetail.DeliverEmployee.Name;
                    item.Discount = _unCreditSaleDetail.MainSaleDetail.LineDiscount;
                    item.Imposition = _unCreditSaleDetail.MainSaleDetail.LineImposition;
                    item.Price = _unCreditSaleDetail.MainSaleDetail.UnitPrice;
                    item.Count = _unCreditSaleDetail.MainSaleDetail.Units;
                    item.RollBackEmployeeName = _unCreditSaleDetail.CreateEmployee.Name;
                    item.RollBackPrice = _unCreditSaleDetail.RollbackPrice;
                    item.RoolBackDate = _unCreditSaleDetail.CreateDate;
                    item.SaleDate = _unCreditSaleDetail.MainSaleDetail.CreateDate;
                    item.Total = _unCreditSaleDetail.LineTotal;
                    item.TotalRollBack = _unCreditSaleDetail.Units;
                    item.SaleEmployeeName = _unCreditSaleDetail.MainSaleDetail.CreateEmployee.Name;

                }
                else
                {


                    item.Bonus = _unCreditSaleDetail.Bonus;
                    item.Comission = _unCreditSaleDetail.Comission;

                    item.DeliverDate = _unCreditSaleDetail.DeliverDate;
                    item.DeliverEmployeeName = _unCreditSaleDetail.DeliverEmployee == null
                        ? ""
                        : (string) _unCreditSaleDetail.DeliverEmployee.Name;
                    item.Discount = _unCreditSaleDetail.LineDiscount;
                    item.Imposition = _unCreditSaleDetail.LineImposition;
                    item.Price = _unCreditSaleDetail.UnitPrice;
                    item.Count = _unCreditSaleDetail.Units;
                    item.SaleDate = _unCreditSaleDetail.CreateDate;
                    item.Total = _unCreditSaleDetail.LineTotal;
                    item.SaleEmployeeName = _unCreditSaleDetail.CreateEmployee.Name;
                    item.CustomerID = _unCreditSaleDetail.Sale.Customer.ID;


                    if (_unCreditSaleDetail.Rollbacked)
                    {
                        Infrastructure.Querying.Query q = new Query();
                        Criterion crt = new Criterion("MainSaleDetail.ID", _unCreditSaleDetail.ID, CriteriaOperator.Equal);
                        q.Add(crt);
                        UncreditSaleDetail RollbakedUnCreditSaleDetail = _uncreditSaleDetailRepository.FindBy(q).FirstOrDefault();
                        if (RollbakedUnCreditSaleDetail != null)
                        {
                            item.RollBackEmployeeName = RollbakedUnCreditSaleDetail.CreateEmployee.Name;
                            item.RollBackPrice = RollbakedUnCreditSaleDetail.LineTotal;
                            item.RoolBackDate = RollbakedUnCreditSaleDetail.CreateDate;
                            item.TotalRollBack = RollbakedUnCreditSaleDetail.Units;

                        }
                    }
                }


                Report.Add(item);
            }

            response.data = Report;
            response.totalCount = Report.Count();

            return response;
        }

        #endregion
    }
}
