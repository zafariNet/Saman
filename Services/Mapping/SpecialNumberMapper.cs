using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class SpecialNumberMapper
    {
        public static IEnumerable<SpecialNumberView> ConvertToSpecialNumberViews(
            this IEnumerable<SpecialNumber> specialNumbers)
        {
            return Mapper.Map<IEnumerable<SpecialNumber>,
                IEnumerable<SpecialNumberView>>(specialNumbers);
        }

        public static SpecialNumberView ConvertToSpecialNumberView(this SpecialNumber specialNumber)
        {
            return Mapper.Map<SpecialNumber, SpecialNumberView>(specialNumber);
        }
    }
}
