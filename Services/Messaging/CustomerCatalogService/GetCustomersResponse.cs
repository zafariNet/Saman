#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;
using System.ComponentModel;
#endregion

namespace Services.Messaging.CustomerCatalogService
{

    [Obsolete("Use GetGeneralResponse instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class GetCustomersResponse
    {
        public IEnumerable<CustomerView> CustomerViews { get; set; }
        
        public int TotalCount { get; set; }

        public bool success { get; set; }
    }
}
