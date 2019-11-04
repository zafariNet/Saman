using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Services.Messaging.CustomerCatalogService
{
    public class GetQueryEmployeesResponse
    {
        public IEnumerable<QueryEmployeeView> QueryEmployeeViews { get; set; }
    }
}
