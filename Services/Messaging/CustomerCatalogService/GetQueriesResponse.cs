#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

#endregion

namespace Services.Messaging.CustomerCatalogService
{
    public class GetQueriesResponse
    {
        public IEnumerable<QueryView> QueryViews { get; set; }

        public int TotalCount { get; set; }
    }
}
