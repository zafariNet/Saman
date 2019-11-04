using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Support
{
    public class SupportStatusView:BaseView
    {
        /// <summary>
        /// نام وضعیت پشتیبانی
        /// </summary>
        public string SupportStatusName { get; set; }

        /// <summary>
        /// کلید
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// وضعیت های وابسته
        /// </summary>
        public IEnumerable<SupportStatusRelationView> SuportStatusRelations { get; set; }

        /// <summary>
        /// آیا از وضعیت های اولیه است ؟
        /// </summary>
        public bool IsFirstSupportStatus { get; set; }

        /// <summary>
        /// آیا از وضعیت های نهایی است؟
        /// </summary>
        public bool IsLastSupportState { get; set; }
        /// <summary>
        /// متن ارسال توسط پیامک
        /// </summary>
        public string SmsText { get; set; }

        /// <summary>
        /// متن ارسال توسط ایمیل
        /// </summary>
        public string EmailText { get; set; }

        /// <summary>
        /// ؟آیا به محض ورود پیامک ارسال شود
        /// </summary>
        public bool SendSmsOnEnter { get; set; }

        /// <summary>
        /// آیا به محض ورود ایمیل ارسال شود؟
        /// </summary>
        public bool SendEmailOnEnter { get; set; }
    }
}
