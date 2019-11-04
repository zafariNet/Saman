using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Employees
{
    public class SmsEmployeeView
    {
        public Guid ID { get; set; }
        public Guid OwneremployeeID { get; set; }
        public string OwnerEmployeeName { get; set; }
        public string CreateDate { get; set; }
        public string Body { get; set; }
        public string Note { get; set; }
    }
}
