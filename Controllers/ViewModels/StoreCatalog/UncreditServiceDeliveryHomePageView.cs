﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Controllers.ViewModels.StoreCatalog
{
    public class UncreditServiceDeliveryHomePageView : BasePageView
    {
        public IEnumerable<UncreditServiceDeliveryView> UncreditServiceDeliveryViews { get; set; }
    }
}
