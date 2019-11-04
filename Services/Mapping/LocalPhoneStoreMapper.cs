using AutoMapper;
using Model.Employees;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Mapping
{
    public static class LocalPhoneStoreMapper
    {
        public static IEnumerable<LocalPhoneStoreView> ConvertToLocalPhoneStoreViews(
        this IEnumerable<LocalPhoneStore> localPhoneStores)
        {
            return Mapper.Map<IEnumerable<LocalPhoneStore>,
                IEnumerable<LocalPhoneStoreView>>(localPhoneStores);
        }

        public static LocalPhoneStoreView ConvertToLocalPhoneStoreView(this LocalPhoneStore localPhoneStore)
        {
            return Mapper.Map<LocalPhoneStore, LocalPhoneStoreView>(localPhoneStore);
        }
    }
}
