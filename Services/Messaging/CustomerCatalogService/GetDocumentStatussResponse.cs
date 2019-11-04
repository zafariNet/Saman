using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Services.Messaging.CustomerCatalogService
{
    public class GetDocumentStatussResponse
    {
        public IEnumerable<DocumentStatusView> DocumentStatusViews { get; set; }
    }
}
