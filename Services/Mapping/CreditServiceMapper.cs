using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using Services.ViewModels.Store;
using AutoMapper;

namespace Services.Mapping
{
    public static class CreditServiceMapper
    {
        public static IEnumerable<CreditServiceView> ConvertToCreditServiceViews(
            this IEnumerable<CreditService> creditServices)
        {
            return Mapper.Map<IEnumerable<CreditService>,
                IEnumerable<CreditServiceView>>(creditServices);
        }

        public static CreditServiceView ConvertToCreditServiceView(this CreditService creditService)
        {
            return Mapper.Map<CreditService, CreditServiceView>(creditService);
        }
    }
}
