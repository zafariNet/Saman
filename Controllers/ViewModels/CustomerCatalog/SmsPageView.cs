using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class SmsPageView : BasePageView
    {
        public SmsView SmsView { get; set; }

        public IEnumerable<SmsView> SmsViews { get; set; }
    }
}
