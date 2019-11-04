using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Support
{
    public class SupportStatusRelationView:BaseView

    {

        /// <summary>
        /// وضعیتی که یک پشتیبانی دارد
        /// </summary>
        public string SupportStatusName { get; set; }

        public Guid SupportStatusID { get; set; }
        /// <summary>
        /// وضعیتی که پشتیبانی به آن ارسال میشود    
        /// </summary>
        public string RelatedSupportStatusName { get; set; }

        public Guid RelatedSupportStatusID { get; set; }

        public string Key { get; set; }
    }
}
