#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Employees;
using Model.Store;
using Model.Support;
using Model.Customers.Validations;
using Model.Fiscals;
using Model.Sales;
using Infrastructure.Persian;
#endregion

namespace Model.Customers
{
    /// <summary>
    /// موجودیت مشتری
    /// </summary>
    public class Customer : EntityBase, IAggregateRoot
    {
        #region Customer Properties
        /// <summary>
        /// مرکز مخابراتی
        /// </summary>
        public virtual Center Center { get; set; }
        /// <summary>
        /// نام نمایندگی
        /// </summary>
        public virtual Agency Agency { get; set; }
        /// <summary>
        /// شبکه مورد استفاده مشتری
        /// </summary>
        public virtual Network Network { get; set; }
        /// <summary>
        /// نحوه جذب
        /// </summary>
        public virtual SuctionMode SuctionMode { get; set; }
        /// <summary>
        /// جزئیات نحوه جذب
        /// </summary>
        public virtual SuctionModeDetail SuctionModeDetail { get; set; }
        /// <summary>
        /// وضعیت مدارک مشتری
        /// </summary>
        public virtual DocumentStatus DocumentStatus { get; set; }
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public virtual string LastName { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        public virtual string FirstName { get; set; }
        /// <summary>
        /// جنسیت
        /// </summary>
        public virtual string Gender { get; set; }
        /// <summary>
        /// نام و نام خانوادگی مشتری
        /// </summary>
        public virtual string Name { get { return FirstName + " " + LastName; } }
        /// <summary>
        /// تاریخ تولد
        /// </summary>
        public virtual string BirthDate { get; set; }
        /// <summary>
        /// شغل
        /// </summary>
        public virtual string Job { get; set; }
        /// <summary>
        /// تلفن
        /// </summary>
        public virtual string Phone { get; set; }
        /// <summary>
        /// موبایل 1
        /// </summary>
        public virtual string Mobile1 { get; set; }
        /// <summary>
        /// موبایل 2
        /// </summary>
        public virtual string Mobile2 { get; set; }
        /// <summary>
        /// نام خانوادگی صاحب خط
        /// </summary>
        public virtual string SLastName { get; set; }
        /// <summary>
        /// نام صاحب خط
        /// </summary>
        public virtual string SFirstName { get; set; }
        /// <summary>
        /// شماره تلفن ADSL
        /// </summary>
        public virtual string ADSLPhone { get; set; }
        /// <summary>
        /// مقدار حقیقی یا حقوقی در آن ذخیره می شود
        /// </summary>
        public virtual string LegalType { get; set; }
        /// <summary>
        /// اعتبار مشتری 
        /// </summary>
        public virtual Int64? Balance { get; set; }
        /// <summary>
        /// ایمیل مشتری
        /// </summary>
        public virtual string Email { get; set; }
        /// <summary>
        /// آدرس مشتری
        /// </summary>
        public virtual string Address { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }
        /// <summary>
        /// قفل شده
        /// </summary>
        public virtual bool Locked { get; set; }
        /// <summary>
        /// کارمند قفل کننده مشتری
        /// </summary>
        public virtual Employee LockEmployee { get; set; }
        /// <summary>
        /// توضیحات قفل
        /// </summary>
        public virtual string LockNote { get; set; }
        /// <summary>
        /// آیا مدارک مشتری به شرکت پپ ارسال شده است یا خیر
        /// </summary>
        public virtual bool SentToPap { get; set; }
        /// <summary>
        /// غیرفعال کردن مشتری - به جای پاک کردن به کار می رود
        /// </summary>
        public virtual bool Discontinued { get; set; }
        /// <summary>
        /// احتمال خرید
        /// </summary>
        public virtual BuyPossibility BuyPossibility { get; set; }
        /// <summary>
        /// وضعیت پیگیری
        /// </summary>
        public virtual FollowStatus FollowStatus { get; set; }

        /// <summary>
        /// وضعیت پشتیبانی
        /// </summary>
        public virtual SupportStatus SupportStatus { get; set; }

        /// <summary>
        /// مبلغ قابل تحویل
        /// </summary>
        public virtual long CanDeliverCost   { get; set; }

        /// <summary>
        /// تاریخ ورود به مرحله
        /// </summary>
        public virtual string LevelEntryDate { get; set; }
        /// <summary>
        /// آیا به سایت اضافه شده است ؟
        /// </summary>
        public virtual bool AddedToSite { get; set; }
        /// <summary>
        /// آیا میتوان این مشتری را حذف کرد؟
        /// </summary>
        public virtual bool CanDelete
        {
            get { return false; }
            //get
            //{
            //    int SalesCount = Sales != null ? Sales.Count() : 0;
            //    int FiscalsCount = Fiscals != null ? Fiscals.Count() : 0;
            //    int EmailsCount = Emails != null ? Emails.Count() : 0;
            //    int DocumentsCount = Documents != null ? Documents.Count() : 0;
            //    int SmssCount = Smss != null ? Smss.Count() : 0;
            //    int ProblemsCount = Problems != null ? Problems.Count() : 0;
            //    int PersenceSupportsCount = PersenceSupports != null ? PersenceSupports.Count() : 0;
            //    return
            //        SalesCount == 0 &&
            //        FiscalsCount == 0 &&
            //        EmailsCount == 0 &&
            //        DocumentsCount == 0 &&
            //        SmssCount == 0 &&
            //        ProblemsCount == 0 &&
            //        PersenceSupportsCount == 0;
            //}
        }
        /// <summary>
        ///  استان
        /// </summary>
        /// <remarks>
        /// این خصوصیت توسط محمد ظفری ایجاد شده است
        /// </remarks>
        //public virtual Province Province { get; set; }
        /// <summary>
        /// شهر
        /// </summary>        
        /// <remarks>
        /// این خصوصیت توسط محمد ظفری ایجاد شده است
        /// </remarks>
        //public virtual City City { get; set; }
        #endregion
        /// <summary>
        /// وظایف مربوط به این مشتری  //Added By Zafari
        /// </summary>
        //public virtual IEnumerable<Task> Tasks { get; set; }

        /// <summary>
        /// مرحله مشتری
        /// </summary>
        public virtual Level Level { get; set; }

        /// <summary>
        /// روزهای معطلی در یک مرحله
        /// </summary>
        public virtual int CurrentLevelWaitingDays { get; set; }
        //{
        //    get
        //    {

        //        return PersianDateTime.DateDiff( PersianDateTime.Now,this.LevelEntryDate);
        //    }
        //}


        #region IEnumerables
         ///<summary>
         //مشتریان و مرحله
         //</summary>
        //public virtual IEnumerable<CustomerLevel> CustomerLevels { get; protected set; }



         //<summary>
        /// مستندات
        /// </summary>
        //public virtual IEnumerable<Document> Documents { get; protected set; }
        /// <summary>
        /// ایمیلها
        /// </summary>
        //public virtual IEnumerable<Email> Emails { get; protected set; }
        /// <summary>
        /// نت ها
        /// </summary>
        //public virtual IEnumerable<Note> Notes { get; protected set; }
        /// <summary>
        /// پیامکها
        /// </summary>
        //public virtual IEnumerable<Sms> Smss { get; protected set; }
        /// <summary>
        /// امور مالی
        /// </summary>
        public virtual IEnumerable<Fiscal> Fiscals { get; protected set; }
        /// <summary>
        /// فروشها
        /// </summary>
        public virtual IList<Sale> Sales { get; protected set; }
        /// <summary>
        /// پشتیبانی حضوری
        /// </summary>
        //public virtual IEnumerable<PersenceSupport> PersenceSupports { get; protected set; }
        /// <summary>
        /// مشکلات
        /// </summary>
        //public virtual IEnumerable<Problem> Problems { get; protected set; }

        public virtual IEnumerable<Support.Support> Supports { get; set; }
        #endregion

        #region Validation
        /// <summary>
        /// اعتبارسنجی مشتری
        /// </summary>
        protected override void Validate()
        {
            if (Email != null && !new EmailValidationSpecification().IsSatisfiedBy(Email))
                base.AddBrokenRule(CustomerBusinessRules.EmailIsInvalid);
        }
        #endregion
    }
}
