using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;
using System.Web.UI.WebControls;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class AgencyHomePageView : BasePageView
    {
        public IEnumerable<AgencyView> AgencyViews { get; set; }

        public AgencyHomePageView()
        {
            
        }

        
    }
}
