using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Employees.Validations;

namespace Model.Employees
{
    public  class SmsEmployee : IAggregateRoot
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public virtual Guid ID { get; set; }

        /// <summary>
        /// تاریخ ارسال
        /// </summary>
        public virtual string CreateDate { get; set; }
        /// <summary>
        /// کارمند جاری
        /// </summary>
        public virtual Employee Employee { get; set; }

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

    }
}
