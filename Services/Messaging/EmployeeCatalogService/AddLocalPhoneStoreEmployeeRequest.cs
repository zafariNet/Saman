using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class AddLocalPhoneStoreEmployeeRequest
    {

        public Guid LocalPhoneStoreID { get; set; } 

        public int DangerousSeconds { get; set; }

        public int DangerousRing { get; set; }

        public bool SendSmsToOnLineUserOnDangerous { get; set; }

        public virtual bool SendSmsToOffLineUserOnDangerous { get; set; }

        public virtual string SmsText { get; set; }
    }
}
