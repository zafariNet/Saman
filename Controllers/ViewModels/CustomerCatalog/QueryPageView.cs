using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;
using Services.ViewModels.Employees;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class QueryPageView : BasePageView
    {
        public QueryView QueryView { get; set; }

        public IEnumerable<QueryView> QueryViews { get; set; }
        /// <summary>
        /// کارمندان مجاز به استفاده از نما
        /// </summary>
        public IEnumerable<EmployeeView> EmployeeViews { get; set; }
    }
}
