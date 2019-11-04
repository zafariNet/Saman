using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class CustomerHomePageView : BasePageView
    {
        public IEnumerable<CustomerView> CustomerViews { get; set; }

        public int Count { get; set; }

        public QueryView QueryView { get; set; }

        public IEnumerable<QueryView> QueryViews { get; set; }
    }
}
