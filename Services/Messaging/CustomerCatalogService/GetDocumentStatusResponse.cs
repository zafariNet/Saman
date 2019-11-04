using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Services.Messaging.CustomerCatalogService
{
    public class GetDocumentStatusResponse
    {
        public DocumentStatusView DocumentStatusView { get; set; }
    }
}
