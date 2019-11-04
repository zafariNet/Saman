using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using Model.Store;

namespace Controllers.ViewModels.Reports
{
    internal class CartexView
    {
        /// <summary>
        /// شماره تلفن ADSL
        /// </summary>
        public string ADSLPhone { get; set; }


        /// <summary>
        /// نام  کالا
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// شرح
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// شرح کالا از لیست قیمت
        /// </summary>
        public string ProductPriceTitle { get; set; }

        ///<summary>
        /// شماره فاکتور یا شماره ورود
        ///</summary> 
        public string TransactionSerialNumber { get; set; }

        /// <summary>
        /// تعداد ورود
        /// </summary>
        public int UnitsInput { get; set; }

        /// <summary>
        /// تعداد خروج
        /// </summary>
        public int UnitsOutput { get; set; }

        /// <summary>
        /// باقیمانده
        /// </summary>
        public int Remain { get; set; }

        /// <summary>
        /// کارمند تحویل
        /// </summary>
        public string DeliverEmployeeName { get; set; }

        /// <summary>
        /// تاریخ تراکنش
        /// </summary>
        public string TransactionDate { get; set; }
        /// <summary>
        /// انبار
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// توضیحاتی که کارشناس هنگام درج تراکنش وارد میکند
        /// </summary>
        public string TransactionComment { get; set; }

        public Guid ID { get; set; }


    }
}
