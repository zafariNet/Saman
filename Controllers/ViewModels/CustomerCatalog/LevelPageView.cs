using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class LevelPageView : BasePageView
    {
        public IEnumerable<LevelView> LevelViews { get; set; }
        public LevelView LevelView { get; set; }

        public LevelConditionView LevelConditionView { get; set; }
        public IEnumerable<LevelConditionView> LevelConditionViews { get; set; }

        public LevelLevelView LevelLevelView { get; set; }
        public IEnumerable<LevelLevelView> LevelLevelViews { get; set; }

        public IEnumerable<LevelTypeView> LevelTypeViews { get; set; }
    }
}
