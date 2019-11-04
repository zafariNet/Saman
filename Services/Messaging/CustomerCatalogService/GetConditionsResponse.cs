using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Services.Messaging.CustomerCatalogService
{
    public class GetConditionsResponse
    {
        public IEnumerable<ConditionView> ConditionViews { get; set; }

        public int Count { get; set; }
    }
}
