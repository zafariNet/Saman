using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Support;

namespace Controllers.ViewModels.SupportCatalog
{
    public class PersenceSupportPageView : BasePageView
    {
        public PersenceSupportView PersenceSupportView { get; set; }
        public IEnumerable<PersenceSupportView> PersenceSupportViews { get; set; }
    }
}
