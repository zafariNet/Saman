using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SupportCatalogService
{
    public class AddSupportDeliverServiceRequest
    {

        public Guid SupportID { get; set; }

        public Guid SupportStatusID { get; set; }

        public string DeliverDate { get; set; }

        public string TimeInput { get; set; }

        public string TimeOutput { get; set; }

        public long AmountRecived { get; set; }

        public string Comment { get; set; }

        public bool SendNotificationToCustomer { get; set; }
    }
}
