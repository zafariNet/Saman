using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Domain;
using Model.Base;

namespace Model.Employees
{
    /// <summary>
    /// تمامی آیتمهای قابل کنترل جهت دسترسی کارمندان در این موجودیت ذخیره می شود.
    /// </summary>
    public class Permission : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// عنوان
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// نوع
        /// </summary>
        public virtual string Group { get; set; }
        /// <summary>
        /// کلید (استفاده در کدنویسی)
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {

        }  
    }
}
