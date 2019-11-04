using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Store.Validations;
using Model.Customers;

namespace Model.Store
{
    public class Network : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// نام شبکه
        /// </summary>
        public virtual string NetworkName { get; set; }

        /// <summary>
        /// موجودی شبکه که در دیتابیس ذخیره نمی شود
        /// </summary>
        public virtual long Balance
        {
           get;set;
        }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }
        /// <summary>
        /// در صورتی که اعتبار شبکه کافی نباشد آیا امکان تحویل شارژ به مشتری وجود داشته باشد یا خیر؟
        /// </summary>
        public virtual bool DeliverWhenCreditLow { get; set; }
        /// <summary>
        /// فعال یا غیر فعال بودن شبکه
        /// </summary>
        public virtual bool Discontinued { get; set; }
        /// <summary>
        /// مرتب سازی
        /// </summary>
        public virtual int SortOrder { get; set; }

        /// <summary>
        /// نام نمایشی شبکه
        /// </summary>
        public virtual string Alias { get; set; }
        /// <summary>
        /// مشتریان
        /// </summary>
        public virtual IEnumerable<Customer> Customers { get; protected set; }
        /// <summary>
        /// شبکه مرکز ها
        /// </summary>
        public virtual IEnumerable<NetworkCenter> NetworkCenters { get; set; }
        /// <summary>
        /// تراکنشهای شبکه
        /// </summary>
        public virtual IEnumerable<NetworkCredit> NetworkCredits { get; set; }
        /// <summary>
        /// خدمات اعتباری
        /// </summary>
        public virtual IEnumerable<CreditService> CreditServices { get; protected set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.NetworkName == null)
                base.AddBrokenRule(NetworkBusinessRules.NetworkNameRequired);
        }

    }
}
