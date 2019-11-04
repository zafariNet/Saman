using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Leads
{
    public class LeadTitleTemplateView:BaseView
    {
        /// <summary>
        /// عنوانی که در لیست نمایش داده میشود
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// آیا با انتخاب این قالب مذاکره بسته شود یا نه
        /// </summary>
        public bool CloseLeadConversation { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }
        public Guid GroupID { get; set; }
    }
}
