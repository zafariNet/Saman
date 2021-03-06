﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SupportCatalogService
{
    public class AddSupportInstallationDelayRequest
    {

        public Guid SupportID { get; set; }

        public Guid SupportStatusID { get; set; }

        public string InstallDate { get; set; }

        public string NextCallDate { get; set; }

        public string Comment { get; set; }

        public bool SendNotificationToCustomer { get; set; }
    }
}
