using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels;
using AutoMapper;
namespace Services.Mapping
{
    public static class SystemCountersMapper
    {
        public static IEnumerable<SystemCountersView> ConvertToSystemCountersViews(
    this IEnumerable<SystemCounters> systemCounters)
        {
            return Mapper.Map<IEnumerable<SystemCounters>,
                IEnumerable<SystemCountersView>>(systemCounters);
        }

        public static SystemCountersView ConvertToSystemCountersView(this SystemCounters systemCounters)
        {
            return Mapper.Map<SystemCounters, SystemCountersView>(systemCounters);
        }
    }
}
