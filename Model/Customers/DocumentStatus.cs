#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;
#endregion

namespace Model.Customers
{
    /// <summary>
    /// موجودیت وضعیت مدارک
    /// </summary>
    public class DocumentStatus : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// وضعیت مدارک
        /// </summary>
        public virtual string DocumentStatusName { get; set; }
        /// <summary>
        /// مشخص می کند که آیا این وضعیت بصورت پیش فرض در بدو ورود به مرحله مدارک انتخاب می شود یا خیر
        /// </summary>
        public virtual bool DefaultStatus{ get; set; }
        /// <summary>
        /// آیا انتخاب این وضعیت نشانگر کامل بودن مدارک است یا خیر
        /// </summary>
        public virtual bool CompleteStatus { get; set; }
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public virtual int? SortOrder { get; set; }

        /// <summary>
        /// مشتریان
        /// </summary>
        //public virtual IEnumerable<Customer> Customers { get; protected set; }

        #region Validation
        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.DocumentStatusName == null)
                base.AddBrokenRule(DocumentStatusBusinessRules.DocumentStatusNameRequired);
        }
        #endregion
    }
}
