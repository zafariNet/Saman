using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Infrastructure.Persian;
using Infrastructure.UnitOfWork;
using Model.Customers.Interfaces;
using Model.Employees;
using Model.Employees.Interfaces;
using Model.Sales;
using Model.Sales.Interfaces;
using Model.Store.Interfaces;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;
using Infrastructure.Querying;
using Infrastructure.Domain;
using Services.Mapping;
using System.Globalization;
using Services.ViewModels.Reports;

namespace Services.Implementations
{
    public class BonusComissionService : IBonusComissionService
    {

        #region Decalre

        private readonly ICustomerRepository _customerRepository;

        private readonly IUncreditSaleDetailRepository _uncreditSaleDetailRepository;

        private readonly ICreditSaleDetailRepository _creditSaleDetailRepository;

        private readonly IProductSaleDetailRepository _propduSaleDetailRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly IUnitOfWork _uow;

        private readonly ICourierRepository _courierRepository;

        private readonly IBonusComissionRepository _bonusComissionRepository;
        #endregion

        #region Ctor

        public BonusComissionService(ICustomerRepository customerRepository, IUncreditSaleDetailRepository uncreditSaleDetailRepository,
            ICreditSaleDetailRepository creditSaleDetailRepository, IProductSaleDetailRepository propduSaleDetailRepository,
            IEmployeeRepository employeeRepository, IUnitOfWork uow, ICourierRepository courierRepository, IBonusComissionRepository bonusComissionRepository)
        {
            _customerRepository = customerRepository;
            _uncreditSaleDetailRepository = uncreditSaleDetailRepository;
            _creditSaleDetailRepository = creditSaleDetailRepository;
            _propduSaleDetailRepository = propduSaleDetailRepository;
            _employeeRepository = employeeRepository;
            _courierRepository = courierRepository;
            _bonusComissionRepository = bonusComissionRepository;
            _uow = uow;
        }

        #endregion

        #region Read

        #region Simple


        public GetGeneralResponse<IEnumerable<SlideShowEmployeeView>> GetTodayBonusComissionSimple()
        {
            GetGeneralResponse<IEnumerable<SlideShowEmployeeView>> response = new GetGeneralResponse<IEnumerable<SlideShowEmployeeView>>();
            IList<SlideShowEmployeeView> list = new List<SlideShowEmployeeView>();
            try
            {
                var xxxx = PersianDateTime.GetFirstDateOfLastMonth();
                var xxxx1 = PersianDateTime.GetFirstDateOfCurrentMonth();
                IList<FilterData> filter = new List<FilterData>();

                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        type = "dateBetween",
                        value = new[] { PersianDateTime.LastMonthStartDate.Substring(0, 10), PersianDateTime.Now.Substring(0, 10) }
                    },
                    field = "BonusDate"
                });

                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        type = "numeric",
                        comparison = "Noteq",
                        value = new[] { "0" }
                    },
                    field = "Bonus"
                });

                IList<FilterData> filterForToday = new List<FilterData>();

                filterForToday.Add(new FilterData()
                {
                    data = new data()
                    {
                        type = "dateOnly",
                        value = new[] { PersianDateTime.Now.Substring(0, 10) }
                    },
                    field = "Sale.CloseDate"
                });

                filterForToday.Add(new FilterData()
                {
                    data = new data()
                    {
                        type = "numeric",
                        comparison = "Noteq",
                        value = new[] { "0" }
                    },
                    field = "Bonus"
                });


                string creditSaleDetailToday = FilterUtilityService.GenerateFilterHQLQuery(filterForToday, "CreditSaleDetail", null);
                IEnumerable<CreditSaleDetail> creditsaleDetailsToday = _creditSaleDetailRepository.FindAll(creditSaleDetailToday).data.ToList();

                string unCreditSaleDetailToday = FilterUtilityService.GenerateFilterHQLQuery(filterForToday, "UncreditSaleDetail", null);
                IEnumerable<UncreditSaleDetail> uncreditsaleDetailsToday = _uncreditSaleDetailRepository.FindAll(unCreditSaleDetailToday).data.ToList();

                string productSaleDetailToday = FilterUtilityService.GenerateFilterHQLQuery(filterForToday, "ProductSaleDetail", null);
                IEnumerable<ProductSaleDetail> productSaleDetailsToday = _propduSaleDetailRepository.FindAll(productSaleDetailToday).data.ToList();


                string creditSaleDetail = FilterUtilityService.GenerateFilterHQLQuery(filter, "CreditSaleDetail", null);
                IEnumerable<CreditSaleDetail> creditsaleDetails = _creditSaleDetailRepository.FindAll(creditSaleDetail).data.ToList();

                string unCreditSaleDetail = FilterUtilityService.GenerateFilterHQLQuery(filter, "UncreditSaleDetail", null);
                IEnumerable<UncreditSaleDetail> uncreditsaleDetails = _uncreditSaleDetailRepository.FindAll(unCreditSaleDetail).data.ToList();

                string productSaleDetail = FilterUtilityService.GenerateFilterHQLQuery(filter, "ProductSaleDetail", null);
                IEnumerable<ProductSaleDetail> productSaleDetails = _propduSaleDetailRepository.FindAll(productSaleDetail).data.ToList();


                var todayCre = creditsaleDetailsToday.Any() ? creditsaleDetailsToday.ToList() : null;
                var todayUnc = uncreditsaleDetailsToday.Any() ? uncreditsaleDetailsToday.ToList() : null;
                var todayPro = productSaleDetailsToday.Any() ? productSaleDetailsToday.ToList() : null;

                var yesteDayCre = creditsaleDetails.Any() ? creditsaleDetails.Where(x => x.BonusDate != null).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.yesterday.Substring(0, 10)) == 0).ToList().ToList() : null;
                var yesteDayUnc = uncreditsaleDetails.Any() ? uncreditsaleDetails.Where(x => x.BonusDate != null).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.yesterday.Substring(0, 10)) == 0).ToList().ToList() : null;
                var yesteDayPro = productSaleDetails.Any() ? productSaleDetails.Where(x => x.BonusDate != null).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.yesterday.Substring(0, 10)) == 0).ToList().ToList() : null;

                var currentWeekUnc = uncreditsaleDetails.Any() ? uncreditsaleDetails.Where(x => x.BonusDate != null).Where(x => x.BonusDate != null).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.WeekStartDate.Substring(0, 10)) >= 0).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.WeekEndDate.Substring(0, 10)) <= 0).ToList() : null;
                var currentWeekCre = creditsaleDetails.Any() ? creditsaleDetails.Where(x => x.BonusDate != null).Where(x => x.BonusDate != null).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.WeekStartDate.Substring(0, 10)) >= 0).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.WeekEndDate.Substring(0, 10)) <= 0).ToList() : null;
                var currentWeekPro = productSaleDetails.Any() ? productSaleDetails.Where(x => x.BonusDate != null).Where(x => x.BonusDate != null).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.WeekStartDate.Substring(0, 10)) >= 0).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.WeekEndDate.Substring(0, 10)) <= 0).ToList() : null;

                var CurrentMonthUnc = uncreditsaleDetails.Any() ? uncreditsaleDetails.Where(x => x.BonusDate != null).Where(x => x.BonusDate != null).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.MonthStartDate.Substring(0, 10)) >= 0).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.MonthEndtDate.Substring(0, 10)) <= 0).ToList() : null;
                var CurrentMonthCre = creditsaleDetails.Any() ? creditsaleDetails.Where(x => x.BonusDate != null).Where(x => x.BonusDate != null).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.MonthStartDate.Substring(0, 10)) >= 0).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.MonthEndtDate.Substring(0, 10)) <= 0).ToList() : null;
                var CurrentMonthPro = productSaleDetails.Any() ? productSaleDetails.Where(x => x.BonusDate != null).Where(x => x.BonusDate != null).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.MonthStartDate.Substring(0, 10)) >= 0).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.MonthEndtDate.Substring(0, 10)) <= 0).ToList() : null;

                var LasttMonthUnc = uncreditsaleDetails.Any() ? uncreditsaleDetails.Where(x => x.BonusDate != null).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.LastMonthStartDate.Substring(0, 10)) >= 0).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.LastMonthEndtDate.Substring(0, 10)) <= 0).ToList() : null;
                var LasttMonthCre = creditsaleDetails.Any() ? creditsaleDetails.Where(x => x.BonusDate != null).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.LastMonthStartDate.Substring(0, 10)) >= 0).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.LastMonthEndtDate.Substring(0, 10)) <= 0).ToList() : null;
                var LasttMonthPro = productSaleDetails.Any() ? productSaleDetails.Where(x => x.BonusDate != null).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.LastMonthStartDate.Substring(0, 10)) >= 0).Where(x => x.BonusDate.Substring(0, 10).CompareTo(PersianDateTime.LastMonthEndtDate.Substring(0, 10)) <= 0).ToList() : null;


                #region Today

                IList<BonusComissionView> Today = new List<BonusComissionView>();
                if(todayCre!=null)
                foreach (var item in todayCre)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.Bonus = item.Bonus;
                    bc.Comission = item.Comission;
                    bc.HasCourier = item.BonusDate != null;
                    bc.CreateDate = item.Sale.CloseDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;
                    bc.BonusDate = item.BonusDate;
                    Today.Add(bc);
                }
                if(todayUnc!=null)
                foreach (var item in todayUnc)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.Bonus = item.Bonus;
                    bc.Comission = item.Comission;
                    bc.HasCourier = item.BonusDate != null;
                    bc.CreateDate = item.Sale.CloseDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;
                    bc.BonusDate = item.BonusDate;
                    Today.Add(bc);
                }
                if (todayPro != null)
                foreach (var item in todayPro)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.Bonus = item.Bonus;
                    bc.Comission = item.Comission;
                    bc.HasCourier = item.BonusDate != null;
                    bc.CreateDate = item.Sale.CloseDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;
                    bc.BonusDate = item.BonusDate;
                    Today.Add(bc);
                }

                #endregion

                #region YesterDay

                IList<BonusComissionView> YesterDay = new List<BonusComissionView>();
                if(yesteDayCre!=null)
                foreach (var item in yesteDayCre)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.Bonus = item.Bonus;
                    bc.Comission = item.Comission;
                    bc.HasCourier = item.BonusDate != null;
                    bc.CreateDate = item.Sale.CloseDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;
                    bc.BonusDate = item.BonusDate;
                    YesterDay.Add(bc);
                }
                if(yesteDayUnc!=null)
                foreach (var item in yesteDayUnc)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.Bonus = item.Bonus;
                    bc.Comission = item.Comission;
                    bc.HasCourier = item.BonusDate != null;
                    bc.CreateDate = item.Sale.CloseDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;
                    bc.BonusDate = item.BonusDate;
                    YesterDay.Add(bc);
                }
                if(yesteDayPro!=null)
                foreach (var item in yesteDayPro)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.Bonus = item.Bonus;
                    bc.Comission = item.Comission;
                    bc.HasCourier = item.BonusDate != null;
                    bc.CreateDate = item.Sale.CloseDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;
                    bc.BonusDate = item.BonusDate;
                    YesterDay.Add(bc);
                }


                #endregion

                #region Current Week

                IList<BonusComissionView> CurrentWeek = new List<BonusComissionView>();
                if(currentWeekUnc!=null)
                foreach (var item in currentWeekUnc)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.Bonus = item.Bonus;
                    bc.Comission = item.Comission;
                    bc.HasCourier = item.BonusDate != null;
                    bc.CreateDate = item.Sale.CloseDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;
                    bc.BonusDate = item.BonusDate;
                    CurrentWeek.Add(bc);
                }
                if(currentWeekCre!=null)
                foreach (var item in currentWeekCre)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.Bonus = item.Bonus;
                    bc.Comission = item.Comission;
                    bc.HasCourier = item.BonusDate != null;
                    bc.CreateDate = item.Sale.CloseDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;
                    bc.BonusDate = item.BonusDate;
                    CurrentWeek.Add(bc);
                }
                if(currentWeekPro!=null)
                foreach (var item in currentWeekPro)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.Bonus = item.Bonus;
                    bc.Comission = item.Comission;
                    bc.HasCourier = item.BonusDate != null;
                    bc.CreateDate = item.Sale.CloseDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;
                    bc.BonusDate = item.BonusDate;
                    CurrentWeek.Add(bc);
                }

                #endregion

                #region Current Month


                IList<BonusComissionView> CurrentMonth = new List<BonusComissionView>();
                if(CurrentMonthUnc!=null)
                foreach (var item in CurrentMonthUnc)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.Bonus = item.Bonus;
                    bc.Comission = item.Comission;
                    bc.HasCourier = item.BonusDate != null;
                    bc.CreateDate = item.Sale.CloseDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;
                    bc.BonusDate = item.BonusDate;
                    CurrentMonth.Add(bc);
                }
                if(CurrentMonthCre!=null)
                foreach (var item in CurrentMonthCre)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.Bonus = item.Bonus;
                    bc.Comission = item.Comission;
                    bc.HasCourier = item.BonusDate != null;
                    bc.CreateDate = item.Sale.CloseDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;
                    bc.BonusDate = item.BonusDate;
                    CurrentMonth.Add(bc);
                }
                if(CurrentMonthPro!=null)
                foreach (var item in CurrentMonthPro)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.Bonus = item.Bonus;
                    bc.Comission = item.Comission;
                    bc.HasCourier = item.BonusDate != null;
                    bc.CreateDate = item.Sale.CloseDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;
                    bc.BonusDate = item.BonusDate;
                    CurrentMonth.Add(bc);
                }

                #endregion

                #region Last Month


                IList<BonusComissionView> LastMonth = new List<BonusComissionView>();
                if(LasttMonthUnc!=null)
                foreach (var item in LasttMonthUnc)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.Bonus = item.Bonus;
                    bc.Comission = item.Comission;
                    bc.HasCourier = item.BonusDate != null;
                    bc.CreateDate = item.Sale.CloseDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;
                    bc.BonusDate = item.BonusDate;
                    LastMonth.Add(bc);
                }
                if(LasttMonthCre!=null)
                foreach (var item in LasttMonthCre)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.Bonus = item.Bonus;
                    bc.Comission = item.Comission;
                    bc.HasCourier = item.BonusDate != null;
                    bc.CreateDate = item.Sale.CloseDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;
                    bc.BonusDate = item.BonusDate;
                    LastMonth.Add(bc);
                }
                if(LasttMonthPro!=null)
                foreach (var item in LasttMonthPro)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.Bonus = item.Bonus;
                    bc.Comission = item.Comission;
                    bc.HasCourier = item.BonusDate != null;
                    bc.CreateDate = item.Sale.CloseDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;
                    bc.BonusDate = item.BonusDate;
                    LastMonth.Add(bc);
                }

                #endregion

                BonusComissionDetail BonusTemp = new BonusComissionDetail();

                var today = Today.GroupBy(l => l.CreateEmployeeID)
                    .Select(lg =>
                        new
                        {
                            CreateEmployee = lg.Key,
                            Sum = lg.Sum(x => x.Bonus)
                        }).OrderByDescending(x => x.Sum).FirstOrDefault();
                if (today != null)
                    list.Add(new SlideShowEmployeeView()
                    {
                        imageSrc = Today.Where((x => x.CreateEmployeeID == today.CreateEmployee)).FirstOrDefault().Photo,
                        title = "فروشنده برتر امروز",
                        alt =
                            Today.Where((x => x.CreateEmployeeID == today.CreateEmployee))
                                .FirstOrDefault()
                                .CreateEmployeeName,
                        Bonus = today.Sum
                    });


                var yesterday = YesterDay.GroupBy(l => l.CreateEmployeeID)
                    .Select(lg =>
                        new
                        {
                            CreateEmployee = lg.Key,
                            Sum = lg.Sum(x => x.Bonus)
                        }).OrderByDescending(x => x.Sum).FirstOrDefault();
                if (yesterday != null)
                    list.Add(new SlideShowEmployeeView()
                    {
                        imageSrc =
                            YesterDay.Where((x => x.CreateEmployeeID == yesterday.CreateEmployee)).FirstOrDefault().Photo,
                        title = "فروشنده برتر دیروز",
                        alt =
                            YesterDay.Where((x => x.CreateEmployeeID == yesterday.CreateEmployee))
                                .FirstOrDefault()
                                .CreateEmployeeName,
                        Bonus = yesterday.Sum
                    });


                var currentWeek = CurrentWeek.GroupBy(l => l.CreateEmployeeID)
                    .Select(lg =>
                        new
                        {
                            CreateEmployee = lg.Key,
                            Sum = lg.Sum(x => x.Bonus)
                        }).OrderByDescending(x => x.Sum).FirstOrDefault();
                if (currentWeek != null)
                    list.Add(new SlideShowEmployeeView()
                    {
                        imageSrc =
                            CurrentWeek.Where((x => x.CreateEmployeeID == currentWeek.CreateEmployee))
                                .FirstOrDefault()
                                .Photo,
                        title = "فروشنده برتر هفته جاری",
                        alt =
                            CurrentWeek.Where((x => x.CreateEmployeeID == currentWeek.CreateEmployee))
                                .FirstOrDefault()
                                .CreateEmployeeName,
                        Bonus = currentWeek.Sum
                    });


                var currentMonth = CurrentMonth.GroupBy(l => l.CreateEmployeeID)
                    .Select(lg =>
                        new
                        {
                            CreateEmployee = lg.Key,
                            Sum = lg.Sum(x => x.Bonus)
                        }).OrderByDescending(x => x.Sum).FirstOrDefault();
                if (currentMonth != null)
                    list.Add(new SlideShowEmployeeView()
                    {
                        imageSrc =
                            CurrentMonth.Where((x => x.CreateEmployeeID == currentMonth.CreateEmployee))
                                .FirstOrDefault()
                                .Photo,
                        title = "فروشنده برتر ماه جاری",
                        alt =
                            CurrentMonth.Where((x => x.CreateEmployeeID == currentMonth.CreateEmployee))
                                .FirstOrDefault()
                                .CreateEmployeeName,
                        Bonus = currentMonth.Sum
                    });


                var lastMonth = LastMonth.GroupBy(l => l.CreateEmployeeID)
                    .Select(lg =>
                        new
                        {
                            CreateEmployee = lg.Key,
                            Sum = lg.Sum(x => x.Bonus)
                        }).OrderByDescending(x => x.Sum).FirstOrDefault();
                if (lastMonth != null)
                {
                    SlideShowEmployeeView temp = new SlideShowEmployeeView();

                    temp.imageSrc = LastMonth.Where((x => x.CreateEmployeeID == lastMonth.CreateEmployee)).
                        FirstOrDefault().picture == null
                        ? ""
                        : LastMonth.Where((x => x.CreateEmployeeID == lastMonth.CreateEmployee)).
                            FirstOrDefault().Photo;
                    temp.title = "فروشنده برتر ماه قبل";
                    temp.alt =
                        LastMonth.Where((x => x.CreateEmployeeID == lastMonth.CreateEmployee))
                            .FirstOrDefault()
                            .CreateEmployeeName;
                    temp.Bonus = lastMonth.Sum;
                    list.Add(temp);

                }

                response.data = list;


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

        public GetGeneralResponse<IEnumerable<BonusComissionView>> GetTodayBonusComission()
        {
            GetGeneralResponse<IEnumerable<BonusComissionView>> response =
                new GetGeneralResponse<IEnumerable<BonusComissionView>>();
            IList<BonusComissionView> list = new List<BonusComissionView>();
            try
            {

                IList<FilterData> filter = new List<FilterData>();

                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        type = "dateBetween",
                        value =
                            new[] { PersianDateTime.LastMonthStartDate.Substring(0, 10), PersianDateTime.Now.Substring(0, 10) }
                    },
                    field = "CreateDate"
                });

                #region Getting Data

                string uncBonus =
                    "from  UncreditSaleDetail m where m.BonusDate between '" +
                   PersianDateTime.LastMonthStartDate.Substring(0, 10) + " 00:00:00' and '" + PersianDateTime.Now.Substring(0, 10) + " 23:59:59' and Bonus<>0";
                IEnumerable<UncreditSaleDetail> unCreditSaleDetailsbonus =
                    _uncreditSaleDetailRepository.FindAll(uncBonus).data.ToList();

                string uncBonusToDay =
                    "from  UncreditSaleDetail m where m.Sale.CloseDate between '" +
                   PersianDateTime.Now.Substring(0, 10) + " 00:00:00' and '" + PersianDateTime.Now.Substring(0, 10) + " 23:59:59' and Bonus<>0";
                IEnumerable<UncreditSaleDetail> unCreditSaleDetailsbonusToday =
                    _uncreditSaleDetailRepository.FindAll(uncBonusToDay).data.ToList();

                string proBonus =
                    "from  ProductSaleDetail m where m.BonusDate between '" +
                    PersianDateTime.LastMonthStartDate.Substring(0, 10) + " 00:00:00' and '" +
                    PersianDateTime.Now.Substring(0, 10) + " 23:59:59' and Bonus<>0";
                IEnumerable<ProductSaleDetail> productSaleDetailsBonus =
                    _propduSaleDetailRepository.FindAll(proBonus).data.ToList();


                string proBonusToday =
                    "from  ProductSaleDetail m where m.Sale.CloseDate between '" +
                    PersianDateTime.Now.Substring(0, 10) + " 00:00:00' and '" +
                    PersianDateTime.Now.Substring(0, 10) + " 23:59:59' and Bonus<>0";
                IEnumerable<ProductSaleDetail> productSaleDetailsBonusToday =
                    _propduSaleDetailRepository.FindAll(proBonusToday).data.ToList();

                string creBonus =
                    "from  CreditSaleDetail m where m.BonusDate between '" +
                    PersianDateTime.LastMonthStartDate.Substring(0, 10) + " 00:00:00' and '" +
                    PersianDateTime.Now.Substring(0, 10) + " 23:59:59' and Bonus<>0";
                IEnumerable<CreditSaleDetail> creditsaleDetailsBonus =
                    _creditSaleDetailRepository.FindAll(creBonus).data.ToList();

                string creBonusToday =
                    "from  CreditSaleDetail m where m.Sale.CloseDate between '" +
                    PersianDateTime.Now.Substring(0, 10) + " 00:00:00' and '" +
                    PersianDateTime.Now.Substring(0, 10) + " 23:59:59' and Bonus<>0";
                IEnumerable<CreditSaleDetail> creditsaleDetailsBonusToday =
    _creditSaleDetailRepository.FindAll(creBonusToday).data.ToList();


                string uncCom =
                    "from  UncreditSaleDetail m where m.ComissionDate between '" +
                    PersianDateTime.LastMonthStartDate.Substring(0, 10) + " 00:00:00' and '" + PersianDateTime.Now.Substring(0, 10) + " 23:59:59' and Comission<>0";
                IEnumerable<UncreditSaleDetail> unCreditSaleDetailsComission =
                    _uncreditSaleDetailRepository.FindAll(uncCom).data.ToList();

                string proCom =
                    "from  ProductSaleDetail m where m.ComissionDate between '" +
                    PersianDateTime.LastMonthStartDate.Substring(0, 10) + " 00:00:00' and '" + PersianDateTime.Now.Substring(0, 10) + " 23:59:59' and Comission<>0";
                IEnumerable<ProductSaleDetail> productSaleDetailsComission =
                    _propduSaleDetailRepository.FindAll(proCom).data.ToList();

                string creCom =
                    "from  CreditSaleDetail m where m.ComissionDate between '" +
                    PersianDateTime.LastMonthStartDate.Substring(0, 10) + " 00:00:00' and '" + PersianDateTime.Now.Substring(0, 10) + " 23:59:59' and Comission<>0";
                IEnumerable<CreditSaleDetail> creditsaleDetailsComission =
                    _creditSaleDetailRepository.FindAll(creCom).data.ToList();

                #endregion

                #region Prepairing Data

                foreach (var item in unCreditSaleDetailsbonus)
                {
                    BonusComissionView bc = new BonusComissionView();
                    if (item.Sale.HasCourier == true)
                    {
                        bc.ActionDate = item.BonusDate;
                    }
                    bc.Bonus = item.Bonus;
                    bc.Comission = 0;
                    bc.CreateDate = item.CreateDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.HasCourier = true;
                    bc.UnCreditSaleDetailName = item.UncreditService.UncreditServiceName;

                    bc.Type = "U";
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;

                    list.Add(bc);
                }


                foreach (var item in unCreditSaleDetailsbonusToday)
                {
                    BonusComissionView bc = new BonusComissionView();
                    bc.ActionDate = item.Sale.CloseDate;
                    bc.Bonus = item.Bonus;
                    bc.Comission = 0;
                    bc.CreateDate = item.CreateDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.HasCourier = item.Sale.HasCourier;
                    bc.UnCreditSaleDetailName = item.UncreditService.UncreditServiceName;

                    bc.Type = "U";
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;

                    list.Add(bc);
                }

                foreach (var item in productSaleDetailsBonus)
                {
                    BonusComissionView bc = new BonusComissionView();
                    if (item.Sale.HasCourier == true)
                    {

                        bc.ActionDate = item.BonusDate;
                    }
                    bc.Bonus = item.Bonus;
                    bc.Comission = 0;
                    bc.CreateDate = item.CreateDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.HasCourier = item.Sale.HasCourier;
                    bc.UnCreditSaleDetailName = item.ProductPrice.ProductPriceTitle;

                    bc.Type = "P";
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;

                    list.Add(bc);
                }

                foreach (var item in productSaleDetailsBonusToday)
                {
                    BonusComissionView bc = new BonusComissionView();
                    bc.ActionDate = item.Sale.CloseDate;
                    bc.Bonus = item.Bonus;
                    bc.Comission = 0;
                    bc.CreateDate = item.CreateDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.HasCourier = item.Sale.HasCourier;
                    bc.UnCreditSaleDetailName = item.ProductPrice.ProductPriceTitle;

                    bc.Type = "P";
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;

                    list.Add(bc);
                }

                foreach (var item in creditsaleDetailsBonus)
                {
                    BonusComissionView bc = new BonusComissionView();
                    if (item.Sale.HasCourier == true)
                    {

                        bc.ActionDate = item.BonusDate;
                    }
                    bc.Bonus = item.Bonus;
                    bc.Comission = 0;
                    bc.CreateDate = item.CreateDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.HasCourier = item.Sale.HasCourier;
                    bc.UnCreditSaleDetailName = item.CreditService.ServiceName;

                    bc.Type = "C";
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;

                    list.Add(bc);
                }

                foreach (var item in creditsaleDetailsBonusToday)
                {
                    BonusComissionView bc = new BonusComissionView();

                    bc.ActionDate = item.Sale.CloseDate;

                    bc.Bonus = item.Bonus;
                    bc.Comission = 0;
                    bc.CreateDate = item.CreateDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.HasCourier = item.Sale.HasCourier;
                    bc.UnCreditSaleDetailName = item.CreditService.ServiceName;

                    bc.Type = "C";
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;

                    list.Add(bc);
                }

                foreach (var item in unCreditSaleDetailsComission)
                {
                    BonusComissionView bc = new BonusComissionView();
                    if (item.ComissionDate != null)
                        bc.ActionDate = item.ComissionDate;
                    bc.Bonus = 0;
                    bc.Comission = item.Comission;
                    bc.CreateDate = item.CreateDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.HasCourier = item.Sale.HasCourier;
                    bc.UnCreditSaleDetailName = item.UncreditService.UncreditServiceName;

                    bc.Type = "U";
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;

                    list.Add(bc);
                }

                foreach (var item in productSaleDetailsComission)
                {
                    BonusComissionView bc = new BonusComissionView();
                    if (item.ComissionDate != null)
                        bc.ActionDate = item.ComissionDate;
                    bc.Bonus = 0;
                    bc.Comission = item.Comission;
                    bc.CreateDate = item.CreateDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.HasCourier = item.Sale.HasCourier;
                    bc.UnCreditSaleDetailName = item.ProductPrice.ProductPriceTitle;

                    bc.Type = "P";
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;

                    list.Add(bc);
                }

                foreach (var item in creditsaleDetailsComission)
                {
                    BonusComissionView bc = new BonusComissionView();
                    if (item.ComissionDate != null)
                        bc.ActionDate = item.ComissionDate;
                    bc.Bonus = 0;
                    bc.Comission = item.Comission;
                    bc.CreateDate = item.CreateDate;
                    bc.CreateEmployeeID = item.SaleEmployee.ID;
                    bc.CreateEmployeeName = item.SaleEmployee.Name;
                    bc.HasCourier = item.Sale.HasCourier;
                    bc.UnCreditSaleDetailName = item.CreditService.ServiceName;

                    bc.Type = "C";
                    bc.ID = item.ID;
                    bc.IsRollback = item.IsRollbackDetail;
                    bc.picture = item.CreateEmployee.Picture;

                    list.Add(bc);
                }

                #endregion


                response.data = list;
                response.totalCount = list.Count();

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

        #region Bonus Report

        public GetGeneralResponse<IEnumerable<BonusMasterReportView>> GetBonusReport(int pageSize, int pageNumber, IList<FilterData> filter, IList<FilterData> Creditfilter, IList<FilterData> unCreditfilter, IList<FilterData> poductfilter, IList<Sort> sort, bool pro, bool cre, bool unc)
        {
            GetGeneralResponse<IEnumerable<BonusMasterReportView>> response = new GetGeneralResponse<IEnumerable<BonusMasterReportView>>();
            IList<BonusMasterReportView> list1 = new List<BonusMasterReportView>();

            try
            {

                if (cre)
                {
                    string query1 = FilterUtilityService.GenerateFilterHQLQuery(Creditfilter, "CreditSaleDetail", sort);
                    Response<CreditSaleDetail> C = _creditSaleDetailRepository.FindAll(query1);
                    foreach (var item in C.data)
                    {
                        var temp = new BonusMasterReportView();
                        temp.SaleEmployeeName = item.SaleEmployee.Name;
                        temp.CreditServiceBonus = item.Bonus;
                        list1.Add(temp);
                    }

                }

                if (unc)
                {
                    string query2 = FilterUtilityService.GenerateFilterHQLQuery(unCreditfilter, "UncreditSaleDetail", sort);
                    //06142226775


                    Response<UncreditSaleDetail> u = _uncreditSaleDetailRepository.FindAll(query2);
                    foreach (var item in u.data)
                    {
                        var temp = new BonusMasterReportView();
                        temp.SaleEmployeeName = item.SaleEmployee.Name;
                        temp.UncreditServiceBonus = item.Bonus;
                        list1.Add(temp);
                    }
                }
                if (pro)
                {
                    string query3 = FilterUtilityService.GenerateFilterHQLQuery(poductfilter, "ProductSaleDetail", sort);

                    Response<ProductSaleDetail> p = _propduSaleDetailRepository.FindAll(query3);
                    foreach (var item in p.data)
                    {
                        var temp = new BonusMasterReportView();

                        temp.SaleEmployeeName = item.SaleEmployee.Name;
                        temp.ProductBonus = item.Bonus;
                        list1.Add(temp);
                    }
                }
                if (!pro && !cre && !unc)
                {
                    string query1 = FilterUtilityService.GenerateFilterHQLQuery(Creditfilter, "CreditSaleDetail", sort);
                    Response<CreditSaleDetail> C = _creditSaleDetailRepository.FindAll(query1);
                    foreach (var item in C.data)
                    {
                        var temp = new BonusMasterReportView();
                        temp.SaleEmployeeName = item.SaleEmployee.Name;
                        temp.CreditServiceBonus = item.Bonus;
                        list1.Add(temp);
                    }

                    string query2 = FilterUtilityService.GenerateFilterHQLQuery(unCreditfilter, "UncreditSaleDetail", sort);

                    Response<UncreditSaleDetail> u = _uncreditSaleDetailRepository.FindAll(query2);
                    foreach (var item in u.data)
                    {
                        var temp = new BonusMasterReportView();
                        temp.SaleEmployeeName = item.SaleEmployee.Name;
                        temp.UncreditServiceBonus = item.Bonus;
                        list1.Add(temp);
                    }

                    string query3 = FilterUtilityService.GenerateFilterHQLQuery(poductfilter, "ProductSaleDetail", sort);

                    Response<ProductSaleDetail> p = _propduSaleDetailRepository.FindAll(query3);
                    foreach (var item in p.data)
                    {
                        var temp = new BonusMasterReportView();
                        temp.SaleEmployeeName = item.SaleEmployee.Name;
                        temp.ProductBonus = item.Bonus;
                        list1.Add(temp);
                    }
                }
                //IList<BonusMasterReportView> list = new List<BonusMasterReportView>();
                //foreach (BonusComission item in list1)
                //{
                //    BonusMasterReportView bonusMasterReportView = new BonusMasterReportView();
                //    bonusMasterReportView.GroupName = item.CreateEmployee.Group.GroupName;
                //    bonusMasterReportView.ID = item.ID;
                //    bonusMasterReportView.SaleEmployeeName = item.CreateEmployee.Name;
                //    if (item.ProductSaleDetail != null)
                //        bonusMasterReportView.ProductBonus = item.Bonus;
                //    if (item.UnCreditSaleDetail != null)
                //        bonusMasterReportView.UncreditServiceBonus = item.Bonus;
                //    if (item.CreditSaleDetail != null)
                //        bonusMasterReportView.UncreditServiceBonus = item.Bonus;

                //    list.Add(bonusMasterReportView);
                //}

                var final = from bs in list1
                            group bs by bs.SaleEmployeeName
                                into g
                                select new BonusMasterReportView
                                {
                                    SaleEmployeeName = g.Key,
                                    UncreditServiceBonus = g.Sum(x => x.UncreditServiceBonus),
                                    ProductBonus = g.Sum(x => x.ProductBonus),
                                    CreditServiceBonus = g.Sum(x => x.CreditServiceBonus)

                                };
                response.data = final.ToList();
                response.totalCount = response.data.Count();


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

        #region Comission Report

        public GetGeneralResponse<IEnumerable<ComissionMasterReportView>> GetComissionReport(int pageSize, int pageNumber, IList<FilterData> filter, IList<FilterData> Creditfilter, IList<FilterData> unCreditfilter, IList<FilterData> poductfilter, IList<Sort> sort, bool pro, bool cre, bool unc)
        {
            GetGeneralResponse<IEnumerable<ComissionMasterReportView>> response = new GetGeneralResponse<IEnumerable<ComissionMasterReportView>>();
            IList<ComissionMasterReportView> list1 = new List<ComissionMasterReportView>();

            try
            {

                if (cre)
                {
                    string query1 = FilterUtilityService.GenerateFilterHQLQuery(Creditfilter, "CreditSaleDetail", sort);
                    Response<CreditSaleDetail> C = _creditSaleDetailRepository.FindAll(query1);
                    foreach (var item in C.data)
                    {
                        var temp = new ComissionMasterReportView();
                        temp.SaleEmployeeName = item.SaleEmployee.Name;
                        temp.CreditServiceComission = item.Comission;
                        temp.CreateEmployeeName = item.CreateEmployee.Name;
                        temp.ComissionDate = item.ComissionDate;
                        temp.DeliverDate = item.DeliverDate;
                        list1.Add(temp);
                    }

                }

                if (unc)
                {
                    string query2 = FilterUtilityService.GenerateFilterHQLQuery(unCreditfilter, "UncreditSaleDetail", sort);

                    Response<UncreditSaleDetail> u = _uncreditSaleDetailRepository.FindAll(query2);
                    foreach (var item in u.data)
                    {
                        var temp = new ComissionMasterReportView();
                        temp.SaleEmployeeName = item.SaleEmployee.Name;
                        temp.UncreditServiceComission = item.Comission;
                        temp.CreateEmployeeName = item.CreateEmployee.Name;
                        temp.ComissionDate = item.ComissionDate;
                        temp.DeliverDate = item.DeliverDate;
                        list1.Add(temp);
                    }
                }
                if (pro)
                {
                    string query3 = FilterUtilityService.GenerateFilterHQLQuery(poductfilter, "ProductSaleDetail", sort);

                    Response<ProductSaleDetail> p = _propduSaleDetailRepository.FindAll(query3);
                    foreach (var item in p.data)
                    {
                        var temp = new ComissionMasterReportView();
                        temp.SaleEmployeeName = item.SaleEmployee.Name;
                        temp.ProductComission = item.Comission;
                        temp.CreateEmployeeName = item.CreateEmployee.Name;
                        temp.ComissionDate = item.ComissionDate;
                        temp.DeliverDate = item.DeliverDate;
                        list1.Add(temp);
                    }
                }
                if (!pro && !cre && !unc)
                {
                    string query1 = FilterUtilityService.GenerateFilterHQLQuery(Creditfilter, "CreditSaleDetail", sort);
                    Response<CreditSaleDetail> C = _creditSaleDetailRepository.FindAll(query1);
                    foreach (var item in C.data)
                    {
                        var temp = new ComissionMasterReportView();
                        temp.SaleEmployeeName = item.SaleEmployee.Name;
                        temp.CreditServiceComission = item.Comission;
                        temp.CreateEmployeeName = item.CreateEmployee.Name;
                        temp.ComissionDate = item.ComissionDate;
                        temp.DeliverDate = item.DeliverDate;
                        list1.Add(temp);
                    }

                    string query2 = FilterUtilityService.GenerateFilterHQLQuery(unCreditfilter, "UncreditSaleDetail", sort);

                    Response<UncreditSaleDetail> u = _uncreditSaleDetailRepository.FindAll(query2);
                    foreach (var item in u.data)
                    {
                        var temp = new ComissionMasterReportView();
                        temp.SaleEmployeeName = item.SaleEmployee.Name;
                        temp.UncreditServiceComission = item.Comission;
                        temp.CreateEmployeeName = item.CreateEmployee.Name;
                        temp.ComissionDate = item.ComissionDate;
                        temp.DeliverDate = item.DeliverDate;
                        list1.Add(temp);
                    }

                    string query3 = FilterUtilityService.GenerateFilterHQLQuery(poductfilter, "ProductSaleDetail", sort);

                    Response<ProductSaleDetail> p = _propduSaleDetailRepository.FindAll(query3);
                    foreach (var item in p.data)
                    {
                        var temp = new ComissionMasterReportView();
                        temp.SaleEmployeeName = item.SaleEmployee.Name;
                        temp.ProductComission = item.Comission;
                        temp.CreateEmployeeName = item.CreateEmployee.Name;
                        temp.ComissionDate = item.ComissionDate;
                        temp.DeliverDate = item.DeliverDate;
                        list1.Add(temp);
                    }
                }

                var final = from bs in list1
                            group bs by bs.SaleEmployeeName
                                into g
                                select new ComissionMasterReportView
                                {
                                    SaleEmployeeName = g.Key,
                                    UncreditServiceComission = g.Sum(x => x.UncreditServiceComission),
                                    ProductComission = g.Sum(x => x.ProductComission),
                                    CreditServiceComission = g.Sum(x => x.CreditServiceComission)

                                };
                response.data = final.ToList();
                response.totalCount = response.data.Count();


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


        #region Update All Bonus and Comission

        //public GeneralResponse UpdateAll()
        //{
        //    GeneralResponse response=new GeneralResponse();
        //    try
        //    {
        //        IList<FilterData> filters=new List<FilterData>();

        //        filters.Add(new FilterData()
        //        {
        //            data = new data()
        //            {
        //                comparison = "eq",
        //                type = "boolean",
        //                value = new []{bool.TrueString}
        //            },
        //            field = "Sale.Closed"
        //        });

        //        filters.Add(new FilterData()
        //        {
        //            data = new data()
        //            {
        //                comparison = "gt",
        //                type = "numeric",
        //                value = new[] { "0" }
        //            },
        //            field = "CreditService.Bonus"
        //        });
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        #endregion
    }
}
