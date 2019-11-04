#region Usings

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Controllers.ViewModels.FiscalCatalog;
using Iesi.Collections;
using Model.Customers;
using Model.Store;
using Services.Messaging;
using Services.Messaging.FiscalCatalogService;
using Services.Messaging.SaleCatalogService;
using Services.ViewModels.Customers;
using Services.ViewModels.Fiscals;
using Services.ViewModels.Store;
using Services.Interfaces;
using Controllers;
using Controllers.ViewModels.Reports;
using Kendo.Mvc.UI;
using Services.ViewModels.Sales;
using StructureMap.Interceptors;
using Controllers.ViewModels.SupportCatalog;
using Services.ViewModels.Support;
using Infrastructure.Querying;
using Services.ViewModels.Reports;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using Services.Implementations;
using Model.Sales;
using Services.ViewModels.Employees;
using Infrastructure.Persian;
using Services.Messaging.ReportCatalogService;
using System.Web.Script.Serialization;
using System.Data.SqlClient;


#endregion

namespace Controllers.Controllers
{


    public class ReportController : BaseController
    {
        #region Delcares

        private readonly IEmployeeService _employeeService;

        private readonly ICustomerService _customerService;

        private readonly IFiscalService _fiscalService;

        private readonly ISaleService _saleService;

        private readonly IProductLogService _productLogService;

        private readonly IProductSaleDetailService _productSaleDetailService;

        private readonly IProductService _productService;

        private readonly IPersenceSupportService _persenceSupportService;

        private readonly IStoreProductService _storeProductService;

        private readonly ICreditSaleDetailService _creditSaleDetailService;

        private readonly IUncreditSaleDetailService _uncreditSaleDetailService;

        private readonly ISupportService _supportService;

        private readonly IBonusComissionService _bonusComissionService;

        private readonly ICampaignPaymentService _campaignPaymentService;



        #endregion

        #region Ctor

        public ReportController(IEmployeeService employeeService, ICustomerService customerService,
            IFiscalService fiscalService, ISaleService saleService,
            IProductLogService productLogService, IProductSaleDetailService productSaleDetailService,
            IProductService productService,
            IPersenceSupportService persenceSupportService, IStoreProductService storeProductService,
            IUncreditSaleDetailService uncreditSaleDetailService,
            ICreditSaleDetailService creditSaleDetailService, ISupportService supportService,
            IBonusComissionService bonusComissionService, ICampaignPaymentService campaignPaymentService)
            : base(employeeService)
        {

            this._employeeService = employeeService;
            this._customerService = customerService;
            this._fiscalService = fiscalService;
            this._saleService = saleService;
            this._productLogService = productLogService;
            this._productSaleDetailService = productSaleDetailService;
            this._productService = productService;
            this._persenceSupportService = persenceSupportService;
            this._storeProductService = storeProductService;
            this._uncreditSaleDetailService = uncreditSaleDetailService;
            this._creditSaleDetailService = creditSaleDetailService;
            this._supportService = supportService;
            this._bonusComissionService = bonusComissionService;
            this._campaignPaymentService = campaignPaymentService;
        }

        #endregion

        #region Cash Report

        /// <summary>
        /// قبض صندوق
        /// </summary>
        /// <param name="FiscalID"></param>
        /// <returns></returns>
        public ActionResult GetCashReport(Guid FiscalID)
        {
            CashReport cashReport = new CashReport();
            GetGeneralResponse<CashReport> response = new GetGeneralResponse<CashReport>();
            cashReport.EmployeeView = GetEmployee();
            FiscalView fiscalView = new FiscalView();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Report_Cash");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                response.data = cashReport;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion


            #region Prepairng FiscalView Data

            fiscalView = _fiscalService.GetFiscal(new GetRequest() { ID = FiscalID }).FiscalView;

            cashReport.CashDate = fiscalView.CreateDate;
            cashReport.ConfirmDate = fiscalView.ConfirmDate;
            cashReport.DisplayCost = fiscalView.DisplayCost;
            cashReport.DocumentType = fiscalView.DocumentTypeTxt;
            cashReport.InvestDate = fiscalView.InvestDate;
            cashReport.CreateEmployeeName = fiscalView.CreateEmployeeName;
            cashReport.SerialNumber = fiscalView.SerialNumber;
            cashReport.ConfiremdCost = fiscalView.ConfirmedCost;
            cashReport.ConfirmEmpoloyeeName = fiscalView.ConfirmEmployeeName;
            cashReport.Note = fiscalView.Note;
            cashReport.FiscalReciptNumber = fiscalView.FiscalReciptNumber;


            #endregion

            #region Prepairing CustomerView Data

            CustomerView customerView =
                _customerService.GetCustomer(new GetRequest() { ID = fiscalView.CustomerID }).CustomerView;

            cashReport.CustomerName = customerView.Name;
            cashReport.ADSLPhone = customerView.ADSLPhone;
            cashReport.Mobile1 = customerView.Mobile1;
            cashReport.Address = customerView.Address;

            #endregion

            response.data = cashReport;

            return View(cashReport);

        }

        #endregion

        #region Factor Report

        /// <summary>
        /// فاکتور فروش
        /// </summary>
        /// <param name="saleID"></param>
        /// <returns></returns>
        public ActionResult GetFactorReport(Guid saleID)
        {
            FactorView factorData = new FactorView();
            factorData.EmployeeView = GetEmployee();
            GetGeneralResponse<FactorView> response = new GetGeneralResponse<FactorView>();

            //#region Access Check

            //bool hasPermission = GetEmployee().IsGuaranteed("Report_Factor");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    response.data = factorData;
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}

            //#endregion


            factorData.SaleView = _saleService.GetSale(saleID).data;

            factorData.CustomerView = _customerService.getCustomerbyPhone(factorData.SaleView.ADSLPhone);

            #region Preparing factor SaleDetailViews

            IList<SaleDetailReportView> saleDetailViews = new List<SaleDetailReportView>();

            if (factorData.SaleView != null && factorData.SaleView.CreditSaleDetails != null)
                foreach (var saleDetail in factorData.SaleView.CreditSaleDetails)
                {
                    SaleDetailReportView saleDetailView = new SaleDetailReportView();

                    saleDetailView.Units = saleDetail.Units;
                    saleDetailView.Discount = saleDetail.Discount;
                    saleDetailView.UnitPrice = saleDetail.UnitPrice;
                    saleDetailView.SaleDetail = saleDetail.CreditServiceName;
                    saleDetailView.Imposition = saleDetail.Imposition;
                    saleDetailView.LineTotalWithoutDiscountAndImposition = saleDetail.Units * saleDetail.UnitPrice;

                    saleDetailViews.Add(saleDetailView);
                }

            if (factorData.SaleView != null && factorData.SaleView.UncreditSaleDetails != null)
                foreach (var saleDetail in factorData.SaleView.UncreditSaleDetails)
                {
                    SaleDetailReportView saleDetailView = new SaleDetailReportView();

                    saleDetailView.Units = saleDetail.Units;
                    saleDetailView.Discount = saleDetail.Discount;
                    saleDetailView.UnitPrice = saleDetail.UnitPrice;
                    saleDetailView.SaleDetail = saleDetail.UncreditServiceName;
                    saleDetailView.Imposition = saleDetail.Imposition;
                    saleDetailView.LineTotalWithoutDiscountAndImposition = saleDetail.Units * saleDetail.UnitPrice;

                    saleDetailViews.Add(saleDetailView);
                }

            if (factorData.SaleView != null && factorData.SaleView.ProductSaleDetails != null)
                foreach (var saleDetail in factorData.SaleView.ProductSaleDetails)
                {
                    SaleDetailReportView saleDetailView = new SaleDetailReportView();

                    saleDetailView.Units = saleDetail.Units;
                    saleDetailView.Discount = saleDetail.Discount;
                    saleDetailView.UnitPrice = saleDetail.UnitPrice;
                    saleDetailView.SaleDetail = saleDetail.ProductPriceTitle;
                    saleDetailView.Imposition = saleDetail.Imposition;
                    saleDetailView.LineTotalWithoutDiscountAndImposition = saleDetail.Units * saleDetail.UnitPrice;

                    saleDetailViews.Add(saleDetailView);
                }


            factorData.SaleDetailViews = saleDetailViews;
            response.data = factorData;

            #endregion

            //return Json(response, JsonRequestBehavior.AllowGet);
            return View(response.data);
        }

        #endregion

        #region Send Factor By Email

        /// <summary>
        /// فاکتور فروش
        /// </summary>
        /// <param name="saleID"></param>
        /// <returns></returns>
        public JsonResult SendfactorByEmail(Guid saleID)
        {
            FactorView factorData = new FactorView();
            factorData.EmployeeView = GetEmployee();
            GetGeneralResponse<FactorView> response = new GetGeneralResponse<FactorView>();
            GeneralResponse finalResponse = new GeneralResponse();

            factorData.SaleView = _saleService.GetSale(saleID).data;

            factorData.CustomerView = _customerService.getCustomerbyPhone(factorData.SaleView.ADSLPhone);

            #region Preparing factor SaleDetailViews

            IList<SaleDetailReportView> saleDetailViews = new List<SaleDetailReportView>();

            if (factorData.SaleView != null && factorData.SaleView.CreditSaleDetails != null)
                foreach (var saleDetail in factorData.SaleView.CreditSaleDetails)
                {
                    SaleDetailReportView saleDetailView = new SaleDetailReportView();

                    saleDetailView.Units = saleDetail.Units;
                    saleDetailView.Discount = saleDetail.Discount;
                    saleDetailView.UnitPrice = saleDetail.UnitPrice;
                    saleDetailView.SaleDetail = saleDetail.CreditServiceName;
                    saleDetailView.Imposition = saleDetail.Imposition;
                    saleDetailView.LineTotalWithoutDiscountAndImposition = saleDetail.Units * saleDetail.UnitPrice;

                    saleDetailViews.Add(saleDetailView);
                }

            if (factorData.SaleView != null && factorData.SaleView.UncreditSaleDetails != null)
                foreach (var saleDetail in factorData.SaleView.UncreditSaleDetails)
                {
                    SaleDetailReportView saleDetailView = new SaleDetailReportView();

                    saleDetailView.Units = saleDetail.Units;
                    saleDetailView.Discount = saleDetail.Discount;
                    saleDetailView.UnitPrice = saleDetail.UnitPrice;
                    saleDetailView.SaleDetail = saleDetail.UncreditServiceName;
                    saleDetailView.Imposition = saleDetail.Imposition;
                    saleDetailView.LineTotalWithoutDiscountAndImposition = saleDetail.Units * saleDetail.UnitPrice;

                    saleDetailViews.Add(saleDetailView);
                }

            if (factorData.SaleView != null && factorData.SaleView.ProductSaleDetails != null)
                foreach (var saleDetail in factorData.SaleView.ProductSaleDetails)
                {
                    SaleDetailReportView saleDetailView = new SaleDetailReportView();

                    saleDetailView.Units = saleDetail.Units;
                    saleDetailView.Discount = saleDetail.Discount;
                    saleDetailView.UnitPrice = saleDetail.UnitPrice;
                    saleDetailView.SaleDetail = saleDetail.ProductPriceTitle;
                    saleDetailView.Imposition = saleDetail.Imposition;
                    saleDetailView.LineTotalWithoutDiscountAndImposition = saleDetail.Units * saleDetail.UnitPrice;

                    saleDetailViews.Add(saleDetailView);
                }

            factorData.SaleDetailViews = saleDetailViews;
            response.data = factorData;

            #endregion

            #region Create HTML Factor

            string body = @"

            <div style=""font-family:tahoma;margin: 0px; padding: 0px;direction:rtl;text-align: right;padding: 2px;font-family:tahoma"">
                    <div style=""font-family:tahoma;text-align: center; font-size: larger;font-weight: bolder;"">صورتحساب</div><br/>
                    <table style=""font-family:tahoma;width: 100%;border: 1px solid black;border-bottom: 2px solid black;border-right: 2px solid black;margin-bottom: 5px;font-family:tahoma"" cellpadding=""0"" cellspacing=""0"">
                        <tr>
                            <td colspan=""12"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">اطلاعات مشترک</td>
                        </tr>
                        <tr>
                            <td colspan=""2"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">نام و نام خانوادگی</td>";
            body +=
                @"<td colspan=""2"" style=""font-family:tahoma;text-align:center;border-left:1px solid black;font-size:small;display:table-cell;border-bottom:1px solid black;border-top:1px solid black;"">" +
                response.data.SaleView.CustomerName + @"</td>
                            <td colspan=""2"" style=""font-family:tahoma;text-align:center;border-left:1px solid black;font-size:small;display:table-cell;border-bottom:1px solid black;border-top:1px solid black"">تلفن تماس</td>";
            body +=
                @"<td colspan=""2"" style=""font-family:tahoma;text-align:center;border-left:1px solid black;font-size:small;display:table-cell;border-bottom:1px solid black;border-top:1px solid black;;"">" +
                response.data.SaleView.ADSLPhone + @"</td>
                            <td colspan=""2"" style=""font-family:tahoma;text-align:center;border-left:1px solid black;font-size:small;display:table-cell;border-bottom:1px solid black;border-top:1px solid black"">مرکز مخابراتی</td>";
            body +=
                @"<td colspan=""2"" style=""font-family:tahoma;text-align:center;border-left:1px solid black;font-size:small;display:table-cell;border-bottom:1px solid black;border-top:1px solid black;"">" +
                response.data.SaleView.CenterName + @"</td>
                        </tr>
                        <tr>
                            <td colspan=""2"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">آدرس</td>";
            body +=
                @"<td colspan=""11"" style=""font-family:tahoma;text-align:center;border-left:1px solid black;font-size:small;display:table-cell;font-family:tahoma"">" +
                response.data.SaleView.Address + @"</td>
                        </tr>
                        <tr>
                            <td colspan=""2"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">تلفن همراه</td>";
            body +=
                @"<td colspan=""4"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">" +
                response.data.SaleView.Mobile1 + @"</td>
                            <td colspan=""2"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">تاریخ صورتحساب</td>";
            body +=
                @"<td colspan=""4"" style=""font-family:tahoma;text-align:center;border-left:1px solid black;border-top:1px solid black;font-size:small;display:table-cell"">" +
                response.data.SaleView.CreateDate + @"</td>
                        </tr>
                        <tr>
                            <td colspan=""2"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">تلفن مورد تقاضا</td>";
            body +=
                @"<td colspan=""4"" style=""font-family:tahoma;text-align:center;border-left:1px solid black;border-top:1px solid black;font-size:small;display:table-cell"">" +
                response.data.SaleView.ADSLPhone + @"</td>
                            <td colspan=""2"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">نام صاحب خط</td>";
            body +=
                @"<td colspan=""4"" style=""font-family:tahoma;text-align:center;border-left:1px solid black;border-top:1px solid black;font-size:small;display:table-cell"">" +
                response.data.SaleView.Sname + @"</td>
                        </tr>
                    </table>
                    <table style=""font-family:tahoma;font-family:tahoma;width: 100%;border: 1px solid black;border-bottom: 2px solid black;border-right: 2px solid black;margin-bottom: 5px;"" cellpadding=""0"" cellspacing=""0"">
                        <tr>
                            <td colspan=""1"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">ردیف</td>
                            <td colspan=""4"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">شرح کالا و خدمات</td>
                            <td colspan=""2"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">فی(ریال)</td>
                            <td colspan=""2"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">تعداد</td>
                            <td colspan=""2"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">جمع کل(ریال)</td>
                        </tr>";
            int i = 0;
            foreach (var item in response.data.SaleDetailViews)
            {
                body +=
                    @"<tr>
                                <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">" +
                    i + @"
                                    
                                </td>
                                <td colspan=""4"" style=""font-family:tahoma;text-align:center;border-top:1px solid black;font-size:small;display:table-cell;border-left:1px solid black"">" +
                    item.SaleDetail + @"
                                </td>
                                <td colspan=""2"" style=""font-family:tahoma;text-align:center;border-top:1px solid black;font-size:small;display:table-cell;border-left:1px solid black"">" +
                    String.Format("{0:0,0}", item.UnitPrice) + @"
                                </td>
                                <td colspan=""2"" style=""font-family:tahoma;text-align:center;border-top:1px solid black;font-size:small;display:table-cell;border-left:1px solid black"">" +
                    item.Units + @"
                                </td>
                                <td colspan=""2"" style=""font-family:tahoma;text-align:center;border-top:1px solid black;font-size:small;display:table-cell;border-left:1px solid black"">" +
                    String.Format("{0:0,0}", item.LineTotalWithoutDiscountAndImposition) + @"
                                </td>
                            </tr>";
                i++;
            }
            body +=
                @"
                    </table>
                    <div style=""font-family:tahoma;text-align: right;padding: 2px;font-family:tahoma"">
                        <table style=""font-family:tahoma;font-family:tahoma;width: 100%;border: 1px solid black;border-bottom: 2px solid black;border-right: 2px solid black;margin-bottom: 5px; clear: left;float: left;width: 50%;"" cellpadding=""0"" cellspacing=""0"">
                            <tr>
                                <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">مبلغ کل</td>
                                <td style=""font-family:tahoma;text-align: center; width: 39%;;border: 1px solid black;font-size: small;display: table-cell;"">" +
                String.Format("{0:0,0}", response.data.SaleView.SaleTotalWithoutDiscountAndImposition) + @"</td>
                            </tr>
                            <tr>
                                <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">تخفیف(کسر می شود)</td>
                                <td style=""font-family:tahoma;text-align: center; width: 39%;;border: 1px solid black;font-size: small;display: table-cell;"">" +
                String.Format("{0:0,0}", response.data.SaleView.TotalDiscount) + @"</td>
                            </tr>
                            <tr>
                                <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">مالیات</td>
                                <td style=""font-family:tahoma;text-align: center; width: 39%;;border: 1px solid black;font-size: small;display: table-cell;"">" +
                String.Format("{0:0,0}", response.data.SaleView.TotalImposition) + @"</td>
                            </tr>
                            <tr>
                                <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">جمع</td>
                                <td style=""font-family:tahoma;text-align: center; width: 39%;;border: 1px solid black;font-size: small;display: table-cell;"">" +
                String.Format("{0:0,0}", response.data.SaleView.SaleTotal) +
                @"</td>
                            </tr>
                        </table>
                    </div>
                    <div style=""font-family:tahoma;rl"">            
                        <table style=""font-family:tahoma;width: 100%;border: 1px solid black;border-bottom: 2px solid black;border-right: 2px solid black;margin-bottom: 5px;     clear: left;float: left;width: 50%;"" cellpadding=""0"" cellspacing=""0"">
                            <tr>
                                <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">قابل پرداخت نهایی</td>
                                <td style=""font-family:tahoma;text-align: center; width: 39%;;border: 1px solid black;font-size: small;display: table-cell;"">" +
                String.Format("{0:0,0}", response.data.SaleView.Customer.Balance) + @"</td>
                            </tr>
                        </table>
                    </div>
                    <div style=""font-family:tahoma;text-align: right;padding: 2px; font-size: small;font-weight: bolder;text-align: justify;text-justify: inter-word; width:300px;"">
                        کاربر گرامی شما می توانید با معرفی هر مشترک جدید، معادل مبلغ 100.000 ريال حجم اضافه به صورت هدیه دریافت نمایید.
                    </div>
                    <br />
                    <div style=""font-family:tahoma;text-align: right;padding: 2px; font-size: small;font-weight: bolder;text-align: justify;text-justify: inter-word; font-size: large;font-weight: bolder;font-style: italic;"">
                        *لطفاً حتماً موارد ذیل را مطالعه بفرمایید:
                    </div>
                    <div style=""font-family:tahoma;text-align: right;padding: 2px; font-size: 12px;text-align: justify;text-justify: inter-word;"">
                        * خط مورد تقاضا برای ارائه سرویس اینترنت حتماً می بایست بدون بدهی قبلی باشد و برای اطمینان از آن می بایست مشترک از طریق شماره تلفن 1818 از صفر بودن بدهی خط  اطمینان حاصل نماید.<br />
                        * آماده سازی مدارک مورد نیاز جهت راه اندازی سرویس به عهده مشترک بوده و در صورت تعلل، زمان ارائه سرویس به تعویق خواهد افتاد. خواهشمند است 48 ساعت پس از ثبت نام جهت اطلاع از عدم نقص مدارک و ارسال آن به مخابرات با داخلی 2101 تماس حاصل فرمایید.<br />
                        * در صورتیکه مشترک سرویس قبلی اینترنت خود که توسط شرکت دیگری راه اندازی شده است را پس از ثبت نام در این شرکت جمع آوری نکرده باشد، این شرکت هیچ گونه مسئولیتی در خصوص تاخیر در راه اندازی سرویس ایشان نخواهد داشت.<br />
                        *  خط تلفن اعلام شده برای ADSL  نباید PCM ، فیبر و یا روتاری باشد و در صورت اتصال به سانترال می بایست از دستگاه سانترال خارج شده و به نزدیکی محل نصب سرویس انتقال یابد.<br />
                        *  سرویس ADSL  صرفا در درگاه خروجی مودم مشترک و روی یک دستگاه کامپیوتر تحویل گردیده و تعهدی برای تحویل سرویس در شبکه محلی وجود ندارد.<br />
                        * قبل از مراجعه کارشناس فنی جهت نصب ابتدا ورودی خط تلفن به ساختمان مشخص گردد. در صورت عدم توجه مشترک به نکات مذکور شرکت هیچ گونه مسئولیتی در قبال تاخیر در راه اندازی سرویس مشترک نخواهد داشت.<br />
                        * با توجه به محاسبه روز شمار آبونمان مخابرات، و محدودیت ارائه خدمات در صورت عدم شارژ مجدد سرویس اینترنت، خدمات ADSL  پس از سه روز جمع آوری و رزرو پورت امکان پذیر نخواهد بود.<br />
                        * مبنای محاسبه حجم مصرفی کاربر، مجموع حجم دریافتی (Receive) و ارسالی (Send) میباشد. و اعتبار اشتراک با اتمام حجم یا زمان (هر کدام زودتر اتفاق بیفتد) به پایان خواهد رسید.<br />
                    </div>
                    <table style=""font-family:tahoma;width: 100%;border: 1px solid black;border-bottom: 2px solid black;border-right: 2px solid black;margin-bottom: 5px;"" cellpadding=""0"" cellspacing=""0"">
                        <tr>
                            <td style=""font-family:tahoma;width: 35%;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">عنوان</td>
                            <td style=""font-family:tahoma;width: 10%;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">داخلی</td>
                            <td rowspan=""2"" colspan=""2"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">اینجانب با تایید موارد فوق و قبول شرایط اشتراک سرویس اینترنت پرسرعت +ADSL2،		
             خرید این سرویس با مشخصات مذکور را تایید می نمایم.		
                            </td>
                        </tr>
                        <tr>
                            <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">مشاور شما در ماهان نت</td>
                            <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">....</td>
                        </tr>
                        <tr>
                            <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">تکمیل مدارک، پیگیری دایری و رانژه مخابراتی</td>
                            <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">2</td>
                            <td style=""font-family:tahoma;width: 27%"" rowspan=""4"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">نماینده شرکت<br />" +
                response.data.EmployeeView.Name + @"</td>
                            <td style=""font-family:tahoma;width: 27%"" rowspan=""4"" style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">امضا مشترک</td>
                        </tr>
                        <tr>
                            <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">نصب، خدمات پس از فروش و مشاور فنی</td>
                            <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">4</td>
                        </tr>
                        <tr>
                            <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">تمدید اشتراک (شارژ مجدد سرویس)</td>
                            <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">3</td>
                        </tr>
                        <tr>
                            <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">شکایات و انتقادات</td>
                            <td style=""font-family:tahoma;text-align: center;border-left: 1px solid black;border-top: 1px solid black;font-size: small;display: table-cell;"">6</td>
                        </tr>
                    </table>
                </div>";

            #endregion

            IList<Guid> recipt = new List<Guid>();
            recipt.Add(response.data.CustomerView.ID);
            finalResponse = _customerService.SendEmail(recipt, body, "ماهان نت - فاکتور فروش", GetEmployee().ID);
            //return Json(response, JsonRequestBehavior.AllowGet);
            return Json(finalResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Ledger Account

        public object temp;

        public JsonResult GetLedgerAccountReport(Guid CustomerID, string ExportTo)
        {


            GetGeneralResponse<IEnumerable<LedgerAccountView>> response =
                new GetGeneralResponse<IEnumerable<LedgerAccountView>>();
            IList<LedgerAccountView> responseData = new List<LedgerAccountView>();

            //#region Access Check

            //bool hasPermission = GetEmployee().IsGuaranteed("Report_LedgerAccount");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}

            //#endregion

            AjaxGetRequest getRequest = new AjaxGetRequest();

            getRequest.PageSize = 1000000;
            getRequest.PageNumber = 1;

            // مالی ها
            IEnumerable<FiscalView> FiscalViews =
                _fiscalService.GetFiscalsOfCustomer(getRequest, CustomerID).FiscalViews;
            foreach (var fiscalView in FiscalViews)
            {
                // فقط تأیید شده ها
                if (fiscalView.ConfirmInt == 2)
                {
                    LedgerAccountView ledgerAccountView = new LedgerAccountView();
                    ledgerAccountView.CustomerID = fiscalView.CustomerID;
                    ledgerAccountView.Date = fiscalView.ConfirmDate;
                    ledgerAccountView.Description = fiscalView.Cost > 0 ? "Recive" : "Pay";
                    ledgerAccountView.SerialNumber = fiscalView.DocumentSerial;
                    ledgerAccountView.BedCost = fiscalView.Cost > 0 ? 0 : Math.Abs(fiscalView.ConfirmedCost);
                    ledgerAccountView.BesCost = fiscalView.Cost > 0 ? Math.Abs(fiscalView.ConfirmedCost) : 0;
                    ledgerAccountView.FiscalType = fiscalView.Cost > 0 ? "Recive" : "Pay";
                    ledgerAccountView.DeliverStatus = null;
                    ledgerAccountView.RollbackStatus = null;
                    ledgerAccountView.RecordType = 'F';

                    responseData.Add(ledgerAccountView);
                }
            }

            // فاکتورها
            FilterData _filter = new FilterData()
            {
                field = "CustomerName",
                data = new data()
                {
                    type = "string",
                    value = new[] { CustomerID.ToString() }
                }
            };

            IList<FilterData> filter = new List<FilterData>();
            filter.Add(_filter);
            IEnumerable<SaleView> SaleViews =
                _saleService.GetSales(
                    new AjaxGetRequest()
                    {
                        PageNumber = getRequest.PageNumber,
                        PageSize = getRequest.PageSize,
                        ID = CustomerID
                    }, null, filter, false).SaleViews;
            if (SaleViews != null)
            {
                foreach (var saleView in SaleViews)
                {
                    // فقط تأیید شده ها
                    if (saleView.Closed)
                    {
                        #region ProductSaleDetail Data

                        // کالاها
                        foreach (var productSaleDetail in saleView.ProductSaleDetails)
                        {

                            LedgerAccountView ledgerAccountView = new LedgerAccountView();
                            ledgerAccountView.CustomerID = saleView.CustomerID;
                            ledgerAccountView.Type = "P";
                            ledgerAccountView.SerialNumber = saleView.SaleNumber;
                            ledgerAccountView.ID = productSaleDetail.ID;
                            ledgerAccountView.SaleID = productSaleDetail.SaleID;
                            ledgerAccountView.FiscalType = null;

                            if (productSaleDetail.RollbackPrice > 0)
                            {
                                ledgerAccountView.CanDeliver = false;
                                ledgerAccountView.CanRollback = false;
                                ledgerAccountView.LineTotalWithoutDiscountAndImposition =
                                    productSaleDetail.RollbackPrice;
                                ledgerAccountView.Units = productSaleDetail.Units;
                                ledgerAccountView.Imposition = productSaleDetail.LineImposition;
                                ledgerAccountView.Discount = productSaleDetail.LineDiscount;

                                ledgerAccountView.Date = productSaleDetail.CreateDate;
                                // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                ledgerAccountView.BedCost = 0;
                                //بستانکار
                                ledgerAccountView.BesCost = productSaleDetail.LineTotal;
                                //ineTotalWithoutDiscountAndImposition + creditSaleDetail.LineImposition;
                                // وضعیت تحویل
                                ledgerAccountView.DeliverStatus = productSaleDetail.DeliverDate != null
                                    ? "Delivered"
                                    : "NotDelivered";
                                // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                ledgerAccountView.RollbackStatus = "Rollbacked";
                            }
                            //اگرفاکتور برگشت از فروش نبود
                            else
                            {
                                if (productSaleDetail.Delivered)
                                    ledgerAccountView.CanDeliver = false;
                                ledgerAccountView.CanRollback = true;
                                ledgerAccountView.LineTotalWithoutDiscountAndImposition =
                                    productSaleDetail.LineTotalWithoutDiscountAndImposition;
                                ledgerAccountView.Units = productSaleDetail.Units;
                                ledgerAccountView.Imposition = productSaleDetail.LineImposition;
                                ledgerAccountView.Discount = productSaleDetail.LineDiscount;


                                ledgerAccountView.Date = productSaleDetail.CreateDate;
                                // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                ledgerAccountView.BedCost = productSaleDetail.LineTotal;
                                //بستانکار
                                ledgerAccountView.BesCost = 0;
                                // وضعیت تحویل
                                ledgerAccountView.DeliverStatus = productSaleDetail.Delivered
                                    ? "Delivered"
                                    : "NotDelivered";
                                // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                ledgerAccountView.RollbackStatus = productSaleDetail.Rollbacked ? "Rollbacked" : null;
                            }

                            // شرح کالا
                            ledgerAccountView.Description = productSaleDetail.ProductPriceTitle;
                            // مبلغ بدهکار شامل قیمت کالاها به اضافه مالیات سطر

                            // وضعیت تحویل
                            ledgerAccountView.DeliverStatus = string.IsNullOrEmpty(productSaleDetail.DeliverDate)
                                ? "NotDelivered"
                                : "Delivered";
                            // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمیگردد
                            ledgerAccountView.RollbackStatus = productSaleDetail.Rollbacked ? "Rollbacked" : null;

                            responseData.Add(ledgerAccountView);
                        }

                        #endregion

                        #region CreditSaleDetail Data

                        // خدمات اعتباری
                        foreach (var creditSaleDetail in saleView.CreditSaleDetails)
                        {
                            LedgerAccountView ledgerAccountView = new LedgerAccountView();
                            ledgerAccountView.CustomerID = saleView.CustomerID;
                            ledgerAccountView.Type = "C";
                            ledgerAccountView.SerialNumber = saleView.SaleNumber;
                            ledgerAccountView.ID = creditSaleDetail.ID;
                            ledgerAccountView.SaleID = creditSaleDetail.SaleID;
                            ledgerAccountView.FiscalType = null;

                            // شرح خدمات اعتباری
                            ledgerAccountView.Description = creditSaleDetail.CreditServiceName;
                            //اگرفاکتور برگشت از فروش بود
                            if (creditSaleDetail.RollbackPrice > 0)
                            {
                                ledgerAccountView.CanDeliver = false;
                                ledgerAccountView.CanRollback = false;
                                ledgerAccountView.LineTotalWithoutDiscountAndImposition =
                                    creditSaleDetail.RollbackPrice;
                                ledgerAccountView.Units = creditSaleDetail.Units;
                                ledgerAccountView.Imposition = creditSaleDetail.LineImposition;
                                ledgerAccountView.Discount = creditSaleDetail.LineDiscount;

                                ledgerAccountView.Date = creditSaleDetail.CreateDate;
                                // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                ledgerAccountView.BedCost = 0;
                                //بستانکار
                                ledgerAccountView.BesCost = creditSaleDetail.LineTotal;
                                //ineTotalWithoutDiscountAndImposition + creditSaleDetail.LineImposition;
                                // وضعیت تحویل
                                ledgerAccountView.DeliverStatus = creditSaleDetail.DeliverDate != null
                                    ? "Delivered"
                                    : "NotDelivered";
                                // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                ledgerAccountView.RollbackStatus = "Rollbacked";
                            }
                            //اگرفاکتور برگشت از فروش نبود
                            else
                            {
                                if (creditSaleDetail.Delivered)
                                    ledgerAccountView.CanDeliver = false;
                                ledgerAccountView.CanRollback = true;

                                ledgerAccountView.LineTotalWithoutDiscountAndImposition =
                                    creditSaleDetail.LineTotalWithoutDiscountAndImposition;
                                ledgerAccountView.Units = creditSaleDetail.Units;
                                ledgerAccountView.Imposition = creditSaleDetail.LineImposition;
                                ledgerAccountView.Discount = creditSaleDetail.LineDiscount;

                                ledgerAccountView.Date = creditSaleDetail.CreateDate;
                                // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                ledgerAccountView.BedCost = creditSaleDetail.LineTotal;
                                //بستانکار
                                ledgerAccountView.BesCost = 0;
                                // وضعیت تحویل
                                ledgerAccountView.DeliverStatus = creditSaleDetail.Delivered
                                    ? "Delivered"
                                    : "NotDelivered";
                                // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                ledgerAccountView.RollbackStatus = creditSaleDetail.Rollbacked ? "Rollbacked" : null;
                            }


                            responseData.Add(ledgerAccountView);
                        }
                        temp = responseData;

                        #endregion

                        #region UncreditSaleDetail Data

                        // خدمات غیر اعتباری
                        foreach (var uncreditSaleDetail in saleView.UncreditSaleDetails)
                        {
                            LedgerAccountView ledgerAccountView = new LedgerAccountView();
                            ledgerAccountView.CustomerID = saleView.CustomerID;
                            ledgerAccountView.Type = "U";
                            ledgerAccountView.SaleID = uncreditSaleDetail.SaleID;
                            ledgerAccountView.ID = uncreditSaleDetail.ID;
                            ledgerAccountView.SerialNumber = saleView.SaleNumber;

                            if (uncreditSaleDetail.RollbackPrice > 0)
                            {
                                ledgerAccountView.CanDeliver = false;
                                ledgerAccountView.CanRollback = false;
                                ledgerAccountView.LineTotalWithoutDiscountAndImposition =
                                    uncreditSaleDetail.RollbackPrice;
                                ledgerAccountView.Units = uncreditSaleDetail.Units;
                                ledgerAccountView.Imposition = uncreditSaleDetail.LineImposition;
                                ledgerAccountView.Discount = uncreditSaleDetail.LineDiscount;

                                ledgerAccountView.Date = uncreditSaleDetail.CreateDate;
                                // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                ledgerAccountView.BedCost = 0;
                                //بستانکار
                                ledgerAccountView.BesCost = uncreditSaleDetail.LineTotal;
                                //ineTotalWithoutDiscountAndImposition + creditSaleDetail.LineImposition;
                                // وضعیت تحویل
                                ledgerAccountView.DeliverStatus = uncreditSaleDetail.DeliverDate != null
                                    ? "Delivered"
                                    : "NotDelivered";
                                // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                ledgerAccountView.RollbackStatus = "Rollbacked";
                            }
                            //اگرفاکتور برگشت از فروش نبود
                            else
                            {
                                if (uncreditSaleDetail.Delivered)
                                    ledgerAccountView.CanDeliver = false;
                                ledgerAccountView.CanRollback = true;

                                ledgerAccountView.LineTotalWithoutDiscountAndImposition =
                                    uncreditSaleDetail.LineTotalWithoutDiscountAndImposition;

                                ledgerAccountView.Units = uncreditSaleDetail.Units;
                                ledgerAccountView.Imposition = uncreditSaleDetail.LineImposition;
                                ledgerAccountView.Discount = uncreditSaleDetail.LineDiscount;
                                ledgerAccountView.LineTotalWithoutDiscountAndImposition =
                                    uncreditSaleDetail.LineTotalWithoutDiscountAndImposition;
                                ledgerAccountView.Date = uncreditSaleDetail.CreateDate;
                                // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                ledgerAccountView.BedCost = uncreditSaleDetail.LineTotal;
                                //بستانکار
                                ledgerAccountView.BesCost = 0;
                                // وضعیت تحویل
                                ledgerAccountView.DeliverStatus = uncreditSaleDetail.Delivered
                                    ? "Delivered"
                                    : "NotDelivered";
                                // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                ledgerAccountView.RollbackStatus = uncreditSaleDetail.Rollbacked ? "Rollbacked" : null;
                            }

                            ledgerAccountView.FiscalType = null;

                            // شرح خدمات غیراعتباری
                            ledgerAccountView.Description = uncreditSaleDetail.UncreditServiceName;

                            // وضعیت تحویل
                            ledgerAccountView.DeliverStatus = string.IsNullOrEmpty(uncreditSaleDetail.DeliverDate)
                                ? "NotDelivered"
                                : "Delivered";
                            // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمیگردد
                            ledgerAccountView.RollbackStatus = uncreditSaleDetail.Rollbacked ? "Rollbacked" : null;

                            responseData.Add(ledgerAccountView);
                        }

                        #endregion

                        #region Discount

                        // تخفیف فاکتور
                        //اگر مبلغ تخفیف صفر بود در معین مشتری نمایش داده نشود
                        //if (saleView.TotalDiscount > 0)
                        //{

                        //    LedgerAccountView ledgerAccountView2 = new LedgerAccountView();

                        //    ledgerAccountView2.Date = saleView.CloseDate;
                        //    ledgerAccountView2.Description = "Discount";
                        //    ledgerAccountView2.SerialNumber = saleView.SaleNumber;
                        //    if (saleView.IsRollbackSale)
                        //    {
                        //        ledgerAccountView2.BedCost = saleView.TotalDiscount;
                        //        // در تخفیف مشتری بستانکار می شود
                        //        ledgerAccountView2.BesCost = 0;
                        //    }
                        //    else
                        //    {
                        //        ledgerAccountView2.BedCost = 0;
                        //        // در تخفیف مشتری بستانکار می شود
                        //        ledgerAccountView2.BesCost = saleView.TotalDiscount;
                        //    }

                        //    ledgerAccountView2.FiscalType = null;
                        //    ledgerAccountView2.DeliverStatus = null;
                        //    ledgerAccountView2.RollbackStatus = null;
                        //    responseData.Add(ledgerAccountView2);
                        //}

                        //if (saleView.TotalImposition > 0)
                        //{

                        //    LedgerAccountView ledgerAccountView2 = new LedgerAccountView();

                        //    ledgerAccountView2.Date = saleView.CloseDate;
                        //    ledgerAccountView2.Description = "مالیات";
                        //    ledgerAccountView2.SerialNumber = saleView.SaleNumber;

                        //    if (saleView.IsRollbackSale)
                        //    {
                        //        ledgerAccountView2.BedCost = 0;
                        //        // گر برگشت از فروش بود مالیات مشتری بستانکا میشود
                        //        ledgerAccountView2.BesCost = saleView.TotalImposition;
                        //    }
                        //    else
                        //    {
                        //        ledgerAccountView2.BedCost = saleView.TotalImposition;
                        //        // در مالیات مشتری بدهکار می شود
                        //        ledgerAccountView2.BesCost = 0;
                        //    }
                        //    ledgerAccountView2.FiscalType = null;
                        //    ledgerAccountView2.DeliverStatus = null;
                        //    ledgerAccountView2.RollbackStatus = null;
                        //    responseData.Add(ledgerAccountView2);
                        //}

                        #endregion
                    }
                }
            }

            // مرتب کردن بر اساس تاریخ 
            IEnumerable<LedgerAccountView> sortedResponseData = responseData.OrderBy(o => o.Date);

            // پر کردن مقدار باقیمانده
            IList<LedgerAccountView> completeResponseData = new List<LedgerAccountView>();
            long remain = 0;
            foreach (var ledgerAccountView in sortedResponseData)
            {
                remain += ledgerAccountView.BesCost - ledgerAccountView.BedCost;
                ledgerAccountView.Remain = remain;
                completeResponseData.Add(ledgerAccountView);
            }



            response.data = completeResponseData;

            #region Export To Excel

            IList<GetLedgerAccount> ledgerAccount = new List<GetLedgerAccount>();
            foreach (var item in response.data)
            {
                ledgerAccount.Add(new GetLedgerAccount()
                {
                    AccountNUmber = item.SerialNumber,
                    BedCost = item.BedCost,
                    BesCost = item.BesCost,
                    Comment = item.Description,
                    Count = item.Units,
                    Date = item.Date,
                    Discount = item.Discount,
                    Imposition = item.Imposition,
                    Nakhalesh = item.LineTotalWithoutDiscountAndImposition,
                    Remain = item.Remain,
                    TransactionType = item.Type,
                    Rollback = item.RollbackStatus

                });
            }


            if (ExportTo == "Excel")
            {
                GridView gridView = new GridView();
                gridView.DataSource = ledgerAccount;

                gridView.DataBind();

                gridView.HeaderRow.Cells[0].Text = "تاریخ";
                gridView.HeaderRow.Cells[1].Text = "شرح";
                gridView.HeaderRow.Cells[2].Text = "مبلغ بدهکار";
                gridView.HeaderRow.Cells[3].Text = "مبلغ بستانکار";
                gridView.HeaderRow.Cells[4].Text = "مانده";
                gridView.HeaderRow.Cells[5].Text = "تخفیف";
                gridView.HeaderRow.Cells[6].Text = "مالیات";
                gridView.HeaderRow.Cells[7].Text = "تعداد";
                gridView.HeaderRow.Cells[8].Text = "مبلغ ناخالص";
                gridView.HeaderRow.Cells[9].Text = "نوع تراکنش";
                gridView.HeaderRow.Cells[10].Text = "شماره صورتحساب";
                gridView.HeaderRow.Cells[11].Text = "برگشت";

                gridView.Font.Names = new[] { "Tahoma" };

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Store_Report.xls");
                Response.ContentType = "application/ms-excel";

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmltextwriter = new HtmlTextWriter(stringWriter);

                gridView.RenderControl(htmltextwriter);

                Response.Output.Write(stringWriter.ToString());
                Response.Flush();
                Response.End();

            }

            #endregion


            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region New Cartex Report

        public JsonResult GetMainCartexReport(Guid ProductID, Guid? StoreID, string StartDate, string EndDate,
            string ExportTo, string sort)
        {
            GetGeneralResponse<IEnumerable<CartexView>> response = new GetGeneralResponse<IEnumerable<CartexView>>();
            IList<CartexView> ListedData = new List<CartexView>();
            IList<FilterData> Filters = new List<FilterData>();



            #region اگر گزارش از یک انبار فرعی بود

            if (StoreID != null)
            {



                FilterData ProductFilter = new FilterData()
                {
                    field = "ProductName",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "string",
                        value = new[] { ProductID.ToString() }
                    }
                };
                Filters.Add(ProductFilter);
                FilterData StoreFilter = new FilterData()
                {
                    field = "StoreName",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "string",
                        value = new[] { StoreID.ToString() }
                    }
                };
                Filters.Add(StoreFilter);

                Guid storeID = StoreID == null ? Guid.Empty : (Guid)StoreID;
                IEnumerable<ProductLogView> productLogs =
                    _productLogService.GetProductLogsByFilter(Filters).data;
                foreach (var productLogView in productLogs)
                {
                    CartexView cartex = new CartexView();
                    cartex.ADSLPhone = null;
                    if (productLogView.UnitsIO < 0)
                        cartex.Comment = "ورود به انبار فرعی";
                    else
                    {
                        cartex.Comment = "خروج از انبار فرعی";
                    }
                    cartex.DeliverEmployeeName = productLogView.CreateEmployeeName;
                    cartex.ProductName = productLogView.ParentProduct;
                    cartex.ProductPriceTitle = productLogView.ProductName;
                    cartex.StoreName = productLogView.Store.StoreName;
                    cartex.TransactionDate = productLogView.LogDate;
                    cartex.TransactionSerialNumber = productLogView.InputSerialNumber;
                    if (productLogView.UnitsIO < 0)
                        cartex.UnitsInput = productLogView.DisplayUnitsIO;
                    else
                    {
                        cartex.UnitsOutput = productLogView.DisplayUnitsIO;
                    }
                    ListedData.Add(cartex);
                }



                //IEnumerable<ProductSaleDetailView> productSaleDetailViews =
                //    _productSaleDetailService.GetProductSaleDetails_ByProductID(ProductID).data.Where(x=>x.DeliverStoreID==storeID).Where(x=>x.Delivered);

                IEnumerable<ProductSaleDetailView> productSaleDetailViews =
                    _productSaleDetailService.GetProductSaleDetails(Filters).data;

                foreach (var productSaleDetailView in productSaleDetailViews)
                {
                    if (productSaleDetailView.Delivered)
                    {
                        CartexView cartex = new CartexView();
                        CustomerView customer = _saleService.GetSale(productSaleDetailView.SaleID).data.Customer;
                        cartex.ADSLPhone = customer.ADSLPhone;
                        if (productSaleDetailView.Delivered)
                            cartex.Comment = "تحویل";
                        cartex.DeliverEmployeeName = productSaleDetailView.CreateEmployeeName;
                        cartex.ProductName = productSaleDetailView.ProductPriceTitle;
                        cartex.ProductPriceTitle = productSaleDetailView.ProductPriceTitle;
                        cartex.StoreName = productSaleDetailView.DeliverStoreName;
                        cartex.TransactionDate = productSaleDetailView.DeliverDate;
                        cartex.TransactionSerialNumber = null;
                        cartex.UnitsOutput = productSaleDetailView.Units;
                        ListedData.Add(cartex);
                    }
                }
            }
            #endregion

            #region اگر از نبار اصلی بود

            else
            {


                FilterData ProductFilter = new FilterData()
                {
                    field = "ProductName",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "string",
                        value = new[] { ProductID.ToString() }
                    }
                };
                Filters.Add(ProductFilter);

                IEnumerable<ProductLogView> productLogs =
                    _productLogService.GetProductLogsByFilter(Filters).data;
                foreach (var productLogView in productLogs)
                {
                    CartexView cartex = new CartexView();
                    cartex.ADSLPhone = null;
                    if (productLogView.UnitsIO < 0)
                        cartex.Comment = "خروج از انبار اصلی";
                    else
                    {
                        cartex.Comment = "ورود به انبار اصلی";
                    }
                    cartex.DeliverEmployeeName = productLogView.CreateEmployeeName;
                    cartex.ProductName = productLogView.ParentProduct;
                    cartex.ProductPriceTitle = productLogView.ProductName;
                    cartex.TransactionComment = productLogView.Note;
                    if (productLogView.Store != null)
                    {
                        cartex.StoreName = productLogView.Store.StoreName;
                        if (productLogView.UnitsIO < 0)
                            cartex.Comment = "خروج به انبار مجازی";
                        else
                        {
                            cartex.Comment = "ورود  از انبار مجازی";
                        }
                    }
                    cartex.TransactionDate = productLogView.LogDate;
                    cartex.TransactionSerialNumber = productLogView.InputSerialNumber;
                    if (productLogView.UnitsIO > 0)
                        cartex.UnitsInput = productLogView.DisplayUnitsIO;
                    else
                    {
                        cartex.UnitsOutput = productLogView.DisplayUnitsIO;
                    }
                    cartex.ID = productLogView.ID;
                    ListedData.Add(cartex);
                }


                FilterData DeliveredFilter = new FilterData()
                {
                    field = "IsRollbackDetail",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { "True" }
                    }
                };
                Filters.Add(DeliveredFilter);
                IEnumerable<ProductSaleDetailView> productSaleDetailViews =
                    _productSaleDetailService.GetProductSaleDetails(Filters).data;

                foreach (var productSaleDetailView in productSaleDetailViews)
                {
                    if (productSaleDetailView.Status == SaleDetailStatus.DeliveredAndRollbacked)
                    {
                        CartexView cartex = new CartexView();
                        CustomerView customer = _saleService.GetSale(productSaleDetailView.SaleID).data.Customer;
                        cartex.ADSLPhone = customer.ADSLPhone;
                        cartex.Comment = "برگشت";
                        cartex.DeliverEmployeeName = productSaleDetailView.CreateEmployeeName;
                        cartex.ProductName = "";
                        cartex.ProductPriceTitle = productSaleDetailView.ProductPriceTitle;
                        cartex.StoreName = "انبار اصلی";
                        cartex.TransactionDate = productSaleDetailView.CreateDate;
                        cartex.TransactionSerialNumber = null;
                        cartex.UnitsInput = productSaleDetailView.Units;
                        ListedData.Add(cartex);
                    }
                }
            }

            #endregion

            #region باقیمانده

            int remain = 0;

            var sortedList = ListedData.OrderBy(x => x.TransactionDate);

            foreach (var item in sortedList)
            {
                if (item.UnitsInput > 0)
                {
                    remain += item.UnitsInput;
                    item.Remain = remain;
                }
                if (item.UnitsOutput > 0)
                {
                    remain -= item.UnitsOutput;
                    item.Remain = remain;
                }
            }

            #endregion

            #region فیلتر کردن بر اساس تاریخ

            IList<CartexView> FilteredData = new List<CartexView>();

            if (StartDate != string.Empty && EndDate == string.Empty)
            {
                CartexView last =
                    sortedList.Where(x => x.TransactionDate.CompareTo(StartDate) < 0).ToList().LastOrDefault();
                CartexView remainCartex = new CartexView();
                if (last != null)
                {

                    remainCartex.TransactionDate = last.TransactionDate;
                    remainCartex.Comment = "انتقال از قبل";
                    remainCartex.Remain = last.Remain;
                    remainCartex.TransactionDate = "-";
                }

                FilteredData = sortedList.Where(x => x.TransactionDate.CompareTo(StartDate) >= 0).ToList();
                if (remainCartex != null)
                    FilteredData.Add(remainCartex);
            }

            if (StartDate == string.Empty && EndDate != string.Empty)
            {


                FilteredData = sortedList.Where(x => x.TransactionDate.CompareTo(EndDate) <= 0).ToList();
            }

            if (StartDate != string.Empty && EndDate != string.Empty)
            {
                CartexView last =
                    sortedList.Where(x => x.TransactionDate.CompareTo(StartDate) < 0).ToList().LastOrDefault();
                CartexView remainCartex = new CartexView();
                if (last != null)
                {

                    remainCartex.TransactionDate = last.TransactionDate;
                    remainCartex.Comment = "انتقال از قبل";
                    remainCartex.Remain = last.Remain;
                    remainCartex.TransactionDate = "-";
                }

                FilteredData =
                    sortedList.Where(x => x.TransactionDate.CompareTo(EndDate) <= 0)
                        .Where(x => x.TransactionDate.CompareTo(StartDate) >= 0)
                        .ToList()
                        .ToList();
                if (remainCartex != null)
                    FilteredData.Add(remainCartex);
            }
            if (StartDate == string.Empty && EndDate == string.Empty)
            {
                FilteredData = sortedList.ToList();
            }

            #endregion

            response.data = FilteredData.OrderBy(x => x.TransactionDate);

            #region preparing Sort

            IList<Sort> sorting = ConvertJsonToObject(sort);

            foreach (var _sort in sorting)
            {
                switch (_sort.SortColumn)
                {
                    case "ProductName":
                        {
                            if (!_sort.Asc)
                                response.data = response.data.OrderBy(x => x.ProductName);
                            else
                            {
                                response.data = response.data.OrderByDescending(x => x.ProductName);
                            }
                            break;
                        }

                    case "ProductPriceTitle":
                        {
                            if (!_sort.Asc)
                                response.data = response.data.OrderBy(x => x.ProductPriceTitle);
                            else
                            {
                                response.data = response.data.OrderByDescending(x => x.ProductPriceTitle);
                            }
                            break;
                        }

                    case "ADSLPhone":
                        {
                            if (!_sort.Asc)
                                response.data = response.data.OrderBy(x => x.ADSLPhone);
                            else
                            {
                                response.data = response.data.OrderByDescending(x => x.ADSLPhone);
                            }
                            break;
                        }
                    case "TransactionSerialNumber":
                        {
                            if (!_sort.Asc)
                                response.data = response.data.OrderBy(x => x.TransactionSerialNumber);
                            else
                            {
                                response.data = response.data.OrderByDescending(x => x.TransactionSerialNumber);
                            }
                            break;
                        }
                    case "Comment":
                        {
                            if (!_sort.Asc)
                                response.data = response.data.OrderBy(x => x.Comment);
                            else
                            {
                                response.data = response.data.OrderByDescending(x => x.Comment);
                            }
                            break;
                        }
                    case "DeliverEmployeeName":
                        {
                            if (!_sort.Asc)
                                response.data = response.data.OrderBy(x => x.DeliverEmployeeName);
                            else
                            {
                                response.data = response.data.OrderByDescending(x => x.DeliverEmployeeName);
                            }
                            break;
                        }
                    case "TransactionDate":
                        {
                            if (!_sort.Asc)
                                response.data = response.data.OrderBy(x => x.TransactionDate);
                            else
                            {
                                response.data = response.data.OrderByDescending(x => x.TransactionDate);
                            }
                            break;
                        }

                }
            }

            #endregion

            #region Export To Excel



            if (ExportTo == "Excel")
            {
                GridView gridView = new GridView();
                gridView.DataSource = response.data;

                gridView.DataBind();

                gridView.HeaderRow.Cells[0].Text = "تلفن مشتری";
                gridView.HeaderRow.Cells[1].Text = "نام کالا";
                gridView.HeaderRow.Cells[2].Text = "توضیحات";
                gridView.HeaderRow.Cells[3].Text = "نام محصول";
                gridView.HeaderRow.Cells[4].Text = "شماره تراکنش";
                gridView.HeaderRow.Cells[5].Text = "تعداد ورود";
                gridView.HeaderRow.Cells[6].Text = "تعداد خروج";
                gridView.HeaderRow.Cells[7].Text = "باقیمانده";
                gridView.HeaderRow.Cells[8].Text = "کارمند تحویل";
                gridView.HeaderRow.Cells[9].Text = "تاریخ تراکنش";
                gridView.HeaderRow.Cells[10].Text = "نام انبار";

                gridView.Font.Names = new[] { "Tahoma" };

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Store_Report.xls");
                Response.ContentType = "application/ms-excel";

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmltextwriter = new HtmlTextWriter(stringWriter);

                gridView.RenderControl(htmltextwriter);

                Response.Output.Write(stringWriter.ToString());
                Response.Flush();
                Response.End();

            }

            #endregion

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Bank and cashe report

        public ActionResult GetBankCasheReport(int? TransactionType, IEnumerable<Guid> MoneyAccountID,
            string InvestStartDate, string InvestEndDate, string ConfirmStartDate, string ConfirmEndDate, string sort, bool? NotConfirmed,
            string ExportTo)
        {
            GetGeneralResponse<IEnumerable<GetBankCasheReportView>> response =
                new GetGeneralResponse<IEnumerable<GetBankCasheReportView>>();

            //bool hasPermission = GetEmployee().IsGuaranteed("FiscalReport_Read");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}

            //response = _fiscalService.GetBankCasheReport(TransactionType, MoneyAccountID, InvestStartDate, InvestEndDate, ConfirmStartDate, ConfirmEndDate
            //    , ConvertJsonToObject(sort));
            bool notConfirmed = NotConfirmed == null ? false : (bool)NotConfirmed;
            response = _fiscalService.GetBankCasheReport(TransactionType, MoneyAccountID, InvestStartDate, InvestEndDate,
                ConfirmStartDate, ConfirmEndDate
                , ConvertJsonToObject(sort), notConfirmed);

            #region Preparing Excel

            if (ExportTo == "Excel")
            {
                GridView gridView = new GridView();
                gridView.DataSource = response.data;

                gridView.DataBind();

                gridView.HeaderRow.Cells[0].Text = "نام مشتری";
                gridView.HeaderRow.Cells[1].Text = "تلفن";
                gridView.HeaderRow.Cells[2].Text = "ثبت کننده";
                gridView.HeaderRow.Cells[3].Text = "تایید کننده";
                gridView.HeaderRow.Cells[4].Text = "تاریخ واریز";
                gridView.HeaderRow.Cells[5].Text = "تاریخ تایید";
                gridView.HeaderRow.Cells[6].Text = "مبلغ واریز شده";
                gridView.HeaderRow.Cells[7].Text = "مبلغ تایید شده";
                gridView.HeaderRow.Cells[8].Text = "شماره سریال";
                gridView.HeaderRow.Cells[9].Text = "بدهکار/بستانکار";
                gridView.HeaderRow.Cells[10].Text = "حساب پولی";
                gridView.HeaderRow.Cells[11].Text = "حساب پولی";

                gridView.HeaderRow.Cells[12].Text = "نوع پرداخت";
                gridView.HeaderRow.Cells[13].Text = "توضیحات";
                gridView.HeaderRow.Cells[14].Text = "شماره سند";
                gridView.HeaderRow.Cells[15].Text = "شماره رسید";
                gridView.Font.Names = new[] { "Tahoma" };
                gridView.Font.Size = 10;

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=FiscalReport.xls");
                Response.ContentType = "application/ms-excel";

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmltextwriter = new HtmlTextWriter(stringWriter);

                gridView.RenderControl(htmltextwriter);

                Response.Output.Write(stringWriter.ToString());
                Response.Flush();
                Response.End();

            }

            #endregion

            #region Preparing PDF

            if (ExportTo == "PDF")
            {


            }

            #endregion

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Sale Report

        public ActionResult Sale_Report(
            IEnumerable<Guid> SaleEmployeeID, IEnumerable<Guid> DeliverEmployeeID, IEnumerable<Guid> RollBackEmployeeID,
            IEnumerable<Guid> Products, IEnumerable<Guid> UncreditServices,
            IEnumerable<Guid> CreditService, string SaleStartDate, string SaleEndDate, string RollBackStartDate,
            string RollBackEndDate, IEnumerable<Guid> Networks,
            IEnumerable<Guid> ProductPrices, string DeliverStartDate, string DeliverEndDate,
            bool? Delivered, bool? Confirmed, bool? RollBacked, string ExportTo, bool? IsCreditService, bool? IsProducts,
            bool? IsUnCreditService, IList<FilterData> filter
            , string sort, int? CourierStatis, string CourierStartDate, string CourierEndDate, int HasCourier = 1
            )
        {
            GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> response =
                new GetGeneralResponse<IEnumerable<GetSaleDetailReportView>>();

            #region Access Check

            //bool hasPermission = GetEmployee().IsGuaranteed("getsuc");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}

            #endregion

            #region Decalres

            IList<GetSaleDetailReportView> Report = new List<GetSaleDetailReportView>();

            GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> uncreditSaleDetailViews =
                new GetGeneralResponse<IEnumerable<GetSaleDetailReportView>>();
            GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> creditSaleDetailViews =
                new GetGeneralResponse<IEnumerable<GetSaleDetailReportView>>();
            GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> productSaleDetailViews =
                new GetGeneralResponse<IEnumerable<GetSaleDetailReportView>>();

            #endregion


            IList<FilterData> Filters = new List<FilterData>();

            if (Confirmed == false)
            {
                Filters.Add(new FilterData()
                {
                    field = "Confirmed",
                    data = new data()
                    {
                        type = "boolean",
                        value = new[] { bool.FalseString }
                    }
                });
            }

            if (Confirmed == true)
            {
                Filters.Add(new FilterData()
                {
                    field = "Confirmed",
                    data = new data()
                    {
                        type = "boolean",
                        value = new[] { bool.TrueString }
                    }
                });
            }

            if (filter != null)
            {
                foreach (var _filter in filter)
                {
                    Filters.Add(_filter);
                }
            }
            //اگر برگشت از فروش بود در برگشت ها جستجو کن
            if (RollBacked == true)
            {

                #region prepairing Filters

                Filters.Add(new FilterData()
                {
                    field = "IsRollbackDetail",
                    data = new data()
                    {
                        type = "boolean",
                        value = new[] { bool.TrueString }
                    }
                });

                if (SaleEmployeeID != null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in SaleEmployeeID)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "SaleEmployee.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    Filters.Add(Filter);
                }

                if (DeliverEmployeeID != null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in DeliverEmployeeID)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "MainSaleDetail.DeliverEmployee.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    Filters.Add(Filter);
                }

                if (RollBackEmployeeID != null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in RollBackEmployeeID)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "CreateEmployee.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    Filters.Add(Filter);
                }


                if (Delivered == true)
                {
                    FilterData Filter = new FilterData();
                    Filter.field = "MainSaleDetail.Delivered";
                    Filter.data = new data()
                    {
                        type = "boolean",
                        value = new[] { bool.TrueString }
                    };
                    Filters.Add(Filter);
                }

                if (Delivered == false)
                {
                    FilterData Filter = new FilterData();
                    Filter.field = "MainSaleDetail.Delivered";
                    Filter.data = new data()
                    {
                        type = "boolean",
                        value = new[] { bool.FalseString }
                    };
                    Filters.Add(Filter);
                }

                #endregion

                #region Sale Date

                if (SaleStartDate != null && SaleEndDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "MainSaleDetail.Sale.CloseDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { SaleStartDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (SaleEndDate != null && SaleStartDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "MainSaleDetail.Sale.CloseDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { SaleEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (SaleStartDate != null && SaleEndDate != null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "MainSaleDetail.Sale.CloseDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { SaleStartDate, SaleEndDate }
                        }

                    };
                    Filters.Add(Filter);
                }

                #endregion

                #region Deliver Date

                if (DeliverStartDate != null && DeliverEndDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "MainSaleDetail.DeliverDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { DeliverStartDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (DeliverEndDate != null && DeliverStartDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "MainSaleDetail.DeliverDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { DeliverEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (DeliverStartDate != null && DeliverEndDate != null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "MainSaleDetail.DeliverDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { DeliverStartDate, DeliverEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                #endregion

                #region RollBack Date

                if (RollBackStartDate != null && RollBackEndDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "CreateDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { RollBackStartDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (RollBackEndDate != null && RollBackStartDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "CreateDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { SaleEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (RollBackStartDate != null && RollBackEndDate != null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "CreateDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { RollBackStartDate, RollBackEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                #endregion

                #region Courier Date

                if (HasCourier != null)
                {

                    if (HasCourier == 3)
                    {
                        FilterData Filter1 = new FilterData()
                        {
                            field = "Sale.HasCourier",
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.FalseString }
                            }
                        };
                    }

                    if (HasCourier == 2)
                    {
                        FilterData Filter12 = new FilterData()
                        {
                            field = "Sale.HasCourier",
                            data = new data()
                            {
                                comparison = "eq",
                                type = "numeric",
                                value = new[] { bool.TrueString }
                            }
                        };
                        Filters.Add(Filter12);



                        if (CourierStartDate != null && CourierEndDate == null)
                        {
                            FilterData Filter = new FilterData()
                            {
                                field = "BonusDate",
                                data = new data()
                                {
                                    comparison = "gteq",
                                    type = "date",
                                    value = new[] { CourierStartDate }
                                }
                            };
                            Filters.Add(Filter);
                        }

                        if (CourierEndDate != null && CourierStartDate == null)
                        {
                            FilterData Filter = new FilterData()
                            {
                                field = "BonusDate",
                                data = new data()
                                {
                                    comparison = "lteq",
                                    type = "date",
                                    value = new[] { CourierEndDate }
                                }
                            };
                            Filters.Add(Filter);
                        }

                        if (CourierStartDate != null && CourierEndDate != null)
                        {
                            FilterData Filter = new FilterData()
                            {
                                field = "BonusDate",
                                data = new data()
                                {
                                    comparison = "lteq",
                                    type = "dateBetween",
                                    value = new[] { CourierStartDate, CourierEndDate }
                                }
                            };
                            Filters.Add(Filter);
                        }
                    }


                }
                #endregion
            }


            if (RollBacked == false || RollBacked == null)
            {
                if (RollBacked == null)
                {

                    Filters.Add(new FilterData()
                    {
                        field = "IsRollbackDetail",
                        data = new data()
                        {
                            type = "boolean",
                            value = new[] { bool.FalseString }
                        }
                    });
                }
                if (RollBacked == false)
                {
                    Filters.Add(new FilterData()
                    {
                        field = "Rollbacked",
                        data = new data()
                        {
                            type = "boolean",
                            value = new[] { bool.FalseString }
                        }
                    });
                    Filters.Add(new FilterData()
                    {
                        field = "IsRollbackDetail",
                        data = new data()
                        {
                            type = "boolean",
                            value = new[] { bool.FalseString }
                        }
                    });
                }

                #region prepairing Filters

                if (SaleEmployeeID != null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in SaleEmployeeID)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "SaleEmployee.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    Filters.Add(Filter);
                }

                if (DeliverEmployeeID != null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in DeliverEmployeeID)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "DeliverEmployee.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    Filters.Add(Filter);
                }


                if (Delivered == true)
                {
                    FilterData Filter = new FilterData();
                    Filter.field = "Delivered";
                    Filter.data = new data()
                    {
                        type = "boolean",
                        value = new[] { bool.TrueString }
                    };
                    Filters.Add(Filter);
                }

                if (Delivered == false)
                {
                    FilterData Filter = new FilterData();
                    Filter.field = "Delivered";
                    Filter.data = new data()
                    {
                        type = "boolean",
                        value = new[] { bool.FalseString }
                    };
                    Filters.Add(Filter);
                }

                #endregion

                #region Sale Date

                if (SaleStartDate != null && SaleEndDate == null)
                {

                    FilterData Filter = new FilterData()
                    {
                        field = "Sale.CloseDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { SaleStartDate }
                        }
                    };
                    Filters.Add(Filter);


                }

                if (SaleEndDate != null && SaleStartDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "Sale.CloseDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { SaleEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (SaleStartDate != null && SaleEndDate != null)
                {

                    FilterData Filter = new FilterData()
                    {
                        field = "Sale.CloseDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { SaleStartDate, SaleEndDate }
                        }

                    };
                    Filters.Add(Filter);
                }


                #endregion

                #region Deliver Date

                if (DeliverStartDate != null && DeliverEndDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "DeliverDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { DeliverStartDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (DeliverEndDate != null && DeliverStartDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "DeliverDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { DeliverEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (DeliverStartDate != null && DeliverEndDate != null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "DeliverDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { DeliverStartDate, DeliverEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                #endregion

                #region Courier Date

                if (HasCourier != null)
                {

                    if (HasCourier == 3)
                    {
                        FilterData Filter1 = new FilterData()
                        {
                            field = "Sale.HasCourier",
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.FalseString }
                            }
                        };
                        Filters.Add(Filter1);
                    }

                    if (HasCourier == 2)
                    {
                        FilterData Filter12 = new FilterData()
                        {
                            field = "Sale.HasCourier",
                            data = new data()
                            {
                                comparison = "eq",
                                type = "numeric",
                                value = new[] { bool.TrueString }
                            }
                        };
                        Filters.Add(Filter12);



                        if (CourierStartDate != null && CourierEndDate == null)
                        {
                            FilterData Filter = new FilterData()
                            {
                                field = "BonusDate",
                                data = new data()
                                {
                                    comparison = "gteq",
                                    type = "date",
                                    value = new[] { CourierStartDate }
                                }
                            };
                            Filters.Add(Filter);
                        }

                        if (CourierEndDate != null && CourierStartDate == null)
                        {
                            FilterData Filter = new FilterData()
                            {
                                field = "BonusDate",
                                data = new data()
                                {
                                    comparison = "lteq",
                                    type = "date",
                                    value = new[] { CourierEndDate }
                                }
                            };
                            Filters.Add(Filter);
                        }

                        if (CourierStartDate != null && CourierEndDate != null)
                        {
                            FilterData Filter = new FilterData()
                            {
                                field = "BonusDate",
                                data = new data()
                                {
                                    comparison = "lteq",
                                    type = "dateBetween",
                                    value = new[] { CourierStartDate, CourierEndDate }
                                }
                            };
                            Filters.Add(Filter);
                        }
                    }


                }
                #endregion

            }



            #region Getting Data

            if (IsUnCreditService == true)
            {
                IList<FilterData> finalFilter1 = new List<FilterData>();
                finalFilter1 = Filters;
                finalFilter1.Remove(finalFilter1.Where(x => x.field == "CreditService.ID").FirstOrDefault());
                finalFilter1.Remove(finalFilter1.Where(x => x.field == "CreditService.Network.ID").FirstOrDefault());
                finalFilter1.Remove(finalFilter1.Where(x => x.field == "ProductPrice.ID").FirstOrDefault());
                finalFilter1.Remove(finalFilter1.Where(x => x.field == "ProductPrice.Product.ID").FirstOrDefault());

                #region Spacial Filters

                if (UncreditServices != null)
                {

                    IList<string> Ids = new List<string>();
                    foreach (var item in UncreditServices)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "UncreditService.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    finalFilter1.Add(Filter);
                }



                #endregion

                uncreditSaleDetailViews = _uncreditSaleDetailService.GetSaleReport(finalFilter1);
            }
            if (IsCreditService == true)
            {

                IList<FilterData> finalFilter2 = new List<FilterData>();
                finalFilter2 = Filters;
                finalFilter2.Remove(finalFilter2.Where(x => x.field == "UncreditService.ID").FirstOrDefault());
                finalFilter2.Remove(finalFilter2.Where(x => x.field == "ProductPrice.ID").FirstOrDefault());
                finalFilter2.Remove(finalFilter2.Where(x => x.field == "ProductPrice.Product.ID").FirstOrDefault());

                #region Spacial Filters

                if (CreditService != null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in CreditService)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "CreditService.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    finalFilter2.Add(Filter);
                }

                if (Networks != null && CreditService == null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in Networks)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "CreditService.Network.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()

                    };
                    finalFilter2.Add(Filter);
                }

                #endregion

                creditSaleDetailViews = _creditSaleDetailService.GetSaleReport(finalFilter2);
            }

            if (IsProducts == true)
            {
                IList<FilterData> finalFilter3 = new List<FilterData>();
                finalFilter3 = Filters;
                finalFilter3.Remove(finalFilter3.Where(x => x.field == "UncreditService.ID").FirstOrDefault());
                finalFilter3.Remove(finalFilter3.Where(x => x.field == "CreditService.ID").FirstOrDefault());
                finalFilter3.Remove(finalFilter3.Where(x => x.field == "CreditService.Network.ID").FirstOrDefault());

                #region Spacial Filters

                if (ProductPrices != null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in ProductPrices)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "ProductPrice.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    finalFilter3.Add(Filter);
                }


                if (Products != null && ProductPrices == null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in Products)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "ProductPrice.Product.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    finalFilter3.Add(Filter);
                }

                #endregion

                productSaleDetailViews = _productSaleDetailService.GetSaleReport(finalFilter3);
            }
            if (IsProducts == null && IsCreditService == null && IsUnCreditService == null)
            {
                productSaleDetailViews = _productSaleDetailService.GetSaleReport(Filters);
                creditSaleDetailViews = _creditSaleDetailService.GetSaleReport(Filters);
                uncreditSaleDetailViews = _uncreditSaleDetailService.GetSaleReport(Filters);
            }


            #endregion

            #region preparing Out put Report

            if (uncreditSaleDetailViews.data != null)
                foreach (GetSaleDetailReportView item in uncreditSaleDetailViews.data)
                    Report.Add(item);

            if (creditSaleDetailViews.data != null)
                foreach (GetSaleDetailReportView item in creditSaleDetailViews.data)
                    Report.Add(item);

            if (productSaleDetailViews.data != null)
                foreach (GetSaleDetailReportView item in productSaleDetailViews.data)
                    Report.Add(item);

            #endregion


            if (Report.Count > 0)
                response.data = Report.OrderBy(x => x.SaleDate);
            else
                response.data = Report.OrderBy(x => x.SaleDate);

            response.totalCount = Report.Count();

            #region Preparing Excel

            if (ExportTo == "Excel")
            {
                GridView gridView = new GridView();
                gridView.DataSource = response.data;

                gridView.DataBind();
                gridView.HeaderRow.Cells[0].Text = "شناسه مشتری";
                gridView.HeaderRow.Cells[1].Text = "نام مشتری";
                gridView.HeaderRow.Cells[2].Text = "تلفن";
                gridView.HeaderRow.Cells[3].Text = "کارشناس فروش";
                gridView.HeaderRow.Cells[4].Text = "نام مرکز";
                gridView.HeaderRow.Cells[5].Text = "تاریخ فروش";
                gridView.HeaderRow.Cells[6].Text = "نام محصول";
                gridView.HeaderRow.Cells[7].Text = "کالا";
                gridView.HeaderRow.Cells[8].Text = "شبکه";
                gridView.HeaderRow.Cells[9].Text = "خدمات اعتباری";
                gridView.HeaderRow.Cells[10].Text = "خدمات غیر اعتباری";
                gridView.HeaderRow.Cells[11].Text = "قیمت";
                gridView.HeaderRow.Cells[12].Text = "تعداد";
                gridView.HeaderRow.Cells[13].Text = "تخفیف";
                gridView.HeaderRow.Cells[14].Text = "مالیات";
                gridView.HeaderRow.Cells[15].Text = "مجموع";
                gridView.HeaderRow.Cells[16].Text = "کارمند تحویل";
                gridView.HeaderRow.Cells[17].Text = "تاریخ تحویل";
                gridView.HeaderRow.Cells[18].Text = "کارمند برگشت";
                gridView.HeaderRow.Cells[19].Text = "تاریخ برگشت";
                gridView.HeaderRow.Cells[20].Text = "تعداد برگشت";
                gridView.HeaderRow.Cells[21].Text = "مبلغ برگشتی";
                gridView.HeaderRow.Cells[22].Text = "نوع فروش";
                gridView.HeaderRow.Cells[23].Text = "تاریخ امتیاز";
                gridView.HeaderRow.Cells[24].Text = "تاریخ پورسانت";
                gridView.Font.Names = new[] { "Tahoma" };

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
                Response.ContentType = "application/ms-excel";

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmltextwriter = new HtmlTextWriter(stringWriter);

                gridView.RenderControl(htmltextwriter);

                Response.Output.Write(stringWriter.ToString());
                Response.Flush();
                Response.End();

            }

            #endregion



            return Json(response, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region New Bonus & Comission Report

        public ActionResult BonusComission_Report(
            IEnumerable<Guid> SaleEmployeeID, IEnumerable<Guid> DeliverEmployeeID, IEnumerable<Guid> RollBackEmployeeID,
            IEnumerable<Guid> Products, IEnumerable<Guid> UncreditServices,
            IEnumerable<Guid> CreditService, string SaleStartDate, string SaleEndDate, string RollBackStartDate,
            string RollBackEndDate, IEnumerable<Guid> Networks,
            IEnumerable<Guid> ProductPrices, string DeliverStartDate, string DeliverEndDate,
            bool? Delivered, bool? Confirmed, bool? RollBacked, string ExportTo, bool? IsCreditService, bool? IsProducts,
            bool? IsUnCreditService, string CourierConfirmStartDate, string CourierConfirmEndDate,
            IList<FilterData> filter
            , string sort
            )
        {
            GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> response =
                new GetGeneralResponse<IEnumerable<GetSaleDetailReportView>>();

            #region Access Check

            //bool hasPermission = GetEmployee().IsGuaranteed("getsuc");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}

            #endregion

            #region Decalres

            IList<GetSaleDetailReportView> Report = new List<GetSaleDetailReportView>();

            GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> uncreditSaleDetailViews =
                new GetGeneralResponse<IEnumerable<GetSaleDetailReportView>>();
            GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> creditSaleDetailViews =
                new GetGeneralResponse<IEnumerable<GetSaleDetailReportView>>();
            GetGeneralResponse<IEnumerable<GetSaleDetailReportView>> productSaleDetailViews =
                new GetGeneralResponse<IEnumerable<GetSaleDetailReportView>>();

            #endregion


            IList<FilterData> Filters = new List<FilterData>();

            if (Confirmed == false)
            {
                Filters.Add(new FilterData()
                {
                    field = "Confirmed",
                    data = new data()
                    {
                        type = "boolean",
                        value = new[] { bool.FalseString }
                    }
                });
            }

            if (Confirmed == true)
            {
                Filters.Add(new FilterData()
                {
                    field = "Confirmed",
                    data = new data()
                    {
                        type = "boolean",
                        value = new[] { bool.TrueString }
                    }
                });
            }

            if (filter != null)
            {
                foreach (var _filter in filter)
                {
                    Filters.Add(_filter);
                }
            }
            //اگر برگشت از فروش بود در برگشت ها جستجو کن
            if (RollBacked == true)
            {

                #region prepairing Filters

                Filters.Add(new FilterData()
                {
                    field = "IsRollbackDetail",
                    data = new data()
                    {
                        type = "boolean",
                        value = new[] { bool.TrueString }
                    }
                });

                if (SaleEmployeeID != null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in SaleEmployeeID)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "MainSaleDetail.CreateEmployee.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    Filters.Add(Filter);
                }

                if (DeliverEmployeeID != null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in DeliverEmployeeID)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "MainSaleDetail.DeliverEmployee.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    Filters.Add(Filter);
                }

                if (RollBackEmployeeID != null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in RollBackEmployeeID)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "CreateEmployee.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    Filters.Add(Filter);
                }


                if (Delivered == true)
                {
                    FilterData Filter = new FilterData();
                    Filter.field = "MainSaleDetail.Delivered";
                    Filter.data = new data()
                    {
                        type = "boolean",
                        value = new[] { bool.TrueString }
                    };
                    Filters.Add(Filter);
                }

                if (Delivered == false)
                {
                    FilterData Filter = new FilterData();
                    Filter.field = "MainSaleDetail.Delivered";
                    Filter.data = new data()
                    {
                        type = "boolean",
                        value = new[] { bool.FalseString }
                    };
                    Filters.Add(Filter);
                }

                #endregion

                #region Sale Date

                if (SaleStartDate != null && SaleEndDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "MainSaleDetail.CreateDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { SaleStartDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (SaleEndDate != null && SaleStartDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "MainSaleDetail.CreateDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { SaleEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (SaleStartDate != null && SaleEndDate != null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "MainSaleDetail.CreateDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { SaleStartDate, SaleEndDate }
                        }

                    };
                    Filters.Add(Filter);
                }

                #endregion

                #region Deliver Date

                if (DeliverStartDate != null && DeliverEndDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "MainSaleDetail.DeliverDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { DeliverStartDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (DeliverEndDate != null && DeliverStartDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "MainSaleDetail.DeliverDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { DeliverEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (DeliverStartDate != null && DeliverEndDate != null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "MainSaleDetail.DeliverDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { DeliverStartDate, DeliverEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                #endregion

                #region RollBack Date

                if (RollBackStartDate != null && RollBackEndDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "CreateDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { RollBackStartDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (RollBackEndDate != null && RollBackStartDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "CreateDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { SaleEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (RollBackStartDate != null && RollBackEndDate != null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "CreateDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { RollBackStartDate, RollBackEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                #endregion

                #region Courier Date

                //if (HasCourier)
                //{
                //    FilterData Filter1 = new FilterData()
                //    {
                //        field = "BonusDate",
                //        data = new data()
                //        {
                //            comparison = "Noteq",
                //            type = "numeric",
                //            value = new[] { "null" }
                //        }
                //    };
                //    if (CourierStatus != null)
                //    {
                //        FilterData Filter12 = new FilterData()
                //        {
                //            field = "Sale.Courier.CourierStatuse",
                //            data = new data()
                //            {
                //                comparison = "Noteq",
                //                type = "numeric",
                //                value = new[] { "null" }
                //            }
                //        };
                //        Filters.Add(Filter12);
                //    }


                //    if (CourierStartDate != null && CourierEndDate == null)
                //    {
                //        FilterData Filter = new FilterData()
                //        {
                //            field = "CreateDate",
                //            data = new data()
                //            {
                //                comparison = "gteq",
                //                type = "date",
                //                value = new[] { CourierStartDate }
                //            }
                //        };
                //        Filters.Add(Filter);
                //    }

                //    if (CourierEndDate != null && CourierStartDate == null)
                //    {
                //        FilterData Filter = new FilterData()
                //        {
                //            field = "CreateDate",
                //            data = new data()
                //            {
                //                comparison = "lteq",
                //                type = "date",
                //                value = new[] { CourierEndDate }
                //            }
                //        };
                //        Filters.Add(Filter);
                //    }

                //    if (CourierStartDate != null && CourierEndDate != null)
                //    {
                //        FilterData Filter = new FilterData()
                //        {
                //            field = "CreateDate",
                //            data = new data()
                //            {
                //                comparison = "lteq",
                //                type = "dateBetween",
                //                value = new[] { CourierEndDate, CourierEndDate }
                //            }
                //        };
                //        Filters.Add(Filter);
                //    }
                //}

                #endregion
            }


            if (RollBacked == false || RollBacked == null)
            {


                Filters.Add(new FilterData()
                {
                    field = "IsRollbackDetail",
                    data = new data()
                    {
                        type = "boolean",
                        value = new[] { bool.FalseString }
                    }
                });

                #region prepairing Filters

                if (SaleEmployeeID != null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in SaleEmployeeID)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "CreateEmployee.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    Filters.Add(Filter);
                }

                if (DeliverEmployeeID != null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in DeliverEmployeeID)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "DeliverEmployee.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    Filters.Add(Filter);
                }


                if (Delivered == true)
                {
                    FilterData Filter = new FilterData();
                    Filter.field = "Delivered";
                    Filter.data = new data()
                    {
                        type = "boolean",
                        value = new[] { bool.TrueString }
                    };
                    Filters.Add(Filter);
                }

                if (Delivered == false)
                {
                    FilterData Filter = new FilterData();
                    Filter.field = "Delivered";
                    Filter.data = new data()
                    {
                        type = "boolean",
                        value = new[] { bool.FalseString }
                    };
                    Filters.Add(Filter);
                }

                #endregion

                #region Sale Date

                if (SaleStartDate != null && SaleEndDate == null)
                {

                    FilterData Filter = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { SaleStartDate }
                        }
                    };
                    Filters.Add(Filter);


                }

                if (SaleEndDate != null && SaleStartDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { SaleEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (SaleStartDate != null && SaleEndDate != null)
                {

                    FilterData Filter = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { SaleStartDate, SaleEndDate }
                        }

                    };
                    Filters.Add(Filter);
                }


                #endregion

                #region Deliver Date

                if (DeliverStartDate != null && DeliverEndDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "DeliverDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { DeliverStartDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (DeliverEndDate != null && DeliverStartDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "DeliverDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { DeliverEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                if (DeliverStartDate != null && DeliverEndDate != null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "DeliverDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { DeliverStartDate, DeliverEndDate }
                        }
                    };
                    Filters.Add(Filter);
                }

                #endregion

            }



            #region Getting Data

            if (IsUnCreditService == true)
            {
                IList<FilterData> finalFilter1 = new List<FilterData>();
                finalFilter1 = Filters;
                finalFilter1.Remove(finalFilter1.Where(x => x.field == "CreditService.ID").FirstOrDefault());
                finalFilter1.Remove(finalFilter1.Where(x => x.field == "CreditService.Network.ID").FirstOrDefault());
                finalFilter1.Remove(finalFilter1.Where(x => x.field == "ProductPrice.ID").FirstOrDefault());
                finalFilter1.Remove(finalFilter1.Where(x => x.field == "ProductPrice.Product.ID").FirstOrDefault());

                #region Spacial Filters

                if (UncreditServices != null)
                {

                    IList<string> Ids = new List<string>();
                    foreach (var item in UncreditServices)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "UncreditService.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    finalFilter1.Add(Filter);
                }



                #endregion

                uncreditSaleDetailViews = _uncreditSaleDetailService.GetSaleReport(finalFilter1);
            }
            if (IsCreditService == true)
            {

                IList<FilterData> finalFilter2 = new List<FilterData>();
                finalFilter2 = Filters;
                finalFilter2.Remove(finalFilter2.Where(x => x.field == "UncreditService.ID").FirstOrDefault());
                finalFilter2.Remove(finalFilter2.Where(x => x.field == "ProductPrice.ID").FirstOrDefault());
                finalFilter2.Remove(finalFilter2.Where(x => x.field == "ProductPrice.Product.ID").FirstOrDefault());

                #region Spacial Filters

                if (CreditService != null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in CreditService)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "CreditService.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    finalFilter2.Add(Filter);
                }

                if (Networks != null && CreditService == null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in Networks)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "CreditService.Network.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()

                    };
                    finalFilter2.Add(Filter);
                }

                #endregion

                creditSaleDetailViews = _creditSaleDetailService.GetSaleReport(finalFilter2);
            }

            if (IsProducts == true)
            {
                IList<FilterData> finalFilter3 = new List<FilterData>();
                finalFilter3 = Filters;
                finalFilter3.Remove(finalFilter3.Where(x => x.field == "UncreditService.ID").FirstOrDefault());
                finalFilter3.Remove(finalFilter3.Where(x => x.field == "CreditService.ID").FirstOrDefault());
                finalFilter3.Remove(finalFilter3.Where(x => x.field == "CreditService.Network.ID").FirstOrDefault());

                #region Spacial Filters

                if (ProductPrices != null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in ProductPrices)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "ProductPrice.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    finalFilter3.Add(Filter);
                }


                if (Products != null && ProductPrices == null)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in Products)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "ProductPrice.Product.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    finalFilter3.Add(Filter);
                }

                #endregion

                productSaleDetailViews = _productSaleDetailService.GetSaleReport(finalFilter3);
            }
            if (IsProducts == null && IsCreditService == null && IsUnCreditService == null)
            {
                productSaleDetailViews = _productSaleDetailService.GetSaleReport(Filters);
                creditSaleDetailViews = _creditSaleDetailService.GetSaleReport(Filters);
                uncreditSaleDetailViews = _uncreditSaleDetailService.GetSaleReport(Filters);
            }


            #endregion

            #region preparing Out put Report

            if (uncreditSaleDetailViews.data != null)
                foreach (GetSaleDetailReportView item in uncreditSaleDetailViews.data)
                    Report.Add(item);

            if (creditSaleDetailViews.data != null)
                foreach (GetSaleDetailReportView item in creditSaleDetailViews.data)
                    Report.Add(item);

            if (productSaleDetailViews.data != null)
                foreach (GetSaleDetailReportView item in productSaleDetailViews.data)
                    Report.Add(item);

            #endregion


            if (Report.Count > 0)
                response.data = Report.OrderBy(x => x.SaleDate);
            else
                response.data = Report.OrderBy(x => x.SaleDate);

            response.totalCount = Report.Count();

            #region Preparing Excel

            if (ExportTo == "Excel")
            {
                GridView gridView = new GridView();
                gridView.DataSource = response.data;

                gridView.DataBind();

                gridView.HeaderRow.Cells[0].Text = "نام مشتری";
                gridView.HeaderRow.Cells[1].Text = "تلفن";
                gridView.HeaderRow.Cells[2].Text = "کارشناس فروش";
                gridView.HeaderRow.Cells[3].Text = "نام مرکز";
                gridView.HeaderRow.Cells[4].Text = "تاریخ فروش";
                gridView.HeaderRow.Cells[5].Text = "نام محصول";
                gridView.HeaderRow.Cells[6].Text = "کالا";
                gridView.HeaderRow.Cells[7].Text = "شبکه";
                gridView.HeaderRow.Cells[8].Text = "خدمات اعتباری";
                gridView.HeaderRow.Cells[9].Text = "خدمات غیر اعتباری";
                gridView.HeaderRow.Cells[10].Text = "قیمت";
                gridView.HeaderRow.Cells[11].Text = "تعداد";
                gridView.HeaderRow.Cells[12].Text = "تخفیف";
                gridView.HeaderRow.Cells[13].Text = "مالیات";
                gridView.HeaderRow.Cells[14].Text = "مجموع";
                gridView.HeaderRow.Cells[15].Text = "کارمند تحویل";
                gridView.HeaderRow.Cells[16].Text = "تاریخ تحویل";
                gridView.HeaderRow.Cells[17].Text = "کارمند برگشت";
                gridView.HeaderRow.Cells[18].Text = "تاریخ برگشت";
                gridView.HeaderRow.Cells[19].Text = "تعداد برگشت";
                gridView.HeaderRow.Cells[20].Text = "مبلغ برگشتی";
                gridView.Font.Names = new[] { "Tahoma" };

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
                Response.ContentType = "application/ms-excel";

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmltextwriter = new HtmlTextWriter(stringWriter);

                gridView.RenderControl(htmltextwriter);

                Response.Output.Write(stringWriter.ToString());
                Response.Flush();
                Response.End();

            }

            #endregion



            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Deference Ledger Account

        public ActionResult GetLedgerAccounting()
        {
            GetGeneralResponse<IEnumerable<CustomerView>> cus = _customerService.GetAllCustomrs();
            IList<rep> r = new List<rep>();
            foreach (var item in cus.data)
            {

                GetGeneralResponse<IEnumerable<LedgerAccountView>> response =
                    new GetGeneralResponse<IEnumerable<LedgerAccountView>>();
                IList<LedgerAccountView> responseData = new List<LedgerAccountView>();

                AjaxGetRequest getRequest = new AjaxGetRequest();

                getRequest.PageSize = 1000000;
                getRequest.PageNumber = 1;

                // مالی ها
                IEnumerable<FiscalView> FiscalViews =
                    _fiscalService.GetFiscalsOfCustomer(getRequest, item.ID).FiscalViews;
                foreach (var fiscalView in FiscalViews)
                {
                    // فقط تأیید شده ها
                    if (fiscalView.ConfirmInt == 2)
                    {
                        LedgerAccountView ledgerAccountView = new LedgerAccountView();
                        ledgerAccountView.CustomerID = fiscalView.CustomerID;
                        ledgerAccountView.Date = fiscalView.ConfirmDate;
                        ledgerAccountView.Description = fiscalView.Cost > 0 ? "Recive" : "Pay";
                        ledgerAccountView.SerialNumber = fiscalView.DocumentSerial;
                        ledgerAccountView.BedCost = fiscalView.Cost > 0 ? 0 : Math.Abs(fiscalView.ConfirmedCost);
                        ledgerAccountView.BesCost = fiscalView.Cost > 0 ? Math.Abs(fiscalView.ConfirmedCost) : 0;
                        ledgerAccountView.FiscalType = fiscalView.Cost > 0 ? "Recive" : "Pay";
                        ledgerAccountView.DeliverStatus = null;
                        ledgerAccountView.RollbackStatus = null;

                        responseData.Add(ledgerAccountView);
                    }
                }

                // فاکتورها
                FilterData _filter = new FilterData()
                {
                    field = "CustomerName",
                    data = new data()
                    {
                        type = "string",
                        value = new[] { item.ID.ToString() }
                    }
                };

                IList<FilterData> filter = new List<FilterData>();
                filter.Add(_filter);
                IEnumerable<SaleView> SaleViews =
                    _saleService.GetSales(
                        new AjaxGetRequest()
                        {
                            PageNumber = getRequest.PageNumber,
                            PageSize = getRequest.PageSize,
                            ID = item.ID
                        }, null, filter, false).SaleViews;
                if (SaleViews != null)
                {
                    foreach (var saleView in SaleViews)
                    {
                        // فقط تأیید شده ها
                        if (saleView.Closed)
                        {
                            #region ProductSaleDetail Data

                            // کالاها
                            foreach (var productSaleDetail in saleView.ProductSaleDetails)
                            {

                                LedgerAccountView ledgerAccountView = new LedgerAccountView();

                                ledgerAccountView.Date = saleView.CloseDate;
                                ledgerAccountView.SerialNumber = saleView.SaleNumber;
                                ledgerAccountView.CustomerID = saleView.CustomerID;
                                ledgerAccountView.FiscalType = null;

                                if (productSaleDetail.RollbackPrice > 0)
                                {
                                    // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                    ledgerAccountView.BedCost = 0;
                                    //بستانکار
                                    ledgerAccountView.BesCost = productSaleDetail.RollbackPrice;
                                    //ineTotalWithoutDiscountAndImposition + creditSaleDetail.LineImposition;
                                    // وضعیت تحویل
                                    ledgerAccountView.DeliverStatus = "Delivered";
                                    // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                    ledgerAccountView.RollbackStatus = "Rollbacked";
                                }
                                //اگرفاکتور برگشت از فروش نبود
                                else
                                {
                                    // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                    ledgerAccountView.BedCost = productSaleDetail.LineTotalWithoutDiscountAndImposition;
                                    //بستانکار
                                    ledgerAccountView.BesCost = 0;
                                    // وضعیت تحویل
                                    ledgerAccountView.DeliverStatus = productSaleDetail.Delivered
                                        ? "Delivered"
                                        : "NotDelivered";
                                    // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                    ledgerAccountView.RollbackStatus = productSaleDetail.Rollbacked
                                        ? "Rollbacked"
                                        : null;
                                }

                                // شرح کالا
                                ledgerAccountView.Description = productSaleDetail.ProductPriceTitle;
                                // مبلغ بدهکار شامل قیمت کالاها به اضافه مالیات سطر

                                // وضعیت تحویل
                                ledgerAccountView.DeliverStatus = string.IsNullOrEmpty(productSaleDetail.DeliverDate)
                                    ? "NotDelivered"
                                    : "Delivered";
                                // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمیگردد
                                ledgerAccountView.RollbackStatus = productSaleDetail.Rollbacked ? "Rollbacked" : null;

                                responseData.Add(ledgerAccountView);
                            }

                            #endregion

                            #region CreditSaleDetail Data

                            // خدمات اعتباری
                            foreach (var creditSaleDetail in saleView.CreditSaleDetails)
                            {
                                LedgerAccountView ledgerAccountView = new LedgerAccountView();

                                ledgerAccountView.Date = saleView.CloseDate;
                                ledgerAccountView.SerialNumber = saleView.SaleNumber;
                                ledgerAccountView.CustomerID = saleView.CustomerID;
                                ledgerAccountView.FiscalType = null;

                                // شرح خدمات اعتباری
                                ledgerAccountView.Description = creditSaleDetail.CreditServiceName;
                                //اگرفاکتور برگشت از فروش بود
                                if (creditSaleDetail.RollbackPrice > 0)
                                {
                                    // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                    ledgerAccountView.BedCost = 0;
                                    //بستانکار
                                    ledgerAccountView.BesCost = creditSaleDetail.RollbackPrice;
                                    //ineTotalWithoutDiscountAndImposition + creditSaleDetail.LineImposition;
                                    // وضعیت تحویل
                                    ledgerAccountView.DeliverStatus = "Delivered";
                                    // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                    ledgerAccountView.RollbackStatus = "Rollbacked";
                                }
                                //اگرفاکتور برگشت از فروش نبود
                                else
                                {
                                    // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                    ledgerAccountView.BedCost = creditSaleDetail.LineTotalWithoutDiscountAndImposition;
                                    //بستانکار
                                    ledgerAccountView.BesCost = 0;
                                    // وضعیت تحویل
                                    ledgerAccountView.DeliverStatus = creditSaleDetail.Delivered
                                        ? "Delivered"
                                        : "NotDelivered";
                                    // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                    ledgerAccountView.RollbackStatus = creditSaleDetail.Rollbacked ? "Rollbacked" : null;
                                }


                                responseData.Add(ledgerAccountView);
                            }
                            temp = responseData;

                            #endregion

                            #region UncreditSaleDetail Data

                            // خدمات غیر اعتباری
                            foreach (var uncreditSaleDetail in saleView.UncreditSaleDetails)
                            {
                                LedgerAccountView ledgerAccountView = new LedgerAccountView();

                                ledgerAccountView.Date = saleView.CloseDate;
                                ledgerAccountView.SerialNumber = saleView.SaleNumber;
                                ledgerAccountView.CustomerID = saleView.CustomerID;
                                if (uncreditSaleDetail.RollbackPrice > 0)
                                {
                                    // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                    ledgerAccountView.BedCost = 0;
                                    //بستانکار
                                    ledgerAccountView.BesCost = uncreditSaleDetail.RollbackPrice;
                                    //ineTotalWithoutDiscountAndImposition + creditSaleDetail.LineImposition;
                                    // وضعیت تحویل
                                    ledgerAccountView.DeliverStatus = "Delivered";
                                    // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                    ledgerAccountView.RollbackStatus = "Rollbacked";
                                }
                                //اگرفاکتور برگشت از فروش نبود
                                else
                                {
                                    // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                    ledgerAccountView.BedCost = uncreditSaleDetail.LineTotalWithoutDiscountAndImposition;
                                    //بستانکار
                                    ledgerAccountView.BesCost = 0;
                                    // وضعیت تحویل
                                    ledgerAccountView.DeliverStatus = uncreditSaleDetail.Delivered
                                        ? "Delivered"
                                        : "NotDelivered";
                                    // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                    ledgerAccountView.RollbackStatus = uncreditSaleDetail.Rollbacked
                                        ? "Rollbacked"
                                        : null;
                                }

                                ledgerAccountView.FiscalType = null;

                                // شرح خدمات غیراعتباری
                                ledgerAccountView.Description = uncreditSaleDetail.UncreditServiceName;

                                // وضعیت تحویل
                                ledgerAccountView.DeliverStatus = string.IsNullOrEmpty(uncreditSaleDetail.DeliverDate)
                                    ? "NotDelivered"
                                    : "Delivered";
                                // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمیگردد
                                ledgerAccountView.RollbackStatus = uncreditSaleDetail.Rollbacked ? "Rollbacked" : null;

                                responseData.Add(ledgerAccountView);
                            }

                            #endregion

                            #region Discount

                            // تخفیف فاکتور
                            //اگر مبلغ تخفیف صفر بود در معین مشتری نمایش داده نشود
                            if (saleView.TotalDiscount > 0)
                            {

                                LedgerAccountView ledgerAccountView2 = new LedgerAccountView();
                                ledgerAccountView2.CustomerID = saleView.CustomerID;
                                ledgerAccountView2.Date = saleView.CloseDate;
                                ledgerAccountView2.Description = "Discount";
                                ledgerAccountView2.SerialNumber = saleView.SaleNumber;
                                if (saleView.IsRollbackSale)
                                {
                                    ledgerAccountView2.BedCost = saleView.TotalDiscount;
                                    // در تخفیف مشتری بستانکار می شود
                                    ledgerAccountView2.BesCost = 0;
                                }
                                else
                                {
                                    ledgerAccountView2.BedCost = 0;
                                    // در تخفیف مشتری بستانکار می شود
                                    ledgerAccountView2.BesCost = saleView.TotalDiscount;
                                }

                                ledgerAccountView2.FiscalType = null;
                                ledgerAccountView2.DeliverStatus = null;
                                ledgerAccountView2.RollbackStatus = null;
                                responseData.Add(ledgerAccountView2);
                            }

                            if (saleView.TotalImposition > 0)
                            {

                                LedgerAccountView ledgerAccountView2 = new LedgerAccountView();

                                ledgerAccountView2.Date = saleView.CloseDate;
                                ledgerAccountView2.Description = "مالیات";
                                ledgerAccountView2.SerialNumber = saleView.SaleNumber;

                                if (saleView.IsRollbackSale)
                                {
                                    ledgerAccountView2.BedCost = 0;
                                    // گر برگشت از فروش بود مالیات مشتری بستانکا میشود
                                    ledgerAccountView2.BesCost = saleView.TotalImposition;
                                }
                                else
                                {
                                    ledgerAccountView2.BedCost = saleView.TotalImposition;
                                    // در مالیات مشتری بدهکار می شود
                                    ledgerAccountView2.BesCost = 0;
                                }
                                ledgerAccountView2.FiscalType = null;
                                ledgerAccountView2.DeliverStatus = null;
                                ledgerAccountView2.RollbackStatus = null;
                                responseData.Add(ledgerAccountView2);
                            }

                            #endregion
                        }
                    }
                }

                // مرتب کردن بر اساس تاریخ 
                IEnumerable<LedgerAccountView> sortedResponseData = responseData.OrderBy(o => o.Date);

                // پر کردن مقدار باقیمانده
                IList<LedgerAccountView> completeResponseData = new List<LedgerAccountView>();
                long remain = 0;
                foreach (var ledgerAccountView in sortedResponseData)
                {
                    remain += ledgerAccountView.BesCost - ledgerAccountView.BedCost;
                    ledgerAccountView.Remain = remain;
                    completeResponseData.Add(ledgerAccountView);
                }

                try
                {
                    response.data = completeResponseData;
                    if (completeResponseData != null || completeResponseData.Count() > 0)
                        if (completeResponseData.LastOrDefault().Remain != item.Balance)
                        {
                            r.Add(new rep()
                            {
                                adslPhone = item.ADSLPhone,
                                Balance = Convert.ToInt32(item.Balance),
                                Moeein = Convert.ToInt32(completeResponseData.LastOrDefault().Remain)
                            });
                        }
                }
                catch (Exception ex)
                {
                }
            }

            return View("GetLedgerAccounting", r);
        }

        #endregion

        #region Deference Cand Deliver Cost

        public ActionResult GetCandeliverCostDerefrence()
        {
            GetGeneralResponse<IEnumerable<rep>> response = new GetGeneralResponse<IEnumerable<rep>>();
            IList<rep> report = new List<rep>();
            GetGeneralResponse<IEnumerable<CustomerView>> Customers = _customerService.GetAllCustomrs();
            foreach (CustomerView customer in Customers.data)
            {

                long fiscalRemain = 0;
                long productRemain = 0;
                long creditRemain = 0;
                long uncreditRemain = 0;
                long FinalRemain = 0;
                GetGeneralResponse<IEnumerable<FiscalView>> Fiscals = _fiscalService.GetFiscals(customer.ID, -1, -1,
                    null, null);
                if (Fiscals.data != null)
                    foreach (FiscalView fiscal in Fiscals.data)
                    {
                        FinalRemain += fiscal.ConfirmedCost;
                    }

                IEnumerable<SaleView> SaleViews =
                    _saleService.GetSales(new AjaxGetRequest() { PageNumber = -1, PageSize = -1, ID = customer.ID }, null,
                        null, false).SaleViews;
                if (SaleViews != null)
                {
                    foreach (var saleView in SaleViews)
                    {
                        // فقط تأیید شده ها
                        if (saleView.Closed)
                        {
                            #region ProductSaleDetail Data

                            // کالاها

                            foreach (var productSaleDetail in saleView.ProductSaleDetails)
                            {
                                if (productSaleDetail.DeliverDate != null)
                                {
                                    productRemain += productSaleDetail.LineTotal;
                                }
                                if (productSaleDetail.RollbackPrice > 0)
                                {
                                    productRemain -= productSaleDetail.LineTotal;
                                }

                            }

                            #endregion

                            #region CreditSaleDetail Data

                            // خدمات اعتباری

                            foreach (var creditSaleDetail in saleView.CreditSaleDetails)
                            {
                                if (creditSaleDetail.DeliverDate != null)
                                {
                                    creditRemain += creditSaleDetail.LineTotal;
                                }
                                if (creditSaleDetail.RollbackPrice > 0)
                                {
                                    productRemain -= creditSaleDetail.LineTotal;
                                }
                            }

                            #endregion

                            #region UncreditSaleDetail Data

                            // خدمات غیر اعتباری

                            foreach (var uncreditSaleDetail in saleView.UncreditSaleDetails)
                            {

                                if (uncreditSaleDetail.DeliverDate != null)
                                {
                                    uncreditRemain += uncreditSaleDetail.LineTotal;
                                }
                                if (uncreditSaleDetail.RollbackPrice > 0)
                                {
                                    productRemain -= uncreditSaleDetail.LineTotal;
                                }
                            }

                            #endregion

                        }
                    }

                }

                FinalRemain = fiscalRemain + productRemain + uncreditRemain + creditRemain;

                if (customer.CanDeliverCost != FinalRemain)
                {
                    report.Add(new rep()
                    {
                        adslPhone = customer.ADSLPhone,
                        Balance = Convert.ToInt32(customer.CanDeliverCost),
                        Moeein = Convert.ToInt32(FinalRemain)
                    });
                }

            }

            return View("GetCandeliverCostDerefrence", report);
        }

        #endregion

        #region Get Ledger Account Sammary

        public JsonResult GetLedgerAccountSammary(Guid CustomerID)
        {
            GetGeneralResponse<IEnumerable<LedgerAccountSammaryView>> response =
                new GetGeneralResponse<IEnumerable<LedgerAccountSammaryView>>();

            #region GetCustomer

            GetRequest request = new GetRequest() { ID = CustomerID };
            CustomerView customer = _customerService.GetCustomer(request).CustomerView;

            #endregion

            #region Preparing Sales

            FilterData _filter = new FilterData()
            {
                field = "CustomerName",
                data = new data()
                {
                    type = "string",
                    value = new[] { CustomerID.ToString() }
                }
            };

            IList<FilterData> filter = new List<FilterData>();
            filter.Add(_filter);

            IEnumerable<SaleView> sales =
                _saleService.GetSales(new AjaxGetRequest() { ID = CustomerID, PageNumber = 1, PageSize = -1 }, null,
                    filter, false).SaleViews;

            IList<LedgerAccountSammaryView> list = new List<LedgerAccountSammaryView>();

            foreach (SaleView sale in sales)
            {
                if (sale.Closed)
                {
                    LedgerAccountSammaryView ledgerAccountSammary = new LedgerAccountSammaryView();

                    ledgerAccountSammary.EditCourier = sale.EditCourier;
                    ledgerAccountSammary.CustomerID = sale.CustomerID;
                    ledgerAccountSammary.RollbackLightOn = sale.RollbackLightOn;
                    ledgerAccountSammary.CanDeliver = sale.CanDeliver;
                    ledgerAccountSammary.CanRollback = sale.CanRollback;
                    ledgerAccountSammary.CreateDate = sale.CreateDate;
                    ledgerAccountSammary.TotalPriceWithoutDiscountAndImposition =
                        sale.SaleTotalWithoutDiscountAndImposition;
                    ledgerAccountSammary.TotalDiscount = sale.TotalDiscount;
                    ledgerAccountSammary.TotalImposition = sale.TotalImposition;
                    ledgerAccountSammary.Documentserial = sale.SaleNumber;
                    ledgerAccountSammary.ID = sale.ID;
                    // اگر فاکتور برگشت بود مشتری بستانکار و در غیر اینصورت بدهکار میشود
                    if (sale.IsRollbackSale)
                    {
                        ledgerAccountSammary.BesCost = sale.SaleTotal;
                        ledgerAccountSammary.Type = "فاکتور برگشت از فروش";
                        ledgerAccountSammary.RecordType = 'R';
                    }
                    else
                    {
                        ledgerAccountSammary.BedCost = sale.SaleTotal;
                        ledgerAccountSammary.Type = "فاکتور فروش";
                        ledgerAccountSammary.RecordType = 'S';
                    }

                    list.Add(ledgerAccountSammary);
                }
            }

            #endregion

            #region Preparing Fiscals

            AjaxGetRequest getRequest = new AjaxGetRequest();

            getRequest.PageSize = 1000000;
            getRequest.PageNumber = 1;

            // مالی ها
            IEnumerable<FiscalView> FiscalViews =
                _fiscalService.GetFiscalsOfCustomer(getRequest, CustomerID).FiscalViews;
            foreach (var fiscalView in FiscalViews)
            {
                // فقط تأیید شده ها
                if (fiscalView.ConfirmInt == 2)
                {
                    LedgerAccountSammaryView ledgerAccountSammary = new LedgerAccountSammaryView();
                    ledgerAccountSammary.CustomerID = fiscalView.CustomerID;
                    ledgerAccountSammary.CreateDate = fiscalView.CreateDate;
                    ledgerAccountSammary.Documentserial = fiscalView.SerialNumber.ToString();
                    ledgerAccountSammary.ID = fiscalView.ID;
                    ledgerAccountSammary.RecordType = 'F';
                    /// اگر دریافت بود مشتری ستانکار در غیر اینصورت مشتری بدهکار میشود
                    if (fiscalView.ConfirmedCost > 0)
                    {
                        ledgerAccountSammary.BesCost = fiscalView.ConfirmedCost;
                        ledgerAccountSammary.Type = "دریافت از مشتری";
                    }
                    else
                    {
                        ledgerAccountSammary.BedCost = Math.Abs(fiscalView.ConfirmedCost);
                        ledgerAccountSammary.Type = "پرداخت به مشتری";
                    }
                    list.Add(ledgerAccountSammary);
                }
            }

            #endregion

            #region Sort By Create Date

            IEnumerable<LedgerAccountSammaryView> sortedList = list.OrderBy(x => x.CreateDate);

            #endregion

            #region Calculate Remain

            IList<LedgerAccountSammaryView> finalList = new List<LedgerAccountSammaryView>();
            long remain = 0;
            foreach (LedgerAccountSammaryView item in sortedList)
            {
                remain += item.BesCost - item.BedCost;
                item.Remain = remain;
                if (item.Remain > 0)
                    item.Status = "بستانکار";
                else if (item.Remain < 0)
                    item.Status = "بدهکار";
                else
                    item.Status = "تسویه";
                finalList.Add(item);
            }

            #endregion

            response.data = finalList;
            response.totalCount = finalList.Count();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get Can Deliver Cost LedgerAccount

        public JsonResult CanDeliverLedgerAccount(Guid CustomerID)
        {
            GetGeneralResponse<IEnumerable<LedgerAccountView>> response =
                new GetGeneralResponse<IEnumerable<LedgerAccountView>>();
            IList<LedgerAccountView> responseData = new List<LedgerAccountView>();

            AjaxGetRequest getRequest = new AjaxGetRequest();

            getRequest.PageSize = 1000000;
            getRequest.PageNumber = 1;

            // مالی ها
            IEnumerable<FiscalView> FiscalViews =
                _fiscalService.GetFiscalsOfCustomer(getRequest, CustomerID).FiscalViews;
            foreach (var fiscalView in FiscalViews)
            {
                // فقط تأیید شده ها
                if (fiscalView.ConfirmInt == 2)
                {
                    LedgerAccountView ledgerAccountView = new LedgerAccountView();
                    ledgerAccountView.CustomerID = fiscalView.CustomerID;
                    ledgerAccountView.Date = fiscalView.ConfirmDate;
                    ledgerAccountView.Description = fiscalView.Cost > 0 ? "Recive" : "Pay";
                    ledgerAccountView.SerialNumber = fiscalView.DocumentSerial;
                    ledgerAccountView.BedCost = fiscalView.Cost > 0 ? 0 : Math.Abs(fiscalView.ConfirmedCost);
                    ledgerAccountView.BesCost = fiscalView.Cost > 0 ? Math.Abs(fiscalView.ConfirmedCost) : 0;
                    ledgerAccountView.FiscalType = fiscalView.Cost > 0 ? "Recive" : "Pay";
                    ledgerAccountView.DeliverStatus = null;
                    ledgerAccountView.RollbackStatus = null;

                    responseData.Add(ledgerAccountView);
                }
            }

            // فاکتورها
            FilterData _filter = new FilterData()
            {
                field = "CustomerName",
                data = new data()
                {
                    type = "string",
                    value = new[] { CustomerID.ToString() }
                }
            };

            IList<FilterData> filter = new List<FilterData>();
            filter.Add(_filter);
            IEnumerable<SaleView> SaleViews =
                _saleService.GetSales(
                    new AjaxGetRequest()
                    {
                        PageNumber = getRequest.PageNumber,
                        PageSize = getRequest.PageSize,
                        ID = CustomerID
                    }, null, filter, false).SaleViews;
            if (SaleViews != null)
            {
                foreach (var saleView in SaleViews)
                {
                    // فقط تأیید شده ها
                    if (saleView.Closed)
                    {
                        #region ProductSaleDetail Data

                        // کالاها
                        foreach (var productSaleDetail in saleView.ProductSaleDetails)
                        {
                            bool add = false;
                            LedgerAccountView ledgerAccountView = new LedgerAccountView();
                            ledgerAccountView.CustomerID = saleView.CustomerID;
                            ledgerAccountView.Date = saleView.CloseDate;
                            ledgerAccountView.SerialNumber = saleView.SaleNumber;

                            ledgerAccountView.FiscalType = null;

                            if (productSaleDetail.RollbackPrice > 0 &&
                                productSaleDetail.Status == SaleDetailStatus.DeliveredAndRollbacked)
                            {
                                ledgerAccountView.Date = productSaleDetail.CreateDate;
                                // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                ledgerAccountView.BedCost = 0;
                                //بستانکار
                                ledgerAccountView.BesCost = productSaleDetail.RollbackPrice -
                                                            productSaleDetail.LineDiscount +
                                                            productSaleDetail.LineImposition;
                                //ineTotalWithoutDiscountAndImposition + creditSaleDetail.LineImposition;
                                // وضعیت تحویل
                                ledgerAccountView.DeliverStatus = "فاکتور برگشتی";
                                // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                ledgerAccountView.RollbackStatus = "Rollbacked";
                                add = true;
                            }
                            //اگرفاکتور برگشت از فروش نبود
                            else if (productSaleDetail.Delivered)
                            {
                                ledgerAccountView.Date = productSaleDetail.DeliverDate;
                                // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                ledgerAccountView.BedCost = productSaleDetail.LineTotal;
                                //بستانکار
                                ledgerAccountView.BesCost = 0;
                                // وضعیت تحویل
                                ledgerAccountView.DeliverStatus = "Delivered";
                                // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                ledgerAccountView.RollbackStatus = productSaleDetail.Rollbacked ? "Rollbacked" : null;
                                add = true;
                            }

                            // شرح کالا
                            ledgerAccountView.Description = productSaleDetail.ProductPriceTitle;
                            // مبلغ بدهکار شامل قیمت کالاها به اضافه مالیات سطر


                            if (add)
                                responseData.Add(ledgerAccountView);
                        }

                        #endregion

                        #region CreditSaleDetail Data

                        // خدمات اعتباری
                        foreach (var creditSaleDetail in saleView.CreditSaleDetails)
                        {
                            bool add = false;
                            LedgerAccountView ledgerAccountView = new LedgerAccountView();


                            ledgerAccountView.SerialNumber = saleView.SaleNumber;
                            ledgerAccountView.CustomerID = saleView.CustomerID;
                            ledgerAccountView.FiscalType = null;

                            // شرح خدمات اعتباری
                            ledgerAccountView.Description = creditSaleDetail.CreditServiceName;
                            //اگرفاکتور برگشت از فروش بود
                            if (creditSaleDetail.RollbackPrice > 0 &&
                                creditSaleDetail.Status == SaleDetailStatus.DeliveredAndRollbacked)
                            {
                                ledgerAccountView.Date = creditSaleDetail.CreateDate;
                                // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                ledgerAccountView.BedCost = 0;
                                //بستانکار
                                ledgerAccountView.BesCost = creditSaleDetail.RollbackPrice -
                                                            creditSaleDetail.LineDiscount +
                                                            creditSaleDetail.LineImposition;
                                // وضعیت تحویل
                                ledgerAccountView.DeliverStatus = "فاکتور برگشتی";
                                // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                ledgerAccountView.RollbackStatus = "Rollbacked";
                                add = true;
                            }
                            //اگرفاکتور برگشت از فروش نبود
                            else if (creditSaleDetail.Delivered)
                            {
                                ledgerAccountView.Date = creditSaleDetail.DeliverDate;
                                // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                ledgerAccountView.BedCost = creditSaleDetail.LineTotal;
                                //بستانکار
                                ledgerAccountView.BesCost = 0;
                                // وضعیت تحویل
                                ledgerAccountView.DeliverStatus = creditSaleDetail.Delivered
                                    ? "Delivered"
                                    : "NotDelivered";
                                // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                ledgerAccountView.RollbackStatus = creditSaleDetail.Rollbacked ? "Rollbacked" : null;
                                add = true;
                            }

                            if (add)
                                responseData.Add(ledgerAccountView);
                        }
                        temp = responseData;

                        #endregion

                        #region UncreditSaleDetail Data

                        // خدمات غیر اعتباری
                        foreach (var uncreditSaleDetail in saleView.UncreditSaleDetails)
                        {
                            bool add = false;
                            LedgerAccountView ledgerAccountView = new LedgerAccountView();


                            ledgerAccountView.SerialNumber = saleView.SaleNumber;
                            ledgerAccountView.CustomerID = saleView.CustomerID;
                            if (uncreditSaleDetail.RollbackPrice > 0 &&
                                uncreditSaleDetail.Status == SaleDetailStatus.DeliveredAndRollbacked)
                            {
                                ledgerAccountView.Date = uncreditSaleDetail.CreateDate;
                                // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                ledgerAccountView.BedCost = 0;
                                //بستانکار
                                ledgerAccountView.BesCost = uncreditSaleDetail.RollbackPrice -
                                                            uncreditSaleDetail.LineDiscount +
                                                            uncreditSaleDetail.LineImposition;
                                ;
                                // وضعیت تحویل
                                ledgerAccountView.DeliverStatus = "فاکتور برگشتی";
                                // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                ledgerAccountView.RollbackStatus = "Rollbacked";
                                add = true;
                            }
                            //اگرفاکتور برگشت از فروش نبود
                            else if (uncreditSaleDetail.Delivered)
                            {
                                ledgerAccountView.Date = uncreditSaleDetail.DeliverDate;
                                // مبلغ بدهکار شامل قیمت خدمات به اضافه مالیات سطر
                                ledgerAccountView.BedCost = uncreditSaleDetail.LineTotal;
                                //بستانکار
                                ledgerAccountView.BesCost = 0;
                                // وضعیت تحویل
                                ledgerAccountView.DeliverStatus = uncreditSaleDetail.Delivered
                                    ? "Delivered"
                                    : "NotDelivered";
                                // وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمی گردد
                                ledgerAccountView.RollbackStatus = uncreditSaleDetail.Rollbacked ? "Rollbacked" : null;
                                add = true;
                            }

                            ledgerAccountView.FiscalType = null;

                            // شرح خدمات غیراعتباری
                            ledgerAccountView.Description = uncreditSaleDetail.UncreditServiceName;

                            //// وضعیت تحویل
                            //ledgerAccountView.DeliverStatus = string.IsNullOrEmpty(uncreditSaleDetail.DeliverDate) ? "NotDelivered" : "Delivered";
                            //// وضعیت برگشت که در صورتی که برگشت نشده باشد نال برمیگردد
                            //ledgerAccountView.RollbackStatus = uncreditSaleDetail.Rollbacked ? "Rollbacked" : null;

                            if (add)
                                responseData.Add(ledgerAccountView);
                        }

                        #endregion


                    }
                }
            }

            // مرتب کردن بر اساس تاریخ 
            IEnumerable<LedgerAccountView> sortedResponseData = responseData.OrderBy(o => o.Date);

            // پر کردن مقدار باقیمانده
            IList<LedgerAccountView> completeResponseData = new List<LedgerAccountView>();
            long remain = 0;
            foreach (var ledgerAccountView in sortedResponseData)
            {
                remain += ledgerAccountView.BesCost - ledgerAccountView.BedCost;
                ledgerAccountView.Remain = remain;
                completeResponseData.Add(ledgerAccountView);
            }

            response.data = completeResponseData;

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Support History

        public JsonResult SupportHistory(Guid SupportID)
        {
            GetGeneralResponse<IEnumerable<SupportHistoryView>> response =
                new GetGeneralResponse<IEnumerable<SupportHistoryView>>();
            IList<SupportHistoryView> list = new List<SupportHistoryView>();
            GetGeneralResponse<SupportView> supports = new GetGeneralResponse<SupportView>();

            supports = _supportService.GetOneSupport(SupportID);

            #region Prepairing Data

            // افزودن اعزام کارشناس

            foreach (var supportInstallationDelay in supports.data.SupportInstallationDelay)
            {
                list.Add(new SupportHistoryView()
                {
                    Comment = supportInstallationDelay.Comment,
                    CreateDate = supportInstallationDelay.CreateDate,
                    EmployeeName = supportInstallationDelay.CreateEmployeeName,
                    StatusName = " اعزام کارشناس",
                    Note =
                        String.Format(" تاریخ هماهنگی نصب : {0} - تاریخ تماس بعدی :  {1}  ",
                            supportInstallationDelay.InstallDate, supportInstallationDelay.NextCallDate)

                });
            }

            foreach (var supporExpertDispatch in supports.data.SupportExpertDispatch)
            {
                list.Add(new SupportHistoryView()
                {
                    Comment = supporExpertDispatch.Comment,
                    CreateDate = supporExpertDispatch.CreateDate,
                    EmployeeName = supporExpertDispatch.CreateEmployeeName,
                    StatusName = " اعزام کارشناس",
                    Note =
                        String.Format(" تاریخ اعزام کارشناس : {0} - ساعت اعزام کارشناس {1} - کارشناس اعزامی {2} ",
                            supporExpertDispatch.DispatchDate, supporExpertDispatch.DispatchTime,
                            supporExpertDispatch.ExpertEmployeeName)

                });
            }

            foreach (var supportDeliverService in supports.data.SupportDeliverServices)
            {
                // تحویل سرویس
                list.Add(new SupportHistoryView()
                {
                    Comment = supportDeliverService.Comment,
                    CreateDate = supportDeliverService.CreateDate,
                    EmployeeName = supportDeliverService.CreateEmployeeName,
                    StatusName = " تحویل سرویس",
                    Note =
                        String.Format("مبلغ دریافتی {0} - تاریخ تحویل {1} - زمان ورود {2} - زمان خروج {3} - ",
                            supportDeliverService.AmountRecived,
                            supportDeliverService.DeliverDate,
                            supportDeliverService.TimeInput,
                            supportDeliverService.TimeOutput)

                });
            }
            //نصب تلفنی
            foreach (var supportPhoneInstallation in supports.data.SupportPhoneInstallation)
            {
                list.Add(new SupportHistoryView()
                {
                    Comment = supportPhoneInstallation.Comment,
                    CreateDate = supportPhoneInstallation.CreateDate,
                    EmployeeName = supportPhoneInstallation.CreateEmployeeName,
                    StatusName = " نصب تلفنی",
                    Note =
                        String.Format("تاریخ نصب {0} - نصب شده : {1} - ",
                            supportPhoneInstallation.InstallDate,
                            supportPhoneInstallation.Installed)
                });
            }

            foreach (var supportQc in supports.data.SupportQc)
            {
                list.Add(new SupportHistoryView()
                {
                    Comment = supportQc.Comment,
                    CreateDate = supportQc.CreateDate,
                    EmployeeName = supportQc.CreateEmployeeName,
                    StatusName = " فرم QC",
                    Note =
                        String.Format(
                            "رفتار کارشناس : {0} - پوشش کارشناس : {1} - فروش و خدمات :{2} - زمان ورود {3} -  زمان خروج {4} - ",
                            supportQc.ExpertBehavior, supportQc.ExpertCover,
                            supportQc.SaleAndService, supportQc.InputTime, supportQc.OutputTime
                            )


                });
            }

            foreach (var supportTicketWaiting in supports.data.SupportTicketWaiting)
            {
                list.Add(new SupportHistoryView()
                {
                    Comment = supportTicketWaiting.Comment,
                    CreateDate = supportTicketWaiting.CreateDate,
                    EmployeeName = supportTicketWaiting.CreateEmployeeName,
                    StatusName = " ارسال تیکت",
                    Note =
                        String.Format(
                            "تاریخ اعزام کارشناس {0} - نام کارشناس {1} - طول خط {2} - اس ان آر {3} - چک سر خط {4} - عنوان تیکت {5} - رنگ سیم {6}",
                            supportTicketWaiting.DateOfPersenceDate,
                            supportTicketWaiting.InstallExpertName,
                            supportTicketWaiting.Selt,
                            supportTicketWaiting.Snr, supportTicketWaiting.SourceWireCheck,
                            supportTicketWaiting.TicketSubject,
                            supportTicketWaiting.WireColor
                            )
                });
            }

            foreach (var supportTicketWaitingResponse in supports.data.SupportTicketWaitingResponse)
            {
                list.Add(new SupportHistoryView()
                {
                    Comment = supportTicketWaitingResponse.Comment,
                    CreateDate = supportTicketWaitingResponse.CreateDate,
                    EmployeeName = supportTicketWaitingResponse.CreateEmployeeName,
                    StatusName = "انتظار پاسخ تیکت",
                    Note = String.Format("تاریخ احتمال پاسخ {0} - تاریخ ارسال تیکت {1} - شماره تیکت {2}",
                        supportTicketWaitingResponse.ResponsePossibilityDate,
                        supportTicketWaitingResponse.SendTicketDate,
                        supportTicketWaitingResponse.TicketNumber
                        )
                });
            }

            #endregion

            response.data = list.OrderByDescending(x => x.CreateDate);
            response.totalCount = list.Count();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get Welcome Bonus and Comission

        public JsonResult EmployeeSlideShow()
        {
            GetGeneralResponse<IEnumerable<SlideShowEmployeeView>> response =
                new GetGeneralResponse<IEnumerable<SlideShowEmployeeView>>();
            GetGeneralResponse<IEnumerable<SlideShowEmployeeView>> slideShowEmployee =
                new GetGeneralResponse<IEnumerable<SlideShowEmployeeView>>();
            IList<SlideShowEmployeeView> list = new List<SlideShowEmployeeView>();
            response = _bonusComissionService.GetTodayBonusComissionSimple();
            slideShowEmployee.data = response.data;
            return Json(slideShowEmployee, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetWelcomeBonusComission()
        {
            GetGeneralResponse<IEnumerable<BonusComissionView>> response =
                new GetGeneralResponse<IEnumerable<BonusComissionView>>();


            //Guid CurrentEmployeeID = GetEmployee().ID;

            response = _bonusComissionService.GetTodayBonusComission();
            //response.data = response.data.Where(x => x.Bonus != 0 || x.Comission != 0).ToList();
            int b = 10;
            //IList<BonusComissionReportView> data = new List<BonusComissionReportView>();
            //IEnumerable<BonusComissionView> ToDay = response.data.Where(x =>x.CreateDate.Substring(0,10) == PersianDateTime.Now.Substring(0,10)).ToList();
            //response.data = response.data.OrderByDescending(x => x.CreateDate).ToList();
            IEnumerable<BonusComissionView> ToDay =
            response.data.Where(
                x => x.ActionDate.Substring(0, 10).CompareTo(PersianDateTime.Now.Substring(0, 10)) == 0).ToList();

            IEnumerable<BonusComissionView> Yesterday =
                response.data.Where(
                    x => x.ActionDate.Substring(0, 10).CompareTo(PersianDateTime.yesterday.Substring(0, 10)) == 0)
                    .Where(x => x.HasCourier)
                    .ToList();
            IEnumerable<BonusComissionView> CurrentWeek =
                response.data
                    .Where(
                        x => x.ActionDate.Substring(0, 10).CompareTo(PersianDateTime.WeekStartDate.Substring(0, 10)) >= 0)
                    .Where(
                        x => x.ActionDate.Substring(0, 10).CompareTo(PersianDateTime.WeekEndDate.Substring(0, 10)) <= 0).Where(x => x.HasCourier)
                    .ToList();
            IEnumerable<BonusComissionView> CurrentMonth =
                response.data
                    .Where(
                        x =>
                            x.ActionDate.Substring(0, 10).CompareTo(PersianDateTime.MonthStartDate.Substring(0, 10)) >= 0)
                    .Where(
                        x => x.ActionDate.Substring(0, 10).CompareTo(PersianDateTime.MonthEndtDate.Substring(0, 10)) <= 0).Where(x => x.HasCourier)
                    .ToList();
            IEnumerable<BonusComissionView> LasttMonth =
                response.data
                    .Where(
                        x =>
                            x.ActionDate.Substring(0, 10).CompareTo(PersianDateTime.LastMonthStartDate.Substring(0, 10)) >=
                            0)
                    .Where(
                        x =>
                            x.ActionDate.Substring(0, 10).CompareTo(PersianDateTime.LastMonthEndtDate.Substring(0, 10)) <=
                            0).Where(x => x.HasCourier)
                    .ToList();
            BonusComissionReportView bonusComissionReportView = new BonusComissionReportView();

            IList<BonusComissionView> list = new List<BonusComissionView>();



            ///ماه قبل
            if (LasttMonth != null)
                if (LasttMonth.Any())
                {
                    foreach (var item in LasttMonth)
                    {
                        BonusComissionView _item = new BonusComissionView();
                        _item.CreateEmployeeID = item.CreateEmployeeID;
                        _item.CreateEmployeeName = item.CreateEmployeeName;
                        if (item.HasCourier)
                            _item.Bonus = item.Bonus;
                        else
                            _item.Bonus = 0;
                        _item.Comission = item.Comission;
                        _item.HasCourier = item.HasCourier;
                        _item.CreditSaleDetailName = item.CreditSaleDetailName;
                        _item.CustomerName = item.CustomerName;
                        _item.ID = item.ID;
                        _item.ActionDate = item.ActionDate;
                        _item.IsRollback = item.IsRollback;
                        _item.ModifiedDate = item.ModifiedDate;
                        _item.ModifiedEmployeeName = item.ModifiedEmployeeName;
                        _item.ProductSaleDetailName = item.ProductSaleDetailName;
                        _item.RowVersion = item.RowVersion;
                        _item.UnCreditSaleDetailName = item.UnCreditSaleDetailName;
                        _item.picture = item.picture;
                        if (item.Type == "U")
                            _item.Type = "خدمات غیر اعتباری";
                        if (item.Type == "C")
                            _item.Type = "خدمات اعتباری";
                        if (item.Type == "P")
                            _item.Type = "کالا";

                        _item.Whene = "ماه گذشته";
                        list.Add(_item);
                    }

                }


            ///ماه جاری
            if (CurrentMonth != null)
                if (CurrentMonth.Count() > 0)
                {

                    foreach (var item in CurrentMonth)
                    {
                        BonusComissionView _item = new BonusComissionView();
                        _item.CreateEmployeeID = item.CreateEmployeeID;
                        _item.CreateEmployeeName = item.CreateEmployeeName;
                        if (item.HasCourier)
                            _item.Bonus = item.Bonus;
                        else
                            _item.Bonus = 0;
                        _item.Comission = item.Comission;
                        _item.HasCourier = item.HasCourier;
                        _item.CreditSaleDetailName = item.CreditSaleDetailName;
                        _item.CustomerName = item.CustomerName;
                        _item.ActionDate = item.ActionDate;
                        _item.ID = item.ID;
                        _item.IsRollback = item.IsRollback;
                        _item.ModifiedDate = item.ModifiedDate;
                        _item.ModifiedEmployeeName = item.ModifiedEmployeeName;
                        _item.ProductSaleDetailName = item.ProductSaleDetailName;
                        _item.RowVersion = item.RowVersion;
                        _item.UnCreditSaleDetailName = item.UnCreditSaleDetailName;
                        _item.picture = item.picture;
                        if (item.Type == "U")
                            _item.Type = "خدمات غیر اعتباری";
                        if (item.Type == "C")
                            _item.Type = "خدمات اعتباری";
                        if (item.Type == "P")
                            _item.Type = "کالا";
                        _item.Whene = "ماه جاری";
                        list.Add(_item);

                    }

                }

            ///هقته جار
            if (CurrentWeek != null)
                if (CurrentWeek.Count() > 0)
                {
                    foreach (var item in CurrentWeek)
                    {
                        BonusComissionView _item = new BonusComissionView();
                        _item.CreateEmployeeID = item.CreateEmployeeID;
                        _item.CreateEmployeeName = item.CreateEmployeeName;
                        if (item.HasCourier)
                            _item.Bonus = item.Bonus;
                        else
                            _item.Bonus = 0;
                        _item.Comission = item.Comission;
                        _item.HasCourier = item.HasCourier;
                        _item.CreditSaleDetailName = item.CreditSaleDetailName;
                        _item.CustomerName = item.CustomerName;
                        _item.ActionDate = item.ActionDate;
                        _item.ID = item.ID;
                        _item.IsRollback = item.IsRollback;
                        _item.ModifiedDate = item.ModifiedDate;
                        _item.ModifiedEmployeeName = item.ModifiedEmployeeName;
                        _item.ProductSaleDetailName = item.ProductSaleDetailName;
                        _item.RowVersion = item.RowVersion;
                        _item.Type = item.Type;
                        _item.UnCreditSaleDetailName = item.UnCreditSaleDetailName;
                        _item.picture = item.picture;
                        if (item.Type == "U")
                            _item.Type = "خدمات غیر اعتباری";
                        if (item.Type == "C")
                            _item.Type = "خدمات اعتباری";
                        if (item.Type == "P")
                            _item.Type = "کالا";
                        _item.Whene = "هفته گذشته";
                        list.Add(_item);
                    }

                }

            ///دیروز
            if (Yesterday != null)
                if (Yesterday.Count() > 0)
                {

                    #region Temp

                    //BonusComissionDetail BonusTemp = new BonusComissionDetail();
                    //BonusComissionDetail ComissionTemp = new BonusComissionDetail();


                    //BonusTemp.CurrentEmployeeBC = Yesterday.Where(x => x.CreateEmployeeID == CurrentEmployeeID).Sum(x => x.Bonus);
                    //BonusTemp.BestEmployeeBC = Yesterday.GroupBy(x => x.CreateEmployeeID).Max(x => x.Sum(s => s.Bonus));
                    //BonusTemp.SumBC = Yesterday.Sum(x => x.Bonus);

                    //ComissionTemp.CurrentEmployeeBC = Yesterday.Where(x => x.CreateEmployeeID == CurrentEmployeeID).Sum(x => x.Comission);
                    //ComissionTemp.BestEmployeeBC = Yesterday.GroupBy(x => x.CreateEmployeeID).Max(x => x.Sum(s => s.Comission));
                    //ComissionTemp.SumBC = Yesterday.Sum(x => x.Comission);

                    //if (BonusTemp != null)
                    //    bonusComissionReportView.Bonus.Today = BonusTemp;
                    //if (ComissionTemp != null)
                    //    bonusComissionReportView.Comission.Today = ComissionTemp;



                    //list.Add(new BonusComissionWelcome()
                    //{
                    //    How = "کاربر جاری",
                    //    Period = "دیروز",
                    //    Bonus = BonusTemp.CurrentEmployeeBC,
                    //    Comission = ComissionTemp.CurrentEmployeeBC,
                    //});

                    //list.Add(new BonusComissionWelcome()
                    //{
                    //    How = "کارمند برتر",
                    //    Period = "دیروز",
                    //    Bonus = BonusTemp.BestEmployeeBC,
                    //    Comission = ComissionTemp.BestEmployeeBC,
                    //});

                    //list.Add(new BonusComissionWelcome()
                    //{
                    //    How = "مجموع",
                    //    Period = "دیروز",
                    //    Bonus = BonusTemp.SumBC,
                    //    Comission = ComissionTemp.SumBC,
                    //});

                    #endregion

                    foreach (var item in Yesterday)
                    {

                        BonusComissionView _item = new BonusComissionView();
                        _item.CreateEmployeeID = item.CreateEmployeeID;
                        _item.CreateEmployeeName = item.CreateEmployeeName;
                        if (item.HasCourier)
                            _item.Bonus = item.Bonus;
                        else
                            _item.Bonus = 0;
                        _item.Comission = item.Comission;
                        _item.HasCourier = item.HasCourier;
                        _item.CreditSaleDetailName = item.CreditSaleDetailName;
                        _item.CustomerName = item.CustomerName;
                        _item.ID = item.ID;
                        _item.ActionDate = item.ActionDate;
                        _item.IsRollback = item.IsRollback;
                        _item.ModifiedDate = item.ModifiedDate;
                        _item.ModifiedEmployeeName = item.ModifiedEmployeeName;
                        _item.ProductSaleDetailName = item.ProductSaleDetailName;
                        _item.RowVersion = item.RowVersion;
                        _item.Type = item.Type;
                        _item.UnCreditSaleDetailName = item.UnCreditSaleDetailName;
                        _item.picture = item.picture;
                        if (item.Type == "U")
                            _item.Type = "خدمات غیر اعتباری";
                        if (item.Type == "C")
                            _item.Type = "خدمات اعتباری";
                        if (item.Type == "P")
                            _item.Type = "کالا";
                        _item.Whene = "دیروز";
                        list.Add(_item);
                    }
                }

            /// امروز
            if (ToDay != null)
                if (ToDay.Count() > 0)
                {
                    #region temp

                    //BonusComissionDetail BonusTemp = new BonusComissionDetail();
                    //BonusComissionDetail ComissionTemp = new BonusComissionDetail();

                    //BonusTemp.CurrentEmployeeBC = ToDay.Where(x => x.CreateEmployeeID == CurrentEmployeeID).Sum(x => x.Bonus);
                    //BonusTemp.BestEmployeeBC = ToDay.GroupBy(x => x.CreateEmployeeID).Max(x => x.Sum(s => s.Bonus));
                    //BonusTemp.SumBC = ToDay.Sum(x => x.Bonus);

                    //ComissionTemp.CurrentEmployeeBC =
                    //    ToDay.Where(x => x.CreateEmployeeID == CurrentEmployeeID).Sum(x => x.Comission);
                    //ComissionTemp.BestEmployeeBC = ToDay.GroupBy(x => x.CreateEmployeeID).Max(x => x.Sum(s => s.Comission));
                    //ComissionTemp.SumBC = ToDay.Sum(x => x.Comission);

                    //if (BonusTemp != null)
                    //    bonusComissionReportView.Bonus.Today = BonusTemp;
                    //if (ComissionTemp != null)
                    //    bonusComissionReportView.Comission.Today = ComissionTemp;

                    //list.Add(new BonusComissionWelcome() {
                    //    How = BonusTemp.CurrentEmployeeBCName,
                    //    Period="امروز",
                    //    Bonus = BonusTemp.CurrentEmployeeBC,
                    //    Comission = ComissionTemp.CurrentEmployeeBC,
                    //});

                    //list.Add(new BonusComissionWelcome()
                    //{
                    //    How = "کارمند برتر",
                    //    Period = "امروز",
                    //    Bonus = BonusTemp.BestEmployeeBC,
                    //    Comission = ComissionTemp.BestEmployeeBC,
                    //});

                    //list.Add(new BonusComissionWelcome()
                    //{
                    //    How = "مجموع",
                    //    Period = "امروز",
                    //    Bonus = BonusTemp.SumBC,
                    //    Comission = ComissionTemp.SumBC,
                    //});

                    #endregion

                    foreach (var item in ToDay)
                    {

                        BonusComissionView _item = new BonusComissionView();
                        _item.CreateEmployeeID = item.CreateEmployeeID;
                        _item.CreateEmployeeName = item.CreateEmployeeName;
                        _item.Bonus = item.Bonus;
                        _item.Comission = item.Comission;
                        _item.HasCourier = item.HasCourier;
                        _item.CreditSaleDetailName = item.CreditSaleDetailName;
                        _item.CustomerName = item.CustomerName;
                        _item.ID = item.ID;
                        _item.ActionDate = item.ActionDate;
                        _item.IsRollback = item.IsRollback;
                        _item.ModifiedDate = item.ModifiedDate;
                        _item.ModifiedEmployeeName = item.ModifiedEmployeeName;
                        _item.ProductSaleDetailName = item.ProductSaleDetailName;
                        _item.RowVersion = item.RowVersion;
                        _item.Type = item.Type;
                        _item.UnCreditSaleDetailName = item.UnCreditSaleDetailName;
                        _item.picture = item.picture;
                        if (item.Type == "U")
                            _item.Type = "خدمات غیر اعتباری";
                        if (item.Type == "C")
                            _item.Type = "خدمات اعتباری";
                        if (item.Type == "P")
                            _item.Type = "کالا";
                        _item.Whene = "امروز";
                        list.Add(_item);
                    }


                }


            //ToDay.data=data.ToList();

            var serializer = new JavaScriptSerializer();

            // For simplicity just use Int32's max value.
            // You could always read the value from the config section mentioned above.
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = list;
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };




            return result;
        }


        #endregion

        #region Get bonus and comission report

        public JsonResult GetBonusReport(int? pageSize, int? pageNumber, GetBonusComissionReportRequest request,
            string sort, string ExportTo)
        {
            GetGeneralResponse<IEnumerable<BonusMasterReportView>> response =
                new GetGeneralResponse<IEnumerable<BonusMasterReportView>>();

            #region Access Check

            //bool hasPermission = GetEmployee().IsGuaranteed("BonusReport_Read");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}

            #endregion

            #region Preparing Filter

            IList<FilterData> filter = new List<FilterData>();

            IList<FilterData> Productfilter = new List<FilterData>();
            IList<FilterData> Creditfilter = new List<FilterData>();
            IList<FilterData> UnCreditfilter = new List<FilterData>();


            bool pro = false;
            bool cre = false;
            bool unc = false;

            #region Get has Bonus

            FilterData FilterBonus = new FilterData()
            {
                data = new data()
                {
                    comparison = "Noteq",
                    type = "numeric",
                    value = new[] { "0" }
                },
                field = "Bonus"

            };
            Productfilter.Add(FilterBonus);
            Creditfilter.Add(FilterBonus);
            UnCreditfilter.Add(FilterBonus);

            FilterData FilterSaleClose = new FilterData()
            {
                data = new data()
                {
                    comparison = "eq",
                    type = "boolean",
                    value = new[] { bool.TrueString }
                },
                field = "Sale.Closed"

            };
            Productfilter.Add(FilterSaleClose);
            Creditfilter.Add(FilterSaleClose);
            UnCreditfilter.Add(FilterSaleClose);

            #endregion

            #region IS Product

            if (request.IsProducts == true && request.ProductIDs == null)
            {
                pro = true;
            }

            #region Product & Product prices

            if (request.ProductPriceIDs != null)
            {
                pro = true;
                if (request.ProductPriceIDs != null)
                    if (request.ProductPriceIDs.Count() > 0)
                    {
                        IList<string> Ids = new List<string>();
                        foreach (var item in request.ProductPriceIDs)
                        {
                            Ids.Add(item.ToString());
                        }


                        Productfilter.Add(new FilterData()
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "list",
                                value = Ids.ToArray()
                            },
                            field = "ProductPrice.ID"

                        });
                    }
            }
            if (request.ProductIDs != null && request.ProductPriceIDs == null)
            {
                pro = true;
                if (request.ProductIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in request.ProductIDs)
                    {
                        Ids.Add(item.ToString());
                    }

                    Productfilter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "list",
                            value = Ids.ToArray()
                        },
                        field = "ProductPrice.Product.ID"

                    });
                }
            }

            #endregion

            #endregion

            #region UncreditService

            if (request.IsUnCreditService == true && request.UncreditServiceIDs == null)
            {
                unc = true;

            }

            #region Uncredit Sale Detail

            if (request.UncreditServiceIDs != null)
            {
                unc = true;
                if (request.UncreditServiceIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in request.UncreditServiceIDs)
                    {
                        Ids.Add(item.ToString());
                    }


                    UnCreditfilter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "list",
                            value = Ids.ToArray()
                        },
                        field = "UnCreditService.ID"

                    });
                }
            }

            #endregion

            #endregion

            #region CreditService

            if (request.IsCreditService == true && request.CreditServiceIDs == null)
            {
                cre = true;

            }

            #region creditService & Network

            if (request.CreditServiceIDs != null)
            {
                cre = true;
                if (request.CreditServiceIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in request.CreditServiceIDs)
                    {
                        Ids.Add(item.ToString());
                    }


                    Creditfilter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "list",
                            value = Ids.ToArray()
                        },
                        field = "CreditService.ID"

                    });
                }
            }
            else if (request.NetworkIDs != null && request.CreditServiceIDs == null)
            {
                cre = true;
                if (request.CreditServiceIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in request.NetworkIDs)
                    {
                        Ids.Add(item.ToString());
                    }

                    Creditfilter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "list",
                            value = Ids.ToArray()
                        },
                        field = "CreditService.Network.ID"

                    });
                }
            }

            #endregion

            #endregion

            #region Deliver Date


            if (request.SaleStartDate != null && request.SaleEndDate == null)
            {
                FilterData Filter = new FilterData()
                {
                    field = "Sale.CloseDate",
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "dateOnly",
                        value = new[] { request.SaleStartDate }
                    }
                };
                Productfilter.Add(Filter);

                FilterData Filter1 = new FilterData()
                {
                    field = "Sale.CloseDate",
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "dateOnly",
                        value = new[] { request.SaleStartDate }
                    }
                };
                UnCreditfilter.Add(Filter1);

                FilterData Filter2 = new FilterData()
                {
                    field = "Sale.CloseDate",
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "dateOnly",
                        value = new[] { request.SaleStartDate }
                    }
                };
                Creditfilter.Add(Filter2);
            }

            if (request.SaleEndDate != null && request.SaleStartDate == null)
            {
                FilterData Filter = new FilterData()
                {
                    field = "Sale.CloseDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateOnly",
                        value = new[] { request.SaleEndDate }
                    }
                };
                Productfilter.Add(Filter);

                FilterData Filter1 = new FilterData()
                {
                    field = "Sale.CloseDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateOnly",
                        value = new[] { request.SaleEndDate }
                    }
                };
                UnCreditfilter.Add(Filter1);

                FilterData Filter2 = new FilterData()
                {
                    field = "Sale.CloseDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateOnly",
                        value = new[] { request.SaleEndDate }
                    }
                };
                Creditfilter.Add(Filter2);
            }

            if (request.SaleStartDate != null && request.SaleEndDate != null)
            {
                FilterData Filter = new FilterData()
                {
                    field = "Sale.CloseDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateBetween",
                        value = new[] { request.SaleStartDate, request.SaleEndDate }
                    }

                };
                Productfilter.Add(Filter);

                FilterData Filter1 = new FilterData()
                {
                    field = "Sale.CloseDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateBetween",
                        value = new[] { request.SaleStartDate, request.SaleEndDate }
                    }

                };
                UnCreditfilter.Add(Filter1);

                FilterData Filter2 = new FilterData()
                {
                    field = "Sale.CloseDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateBetween",
                        value = new[] { request.SaleStartDate, request.SaleEndDate }
                    }

                };
                Creditfilter.Add(Filter2);
            }


            #endregion

            #region Sale Date

            if (request.HasCourier == 3)
            {
                Productfilter.Add(new FilterData()
                {
                    field = "Sale.HasCourier",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { bool.FalseString }
                    }
                });
                UnCreditfilter.Add(new FilterData()
                {
                    field = "Sale.HasCourier",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { bool.FalseString }
                    }
                });
                Creditfilter.Add(new FilterData()
                {
                    field = "Sale.HasCourier",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { bool.FalseString }
                    }
                });

            }
            if (request.HasCourier == 2)
            {
                Productfilter.Add(new FilterData()
                {
                    field = "Sale.HasCourier",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { bool.TrueString }
                    }
                });
                UnCreditfilter.Add(new FilterData()
                {
                    field = "Sale.HasCourier",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { bool.TrueString }
                    }
                });
                Creditfilter.Add(new FilterData()
                {
                    field = "Sale.HasCourier",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { bool.TrueString }
                    }
                });
                if (request.CourierConfirmedStartDate != null && request.CourierConfirmedEndDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { request.CourierConfirmedStartDate + " 00:00:00" }
                        }
                    };
                    Productfilter.Add(Filter);

                    FilterData Filter1 = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { request.CourierConfirmedStartDate }
                        }
                    };
                    UnCreditfilter.Add(Filter1);

                    FilterData Filter2 = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { request.CourierConfirmedStartDate }
                        }
                    };
                    Creditfilter.Add(Filter2);
                }

                if (request.CourierConfirmedEndDate != null && request.CourierConfirmedStartDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { request.CourierConfirmedEndDate }
                        }
                    };
                    Productfilter.Add(Filter);

                    FilterData Filter1 = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { request.CourierConfirmedEndDate }
                        }
                    };
                    UnCreditfilter.Add(Filter1);

                    FilterData Filter2 = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { request.CourierConfirmedEndDate }
                        }
                    };
                    Creditfilter.Add(Filter2);
                }

                if (request.CourierConfirmedStartDate != null && request.CourierConfirmedEndDate != null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { request.CourierConfirmedStartDate, request.CourierConfirmedEndDate }
                        }

                    };
                    Productfilter.Add(Filter);

                    FilterData Filter1 = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { request.CourierConfirmedStartDate, request.CourierConfirmedEndDate }
                        }

                    };
                    UnCreditfilter.Add(Filter1);

                    FilterData Filter2 = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { request.CourierConfirmedStartDate, request.CourierConfirmedEndDate }
                        }

                    };
                    Creditfilter.Add(Filter2);
                }

            }

            #endregion




            #endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _bonusComissionService.GetBonusReport(PageSize, PageNumber, filter, Creditfilter, UnCreditfilter,
                Productfilter, ConvertJsonToObject(sort), pro, cre, unc);

            if (ExportTo == "Excel")
            {
                GridView gridView = new GridView();
                gridView.DataSource = response.data;

                gridView.DataBind();


                gridView.HeaderRow.Cells[0].Text = "کارمند فروش";
                gridView.HeaderRow.Cells[1].Text = "امتیاز محصولات";
                gridView.HeaderRow.Cells[2].Text = "امتیاز خدمات اعتباری ";
                gridView.HeaderRow.Cells[3].Text = "امتیاز خدمات غیر اعتباری ";



                gridView.Font.Names = new[] { "Tahoma" };

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
                Response.ContentType = "application/ms-excel";

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmltextwriter = new HtmlTextWriter(stringWriter);

                gridView.RenderControl(htmltextwriter);

                Response.Output.Write(stringWriter.ToString());
                Response.Flush();
                Response.End();

            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComissionReport(int? pageSize, int? pageNumber, GetBonusComissionReportRequest request,
            string sort, string ExportTo)
        {
            GetGeneralResponse<IEnumerable<ComissionMasterReportView>> response =
                new GetGeneralResponse<IEnumerable<ComissionMasterReportView>>();

            #region Access Check

            //bool hasPermission = GetEmployee().IsGuaranteed("BonusReport_Read");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}

            #endregion

            #region Preparing Filter

            IList<FilterData> filter = new List<FilterData>();

            IList<FilterData> Productfilter = new List<FilterData>();
            IList<FilterData> Creditfilter = new List<FilterData>();
            IList<FilterData> UnCreditfilter = new List<FilterData>();


            bool pro = false;
            bool cre = false;
            bool unc = false;

            #region Get has Bonus

            FilterData FilterBonus = new FilterData()
            {
                data = new data()
                {
                    comparison = "Noteq",
                    type = "numeric",
                    value = new[] { "0" }
                },
                field = "Comission"

            };
            Productfilter.Add(FilterBonus);
            Creditfilter.Add(FilterBonus);
            UnCreditfilter.Add(FilterBonus);

            FilterData FilterSaleClose = new FilterData()
            {
                data = new data()
                {
                    comparison = "eq",
                    type = "boolean",
                    value = new[] { bool.TrueString }
                },
                field = "Sale.Closed"

            };
            Productfilter.Add(FilterSaleClose);
            Creditfilter.Add(FilterSaleClose);
            UnCreditfilter.Add(FilterSaleClose);

            #endregion


            #region IS Product

            if (request.IsProducts == true && request.ProductIDs == null)
            {
                pro = true;
            }

            #region Product & Product prices

            if (request.ProductPriceIDs != null)
            {
                pro = true;
                if (request.ProductPriceIDs != null)
                    if (request.ProductPriceIDs.Count() > 0)
                    {
                        IList<string> Ids = new List<string>();
                        foreach (var item in request.ProductPriceIDs)
                        {
                            Ids.Add(item.ToString());
                        }


                        Productfilter.Add(new FilterData()
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "list",
                                value = Ids.ToArray()
                            },
                            field = "ProductPrice.ID"

                        });
                    }
            }
            if (request.ProductIDs != null && request.ProductPriceIDs == null)
            {
                pro = true;
                if (request.ProductIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in request.ProductIDs)
                    {
                        Ids.Add(item.ToString());
                    }

                    Productfilter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "list",
                            value = Ids.ToArray()
                        },
                        field = "ProductPrice.Product.ID"

                    });
                }
            }

            #endregion

            #endregion

            #region UncreditService

            if (request.IsUnCreditService == true && request.UncreditServiceIDs == null)
            {
                unc = true;

            }

            #region Uncredit Sale Detail

            if (request.UncreditServiceIDs != null)
            {
                unc = true;
                if (request.UncreditServiceIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in request.UncreditServiceIDs)
                    {
                        Ids.Add(item.ToString());
                    }


                    UnCreditfilter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "list",
                            value = Ids.ToArray()
                        },
                        field = "UnCreditService.ID"

                    });
                }
            }

            #endregion

            #endregion

            #region CreditService

            if (request.IsCreditService == true && request.CreditServiceIDs == null)
            {
                cre = true;

            }

            #region creditService & Network

            if (request.CreditServiceIDs != null)
            {
                cre = true;
                if (request.CreditServiceIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in request.CreditServiceIDs)
                    {
                        Ids.Add(item.ToString());
                    }


                    Creditfilter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "list",
                            value = Ids.ToArray()
                        },
                        field = "CreditService.ID"

                    });
                }
            }
            else if (request.NetworkIDs != null && request.CreditServiceIDs == null)
            {
                cre = true;
                if (request.CreditServiceIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in request.NetworkIDs)
                    {
                        Ids.Add(item.ToString());
                    }

                    Creditfilter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "list",
                            value = Ids.ToArray()
                        },
                        field = "CreditService.Network.ID"

                    });
                }
            }

            #endregion

            #endregion

            #region Deliver Date


            if (request.SaleStartDate != null && request.SaleEndDate == null)
            {
                FilterData Filter = new FilterData()
                {
                    field = "ComissionDate",
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "dateOnly",
                        value = new[] { request.SaleStartDate }
                    }
                };
                Productfilter.Add(Filter);

                FilterData Filter1 = new FilterData()
                {
                    field = "ComissionDate",
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "dateOnly",
                        value = new[] { request.SaleStartDate }
                    }
                };
                UnCreditfilter.Add(Filter1);

                FilterData Filter2 = new FilterData()
                {
                    field = "ComissionDate",
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "dateOnly",
                        value = new[] { request.SaleStartDate }
                    }
                };
                Creditfilter.Add(Filter2);
            }

            if (request.SaleEndDate != null && request.SaleStartDate == null)
            {
                FilterData Filter = new FilterData()
                {
                    field = "ComissionDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateOnly",
                        value = new[] { request.SaleEndDate }
                    }
                };
                Productfilter.Add(Filter);

                FilterData Filter1 = new FilterData()
                {
                    field = "ComissionDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateOnly",
                        value = new[] { request.SaleEndDate }
                    }
                };
                UnCreditfilter.Add(Filter1);

                FilterData Filter2 = new FilterData()
                {
                    field = "Sale.CloseDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateOnly",
                        value = new[] { request.SaleEndDate }
                    }
                };
                Creditfilter.Add(Filter2);
            }

            if (request.SaleStartDate != null && request.SaleEndDate != null)
            {
                FilterData Filter = new FilterData()
                {
                    field = "ComissionDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateBetween",
                        value = new[] { request.SaleStartDate, request.SaleEndDate }
                    }

                };
                Productfilter.Add(Filter);

                FilterData Filter1 = new FilterData()
                {
                    field = "ComissionDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateBetween",
                        value = new[] { request.SaleStartDate, request.SaleEndDate }
                    }

                };
                UnCreditfilter.Add(Filter1);

                FilterData Filter2 = new FilterData()
                {
                    field = "ComissionDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateBetween",
                        value = new[] { request.SaleStartDate, request.SaleEndDate }
                    }

                };
                Creditfilter.Add(Filter2);
            }


            #endregion

            #region Sale Date

            if (request.HasCourier == 2)
            {
                Productfilter.Add(new FilterData()
                {
                    field = "Sale.HasCourier",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { bool.FalseString }
                    }
                });
                UnCreditfilter.Add(new FilterData()
                {
                    field = "Sale.HasCourier",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { bool.FalseString }
                    }
                });
                Creditfilter.Add(new FilterData()
                {
                    field = "Sale.HasCourier",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { bool.FalseString }
                    }
                });

            }
            if (request.HasCourier == 3)
            {
                Productfilter.Add(new FilterData()
                {
                    field = "Sale.HasCourier",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { bool.TrueString }
                    }
                });
                UnCreditfilter.Add(new FilterData()
                {
                    field = "Sale.HasCourier",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { bool.TrueString }
                    }
                });
                Creditfilter.Add(new FilterData()
                {
                    field = "Sale.HasCourier",
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { bool.TrueString }
                    }
                });
                if (request.CourierConfirmedStartDate != null && request.CourierConfirmedEndDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { request.CourierConfirmedStartDate + " 00:00:00" }
                        }
                    };
                    Productfilter.Add(Filter);

                    FilterData Filter1 = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { request.CourierConfirmedStartDate }
                        }
                    };
                    UnCreditfilter.Add(Filter1);

                    FilterData Filter2 = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "gteq",
                            type = "date",
                            value = new[] { request.CourierConfirmedStartDate }
                        }
                    };
                    Creditfilter.Add(Filter2);
                }

                if (request.CourierConfirmedEndDate != null && request.CourierConfirmedStartDate == null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { request.CourierConfirmedEndDate }
                        }
                    };
                    Productfilter.Add(Filter);

                    FilterData Filter1 = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { request.CourierConfirmedEndDate }
                        }
                    };
                    UnCreditfilter.Add(Filter1);

                    FilterData Filter2 = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "date",
                            value = new[] { request.CourierConfirmedEndDate }
                        }
                    };
                    Creditfilter.Add(Filter2);
                }

                if (request.CourierConfirmedStartDate != null && request.CourierConfirmedEndDate != null)
                {
                    FilterData Filter = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { request.CourierConfirmedStartDate, request.CourierConfirmedEndDate }
                        }

                    };
                    Productfilter.Add(Filter);

                    FilterData Filter1 = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { request.CourierConfirmedStartDate, request.CourierConfirmedEndDate }
                        }

                    };
                    UnCreditfilter.Add(Filter1);

                    FilterData Filter2 = new FilterData()
                    {
                        field = "BonusDate",
                        data = new data()
                        {
                            comparison = "lteq",
                            type = "dateBetween",
                            value = new[] { request.CourierConfirmedStartDate, request.CourierConfirmedEndDate }
                        }

                    };
                    Creditfilter.Add(Filter2);
                }

            }

            #endregion


            #endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _bonusComissionService.GetComissionReport(PageSize, PageNumber, filter, Creditfilter,
                UnCreditfilter, Productfilter, ConvertJsonToObject(sort), pro, cre, unc);

            if (ExportTo == "Excel")
            {
                GridView gridView = new GridView();
                gridView.DataSource = response.data;

                gridView.DataBind();

                gridView.HeaderRow.Cells[0].Text = "کارشناس فروش";
                gridView.HeaderRow.Cells[1].Text = "پورسانت محصولات";
                gridView.HeaderRow.Cells[2].Text = "پورسانت خدمات اعتباری";
                gridView.HeaderRow.Cells[3].Text = "پورسانت خدمات غیر اعتباری";


                gridView.Font.Names = new[] { "Tahoma" };

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
                Response.ContentType = "application/ms-excel";

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmltextwriter = new HtmlTextWriter(stringWriter);

                gridView.RenderControl(htmltextwriter);

                Response.Output.Write(stringWriter.ToString());
                Response.Flush();
                Response.End();

            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get Install Form Report

        public ActionResult GetInstallForm(Guid SupportID)
        {

            Services.ViewModels.Reports.InstallFormReportView response =
                new Services.ViewModels.Reports.InstallFormReportView();

            response = _supportService.GetInstallReport(SupportID);

            return View(response);
        }

        #endregion

        #region Get Suction Mode Report

        public JsonResult GetSuctionModeReport(GetSuctionModeRequest request, string ExportTo)
        {

            GetGeneralResponse<IEnumerable<SuctionModeReportView>> response =
                new GetGeneralResponse<IEnumerable<SuctionModeReportView>>();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("SuctionModeReport_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _customerService.GetSuctionModeReport(request);

            if (ExportTo == "Excel")
            {
                GridView gridView = new GridView();
                gridView.DataSource = response.data;

                gridView.DataBind();

                gridView.HeaderRow.Cells[0].Text = "نام شیوه جذب";
                gridView.HeaderRow.Cells[1].Text = "تعداد";



                gridView.Font.Names = new[] { "Tahoma" };

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
                Response.ContentType = "application/ms-excel";

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmltextwriter = new HtmlTextWriter(stringWriter);

                gridView.RenderControl(htmltextwriter);

                Response.Output.Write(stringWriter.ToString());
                Response.Flush();
                Response.End();

            }


            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Get Suction mode  Network Report

        public JsonResult GetNetworkReport(GetSuctionModeRequest request, string ExportTo)
        {

            GetGeneralResponse<IEnumerable<GetNetworkReportView>> response =
                new GetGeneralResponse<IEnumerable<GetNetworkReportView>>();

            response = _customerService.GetNetworkReport(request);

            #region report Export To Excel

            if (ExportTo == "Excel")
            {

                IList<GetNetworkReport> getNetworkReport = new List<GetNetworkReport>();
                foreach (var item in response.data)
                {
                    getNetworkReport.Add(new GetNetworkReport()
                    {
                        NetworkName = item.NetworkName,
                        Count = item.Count,
                    });
                }
                GridView gridView = new GridView();
                gridView.DataSource = getNetworkReport;

                gridView.DataBind();

                gridView.HeaderRow.Cells[0].Text = "نام شبکه";
                gridView.HeaderRow.Cells[1].Text = "تعداد ";


                gridView.Font.Names = new[] { "Tahoma" };

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
                Response.ContentType = "application/ms-excel";

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmltextwriter = new HtmlTextWriter(stringWriter);

                gridView.RenderControl(htmltextwriter);

                Response.Output.Write(stringWriter.ToString());
                Response.Flush();
                Response.End();

            }

            #endregion


            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetCenterReport(GetSuctionModeRequest request)
        {

            GetGeneralResponse<IEnumerable<CenterReportView>> response =
                new GetGeneralResponse<IEnumerable<CenterReportView>>();

            response = _customerService.GetCenterReport(request);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Suction mode Cost

        public JsonResult GetSuctionModeCost1(string StartDate, string EndDate, IEnumerable<Guid?> SuctionModeIDs,
            IEnumerable<Guid?> SuctionModeDetailsIDs, IEnumerable<Guid?> CampaignAgentIDs, string sort, string ExportTo)
        {
            GetGeneralResponse<IEnumerable<GetSuctionModeCost1View>> response =
                new GetGeneralResponse<IEnumerable<GetSuctionModeCost1View>>();

            #region Access Check

            //bool hasPermission = GetEmployee().IsGuaranteed("SuctionModeCost_Read");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}

            #endregion


            IList<FilterData> filter = new List<FilterData>();

            #region Preparing Filter

            #region Payment Date

            if (StartDate != null && EndDate == null)
            {
                FilterData Filter = new FilterData()
                {
                    field = "PaymentDate",
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "date",
                        value = new[] { StartDate }
                    }
                };
                filter.Add(Filter);
            }

            if (EndDate != null && StartDate == null)
            {
                FilterData Filter = new FilterData()
                {
                    field = "PaymentDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "date",
                        value = new[] { EndDate }
                    }
                };
                filter.Add(Filter);
            }

            if (StartDate != null && EndDate != null)
            {
                FilterData Filter = new FilterData()
                {
                    field = "PaymentDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateBetween",
                        value = new[] { StartDate, EndDate }
                    }

                };
                filter.Add(Filter);
            }

            #endregion

            #region Suction mode & Suction Mode Details

            if (SuctionModeDetailsIDs != null)
            {
                if (SuctionModeDetailsIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in SuctionModeDetailsIDs)
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
            else if (SuctionModeIDs != null && SuctionModeDetailsIDs == null)
            {
                if (SuctionModeIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in SuctionModeIDs)
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
                        field = "SuctionModeDetail.SuctionMode.ID"

                    });
                }
            }

            #endregion

            #region Campaign Agent

            if (CampaignAgentIDs != null)
            {
                if (CampaignAgentIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in CampaignAgentIDs)
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
                        field = "CampaignAgent.ID"

                    });
                }
            }

            #endregion


            #endregion

            response = _campaignPaymentService.GetSuctionModeCostReport1(filter, ConvertJsonToObject(sort));


            #region report Export To Excel

            if (ExportTo == "Excel")
            {
                GridView gridView = new GridView();
                gridView.DataSource = response.data;

                gridView.DataBind();

                gridView.HeaderRow.Cells[0].Text = "تاریخ پرداخت";
                gridView.HeaderRow.Cells[1].Text = "نام عامل تبلیغاتی ";
                gridView.HeaderRow.Cells[2].Text = "شیوه جذب";
                gridView.HeaderRow.Cells[3].Text = "جزئیات روش جذب";
                gridView.HeaderRow.Cells[4].Text = "مبلغ";


                gridView.Font.Names = new[] { "Tahoma" };

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
                Response.ContentType = "application/ms-excel";

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmltextwriter = new HtmlTextWriter(stringWriter);

                gridView.RenderControl(htmltextwriter);

                Response.Output.Write(stringWriter.ToString());
                Response.Flush();
                Response.End();

            }

            #endregion

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetSuctionModeCost2(string StartDate, string EndDate, IEnumerable<Guid?> SuctionModeIDs,
            IEnumerable<Guid?> SuctionModeDetailsIDs, IEnumerable<Guid?> CampaignAgentIDs, string sort, string ExportTo)
        {
            GetGeneralResponse<IEnumerable<GetSuctionModeCost2View>> response =
                new GetGeneralResponse<IEnumerable<GetSuctionModeCost2View>>();
            IList<FilterData> filter = new List<FilterData>();

            #region Access Check

            //bool hasPermission = GetEmployee().IsGuaranteed("SuctionModeCost_Read");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}

            #endregion

            #region Preparing Filter

            #region Payment Date

            if (StartDate != null && EndDate == null)
            {
                FilterData Filter = new FilterData()
                {
                    field = "PaymentDate",
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "date",
                        value = new[] { StartDate }
                    }
                };
                filter.Add(Filter);
            }

            if (EndDate != null && StartDate == null)
            {
                FilterData Filter = new FilterData()
                {
                    field = "PaymentDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "date",
                        value = new[] { EndDate }
                    }
                };
                filter.Add(Filter);
            }

            if (StartDate != null && EndDate != null)
            {
                FilterData Filter = new FilterData()
                {
                    field = "PaymentDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateBetween",
                        value = new[] { StartDate, EndDate }
                    }

                };
                filter.Add(Filter);
            }

            #endregion

            #region Suction mode & Suction Mode Details

            if (SuctionModeDetailsIDs != null)
            {
                if (SuctionModeDetailsIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in SuctionModeDetailsIDs)
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
            else if (SuctionModeIDs != null && SuctionModeDetailsIDs == null)
            {
                if (SuctionModeIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in SuctionModeIDs)
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
                        field = "SuctionModeDetail.SuctionMode.ID"

                    });
                }
            }

            #endregion

            #region Campaign Agent

            if (CampaignAgentIDs != null)
            {
                if (CampaignAgentIDs.Count() > 0)
                {
                    IList<string> Ids = new List<string>();
                    foreach (var item in CampaignAgentIDs)
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
                        field = "CampaignAgent.ID"

                    });
                }
            }

            #endregion


            #endregion

            response = _campaignPaymentService.GetSuctionModeCostReport2(filter, ConvertJsonToObject(sort));


            var list = response.data.GroupBy(x => x.CampaignAgentName).Select(c => new GetSuctionModeCost2View()
            {
                Amount = c.Sum(x => x.Amount),
                CampaignAgentName = c.Key,

            });

            response.data = list;

            #region report Export To Excel

            if (ExportTo == "Excel")
            {
                IList<GetSuctionModeCost2> getSuctionModeCost3 = new List<GetSuctionModeCost2>();

                foreach (var item in response.data)
                {
                    getSuctionModeCost3.Add(new GetSuctionModeCost2()
                    {
                        CampaignAgentName = item.CampaignAgentName,
                        Amount = item.Amount
                    });
                }
                GridView gridView = new GridView();
                gridView.DataSource = getSuctionModeCost3;

                gridView.DataBind();

                gridView.HeaderRow.Cells[0].Text = "نام عامل تبلیغاتی";
                gridView.HeaderRow.Cells[1].Text = "مبلغ";

                gridView.Font.Names = new[] { "Tahoma" };

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
                Response.ContentType = "application/ms-excel";

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmltextwriter = new HtmlTextWriter(stringWriter);

                gridView.RenderControl(htmltextwriter);

                Response.Output.Write(stringWriter.ToString());
                Response.Flush();
                Response.End();

            }

            #endregion


            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #region Sction3

        public JsonResult GetSuctionModeCost3(GetsuctionModeRequestForReport3 request, string ExportTo)
        {
            GetGeneralResponse<IEnumerable<SuctionModeCost>> data =
                new GetGeneralResponse<IEnumerable<SuctionModeCost>>();
            GetGeneralResponse<IEnumerable<GetSuctionModeCost3View>> response =
                new GetGeneralResponse<IEnumerable<GetSuctionModeCost3View>>();

            #region Access Check

            //bool hasPermission = GetEmployee().IsGuaranteed("SuctionModeCost_Read");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}

            #endregion

            #region Fuck

            //#region Campaign Payment

            //#region Prepairing Filters

            //IList<FilterData> filter = new List<FilterData>();


            //if (request.PaymentStartDate != null && request.PaymentEndDate == null)
            //{
            //    FilterData Filter = new FilterData()
            //    {
            //        field = "PaymentDate",
            //        data = new data()
            //        {
            //            comparison = "gteq",
            //            type = "date",
            //            value = new[] {request.PaymentStartDate}
            //        }
            //    };
            //    filter.Add(Filter);
            //}

            //if (request.PaymentEndDate != null && request.PaymentStartDate == null)
            //{
            //    FilterData Filter = new FilterData()
            //    {
            //        field = "PaymentDate",
            //        data = new data()
            //        {
            //            comparison = "lteq",
            //            type = "date",
            //            value = new[] {request.PaymentEndDate}
            //        }
            //    };
            //    filter.Add(Filter);
            //}

            //if (request.PaymentStartDate != null && request.PaymentEndDate != null)
            //{
            //    FilterData Filter = new FilterData()
            //    {
            //        field = "PaymentDate",
            //        data = new data()
            //        {
            //            comparison = "lteq",
            //            type = "dateBetween",
            //            value = new[] {request.PaymentStartDate, request.PaymentEndDate}
            //        }

            //    };
            //    filter.Add(Filter);
            //}


            //#endregion

            //#region Suction mode & Suction Mode Details

            //if (request.SuctionModeDetailsIDs != null)
            //{
            //    if (request.SuctionModeDetailsIDs.Count() > 0)
            //    {
            //        IList<string> Ids = new List<string>();
            //        foreach (var item in request.SuctionModeDetailsIDs)
            //        {
            //            Ids.Add(item.ToString());
            //        }


            //        filter.Add(new FilterData()
            //        {
            //            data = new data()
            //            {
            //                comparison = "eq",
            //                type = "list",
            //                value = Ids.ToArray()
            //            },
            //            field = "SuctionModeDetail.ID"

            //        });
            //    }
            //}

            //else if (request.SuctionModeIDs != null && request.SuctionModeDetailsIDs == null)
            //{
            //    if (request.SuctionModeIDs.Count() > 0)
            //    {
            //        IList<string> Ids = new List<string>();
            //        foreach (var item in request.SuctionModeIDs)
            //        {
            //            Ids.Add(item.ToString());
            //        }

            //        filter.Add(new FilterData()
            //        {
            //            data = new data()
            //            {
            //                comparison = "eq",
            //                type = "list",
            //                value = Ids.ToArray()
            //            },
            //            field = "SuctionModeDetail.SuctionMode.ID"

            //        });
            //    }
            //}

            //#endregion

            //GetGeneralResponse<IEnumerable<GetcampaignAgents>> campaignPayment =
            //    new GetGeneralResponse<IEnumerable<GetcampaignAgents>>();

            //campaignPayment = _campaignPaymentService.GetSuctionModeCostReport3(filter, null);



            //var query1 = from bs in campaignPayment.data
            //    group bs by bs.SuctionModeDetailID
            //    into g
            //    select new GetcampaignAgents
            //    {
            //        Amount = g.Sum(x => x.Amount),
            //        SuctionModeDetailID = g.Key,
            //        SuctionModeDetailName = g.First().SuctionModeDetailName,
            //        SuctionModeID = g.First().SuctionModeID,
            //        SuctionMoedName = g.First().SuctionMoedName,

            //    };
            //IEnumerable<GetcampaignAgents> query2 = query1.ToList();

            //#endregion

            //#region Customers

            //IList<FilterData> cusotomerFlter = new List<FilterData>();

            //#region Suction mode & Suction Mode Details



            //if (request.SuctionModeDetailsIDs != null)
            //{
            //    if (request.SuctionModeDetailsIDs.Count() > 0)
            //    {
            //        IList<string> Ids = new List<string>();
            //        foreach (var item in request.SuctionModeDetailsIDs)
            //        {
            //            Ids.Add(item.ToString());
            //        }


            //        cusotomerFlter.Add(new FilterData()
            //        {
            //            data = new data()
            //            {
            //                comparison = "eq",
            //                type = "list",
            //                value = Ids.ToArray()
            //            },
            //            field = "SuctionModeDetail.ID"

            //        });
            //    }
            //}

            //else if (request.SuctionModeIDs != null && request.SuctionModeDetailsIDs == null)
            //{
            //    if (request.SuctionModeIDs.Count() > 0)
            //    {
            //        IList<string> Ids = new List<string>();
            //        foreach (var item in request.SuctionModeIDs)
            //        {
            //            Ids.Add(item.ToString());
            //        }

            //        cusotomerFlter.Add(new FilterData()
            //        {
            //            data = new data()
            //            {
            //                comparison = "eq",
            //                type = "list",
            //                value = Ids.ToArray()
            //            },
            //            field = "SuctionModeDetail.SuctionMode.ID"

            //        });
            //    }
            //}

            //#endregion

            //#region register Date

            //if (request.RegisterStartDate != null && request.RegisterEndDate == null)
            //{
            //    FilterData Filter = new FilterData()
            //    {
            //        field = "CreateDate",
            //        data = new data()
            //        {
            //            comparison = "gteq",
            //            type = "date",
            //            value = new[] {request.RegisterStartDate}
            //        }
            //    };
            //    cusotomerFlter.Add(Filter);
            //}

            //if (request.RegisterEndDate != null && request.RegisterStartDate == null)
            //{
            //    FilterData Filter = new FilterData()
            //    {
            //        field = "CreateDate",
            //        data = new data()
            //        {
            //            comparison = "lteq",
            //            type = "date",
            //            value = new[] {request.RegisterEndDate}
            //        }
            //    };
            //    cusotomerFlter.Add(Filter);
            //}

            //if (request.RegisterStartDate != null && request.RegisterEndDate != null)
            //{
            //FilterData Filter1 = new FilterData()
            //{
            //    field = "CreateDate",
            //    data = new data()
            //    {
            //        comparison = "lteq",
            //        type = "dateBetween",
            //        //value = new[] {"1393/05/05", "1393/05/20"}
            //        value = new[] { request.RegisterStartDate, request.RegisterEndDate }
            //    }

            //};
            //cusotomerFlter.Add(Filter1);
            //}

            //#endregion

            //#region Center

            //if (request.CenterIDs != null)
            //{
            //    if (request.CenterIDs.Count() > 0)
            //    {
            //        IList<string> Ids = new List<string>();
            //        foreach (var item in request.CenterIDs)
            //        {
            //            Ids.Add(item.ToString());
            //        }


            //        cusotomerFlter.Add(new FilterData()
            //        {
            //            data = new data()
            //            {
            //                comparison = "eq",
            //                type = "list",
            //                value = Ids.ToArray()
            //            },
            //            field = "Center.ID"

            //        });
            //    }
            //}

            //#endregion

            //GetGeneralResponse<IEnumerable<GetCustomerCampaignView>> customerCampaign =
            //    new GetGeneralResponse<IEnumerable<GetCustomerCampaignView>>();

            //customerCampaign = _customerService.GetCustomerForCampaign(cusotomerFlter);

            //#region other Filter

            //if (request.IsRanje)
            //{
            //    if (request.SupportInputStartDate != null && request.SupportInputEndDate == null)

            //    customerCampaign.data =
            //        customerCampaign.data.Where(x=>!string.IsNullOrEmpty(x.InputSupporttDate)).Where(
            //            x => String.Compare(x.InputSupporttDate.Substring(0, 10), request.SupportInputStartDate)>=0).ToList();

            //    if (request.SupportInputEndDate != null && request.SupportInputStartDate == null)
            //        customerCampaign.data =
            //            customerCampaign.data.Where(x => !string.IsNullOrEmpty(x.InputSupporttDate)).Where(
            //                x => String.Compare(x.InputSupporttDate.Substring(0, 10), request.SupportInputEndDate) <= 0).ToList();

            //    if (request.SupportInputStartDate != null && request.SupportInputEndDate != null)
            //    {

            //        customerCampaign.data =
            //            customerCampaign.data.Where(x => !string.IsNullOrEmpty(x.InputSupporttDate)).Where(
            //                x => String.Compare(x.InputSupporttDate.Substring(0, 10), request.SupportInputStartDate) >= 0).ToList();
            //        customerCampaign.data =
            //            customerCampaign.data.Where(x => !string.IsNullOrEmpty(x.InputSupporttDate)).Where(
            //                x => String.Compare(x.InputSupporttDate.Substring(0, 10), request.SupportInputEndDate) <= 0).ToList();
            //    }
            //}

            //if (request.HasFiscal)
            //    customerCampaign.data = customerCampaign.data.Where(x => x.HasFiscal);

            //IEnumerable<GetCustomerCampaignView> list2 = customerCampaign.data.ToList();

            //#endregion

            //#endregion


            //IList<GetSuctionModeCost3View> resultList = new List<GetSuctionModeCost3View>();
            //foreach (var item in query2)
            //{
            //    int counter = 0;


            //    foreach (var _item in list2)
            //    {
            //        if (item.SuctionModeDetailID == _item.SuctionModeDetailID)
            //        {
            //            GetSuctionModeCost3View temp = new GetSuctionModeCost3View();
            //            counter++;
            //            temp.CostAmount = item.Amount;
            //            temp.CenterName = _item.CenterName;
            //            temp.SuctionModeDetailName = item.SuctionModeDetailName;
            //            temp.SuctionModeName = item.SuctionMoedName;
            //            temp.UserCountPerCenter = counter;
            //            temp.SuctionModeDetailID = item.SuctionModeDetailID;
            //            temp.suctionModeID = item.SuctionModeID;
            //            resultList.Add(temp);
            //        }
            //    }
            //    int it =
            //        resultList.Where(x => x.SuctionModeDetailID == item.SuctionModeDetailID).Count(x => x.suctionModeID == item.SuctionModeID);
            //    foreach(var index in resultList)
            //        if (index.SuctionModeDetailID == item.SuctionModeDetailID)
            //            index.UserCountPerSuctionModeDetal = it;
            //}


            //var finalQuery =
            //     resultList.GroupBy(c => new { c.SuctionModeDetailID, c.CenterName })
            //        .Select(g => new GetSuctionModeCost3View()
            //        {
            //            CenterName = g.Key.CenterName,
            //            suctionModeID = g.FirstOrDefault().suctionModeID,
            //            SuctionModeDetailID = g.FirstOrDefault().SuctionModeDetailID,
            //            SuctionModeDetailName = g.FirstOrDefault().SuctionModeDetailName,
            //            SuctionModeName = g.FirstOrDefault().SuctionModeName,
            //            CostAmount = g.FirstOrDefault().CostAmount,
            //            UserCountPerCenter = g.Count(),
            //            UserCountPerSuctionModeDetal = g.FirstOrDefault().UserCountPerSuctionModeDetal
            //        });


            //// var listItemQuery = from qu2 in query2
            ////                    join l2 in list2 on qu2.SuctionModeDetailID equals l2.SuctionModeDetailID
            ////                    select new GetSuctionModeCost3View()
            ////                    {
            ////                        CostAmount = qu2.Amount,
            ////                        CenterName = l2.CenterName,
            ////                        SuctionModeDetailName = qu2.SuctionModeDetailName,
            ////                        SuctionModeName = qu2.SuctionMoedName,
            ////                        SuctionModeDetailID = qu2.SuctionModeDetailID,
            ////                        suctionModeID = qu2.SuctionModeID,
            ////                    };

            ////var centers = from bs in listItemQuery
            ////              group bs by bs.CenterName
            ////                  into g
            ////                  select new GetcampaignAgents
            ////                  {
            ////                      Amount = g.Sum(x => x.CostAmount),
            ////                      SuctionModeDetailID = g.FirstOrDefault().SuctionModeDetailID,
            ////                      SuctionModeDetailName = g.First().SuctionModeDetailName,
            ////                      SuctionModeID = g.First().suctionModeID,
            ////                      SuctionMoedName = g.First().SuctionModeName,
            ////                  };

            ////var preQuery = Enumerable.Zip(listItemQuery.ToList(), customerCampaign.data, (x, y) =>
            ////{


            ////    if (centers.Any(z => z.SuctionModeDetailID == x.SuctionModeDetailID))
            ////    {
            ////        var count = customerCampaign.data.Count(z => z.SuctionModeDetailID == y.SuctionModeDetailID);
            ////        return new GetSuctionModeCost3View()
            ////        {
            ////            CostAmount = x.CostAmount,
            ////            CenterName = x.CenterName,
            ////            SuctionModeDetailName = x.SuctionModeDetailName,
            ////            SuctionModeName = y.SuctionMoedName,
            ////            UserCountPerCenter = count,
            ////            SuctionModeDetailID = x.SuctionModeDetailID,
            ////            suctionModeID = y.SuctionModeID
            ////        };
            ////    }
            ////    return null;
            ////}).Where(x => x != null).ToList();

            ////var finalQuery =
            ////         preQuery.GroupBy(c => new { c.SuctionModeDetailID, c.SuctionModeName, c.CenterName })
            ////            .Select(g => new GetSuctionModeCost3View()
            ////            {
            ////                CenterName = g.Key.CenterName,
            ////                suctionModeID = g.FirstOrDefault().suctionModeID,
            ////                SuctionModeDetailID = g.FirstOrDefault().SuctionModeDetailID,
            ////                SuctionModeDetailName = g.FirstOrDefault().SuctionModeDetailName,
            ////                SuctionModeName = g.FirstOrDefault().SuctionModeName,
            ////                CostAmount = g.Sum(x => x.CostAmount),
            ////                UserCountPerCenter = g.Sum(x => x.UserCountPerCenter),
            ////                UserCountPerSuctionModeDetal = g.FirstOrDefault().UserCountPerSuctionModeDetal
            ////            }).OrderBy((x => x.CenterName));
            ////response.data = finalQuery;



            //response.data = finalQuery;

            //#region report Export To Excel
            //if (ExportTo == "Excel")
            //{
            //    GridView gridView = new GridView();
            //    gridView.DataSource = response.data;

            //    gridView.DataBind();


            //    gridView.HeaderRow.Cells[0].Text = "شناسه";
            //    gridView.HeaderRow.Cells[1].Text = "کل مبلغ پرداختی";
            //    gridView.HeaderRow.Cells[2].Text = "شناسه";
            //    gridView.HeaderRow.Cells[3].Text = "شیوه جذب";
            //    gridView.HeaderRow.Cells[4].Text = "جزئیات جذب";
            //    gridView.HeaderRow.Cells[5].Text = "شناسه";
            //    gridView.HeaderRow.Cells[6].Text = "تعداد در مرکز";
            //    gridView.HeaderRow.Cells[7].Text = "هزینه در شیوه جذب";
            //    gridView.HeaderRow.Cells[7].Text = "هزینه در مرکز مخابراتی";


            //    gridView.Font.Names = new[] { "Tahoma" };

            //    Response.ClearContent();
            //    Response.Buffer = true;
            //    Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
            //    Response.ContentType = "application/ms-excel";

            //    Response.ContentEncoding = System.Text.Encoding.UTF8;
            //    Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            //    StringWriter stringWriter = new StringWriter();
            //    HtmlTextWriter htmltextwriter = new HtmlTextWriter(stringWriter);

            //    gridView.RenderControl(htmltextwriter);

            //    Response.Output.Write(stringWriter.ToString());
            //    Response.Flush();
            //    Response.End();

            //}
            //#endregion

            #endregion

            data = _campaignPaymentService.GetSuctionModeReport3(request);
            var group = data.data.GroupBy(x => x.SuctionModeDetailID).Select(g => new
            {
                SuctionModeDetailID = g.FirstOrDefault().SuctionModeDetailID,
                CustomerperSuctionMode = g.Sum(x => x.CustomerPerCenter)
            });
            foreach (var item in group)
            {
                foreach (var _item in data.data)
                {
                    if (item.SuctionModeDetailID == _item.SuctionModeDetailID)
                        _item.UserCountPerSuctionModeDetal = item.CustomerperSuctionMode;

                }
            }
            IList<GetSuctionModeCost3View> list = new List<GetSuctionModeCost3View>();
            foreach (var item in data.data)
            {
                list.Add(new GetSuctionModeCost3View()
                {
                    CenterName = item.CenterName,
                    CostAmount = item.Amount,
                    SuctionModeDetailID = item.SuctionModeDetailID,
                    SuctionModeDetailName = item.SuctionModeDetailName,
                    UserCountPerCenter = item.CustomerPerCenter,
                    UserCountPerSuctionModeDetal = item.UserCountPerSuctionModeDetal,
                    SuctionModeName = item.SuctionModeName
                });
            }

            response.data = list;

            #region report Export To Excel

            if (ExportTo == "Excel")
            {
                IList<GetSuctionModeCost3> getSuctionModeCost3 = new List<GetSuctionModeCost3>();

                foreach (var item in response.data)
                {
                    getSuctionModeCost3.Add(new GetSuctionModeCost3()
                    {
                        CenterName = item.CenterName,
                        ConstAmount = item.CostAmount,
                        CostPerUserSuctionCenter = item.CostPerUserSuctionCenter,
                        CostPerUserSuctionMode = item.CostPerUserSuctionMode,
                        SuctionModeDetailName = item.SuctionModeDetailName,
                        SuctionModeName = item.SuctionModeName,
                        UserCountPerCenter = item.UserCountPerCenter,
                        UserCountPerSuctionModeDetal = item.UserCountPerSuctionModeDetal,
                    });
                }
                GridView gridView = new GridView();
                gridView.DataSource = getSuctionModeCost3;

                gridView.DataBind();

                gridView.HeaderRow.Cells[0].Text = "هزینه";
                gridView.HeaderRow.Cells[1].Text = "شیوه جذب";
                gridView.HeaderRow.Cells[2].Text = "جزئیات جذب";
                gridView.HeaderRow.Cells[3].Text = "تعداد کاربر در هر مرکز";
                gridView.HeaderRow.Cells[4].Text = "هزینه جذب هر شیوه جذب";
                gridView.HeaderRow.Cells[5].Text = "هزینه جذب هر مرکز";
                gridView.HeaderRow.Cells[6].Text = "نام مرکز";
                gridView.HeaderRow.Cells[7].Text = "تعداد کاربر در هر شیوه جذب  ";



                gridView.Font.Names = new[] { "Tahoma" };

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
                Response.ContentType = "application/ms-excel";

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmltextwriter = new HtmlTextWriter(stringWriter);

                gridView.RenderControl(htmltextwriter);

                Response.Output.Write(stringWriter.ToString());
                Response.Flush();
                Response.End();

            }

            #endregion

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Sction3

        public JsonResult GetSuctionModeCost4(GetsuctionModeRequestForReport3 request, string ExportTo)
        {
            #region Fuck

            //GetGeneralResponse<IEnumerable<GetSuctionModeCost3View>> response =
            //    new GetGeneralResponse<IEnumerable<GetSuctionModeCost3View>>();

            //#region Campaign Payment

            //#region Prepairing Filters

            //IList<FilterData> filter = new List<FilterData>();


            //if (request.PaymentStartDate != null && request.PaymentEndDate == null)
            //{
            //    FilterData Filter = new FilterData()
            //    {
            //        field = "PaymentDate",
            //        data = new data()
            //        {
            //            comparison = "gteq",
            //            type = "date",
            //            value = new[] { request.PaymentStartDate }
            //        }
            //    };
            //    filter.Add(Filter);
            //}

            //if (request.PaymentEndDate != null && request.PaymentStartDate == null)
            //{
            //    FilterData Filter = new FilterData()
            //    {
            //        field = "PaymentDate",
            //        data = new data()
            //        {
            //            comparison = "lteq",
            //            type = "date",
            //            value = new[] { request.PaymentEndDate }
            //        }
            //    };
            //    filter.Add(Filter);
            //}

            //if (request.PaymentStartDate != null && request.PaymentEndDate != null)
            //{
            //    FilterData Filter = new FilterData()
            //    {
            //        field = "PaymentDate",
            //        data = new data()
            //        {
            //            comparison = "lteq",
            //            type = "dateBetween",
            //            value = new[] { request.PaymentStartDate, request.PaymentEndDate }
            //        }

            //    };
            //    filter.Add(Filter);
            //}


            //#endregion

            //#region Suction mode & Suction Mode Details

            //if (request.SuctionModeDetailsIDs != null)
            //{
            //    if (request.SuctionModeDetailsIDs.Count() > 0)
            //    {
            //        IList<string> Ids = new List<string>();
            //        foreach (var item in request.SuctionModeDetailsIDs)
            //        {
            //            Ids.Add(item.ToString());
            //        }


            //        filter.Add(new FilterData()
            //        {
            //            data = new data()
            //            {
            //                comparison = "eq",
            //                type = "list",
            //                value = Ids.ToArray()
            //            },
            //            field = "SuctionModeDetail.ID"

            //        });
            //    }
            //}

            //else if (request.SuctionModeIDs != null && request.SuctionModeDetailsIDs == null)
            //{
            //    if (request.SuctionModeIDs.Count() > 0)
            //    {
            //        IList<string> Ids = new List<string>();
            //        foreach (var item in request.SuctionModeIDs)
            //        {
            //            Ids.Add(item.ToString());
            //        }

            //        filter.Add(new FilterData()
            //        {
            //            data = new data()
            //            {
            //                comparison = "eq",
            //                type = "list",
            //                value = Ids.ToArray()
            //            },
            //            field = "SuctionModeDetail.SuctionMode.ID"

            //        });
            //    }
            //}

            //#endregion

            //GetGeneralResponse<IEnumerable<GetcampaignAgents>> campaignPayment =
            //    new GetGeneralResponse<IEnumerable<GetcampaignAgents>>();

            //campaignPayment = _campaignPaymentService.GetSuctionModeCostReport3(filter, null);



            //var query1 = from bs in campaignPayment.data
            //             group bs by bs.SuctionModeDetailID
            //                 into g
            //                 select new GetcampaignAgents
            //                 {
            //                     Amount = g.Sum(x => x.Amount),
            //                     SuctionModeDetailID = g.Key,
            //                     SuctionModeDetailName = g.First().SuctionModeDetailName,
            //                     SuctionModeID = g.First().SuctionModeID,
            //                     SuctionMoedName = g.First().SuctionMoedName,

            //                 };
            //IEnumerable<GetcampaignAgents> query2 = query1.ToList();

            //#endregion

            //#region Customers

            //IList<FilterData> cusotomerFlter = new List<FilterData>();

            //#region Suction mode & Suction Mode Details



            //if (request.SuctionModeDetailsIDs != null)
            //{
            //    if (request.SuctionModeDetailsIDs.Count() > 0)
            //    {
            //        IList<string> Ids = new List<string>();
            //        foreach (var item in request.SuctionModeDetailsIDs)
            //        {
            //            Ids.Add(item.ToString());
            //        }


            //        cusotomerFlter.Add(new FilterData()
            //        {
            //            data = new data()
            //            {
            //                comparison = "eq",
            //                type = "list",
            //                value = Ids.ToArray()
            //            },
            //            field = "SuctionModeDetail.ID"

            //        });
            //    }
            //}

            //else if (request.SuctionModeIDs != null && request.SuctionModeDetailsIDs == null)
            //{
            //    if (request.SuctionModeIDs.Count() > 0)
            //    {
            //        IList<string> Ids = new List<string>();
            //        foreach (var item in request.SuctionModeIDs)
            //        {
            //            Ids.Add(item.ToString());
            //        }

            //        cusotomerFlter.Add(new FilterData()
            //        {
            //            data = new data()
            //            {
            //                comparison = "eq",
            //                type = "list",
            //                value = Ids.ToArray()
            //            },
            //            field = "SuctionModeDetail.SuctionMode.ID"

            //        });
            //    }
            //}

            //#endregion

            //#region register Date

            //if (request.RegisterStartDate != null && request.RegisterEndDate == null)
            //{
            //    FilterData Filter = new FilterData()
            //    {
            //        field = "CreateDate",
            //        data = new data()
            //        {
            //            comparison = "gteq",
            //            type = "date",
            //            value = new[] { request.RegisterStartDate }
            //        }
            //    };
            //    cusotomerFlter.Add(Filter);
            //}

            //if (request.RegisterEndDate != null && request.RegisterStartDate == null)
            //{
            //    FilterData Filter = new FilterData()
            //    {
            //        field = "CreateDate",
            //        data = new data()
            //        {
            //            comparison = "lteq",
            //            type = "date",
            //            value = new[] { request.RegisterEndDate }
            //        }
            //    };
            //    cusotomerFlter.Add(Filter);
            //}

            ////if (request.RegisterStartDate != null && request.RegisterEndDate != null)
            ////{
            //    FilterData Filter1 = new FilterData()
            //    {
            //        field = "CreateDate",
            //        data = new data()
            //        {
            //            comparison = "lteq",
            //            type = "dateBetween",
            //            value = new[] {"1393/05/05", "1393/05/20"}
            //            //value = new[] { request.RegisterStartDate, request.RegisterEndDate }
            //        }

            //    };
            //    cusotomerFlter.Add(Filter1);
            ////}

            //#endregion

            //#region Center

            //if (request.CenterIDs != null)
            //{
            //    if (request.CenterIDs.Count() > 0)
            //    {
            //        IList<string> Ids = new List<string>();
            //        foreach (var item in request.CenterIDs)
            //        {
            //            Ids.Add(item.ToString());
            //        }


            //        cusotomerFlter.Add(new FilterData()
            //        {
            //            data = new data()
            //            {
            //                comparison = "eq",
            //                type = "list",
            //                value = Ids.ToArray()
            //            },
            //            field = "Center.ID"

            //        });
            //    }
            //}

            //#endregion

            //GetGeneralResponse<IEnumerable<GetCustomerCampaignView>> customerCampaign =
            //    new GetGeneralResponse<IEnumerable<GetCustomerCampaignView>>();

            //customerCampaign = _customerService.GetCustomerForCampaign(cusotomerFlter);

            //#region other Filter

            //if (request.IsRanje)
            //{
            //    if (request.SupportInputStartDate != null && request.SupportInputEndDate == null)

            //        customerCampaign.data =
            //            customerCampaign.data.Where(x => !string.IsNullOrEmpty(x.InputSupporttDate)).Where(
            //                x => String.Compare(x.InputSupporttDate.Substring(0, 10), request.SupportInputStartDate) >= 0).ToList();

            //    if (request.SupportInputEndDate != null && request.SupportInputStartDate == null)
            //        customerCampaign.data =
            //            customerCampaign.data.Where(x => !string.IsNullOrEmpty(x.InputSupporttDate)).Where(
            //                x => String.Compare(x.InputSupporttDate.Substring(0, 10), request.SupportInputEndDate) <= 0).ToList();

            //    if (request.SupportInputStartDate != null && request.SupportInputEndDate != null)
            //    {

            //        customerCampaign.data =
            //            customerCampaign.data.Where(x => !string.IsNullOrEmpty(x.InputSupporttDate)).Where(
            //                x => String.Compare(x.InputSupporttDate.Substring(0, 10), request.SupportInputStartDate) >= 0).ToList();
            //        customerCampaign.data =
            //            customerCampaign.data.Where(x => !string.IsNullOrEmpty(x.InputSupporttDate)).Where(
            //                x => String.Compare(x.InputSupporttDate.Substring(0, 10), request.SupportInputEndDate) <= 0).ToList();
            //    }
            //}

            //if (request.HasFiscal)
            //    customerCampaign.data = customerCampaign.data.Where(x => x.HasFiscal);

            //IEnumerable<GetCustomerCampaignView> list2 = customerCampaign.data.ToList();

            //#endregion

            //#endregion


            //IList<GetSuctionModeCost3View> resultList = new List<GetSuctionModeCost3View>();
            //foreach (var item in query2)
            //{
            //    int counter = 0;


            //    foreach (var _item in list2)
            //    {
            //        if (item.SuctionModeDetailID == _item.SuctionModeDetailID)
            //        {
            //            GetSuctionModeCost3View temp = new GetSuctionModeCost3View();
            //            counter++;
            //            temp.CostAmount = item.Amount;
            //            temp.CenterName = _item.CenterName;
            //            temp.SuctionModeDetailName = item.SuctionModeDetailName;
            //            temp.SuctionModeName = item.SuctionMoedName;
            //            temp.UserCountPerCenter = counter;
            //            temp.SuctionModeDetailID = item.SuctionModeDetailID;
            //            temp.suctionModeID = item.SuctionModeID;
            //            resultList.Add(temp);
            //        }
            //    }
            //    int it =
            //        resultList.Where(x => x.SuctionModeDetailID == item.SuctionModeDetailID)
            //            .Where(x => x.suctionModeID == item.SuctionModeID)
            //            .Count();
            //    foreach (var index in resultList)
            //        if (index.SuctionModeDetailID == item.SuctionModeDetailID)
            //            index.UserCountPerSuctionModeDetal = it;
            //}


            //var listItemQuery = from qu2 in query2
            //                    join l2 in list2 on qu2.SuctionModeDetailID equals l2.SuctionModeDetailID
            //                    select new GetSuctionModeCost3View()
            //                    {
            //                        CostAmount = qu2.Amount,
            //                        CenterName = l2.CenterName,
            //                        SuctionModeDetailName = qu2.SuctionModeDetailName,
            //                        SuctionModeName = qu2.SuctionMoedName,
            //                        SuctionModeDetailID = qu2.SuctionModeDetailID,
            //                        suctionModeID = qu2.SuctionModeID,
            //                    };

            //var centers = from bs in listItemQuery
            //              group bs by bs.CenterName
            //                  into g
            //                  select new GetcampaignAgents
            //                  {
            //                      Amount = g.Sum(x => x.CostAmount),
            //                      SuctionModeDetailID = g.FirstOrDefault().SuctionModeDetailID,
            //                      SuctionModeDetailName = g.First().SuctionModeDetailName,
            //                      SuctionModeID = g.First().suctionModeID,
            //                      SuctionMoedName = g.First().SuctionModeName,
            //                  };

            //var preQuery = Enumerable.Zip(listItemQuery.ToList(), customerCampaign.data, (x, y) =>
            //{


            //    if (centers.Any(z => z.SuctionModeDetailID == x.SuctionModeDetailID))
            //    {
            //        var count = customerCampaign.data.Count(z => z.SuctionModeDetailID == y.SuctionModeDetailID);
            //        return new GetSuctionModeCost3View()
            //        {
            //            CostAmount = x.CostAmount,
            //            CenterName = x.CenterName,
            //            SuctionModeDetailName = x.SuctionModeDetailName,
            //            SuctionModeName = y.SuctionMoedName,
            //            UserCountPerCenter = count,
            //            SuctionModeDetailID = x.SuctionModeDetailID,
            //            suctionModeID = y.SuctionModeID
            //        };
            //    }
            //    return null;
            //}).Where(x => x != null).ToList();

            //var finalQuery =
            //         preQuery.GroupBy(c => new { c.SuctionModeDetailID, c.SuctionModeName, c.CenterName })
            //            .Select(g => new GetSuctionModeCost3View()
            //            {
            //                CenterName = g.Key.CenterName,
            //                suctionModeID = g.FirstOrDefault().suctionModeID,
            //                SuctionModeDetailID = g.FirstOrDefault().SuctionModeDetailID,
            //                SuctionModeDetailName = g.FirstOrDefault().SuctionModeDetailName,
            //                SuctionModeName = g.FirstOrDefault().SuctionModeName,
            //                CostAmount = g.Sum(x => x.CostAmount),
            //                UserCountPerCenter = g.Sum(x => x.UserCountPerCenter),
            //                UserCountPerSuctionModeDetal = g.FirstOrDefault().UserCountPerSuctionModeDetal
            //            }).OrderBy((x => x.CenterName));
            //response.data = finalQuery;

            //#region report Export To Excel
            //if (ExportTo == "Excel")
            //{
            //    GridView gridView = new GridView();
            //    gridView.DataSource = response.data;

            //    gridView.DataBind();

            //    gridView.HeaderRow.Cells[0].Text = "شناسه";
            //    gridView.HeaderRow.Cells[1].Text = "کل مبلغ پرداختی";
            //    gridView.HeaderRow.Cells[2].Text = "شناسه";
            //    gridView.HeaderRow.Cells[3].Text = "شیوه جذب";
            //    gridView.HeaderRow.Cells[4].Text = "جزئیات جذب";
            //    gridView.HeaderRow.Cells[5].Text = "شناسه";
            //    gridView.HeaderRow.Cells[6].Text = "تعداد در مرکز";
            //    gridView.HeaderRow.Cells[7].Text = "هزینه در شیوه جذب";
            //    gridView.HeaderRow.Cells[7].Text = "هزینه در مرکز مخابراتی";


            //    gridView.Font.Names = new[] { "Tahoma" };

            //    Response.ClearContent();
            //    Response.Buffer = true;
            //    Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
            //    Response.ContentType = "application/ms-excel";

            //    Response.ContentEncoding = System.Text.Encoding.UTF8;
            //    Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            //    StringWriter stringWriter = new StringWriter();
            //    HtmlTextWriter htmltextwriter = new HtmlTextWriter(stringWriter);

            //    gridView.RenderControl(htmltextwriter);

            //    Response.Output.Write(stringWriter.ToString());
            //    Response.Flush();
            //    Response.End();

            //}
            //#endregion

            #endregion

            GetGeneralResponse<IEnumerable<SuctionModeCost>> data =
                new GetGeneralResponse<IEnumerable<SuctionModeCost>>();
            GetGeneralResponse<IEnumerable<GetSuctionModeCost3View>> response =
                new GetGeneralResponse<IEnumerable<GetSuctionModeCost3View>>();

            #region Access Check

            //bool hasPermission = GetEmployee().IsGuaranteed("SuctionModeCost_Read");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}

            #endregion

            data = _campaignPaymentService.GetSuctionModeReport4(request);
            var group = data.data.GroupBy(x => x.SuctionModeDetailID).Select(g => new
            {
                SuctionModeDetailID = g.FirstOrDefault().SuctionModeDetailID,
                CustomerperSuctionMode = g.Sum(x => x.CustomerPerCenter)
            });
            foreach (var item in group)
            {
                foreach (var _item in data.data)
                {
                    if (item.SuctionModeDetailID == _item.SuctionModeDetailID)
                        _item.UserCountPerSuctionModeDetal = item.CustomerperSuctionMode;

                }
            }
            IList<GetSuctionModeCost3View> list = new List<GetSuctionModeCost3View>();
            foreach (var item in data.data)
            {
                list.Add(new GetSuctionModeCost3View()
                {
                    CenterName = item.CenterName,
                    CostAmount = item.Amount,
                    SuctionModeDetailID = item.SuctionModeDetailID,
                    SuctionModeDetailName = item.SuctionModeDetailName,
                    UserCountPerCenter = item.CustomerPerCenter,
                    UserCountPerSuctionModeDetal = item.UserCountPerSuctionModeDetal,
                    SuctionModeName = item.SuctionModeName
                });
            }

            response.data = list;

            #region report Export To Excel

            if (ExportTo == "Excel")
            {
                IList<GetSuctionModeCost4> getSuctionModeCost3 = new List<GetSuctionModeCost4>();

                foreach (var item in response.data)
                {
                    getSuctionModeCost3.Add(new GetSuctionModeCost4()
                    {
                        CenterName = item.CenterName,
                        CostAmount = item.CostAmount,
                        CostPerUserSuctionCenter = item.CostPerUserSuctionCenter,
                        CostPerUserSuctionMode = item.CostPerUserSuctionMode,
                        SuctionModeDetailName = item.SuctionModeDetailName,
                        SuctionModeName = item.SuctionModeName,
                        UserCountPerCenter = item.UserCountPerCenter,
                        UserCountPerSuctionModeDetal = item.UserCountPerSuctionModeDetal,
                    });
                }
                GridView gridView = new GridView();
                gridView.DataSource = getSuctionModeCost3;

                gridView.DataBind();

                gridView.HeaderRow.Cells[0].Text = "هزینه";
                gridView.HeaderRow.Cells[1].Text = "شیوه جذب";
                gridView.HeaderRow.Cells[2].Text = "جزئیات جذب";
                gridView.HeaderRow.Cells[3].Text = "تعداد کاربر در هر مرکز";
                gridView.HeaderRow.Cells[4].Text = "هزینه جذب هر شیوه جذب";
                gridView.HeaderRow.Cells[5].Text = "هزینه جذب هر مرکز";
                gridView.HeaderRow.Cells[6].Text = "نام مرکز";
                gridView.HeaderRow.Cells[7].Text = "تعداد کاربر در هر شیوه جذب";



                gridView.Font.Names = new[] { "Tahoma" };

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
                Response.ContentType = "application/ms-excel";

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmltextwriter = new HtmlTextWriter(stringWriter);

                gridView.RenderControl(htmltextwriter);

                Response.Output.Write(stringWriter.ToString());
                Response.Flush();
                Response.End();

            }

            #endregion

            return Json(response, JsonRequestBehavior.AllowGet);
        }



        #endregion


        #endregion

        #region Get All CanDeliverCost

        public JsonResult GetAll_CanDeliverCost(string StartDate, string EndDate)
        {
            var response = _saleService.GetSaleCanDeliverCost(StartDate, EndDate);
            //return View(response.data);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get All Balance

        public ActionResult GetAll_Balance(string StartDate, string EndDate)
        {
            var response = _saleService.GetSaleBalance(StartDate, EndDate);
            return View(response.data);

            //return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DateDiff()
        {
            IList<temp> t = new List<temp>();
            var temp = _fiscalService.GetAllFiscals(100000, 1, null, null);
            var f = temp.data.Where(x => x.Confirm == Model.Fiscals.ConfirmEnum.Confirmed).Where(x => x.InvestDate != x.ConfirmDate.Substring(0, 10));
            foreach (var item in f)
            {

                t.Add(new temp()
                {
                    ADSLPhone = item.ADSLPhone,
                    InvestDate = item.InvestDate,
                    ConfirmDate = item.ConfirmDate,
                    DateDiff = PersianDateTime.DateDiff(item.InvestDate, item.ConfirmDate),
                    MoneyAccountName = item.MoneyAccountName,
                    CreateEmployeeName = item.CreateEmployeeName,
                    ConfirmEmployeeName = item.ConfirmEmployeeName,
                    ConfirmedCost = item.ConfirmedCost
                });
            }


            return View(t);

        #endregion
        }
    }


}
public class rep
{
    public string adslPhone { get; set; }
    public int Balance { get; set; }
    public int Moeein { get; set; }
    public int Deference { get; set; }
}

public class BonusComissionWelcome
{
    public string Period { get; set; }
    public string How { get; set; }
    public long Comission { get; set; }
    public long Bonus { get; set; }
}
public class GetLedgerAccount
{

    public string Date { get; set; }
    public string Comment { get; set; }
    public long BedCost { get; set; }
    public long BesCost { get; set; }
    public long Remain { get; set; }
    public long Discount { get; set; }
    public long Imposition { get; set; }
    public int Count { get; set; }
    public long Nakhalesh { get; set; }
    public string TransactionType { get; set; }
    public string AccountNUmber { get; set; }
    public string Rollback { get; set; }
}

public class GetSuctionModeCost3
{


    public long ConstAmount { get; set; }
    public string SuctionModeName { get; set; }
    public string SuctionModeDetailName { get; set; }
    public int UserCountPerCenter { get; set; }
    public long CostPerUserSuctionMode { get; set; }
    public long CostPerUserSuctionCenter { get; set; }
    public string CenterName { get; set; }
    public long UserCountPerSuctionModeDetal { get; set; }


}

public class GetSuctionModeCost2
{
    public string CampaignAgentName { get; set; }
    public long Amount { get; set; }
}
public class GetNetworkReport
{
    public string NetworkName { get; set; }
    public long Count { get; set; }
}
public class GetSuctionModeCost4
{
    public long CostAmount { get; set; }
    public string SuctionModeName { get; set; }
    public string SuctionModeDetailName { get; set; }
    public int UserCountPerCenter { get; set; }
    public long CostPerUserSuctionMode { get; set; }
    public long CostPerUserSuctionCenter { get; set; }
    public string CenterName { get; set; }
    public int UserCountPerSuctionModeDetal { get; set; }
}

