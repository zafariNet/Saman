using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Services.Implementations;
using Services.ViewModels.Customers;

namespace Services.ViewModels.Sales
{
    public class SimpleSaleView:BaseView
    {

        public bool HasUnDeliveredAndUnRollbackedSaleDetail { get; set; }
        public Guid CustomerID { get; set; }

        [Display(Name = "نام مشتری")]
        public string CustomerName { get; set; }

        public string ADSLPhone { get; set; }

        public string Address { get; set; }

        public string Sname { get; set; }

        public string CenterName { get; set; }

        public string Mobile1 { get; set; }

        public string SaleDate { get; set; }

        public CustomerView Customer { get; set; }

        public Guid MainSaleID { get; set; }

        [Display(Name = "فروش برگشت")]
        public string MainSaleNumber { get; set; }

        public Guid CloseEmployeeID { get; set; }

        [Display(Name = "کارمند")]
        public string CloseEmployeeName { get; set; }

        [Display(Name = "شماره")]
        public string SaleNumber { get; set; }

        [Display(Name = "بسته شده")]
        public bool Closed { get; set; }

        [Display(Name = "تاریخ بستن")]
        public string CloseDate { get; set; }

        // آیا فاکتور برگشت از فروش است؟
        public bool IsRollbackSale { get; set; }

        public long SaleTotal { get; set; }

        public long TotalDiscount { get; set; }

        public long TotalImposition { get; set; }

        public bool CanDeliver { get; set; }

        public bool CanRollback { get; set; }

        public CourierView Courier { get; set; }

        public string LevelTitle { get; set; }
        
        public IEnumerable<ClientSaleDetailView>  SaleDetails { get; set; }
        

        /// <summary>
        /// آیا دارای پشتیبانی هست یا نه
        /// </summary>
        public bool HasCourier
        {
            get
            {
                return this.Courier != null ? true : false;
            }
        }

        public bool EditCourier
        {
            get
            {
                if (Courier != null)
                    return this.Courier.CourierStatuse == 2 ? false : true;
                return true;
            }
        }

        [Display(Name = "فروش اعتباری")]
        public IEnumerable<CreditSaleDetailView> CreditSaleDetails { get; set; }

        [Display(Name = "فروش کالا")]
        public IEnumerable<ProductSaleDetailView> ProductSaleDetails { get; set; }

        [Display(Name = "فروشهای غیر اعتباری")]
        public IEnumerable<UncreditSaleDetailView> UncreditSaleDetails { get; set; }
    }
}
