using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Services.Messaging.CustomerCatalogService
{
    public class GetLevelConditionsResponse
    {
        public IEnumerable<LevelConditionView> LevelConditionViews { get; set; }
        public int Count { get; set; }
    }
}
