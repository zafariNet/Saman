using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;
using Services.ViewModels.Employees;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class LevelDetailView : BasePageView
    {
        public LevelView LevelView { get; set; }

        public IEnumerable<LevelTypeView> LevelTypeViews { get; set; }

        public IEnumerable<EmployeeView> EmployeeViews { get; set; }

        public IEnumerable<LevelConditionView> LevelConditionViews { get; set; }
    }
}
