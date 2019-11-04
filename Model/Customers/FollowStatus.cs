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
    /// موجودیت وضعیت پیگیری
    /// </summary>
    public class FollowStatus : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// وضعیت پیگیری 
        /// </summary>
        public virtual string FollowStatusName { get; set; }
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public virtual int? SortOrder { get; set; }

        /// <summary>
        /// مشتریان
        /// </summary>
        //public virtual IEnumerable<Customer> Customers { get; protected set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.FollowStatusName == null)
                base.AddBrokenRule(FollowStatusBusinessRules.FollowStatusNameRequired);
        }
    }
}
