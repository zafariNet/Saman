using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SupportCatalogService
{
    public class AddSupportRequest
    {

        public Guid CustomerID { get; set; }
        /// <summary>
        /// عنوان پشتیبانی
        /// </summary>
        public string SupportTitle { get; set; }

        /// <summary>
        /// توضیحات پشتیبانی
        /// </summary>
        public string SupportComment { get; set; }

        /// <summary>
        /// تایید شده؟
        /// </summary>
        public bool Confirmed { get; set; }

        /// <summary>
        /// ایجاد توسط
        /// </summary>
        public int CreateBy { get; set; }

        /// <summary>
        /// شناسه وضعیت
        /// </summary>
        public Guid SupportStatusRelationID { get; set; }

        public Guid SupportStatusID { get; set; }
    }
}
