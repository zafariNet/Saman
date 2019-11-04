using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using Services.ViewModels.Store;
using AutoMapper;

namespace Services.Mapping
{
    public static class StoreMapper
    {
        public static IEnumerable<StoreView> ConvertToStoreViews(
            this IEnumerable<Store> stores)
        {
            return Mapper.Map<IEnumerable<Store>,
                IEnumerable<StoreView>>(stores);
        }

        public static StoreView ConvertToStoreView(this Store store)
        {
            return Mapper.Map<Store, StoreView>(store);
        }
    }
}
