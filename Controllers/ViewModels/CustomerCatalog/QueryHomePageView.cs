using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class QueryHomePageView : BasePageView
    {
        public IEnumerable<QueryView> QueryViews { get; set; }

        public int Count { get; set; }
    }
}
