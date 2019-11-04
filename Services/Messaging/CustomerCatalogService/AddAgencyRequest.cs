using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddAgencyRequestOld
    {
        public string AgencyName { get; set; }
        public string ManagerName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public Guid CreateEmployeeID { get; set; }
        public bool Discontinued { get; set; }
    }
    public class AddAgencyRequest
    {
        public string AgencyName { get; set; }
        public string ManagerName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public bool Discontinued { get; set; }
    }
}
