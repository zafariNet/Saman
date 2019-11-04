using System.Collections.Generic;
using Services.ViewModels.Customers;

namespace Services.Messaging.CustomerCatalogService
{
    public class GetDocumentsResponse
    {
        public IEnumerable<DocumentView> DocumentViews { get; set; }
    }
}
