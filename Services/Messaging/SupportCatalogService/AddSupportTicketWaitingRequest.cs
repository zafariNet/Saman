using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SupportCatalogService
{
    public class AddSupportTicketWaitingRequest
    {

        public Guid SupportID { get; set; }

        public Guid SupportStatusID { get; set; }

        public string DateOfPersenceDate { get; set; }

        public string WireColor { get; set; }

        public string Snr { get; set; }

        public string Selt { get; set; }

        public Guid InstallExpertID { get; set; }

        public string TicketSubject { get; set; }

        public bool SourceWireCheck { get; set; }

        public string Comment { get; set; }

        public bool SendNotificationToCustomer { get; set; }
    }
}
