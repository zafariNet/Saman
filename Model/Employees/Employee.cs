#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Infrastructure.Domain;
using Infrastructure.Encrypting;
using Model.Base;
using Model.Employees.Validations;
using Model.Customers;
using Model.Fiscals;
using Model.Sales;
#endregion

namespace Model.Employees
{
    /// <summary>
    /// موجودیت کارمند
    /// </summary>
    public class Employee : EntityBase, IAggregateRoot
    {
        readonly EncryptionService _encryptionService = new EncryptionService();


        #region Properties
        /// <summary>
        /// کارمند رئیس یا بالادستی
        /// </summary>
        public virtual Employee ParentEmployee { get; set; }

        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public virtual string LastName { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// نام کارمند
        /// </summary>
        public virtual string Name { get { return FirstName + " " + LastName; } }
        /// <summary>
        /// گروه کاربری
        /// </summary>
        public virtual Group Group { get; set; }
        /// <summary>
        /// دسترسیها
        /// </summary>
        public virtual IEnumerable<Permit> Permissions { get; set; }
        /// <summary>
        /// تلفن
        /// </summary>
        public virtual string Phone { get; set; }
        /// <summary>
        /// همراه
        /// </summary>
        public virtual string Mobile { get; set; }
        /// <summary>
        /// آدرس
        /// </summary>
        public virtual string Address { get; set; }
        /// <summary>
        /// تاریخ تولد
        /// </summary>
        public virtual string BirthDate { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }
        /// <summary>
        /// کارشناس نصب می باشد یا خیر
        /// </summary>
        public virtual bool InstallExpert { get; set; }
        /// <summary>
        /// نام کاربری برای ورود به برنامه
        /// </summary>
        public virtual string LoginName { get; set; }
        /// <summary>
        /// رمز عبور
        /// </summary>
        public virtual string Password
        {
            get
            {
                return _encryptionService.DecryptObject<string>(EncryptedPassword, ID.ToString());
            }

            set
            {
                EncryptedPassword = _encryptionService.EncryptString(value);
            }
        }

        public virtual string EncryptedPassword { get; set; }

        //public virtual string Password { get; set; }
        /// <summary>
        /// تاریخ استخدام
        /// </summary>
        public virtual string HireDate { get; set; }
        /// <summary>
        /// قطع همکاری با شرکت - به جای حذف به کار می رود
        /// </summary>
        public virtual bool Discontinued { get; set; }
        

        /// <summary>
        /// عکس
        /// </summary>
        public virtual string Picture { get; set; }

        #endregion

        #region Methods

        public virtual bool CanView(Query query)
        {
            QueryEmployee queryEmployee = new QueryEmployee();
            queryEmployee.Query = query;
            queryEmployee.Employee = this;

            return QueriesThisEmployeeCanSee.Contains(queryEmployee);
        }

        #endregion



        #region IEnuberables


        //public virtual IEnumerable<Customer> OwnCustomers { get; protected set; }
        /// <summary>
        ///  پرسمان مشتری ها
        /// </summary>
        //public virtual IEnumerable<QueryEmployee> QueryEmployees { get; protected set; }
        /// <summary>
        /// کارمندان زیر مجموعه
        /// </summary>
        public virtual IEnumerable<Employee> ChildEmployees { get; protected set; }
        /// <summary>
        /// امور مالی
        /// </summary>
        //public virtual IEnumerable<Fiscal> Fiscals { get; protected set; }
        /// <summary>
        /// حسابهای کارمندان
        /// </summary>
        //public virtual IEnumerable<MoneyAccountEmployee> MoneyAccountEmployees { get; protected set; }
        /// <summary>
        /// فروشها
        /// </summary>
        //public virtual IEnumerable<Sale> Sales { get; protected set; }
        /// <summary>
        /// پشتیبانی های حضوری
        /// </summary>
        //public virtual IEnumerable<Support.PersenceSupport> PersenceSupports { get; protected set; }
        /// <summary>
        /// انبارها
        /// </summary>
        public virtual IEnumerable<Store.Store> Stores { get; set; }
        /// <summary>
        /// نماهایی که این مشتری می تواند ببیند
        /// </summary>
        public virtual IEnumerable<QueryEmployee> QueriesThisEmployeeCanSee { get; set; }

        public virtual IEnumerable<Notification> Notifications { get; set; }

        
        #endregion

        #region Validation
        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.LastName == null)
                base.AddBrokenRule(EmployeeBusinessRules.LastNameRequired);
            if (this.FirstName == null)
                base.AddBrokenRule(EmployeeBusinessRules.FirstNameRequired);
            if (this.LoginName == null)
                base.AddBrokenRule(EmployeeBusinessRules.LoginNameRequired);
            if (this.Group == null)
                base.AddBrokenRule(EmployeeBusinessRules.GroupRequired);
            //if (this.Permissions == null)
            //    base.AddBrokenRule(EmployeeBusinessRules.PermissionsRequired);
        }
        #endregion

        //public virtual IList<Employee>  AllChildEmployees { get { return GetAllChild(); } }

        #region Get All Child

        public virtual IList<Employee> GetAllChild()
        {
            IList<Employee> emp=new List<Employee>();
            foreach (var item in ChildEmployees)
            {
                emp.Add(item);
                var temp=item.GetAllChild();
                foreach(var _item in temp)
                    emp.Add(_item);
            }
            return emp;
        }

        #endregion


    }
}
