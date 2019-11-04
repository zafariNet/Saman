using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SaleCatalogService
{
    public class EditCampignPaymentRequest:AddCampignPaymentRequest
    {
        public Guid ID { get; set; }

        public int RowVersion { get; set; }
    }
}
