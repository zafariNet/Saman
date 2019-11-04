using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class AddQueueLocalPhoneRequest
    {
        public Guid QueueID { get; set; }
        public bool SendSms { get; set; }
        public int DangerousSeconds { get; set; }
        public int DangerousRing { get; set; }
        public bool SendSmsToOnLineUserOnDangerous { get; set; }
        public bool SendSmsToOffLineUserOnDangerous { get; set; }
        public string Smstext { get; set; }
        public bool CanViewQueue { get; set; }
    }
}
