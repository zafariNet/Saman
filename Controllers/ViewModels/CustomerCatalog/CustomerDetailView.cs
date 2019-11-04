#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;
#endregion

namespace Controllers.ViewModels.CustomerCatalog
{
    public class CustomerDetailView : BasePageView
    {
        public IEnumerable<LevelTypeView> LevelTypeViews { get; set; }

        public IEnumerable<LevelView> LevelViews { get; set; }

        public CustomerView CustomerView { get; set; }

        // For Show Link of queries in some pages like ChangeLevelsuccess
        public IEnumerable<QueryView> QueryViews { get; set; }
    }
}
