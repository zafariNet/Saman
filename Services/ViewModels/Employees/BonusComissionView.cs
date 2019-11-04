using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Sales;

namespace Services.ViewModels.Employees
{
    public class BonusComissionView : BaseView
    {
        public string ActionDate { get; set; }
        /// <summary>
        /// چه زمانی
        /// </summary>
        public string Whene { get; set; }
        /// <summary>
        /// فروش کالا
        /// </summary>
        public string ProductSaleDetailName { get; set; }

        /// <summary>
        /// خدمات غیر اعتباری
        /// </summary>
        public string UnCreditSaleDetailName { get; set; }

        /// <summary>
        /// خدمات اعتباری
        /// </summary>
        public string CreditSaleDetailName { get; set; }

        /// <summary>
        /// پیک
        /// </summary>
        public bool HasCourier { get; set; }

        /// <summary>
        /// مشتری
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// پورسانت
        /// </summary>
        public long Comission { get; set; }

        /// <summary>
        /// امتیاز
        /// </summary>
        public long Bonus { get; set; }

        /// <summary>
        /// برگشتی
        /// </summary>
        public bool IsRollback { get; set; }

        public string Type
        {
            get;
            set;
            //get
            //{
            //    if (ProductSaleDetailName != null)
            //        return "کالا";
            //    if (CreditSaleDetailName != null)
            //        return "خدمات اعتباری";
            //    if (UnCreditSaleDetailName != null)
            //        return "خدمات غیر اعتباری";
            //    return null;
            //}
            //set
            //{

            //}
        }

        public string picture { get; set; }

        public string Photo
        {
            get
            {
                if (picture != null && picture.IndexOf("data") > 0)
                    return picture.Replace(@"\", "/").Substring(picture.IndexOf("data"));
                return string.Empty;
            }
        }
        public string BonusDate { get; set; }
        
    }
}