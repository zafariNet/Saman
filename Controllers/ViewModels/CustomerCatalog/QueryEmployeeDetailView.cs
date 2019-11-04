using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;
using Services.ViewModels.Employees;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class QueryEmployeeDetailView : BasePageView
    {
        public IEnumerable<QueryEmployeeView> QueryEmployeeViews { get; set; }

        public QueryView QueryView { get; set; }  // For QueryEmployee View needed

        public IEnumerable<EmployeeView> EmployeeViews { get; set; }

        public EmployeeView EmployeeViewForInsert { get; set; }  // For Insert data needed
    }
}
