using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SupportCatalogService
{
    public class EditSupportInstallationDelayRequest : AddSupportInstallationDelayRequest
    {
        public Guid ID { get; set; }

        public int RowVersion { get; set; }
    }
}
