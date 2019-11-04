using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Services.Implementations
{
    public static class AppPath
    {
        public static string ApplicationPath
        {
            get
            {
                    return HttpRuntime.AppDomainAppPath;

            }
        }

        private static bool isInWeb
        {
            get
            {
                return HttpContext.Current != null;
            }
        }
    }
}
