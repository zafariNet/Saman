using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Persian;
using Services.ViewModels.Sales;
using Services.ViewModels.Fiscals;
using Services.ViewModels.Support;

namespace Services.ViewModels.Customers
{
    public class CustomerView : BaseView
    {
        [Display(Name = "مرکز مخابراتی")]
        public string CenterName { get; set; }

        [Display(Name = "شناسه مخابراتی")]
        public Guid CenterID { get; set; }

        [Display(Name = "نام نمایندگی")]
        public string AgencyName { get; set; }

        [Display(Name = "شناسه نمایندگی")]
        public Guid AgencyID { get; set; }

        [Display(Name = "شبکه")]
        public string NetworkName { get; set; }

        [Display(Name = "شناسه شبکه")]
        public Guid NetworkID { get; set; }

        [Display(Name = "شیوه جذب")]
        public string SuctionModeName { get; set; }

        [Display(Name = "جزئیات شیوه جذب")]
        public string SuctionModeDetailName { get; set; }

        [Display(Name = "شناسه جزئیات شیوه جذب")]
        public Guid SuctionModeDetailID { get; set; }

        [Display(Name = "شناسه شیوه جذب")]
        public Guid SuctionModeID { get; set; }

        [Display(Name = "وضعیت پیگیری")]
        public string FollowStatusName { get; set; }

        [Display(Name = "شناسه وضعیت پیگیری")]
        public Guid FollowStatusID { get; set; }

        [Display(Name = "احتمال خرید")]
        public string BuyPossibilityName { get; set; }

        [Display(Name = "شناسه احتمال خرید")]
        public Guid BuyPossibilityID { get; set; }

        [Display(Name = "وضعیت مدارک")]
        public string DocumentStatusName { get; set; }

        [Display(Name = "شناسه وضعیت مدارک")]
        public Guid DocumentStatusID { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفاَ {0} را وارد کنید.")]
        public string LastName { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        public string FirstName { get; set; }

        /// <summary>
        /// جنسیت
        /// </summary>
        public string Gender { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        public string Name { get; set; }

        [Display(Name = "تاریخ تولد")]
        public string BirthDate { get; set; }

        [Display(Name = "شغل")]
        public string Job { get; set; }

        [Display(Name = "تلفن")]
        public string Phone { get; set; }

        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفاَ {0} را وارد کنید.")]
        public string Mobile1 { get; set; }

        [Display(Name = "تلفن همراه 2")]
        public string Mobile2 { get; set; }

        [Display(Name = "نام خانوادگی صاحب خط")]
        public string SLastName { get; set; }

        [Display(Name = "نام صاحب خط")]
        public string SFirstName { get; set; }

        [Display(Name = "شماره تلفن (ADSL)")]
        [Required(ErrorMessage = "لطفاَ {0} را وارد کنید.")]
        public string ADSLPhone { get; set; }

        [Display(Name = "نوع مشتری")]
        public string LegalType { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }

        [Display(Name = "قفل")]
        public bool Locked { get; set; }

        [Display(Name = "توضیحات")]
        public string LockEmployeeName { get; set; }

        public Guid LockEmployeeID { get; set; }

        [Display(Name = "توضیحات قفل")]
        public string LockNote { get; set; }

        [Display(Name = "غیرفعال")]
        public bool Discontinued { get; set; }

        [Display(Name = "شناسه چرخه")]
        public Guid LevelTypeID { get; set; }

        [Display(Name = "چرخه مشتری")]
        public string LevelTypeTitle { get; set; }

        [Display(Name = "مرحله مشتری")]
        public Guid LevelID { get; set; }

        //public LevelView Level { get; set; }

        [Display(Name = "مرحله مشتری")]
        public string LevelTitle { get; set; }

        // تاریخ ورود به مرحله
        public string LevelEntryDate { get; set; }

        [Display(Name = "نام مستعار مرحله")]
        public string LevelNikname { get; set; }

        [Display(Name = "مرکز خالی باشد")]
        public bool SkipCenter { get; set; }

        // آیا میتوان این مشتری را حذف کرد؟
        public bool CanDelete { get; set; }

        // اعتبار مشتری
        public long Balance { get; set; }

        // مبلغ قابل تحویل
        public long CanDeliverCost { get; set; }



        public LevelOptionsView LevelOptionsView { get; set; }

        public string LastChangLevelDate { get; set; }

        public string Picture { get; set; }

        // IEnumerables

        //public virtual IEnumerable<CustomerLevelView> CustomerLevels { get; protected set; }

        public int CurrentLevelWaitingDays {
            get
            {
                if(this.LevelEntryDate != null)
                return PersianDateTime.DateDiff( this.LevelEntryDate,PersianDateTime.Now);
                return 0;
            }
        }

        //{
        //    get
        //    {
        //        string temp = CustomerLevels.FirstOrDefault().CreateDate;
        //        var list = CustomerLevels.ToArray();
        //        IList<CustomerLevelView> data = new List<CustomerLevelView>();
        //        if (list.Count() == 1)
        //        {
        //            list[0].WaitingDays = PersianDateTime.DateDiff(CreateDate, list[0].CreateDate);
        //        }
        //        for (int i = 1; i <= list.Length - 1; i++)
        //        {
        //            if (i == 1)
        //                list[i].WaitingDays = PersianDateTime.DateDiff(temp, list[i].CreateDate);
        //            else
        //            {
        //                list[i].WaitingDays = PersianDateTime.DateDiff(list[i - 1].CreateDate, list[i].CreateDate);
        //            }
        //        }

        //        if (CustomerLevels != null)
        //        {
        //            foreach (var customerLevel in list)
        //            {
        //                if (customerLevel.LevelID == this.LevelID)
        //                    data.Add(customerLevel);

        //            }
        //            return data.LastOrDefault().WaitingDays;
        //        }
        //        return 0;
        //    }
        //}

        /// <summary>
        /// عکس
        /// </summary>

        //[Display(Name = "مدارک مشتری")]
        //public IEnumerable<DocumentView> Documents { get; protected set; }

        //[Display(Name = "ایمیلها")]
        //public IEnumerable<EmailView> Emails { get; protected set; }

        //[Display(Name = "نت ها")]
        //public IEnumerable<NoteView> Notes { get; protected set; }

        //[Display(Name = "پیامکها")]
        //public IEnumerable<SmsView> Smss { get; protected set; }

        //[Display(Name = "امور مالی")]
        //public IEnumerable<FiscalView> Fiscals { get; protected set; }

        //[Display(Name = "فروشها")]
        //public IEnumerable<SaleView> Sales { get; protected set; }

        //[Display(Name = "پشتیبانی های حضوری از مشتری")]
        //public IEnumerable<PersenceSupportView> PersenceSupports { get; protected set; }

        //[Display(Name = "مشکلات")]
        //public IEnumerable<ProblemView> Problems { get; protected set; }
    }

    ///////////////////////////////////////////////////////////////
    public class CustomerView1 //: BaseView
    {
        public string FirstName { get; set; }
    }
}
