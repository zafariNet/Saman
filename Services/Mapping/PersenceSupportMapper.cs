using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Support;
using Services.ViewModels.Support;
using AutoMapper;

namespace Services.Mapping
{
    public static class PersenceSupportMapper
    {
        public static IEnumerable<PersenceSupportView> ConvertToPersenceSupportViews(
            this IEnumerable<PersenceSupport> presenceSupports)
        {
            return Mapper.Map<IEnumerable<PersenceSupport>,
                IEnumerable<PersenceSupportView>>(presenceSupports);
        }

        public static PersenceSupportView ConvertToPersenceSupportView(this PersenceSupport presenceSupport)
        {
            return Mapper.Map<PersenceSupport, PersenceSupportView>(presenceSupport);
        }
    }
}
