using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class AddLocalPhoneStoreRequest
    {

        public int AsteriskID { get; set; }

        public string LocalPhoneNumber { get; set; }

        public bool Reserved { get; set; }
    }
}
