using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Sales;
using Services.ViewModels.Sales;
using AutoMapper;

namespace Services.Mapping
{
    public static class CreditSaleDetailMapper
    {
        public static IEnumerable<CreditSaleDetailView> ConvertToCreditSaleDetailViews(
            this IEnumerable<CreditSaleDetail> creditSaleDetails)
        {
            return Mapper.Map<IEnumerable<CreditSaleDetail>,
                IEnumerable<CreditSaleDetailView>>(creditSaleDetails);
        }

        public static CreditSaleDetailView ConvertToCreditSaleDetailView(this CreditSaleDetail creditSaleDetail)
        {
            return Mapper.Map<CreditSaleDetail, CreditSaleDetailView>(creditSaleDetail);
        }
    }
}
