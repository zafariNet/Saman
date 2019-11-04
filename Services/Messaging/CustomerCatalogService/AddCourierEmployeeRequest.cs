using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddCourierEmployeeRequest
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }
    }
}
