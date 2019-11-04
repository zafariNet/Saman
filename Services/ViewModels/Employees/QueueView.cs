using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Employees
{
    public class QueueView
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// شناسه استریسک
        /// </summary>
        public int AsteriskID { get; set; }

        /// <summary>
        /// نام صف
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// نام فارسی
        /// </summary>
        public string PersianName { get; set; }
    }
}
