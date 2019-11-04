using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SaleCatalogService
{
    public class AddCampignPaymentRequest
    {

        public Guid SuctionModeDetailID { get; set; }

        

        public string PaymentDate { get; set; }

        public long Amount { get; set; }

        public string Note { get; set; }
    }
}
