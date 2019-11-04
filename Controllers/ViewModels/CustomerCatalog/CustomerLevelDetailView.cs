#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;
#endregion

namespace Controllers.ViewModels.CustomerCatalog
{
    public class CustomerLevelDetailView : BasePageView
    {
        public CustomerLevelView CustomerLevelView { get; set; }

        public CustomerView CustomerView { get; set; }

        public IEnumerable<LevelTypeView> LevelTypeViews { get; set; }

        // For Show Link of queries in some pages like ChangeLevelsuccess
        public IEnumerable<QueryView> QueryViews { get; set; }
    }
}
