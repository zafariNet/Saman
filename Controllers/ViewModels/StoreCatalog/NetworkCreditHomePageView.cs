using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;
using Model.Store;

namespace Controllers.ViewModels.StoreCatalog
{
    public class NetworkCreditHomePageView : BasePageView
    {
        public IEnumerable<NetworkCreditView> NetworkCreditViews { get; set; }

        public NetworkView NetworkVeiw { get; set; }
    }
}
