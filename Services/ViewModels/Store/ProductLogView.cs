using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Store
{
    public class ProductLogView : BaseView
    {
        public Guid ProductID { get; set; }

        public string ParentProduct { get; set; }

        [Display(Name = "نام کالا")]
        public string ProductName { get; set; }

        [Display(Name = "تاریخ تراکنش")]
        public string LogDate { get; set; }

        [Display(Name = "تعداد")]
        public int UnitsIO { get; set; }

        [Display(Name = "تعداد")]
        public int DisplayUnitsIO { get { return Math.Abs(UnitsIO); } set { } }

        [Display(Name = "ورود/خروج")]
        public string IO 
        {
            get
            {
                if (Store == null)
                {
                    return UnitsIO >= 0 ? "ورود به انبار اصلی" : "خروج از انبار اصلی";
                }
                else
                {
                    return UnitsIO >= 0 ?  "انتقال به انبار اصلی " : "انتقال  به انبار مجازی" ;
                }
            }
        }

        [Display(Name = "نوع تراکنش")]
        public string IOTypeForCreate { get; set; }

        [Display(Name = "قیمت خرید")]
        public long PurchaseUnitPrice { get; set; }

        [Display(Name = "جمع سطر")]
        public long TotalLine { get; set; }

        [Display(Name = "تاریخ خرید")]
        public string PurchaseDate { get; set; }

        [Display(Name = "نام فروشنده")]
        public string SellerName { get; set; }

        [Display(Name = "شماره فاکتور")]
        public string PurchaseBillNumber { get; set; }

        [Display(Name = "بسته شده")]
        public bool Closed { get; set; }

        [Display(Name = "شماره سریال ورود")]
        public string InputSerialNumber { get; set; }

        [Display(Name = "شماره سریال کالا از")]
        public string ProductSerialFrom { get; set; }

        [Display(Name = "شماره سریال کالا تا")]
        public string ProductSerialTo { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }
        
        public StoreView Store { get; set; }

        public string OwnerEmployeeName { get; set; }
    }
}
