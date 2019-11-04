using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Services.Messaging.CustomerCatalogService
{
    public class GetEmailsResponse
    {
        public IEnumerable<EmailView> EmailViews { get; set; }

        public int TotalCount { get; set; }
    }
}
