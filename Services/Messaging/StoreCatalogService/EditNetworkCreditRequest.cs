﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.StoreCatalogService
{
    public class EditNetworkCreditRequestOld : AddNetworkCreditRequestOld
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

        public Guid ModifiedEmployeeID { get; set; }
    }
    public class EditNetworkCreditRequest : AddNetworkCreditRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

    }
}
