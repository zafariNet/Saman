using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;

namespace Model.Customers
{
    /// <summary>
    /// موجودیت مشتری ساده
    /// </summary>
    public class SimpleCustomer : IAggregateRoot
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public virtual Guid CustomerID { get; set; }

        /// <summary>
        /// نام
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// شماره تلفن
        /// </summary>
        public virtual string ADSLPhone { get; set; }

        /// <summary>
        /// مرحله
        /// </summary>
        public virtual string  LevelTitle { get; set; }
    }
}
