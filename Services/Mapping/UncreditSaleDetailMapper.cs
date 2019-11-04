using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Sales;
using Services.ViewModels.Sales;
using AutoMapper;

namespace Services.Mapping
{
    public static class UncreditSaleDetailMapper
    {
        public static IEnumerable<UncreditSaleDetailView> ConvertToUncreditSaleDetailViews(
            this IEnumerable<UncreditSaleDetail> uncreditSaleDetails)
        {
            //return Mapper.Map<IEnumerable<UncreditSaleDetail>,
            //    IEnumerable<UncreditSaleDetailView>>(uncreditSaleDetails);

            List<UncreditSaleDetailView> response = new List<UncreditSaleDetailView>();

            foreach (UncreditSaleDetail uncreditSaleDetail in uncreditSaleDetails)
            {
                response.Add(uncreditSaleDetail.ConvertToUncreditSaleDetailView());
            }

            return response;
        }

        public static UncreditSaleDetailView ConvertToUncreditSaleDetailView(this UncreditSaleDetail uncreditSaleDetail)
        {
            return Mapper.Map<UncreditSaleDetail, UncreditSaleDetailView>(uncreditSaleDetail);
        }
    }
}
