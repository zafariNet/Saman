using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class AddQueueRequest
    {

        public int AsteriskID { get; set; }

        public string QueueName { get; set; }

        public string PersianName { get; set; }
    }
}
