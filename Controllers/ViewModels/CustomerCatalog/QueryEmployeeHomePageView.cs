﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class QueryEmployeeHomePageView : BasePageView
    {
        public IEnumerable<QueryEmployeeView> QueryEmployeeViews { get; set; }
    }
}
