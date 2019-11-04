using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Employees
{
    public class MessageTemplateView : BaseView
    {
        /// <summary>
        /// عنوان قالب
        /// </summary>
        public string MessageTemplateName { get; set; }
        /// <summary>
        /// متن پیامک
        /// </summary>
        public string MessageSmsTemplateText { get; set; }

        /// <summary>
        /// متن ایمیل
        /// </summary>
        public string MessageEmailTemplateText { get; set; }
        /// <summary>
        /// نوع قالب
        /// </summary>
        public int MessageType { get; set; }
    }
}
