using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SupportCatalogService
{
    public class AddSupportTicketWaitingResponseRequest
    {

        public Guid SupportID { get; set; }

        public Guid SupportStatusID { get; set; }

        public string SendTicketDate { get; set; }

        public string TicketNumber { get; set; }

        public string ResponsePossibilityDate { get; set; }

        public bool SendNotificationToCustomer { get; set; }

        public string Comment { get; set; }
    }
}
