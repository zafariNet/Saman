using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class CodeHomePageView : BasePageView
    {
        public IEnumerable<CodeView> CodeViews { get; set; }

        public CenterView CenterView { get; set; }

        public IEnumerable<CenterView> CenterViews { get; set; }
    }
}
