using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class SmtpSettingsModel
    {
        public string Host { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }

        public bool EnableSsl { get; set; }
    }
}
