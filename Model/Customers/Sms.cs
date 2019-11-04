using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;


namespace Model.Customers
{
    public class Sms : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// مشتری
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// متن پیام کوتاه
        /// </summary>
        public virtual string Body { get; set; }
        /// <summary>
        /// پس از فرستاده شدن مقدار آن TRUE میشود
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
