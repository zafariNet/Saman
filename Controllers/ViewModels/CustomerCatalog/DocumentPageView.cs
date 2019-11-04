#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;
#endregion

namespace Controllers.ViewModels.CustomerCatalog
{
    public class DocumentPageView : BasePageView
    {
        //public IEnumerable<DocumentView> DocumentViews { get; set; }

        public CustomerView CustomerView { get; set; }
    }
}
