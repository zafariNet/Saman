using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Fiscals;
using Services.ViewModels.Fiscals;
using AutoMapper;

namespace Services.Mapping
{
    public static class FiscalMapper
    {
        public static IEnumerable<FiscalView> ConvertToFiscalViews(
            this IEnumerable<Fiscal> fiscals)
        {
            List<FiscalView> returnFiscals = new List<FiscalView>();

            foreach (Fiscal fiscal in fiscals)
            {
                returnFiscals.Add(fiscal.ConvertToFiscalView());
            }

            return returnFiscals;
        }

        public static FiscalView ConvertToFiscalView(this Fiscal fiscal)
        {
            return Mapper.Map<Fiscal, FiscalView>(fiscal);
        }

        public static IEnumerable<FiscalRealView> ConvertToFiscalRealViews(
            this IEnumerable<Fiscal> fiscals)
        {
            List<FiscalRealView> returnFiscals = new List<FiscalRealView>();

            foreach (Fiscal fiscal in fiscals)
            {
                returnFiscals.Add(fiscal.ConvertToFiscalRealView());
            }

            return returnFiscals;
        }

        public static FiscalRealView ConvertToFiscalRealView(this Fiscal fiscal)
        {
            return Mapper.Map<Fiscal, FiscalRealView>(fiscal);
        }

    }
}
