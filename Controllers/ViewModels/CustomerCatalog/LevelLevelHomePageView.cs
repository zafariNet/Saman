using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class LevelLevelHomePageView : BasePageView
    {
        public IEnumerable<LevelLevelView> LevelLevelViews { get; set; }

        public LevelView LevelView { get; set; }
    }
}
