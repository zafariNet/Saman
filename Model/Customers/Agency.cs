using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;

namespace Model.Customers
{
    /// <summary>
    /// موجودیت نمایندگی فروش
    /// </summary>
    public class Agency : EntityBase, IAggregateRoot
    {
        public Agency()
        {
            Address = "";
            ManagerName = "ثبت نشده";

        }
        /// <summary>
        /// نام نمایندگی
        /// </summary>
        public virtual string AgencyName { get; set; }
        /// <summary>
        /// نام مدیر نمایندگی
        /// </summary>
        public virtual string ManagerName { get; set; }
        /// <summary>
        /// تلفن 1
        /// </summary>
        public virtual string Phone1 { get; set; }
        /// <summary>
        /// تلفن 2
        /// </summary>
        public virtual string Phone2 { get; set; }
        /// <summary>
        /// تلفن همراه
        /// </summary>
        public virtual string Mobile { get; set; }
        /// <summary>
        /// آدرس
        /// </summary>
        public virtual string Address { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public virtual int SortOrder { get; set; }
        /// <summary>
        /// قطع همکاری - به جای پاک کردن به کار می رود
        /// </summary>
        public virtual bool Discontinued { get; set; }

        /// <summary>
        /// مشتریان
        /// </summary>
        //public virtual IEnumerable<Customer> Customers { get; protected set; }
        //public virtual int CustomerCount { get; protected set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.AgencyName == null)
                base.AddBrokenRule(AgencyBusinessRules.AgencyNameRequired);
        }
    }
}
