using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SupportCatalogService
{
    public class EditSupportPhoneInstallationRequst : AddSupportPhoneInstallationRequst
    {
        public Guid ID { get; set; }

        public int RowVersion { get; set; }
    }
}
