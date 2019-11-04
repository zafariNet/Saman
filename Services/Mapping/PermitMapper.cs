using AutoMapper;
using Model.Employees;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Mapping
{
    public static class PermitMapper
    {

            public static IEnumerable<PermitView> ConvertTopermiViews(
                this IEnumerable<Permit> permits)
            {
                return Mapper.Map<IEnumerable<Permit>,
                    IEnumerable<PermitView>>(permits);
            }

            public static PermitView ConvertToProductLogView(this Permit permit)
            {
                return Mapper.Map<Permit, PermitView>(permit);
            }
    }
}
