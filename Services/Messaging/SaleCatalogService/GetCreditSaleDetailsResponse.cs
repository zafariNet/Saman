﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Sales;

namespace Services.Messaging.SaleCatalogService
{
    public class GetCreditSaleDetailsResponse
    {
        public IEnumerable<BaseSaleDetailView> CreditSaleDetailViews { get; set; }
    }
}
