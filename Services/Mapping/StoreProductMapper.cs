using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using Services.ViewModels.Store;
using AutoMapper;

namespace Services.Mapping
{
    public static class StoreProductMapper
    {
        public static IEnumerable<StoreProductView> ConvertToStoreProductViews(
            this IEnumerable<StoreProduct> storeProducts)
        {
            List<StoreProductView> returnStoreProducts = new List<StoreProductView>();

            foreach (StoreProduct storeProduct in storeProducts)
            {
                returnStoreProducts.Add(storeProduct.ConvertToStoreProductView());
            }

            return returnStoreProducts;
        }

        public static StoreProductView ConvertToStoreProductView(this StoreProduct storeProduct)
        {
            return Mapper.Map<StoreProduct, StoreProductView>(storeProduct);
        }
    }
}
