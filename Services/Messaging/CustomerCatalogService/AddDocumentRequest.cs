using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddDocumentRequest
    {
        public Guid CustomerID { get; set; }
        public string DocumentName { get; set; }
        public string Photo { get; set; }
        public string ImageType { get; set; }
        //public string ReceiptDate { get; set; }
        public string Note { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }
}
