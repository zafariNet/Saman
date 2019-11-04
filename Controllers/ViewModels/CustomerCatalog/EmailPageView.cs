using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class EmailPageView : BasePageView
    {
        public IEnumerable<EmailView> EmailViews { get; set; }

        public EmailView EmailView { get; set; }
    }
}
