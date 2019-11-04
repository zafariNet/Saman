using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Services.ViewModels.Customers;
using Services.ViewModels.Fiscals;
using Model.Employees;

namespace Services.ViewModels.Employees
{
    public class EmployeeView : BaseView
    {
        public Guid ParentEmployeeID { get; set; }

        [Display(Name = "کارمند بالادستی")]
        public string ParentEmployeeName { get; set; }

        public Guid GroupID { get; set; }

        [Display(Name = "نام گروه")]
        public string GroupName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام کارمند")]
        public string Name { get; set; }

        [Display(Name = "تلفن ثابت")]
        public string Phone { get; set; }

        [Display(Name = "تلفن همراه")]
        public string Mobile { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "تاریخ تولد")]
        public string BirthDate { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }

        [Display(Name = "کارشناس نصب")]
        public bool InstallExpert { get; set; }

        [Display(Name = "نام کاربری")]
        public string LoginName { get; set; }

        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [Display(Name = "تاریخ استخدام")]
        public string HireDate { get; set; }

        public  IList<Employee>  AllChildEmployees { get; set; }

        /// <summary>
        /// آخرین ارسال خطرناک
        /// </summary>
        public int LastDangerQueueNumber { get; set; }

        [Display(Name = "دسترسیها")]
        public IEnumerable<PermitView> Permissions { get; set; }

        // قابلیت دسترسی
        public virtual bool IsGuaranteed(string PermitKey)
        {       
            if (Permissions == null)
                return false;

            var permit = (from p in Permissions
                          where p.PermitKey == PermitKey
                          select p).FirstOrDefault();

            if (permit == null)
                return false;

            return permit.Guaranteed;
        }

        [Display(Name = "قطع همکاری")]
        public bool Discontinued { get; set; }

        /// <summary>
        /// تصویر
        /// </summary>
        public string Picture { get; set; }

        public string Photo { get {
            if (Picture != null && Picture.IndexOf("data")>0)
            return Picture.Replace(@"\", "/").Substring(Picture.IndexOf("data"));
            return string.Empty;
        } }

        //لیست صف ها
        public IEnumerable<QueueView>  Queues { get; set; }
        //IEnumerables

        //[Display(Name = "مشتریان")]
        //public IEnumerable<CustomerView> OwnCustomers { get; protected set; }

        //[Display(Name = "پرسمان مشتری ها")]
        //public IEnumerable<QueryEmployeeView> QueryEmployees { get; protected set; }

        //[Display(Name = "کارمندان زیر مجموعه")]
        //public IEnumerable<EmployeeView> ChildEmployees { get; protected set; }

        //[Display(Name = "امور مالی")]
        //public IEnumerable<FiscalView> Fiscals { get; protected set; }

        //[Display(Name = "حسابهای کارمندان")]
        //public IEnumerable<Fiscals.MoneyAccountEmployeeView> MoneyAccountEmployees { get; protected set; }

        //[Display(Name = "فروشها")]
        //public IEnumerable<Sales.SaleView> Sales { get; protected set; }

        //[Display(Name = "پشتیبانی حضوری")]
        //public IEnumerable<Support.PersenceSupportView> PersenceSupports { get; protected set; }

        //[Display(Name = "تحویل خدمات اعتباری")]
        //public IEnumerable<Store.CreditServiceDeliveryView> CreditServiceDeliverys { get; protected set; }

        //[Display(Name = "تحویل کالاها")]
        //public IEnumerable<Store.ProductDeliveryView> ProductDeliverys { get; protected set; }

        //[Display(Name = "انبارها")]
        //public IEnumerable<Store.StoreView> Stores { get; protected set; }

        //[Display(Name = "تحویل خدمات غیر اعتباری")]
        //public IEnumerable<Store.UncreditServiceDeliveryView> UncreditServiceDeliverys { get; protected set; }
    
    }
}
