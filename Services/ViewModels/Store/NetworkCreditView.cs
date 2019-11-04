using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Store
{
    public class NetworkCreditView : BaseView
    {
        public Guid NetworkID { get; set; }

        [Display(Name = "شبکه")]
        public string NetworkName { get; set; }

        public Guid FromAccountID { get; set; }

        [Display(Name = "از حساب")]
        public string FromAccountTitle { get; set; }

        [Display(Name = "مبلغ")]
        public long Amount { get; set; }

        [Display(Name = "مبلغ")]
        public long DisplayAmount { get { return Math.Abs(Amount); } set { } }

        [Display(Name = "نوع")]
        public string Type
        {
            get { return Amount >= 0 ? "واریز" : "برداشت"; }
        }

        [Display(Name = "نوع")]
        public string TypeForCreate
        {
            get;
            set;
        }

        [Display(Name = "تاریخ تراکنش")]
        public string TransactionDate
        {
            get { return CreateDate; }
        }

        [Display(Name = "موجودی لحظه ای")]
        public long Balance { get; set; }

        [Display(Name = "تاریخ واریز")]
        public string InvestDate { get; set; }

        [Display(Name = "واریز به شماره حساب")]
        public string ToAccount { get; set; }

        [Display(Name = "شماره فیش")]
        public string TransactionNo { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }
    }
}
