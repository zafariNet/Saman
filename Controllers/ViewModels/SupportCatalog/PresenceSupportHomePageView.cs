using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Support;

namespace Controllers.ViewModels.SupportCatalog
{
    public class PersenceSupportHomePageView : BasePageView
    {
        public IEnumerable<PersenceSupportView> PersenceSupportViews { get; set; }
    }
}
