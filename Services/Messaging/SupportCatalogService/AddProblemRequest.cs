using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SupportCatalogService
{
    public class AddProblemRequest
    {
        public Guid CustomerID { get; set; }
        public string ProblemTitle { get; set; }
        public string ProblemDescription { get; set; }
        public short Priority { get; set; }
        public short State { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }
}
