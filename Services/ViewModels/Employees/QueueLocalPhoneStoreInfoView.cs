using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Employees
{
    public class QueueLocalPhoneStoreInfoView
    {
        public Guid EmployeeID { get; set; }
        public string Mobile { get; set; }
        public string QueueName { get; set; }
        public int DangerousRing { get; set; }
        public bool SendSmsToOffLineUserOnDangerous { get; set; }
        public bool SendSmsToOnLineUserOnDangerous { get; set; }
        public string SmsText { get; set; }
        public bool CanViewQueue { get; set; }
        public bool SendSMS { get; set; }
    }

    public class QueueLocalPhoneStoreInfoView1
    {
       public Guid EmployeeID { get; set; }
        public bool ViewAlarm { get; set; }
    }
}
