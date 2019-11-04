using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class CenterPageView : BasePageView
    {
        public IEnumerable<CenterView> CenterViews { get; set; }

        public CenterView CenterView { get; set; }

        public CodeView CodeView { get; set; }
    }
}
