using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using Services.ViewModels.Store;
using AutoMapper;

namespace Services.Mapping
{
    public static class UncreditServiceMapper
    {
        public static IEnumerable<UncreditServiceView> ConvertToUncreditServiceViews(
            this IEnumerable<UncreditService> uncreditServices)
        {
            return Mapper.Map<IEnumerable<UncreditService>,
                IEnumerable<UncreditServiceView>>(uncreditServices);
        }

        public static UncreditServiceView ConvertToUncreditServiceView(this UncreditService uncreditService)
        {
            return Mapper.Map<UncreditService, UncreditServiceView>(uncreditService);
        }
    }
}
