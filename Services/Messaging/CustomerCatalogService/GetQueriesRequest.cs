using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging
{
    public class GetQueriesRequest
    {
        /// <summary>
        /// کارمند جاری برای چک کردن اینکه این کارمند کدام نماها را می تواند ببیند
        /// </summary>
        public Guid EmployeeID { get; set; }
    }
}
