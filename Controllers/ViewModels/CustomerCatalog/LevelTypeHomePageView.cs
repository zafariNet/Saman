using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class LevelTypeHomePageView : BasePageView
    {
        public IEnumerable<LevelTypeView> LevelTypeViews { get; set; }

        public int Count { get; set; }
    }
}
