using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Model.Fiscals;
using Services.ViewModels.Employees;

namespace Services.ViewModels.Fiscals
{
    public class FiscalView : BaseView
    {
        public Guid CustomerID { get; set; }

        [Display(Name = "نام مشتری")]
        public string CustomerName { get; set; }

        public Guid MoneyAccountID { get; set; }

        [Display(Name = "حساب پولی")]
        public string MoneyAccountName { get; set; }

        public Guid ConfirmEmployeeID { get; set; }
        /// <summary>
        /// شماره اختصاصی در صورت تایید مالی
        /// </summary>
        public long AccountingSerialNumber { get; set; }

        [Display(Name = "کارمند تأیید کننده")]
        public string ConfirmEmployeeName { get; set; }

        public string ADSLPhone { get; set; }

        [Display(Name = "شماره سند")]
        public string DocumentSerial { get; set; }

        [Display(Name = "نوع سند")]
        public DocType DocumentType { get; set; }

        [Display(Name = "نوع سند")]
        public string DocumentTypeTxt
        {
            get
            {
                switch ((int)DocumentType)
                {
                    case 1:
                        return "رسید عابربانک";
                    case 2:
                        return "فیش بانکی";
                    case 3:
                        return "چک";
                    case 4:
                        return "قبض صندوق";
                    case 5:
                        return "رسید دستگاه POS";
                    case 6:
                        return "کارت به کارت توسط اینترنت";
                    case 7:
                        return "پرداخت توسط سایت ماهان";
                    case 8:
                        return "سایر";
                    default:
                        return "سایر";
                }
            }
        }

        [Display(Name = "مبلغ")]
        public long Cost { get; set; }

        [Display(Name = "مبلغ")]
        public long DisplayCost { get { return Math.Abs(Cost); } set { } }

        [Display(Name = "نوع تراکنش")]
        public string TransactionType {
            get
            {
                if (Cost >= 0)
                {
                    return "دریافت";
                }
                else
                {
                    return "پرداخت";
                }
            }
        }

        [Display(Name = "نوع تراکنش")]
        public string TypeForCreate { get; set; }

        [Display(Name = "تاریخ واریز")]
        public string InvestDate { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }

        [Display(Name = "وضعیت تأیید")]
        public ConfirmEnum Confirm { get; set; }

        public int ConfirmInt
        {
            get
            {
                return (int)Confirm;
            }
        }

        [Display(Name = "وضعیت تأیید")]
        public string ConfirmTxt
        {
            get
            {
                switch ((int)Confirm)
                {
                    case 1:
                        return "بررسی نشده";
                    case 2:
                        return "تأیید شد";
                    case 3:
                        return "تأیید نشد";
                    default:
                        return "بررسی نشده";
                }
            }
        }

        [Display(Name = "مبلغ تأیید شده")]
        public long ConfirmedCost { get; set; }

        [Display(Name = "تاریخ تأیید")]
        public string ConfirmDate { get; set; }
        public bool CanConfirm { get { return true; } set { } }
        public ChargeStatus ChargeStatus { get; set; }
        public long FollowNumber { get; set; }
        public long SerialNumber { get; set; }
        public bool ForCharge { get; set; }

        /// <summary>
        /// شماره رسید مالی
        /// </summary>
        public long FiscalReciptNumber { get; set; }

        /// <summary>
        /// اعتبار فعلی مشتری
        /// </summary>
        public long Balance { get; set; }


    }

    public class FiscalRealView : BaseView
    {
        public Guid CustomerID { get; set; }

        [Display(Name = "نام مشتری")]
        public string CustomerName { get; set; }

        public Guid MoneyAccountID { get; set; }

        [Display(Name = "حساب پولی")]
        public string MoneyAccountName { get; set; }

        public Guid ConfirmEmployeeID { get; set; }
        /// <summary>
        /// شماره اختصاصی در صورت تایید مالی
        /// </summary>
        public long AccountingSerialNumber { get; set; }

        [Display(Name = "کارمند تأیید کننده")]
        public string ConfirmEmployeeName { get; set; }

        public string ADSLPhone { get; set; }

        [Display(Name = "شماره سند")]
        public string DocumentSerial { get; set; }

        [Display(Name = "نوع سند")]
        public DocType DocumentType { get; set; }

        [Display(Name = "نوع سند")]
        public string DocumentTypeTxt
        {
            get
            {
                switch ((int)DocumentType)
                {
                    case 1:
                        return "رسید عابربانک";
                    case 2:
                        return "فیش بانکی";
                    case 3:
                        return "چک";
                    case 4:
                        return "قبض صندوق";
                    case 5:
                        return "رسید دستگاه POS";
                    case 6:
                        return "کارت به کارت توسط اینترنت";
                    case 7:
                        return "پرداخت توسط سایت ماهان";
                    case 8:
                        return "سایر";
                    default:
                        return "سایر";
                }
            }
        }

        [Display(Name = "مبلغ")]
        public long Cost { get; set; }

        [Display(Name = "مبلغ")]
        public long DisplayCost { get { return Cost; } set { } }

        [Display(Name = "نوع تراکنش")]
        public string TransactionType
        {
            get
            {
                if (Cost >= 0)
                {
                    return "دریافت";
                }
                else
                {
                    return "پرداخت";
                }
            }
        }

        [Display(Name = "نوع تراکنش")]
        public string TypeForCreate { get; set; }

        [Display(Name = "تاریخ واریز")]
        public string InvestDate { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }

        [Display(Name = "وضعیت تأیید")]
        public ConfirmEnum Confirm { get; set; }

        public int ConfirmInt
        {
            get
            {
                return (int)Confirm;
            }
        }

        [Display(Name = "وضعیت تأیید")]
        public string ConfirmTxt
        {
            get
            {
                switch ((int)Confirm)
                {
                    case 1:
                        return "بررسی نشده";
                    case 2:
                        return "تأیید شد";
                    case 3:
                        return "تأیید نشد";
                    default:
                        return "بررسی نشده";
                }
            }
        }

        [Display(Name = "مبلغ تأیید شده")]
        public long ConfirmedCost { get; set; }

        [Display(Name = "تاریخ تأیید")]
        public string ConfirmDate { get; set; }
        public bool CanConfirm { get { return true; } set { } }
        public ChargeStatus ChargeStatus { get; set; }
        public long FollowNumber { get; set; }
        public long SerialNumber { get; set; }
        public bool ForCharge { get; set; }

        /// <summary>
        /// شماره رسید مالی
        /// </summary>
        public long FiscalReciptNumber { get; set; }

        /// <summary>
        /// اعتبار فعلی مشتری
        /// </summary>
        public long Balance { get; set; }


    }
}
