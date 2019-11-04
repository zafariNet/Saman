using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Employees
{
    public class LocalPhoneStoreView
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public virtual Guid ID { get; set; }

        /// <summary>
        /// شناسه استریسک
        /// </summary>
        public string AsteriskID { get; set; }

        /// <summary>
        /// داخلی
        /// </summary>
        public string LocalPhoneNumber { get; set; }

        /// <summary>
        /// رزرو شده
        /// </summary>
        public bool Reserved { get; set; }
    }
}
