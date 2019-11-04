using Model.Fiscals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.FiscalCatalogService
{
    public class AddFiscalByVoipRequest
    {
        public string customer_phone { get; set; }
        /// <summary>
        /// تاریخ واریز
        /// </summary>
        public string deposit_date { get; set; }
        /// <summary>
        /// حساب پولی
        /// </summary>
        public Guid bank { get; set; }
        /// <summary>
        /// مبلغ واریزی
        /// </summary>
        public long amount { get; set; }
        /// <summary>
        /// پهنای باند
        /// </summary>
        public string bandwith { get; set; }
        /// <summary>
        /// حجم رافیک
        /// </summary>
        public string volume { get; set; }
        /// <summary>
        /// مدت زمان
        /// </summary>
        public string duration { get; set; }
        /// <summary>
        /// نوع پرداخت
        /// </summary>
        public DocType deposit_type { get; set; }
        /// <summary>
        /// شماره فیش
        /// </summary>
        public string receipt_number { get; set; }

        public bool forCharge { get; set; }

        public string Note { get {
            return "سرویس" + bandwith + "کیلو بایت" + duration + "ماهه " + volume + "ترافیک";
        } }

    }
}
