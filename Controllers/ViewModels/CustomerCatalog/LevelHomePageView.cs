using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class LevelHomePageView : BasePageView
    {
        public LevelView LevelView { get; set; }

        public IEnumerable<LevelView> LevelViews { get; set; }

        public int Count { get; set; }
    }
}
