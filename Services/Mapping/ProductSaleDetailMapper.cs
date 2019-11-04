using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Sales;
using Services.ViewModels.Sales;
using AutoMapper;

namespace Services.Mapping
{
    public static class ProductSaleDetailMapper
    {
        public static IEnumerable<ProductSaleDetailView> ConvertToProductSaleDetailViews(
            this IEnumerable<ProductSaleDetail> productSaleDetails)
        {
            List<ProductSaleDetailView> res = new List<ProductSaleDetailView>();

            foreach (ProductSaleDetail productSaleDetail in productSaleDetails)
            {
                res.Add(productSaleDetail.ConvertToProductSaleDetailView());
            }

            return res;
        }

        public static ProductSaleDetailView ConvertToProductSaleDetailView(this ProductSaleDetail productSaleDetail)
        {
            return Mapper.Map<ProductSaleDetail, ProductSaleDetailView>(productSaleDetail);
        }
    }
}
