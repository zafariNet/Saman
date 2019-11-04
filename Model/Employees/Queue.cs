using Infrastructure.Domain;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Employees
{
    /// <summary>
    /// موجودیت صف
    /// </summary>
    public class Queue:IAggregateRoot
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public virtual Guid ID { get; set; }
        
        /// <summary>
        /// شناسه استریسک
        /// </summary>
        public virtual int AsteriskID { get; set; }
        
        /// <summary>
        /// نام صف
        /// </summary>
        public virtual string QueueName { get; set; }
        
        /// <summary>
        /// نام فارسی
        /// </summary>
        public virtual string PersianName { get; set; }
    }
}
