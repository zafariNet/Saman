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
    /// پست الکترونیک
    /// </summary>
    public class Email : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// مشتری
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// موضوع ایمیل
        /// </summary>
        public virtual string Subject { get; set; }
        /// <summary>
        /// متن ایمیل
        /// </summary>
        public virtual string Body { get; set; }
        /// <summary>
        /// آیا ایمیل فرستاده شده است؟
        /// </summary>
        public virtual bool Sent { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {

        }  
    }
}
