﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Store;

namespace Services.Messaging.StoreCatalogService
{
    public class GetProductLogsResponse
    {
        public IEnumerable<ProductLogView> ProductLogViews { get; set; }
    }
}
