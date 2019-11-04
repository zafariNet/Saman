using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class QueryDetailView : BasePageView
    {
        public QueryView QueryView { get; set; }

        public IEnumerable<QueryView> QueryViews { get; set; }
    }
}
