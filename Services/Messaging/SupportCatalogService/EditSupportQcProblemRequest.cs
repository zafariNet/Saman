using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SupportCatalogService
{
    public class EditSupportQcProblemRequest:AddSupportQcProblemRequest
    {
        public Guid ID { get; set; }

        public int Rowversion { get; set; }
    }
}
