using Infrastructure.Domain;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Employees
{
    /// <summary>
    /// موجودیت مخزن شماره های داخلی
    /// </summary>
    public class LocalPhoneStore:IAggregateRoot
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public virtual Guid ID { get; set; }

        /// <summary>
        /// شناسه استریسک
        /// </summary>
        public virtual string AsteriskID { get; set; }

        /// <summary>
        /// داخلی
        /// </summary>
        public virtual string LocalPhoneStoreNumber { get; set; }

        /// <summary>
        /// رزرو شده
        /// </summary>
        public virtual bool Reserved { get; set; }


    }
}
