using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SupportCatalogService
{
    public class AddSupportStatusRequest
    {
        public string SupportStatusName { get; set; }
        public bool IsFirstSupportStatus { get; set; }
        public bool IsLastSupportState { get; set; }
        public string SmsText { get; set; }
        public string EmailText { get; set; }
        public bool SendSmsOnEnter { get; set; }
        public bool SendEmailOnEnter { get; set; }
    }
}
