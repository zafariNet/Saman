using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Infrastructure.Configuration
{
    public class WebConfigApplicationSettions : IApplicationSettings
    {
        public string LoggerName
        {
            get { return ConfigurationManager.AppSettings["LoggerName"]; }
        }
    }
}
