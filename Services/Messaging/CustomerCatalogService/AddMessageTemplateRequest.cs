using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddMessageTemplateRequest
    {
        public string MessageTemplateName { get; set; }
        public string MessageEmailTemplateText { get; set; }
        public string MessageSmsTemplateText { get; set; }
        public bool enableemail { get; set; }
        public bool enablesms { get; set; }

    }
}
