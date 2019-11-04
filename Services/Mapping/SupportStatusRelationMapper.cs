using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Support;
using Services.ViewModels.Support;

namespace Services.Mapping
{
    public static class SupportStatusRelationMapper
    {
        public static IEnumerable<SupportStatusRelationView> ConvertToSupportStatusRelationViews(
    this IEnumerable<SupportStatusRelation> supportStatuses)
        {
            return Mapper.Map<IEnumerable<SupportStatusRelation>,
                IEnumerable<SupportStatusRelationView>>(supportStatuses);
        }

        public static SupportStatusRelationView ConvertToSupportStatusRelationView(this SupportStatusRelation supportStatusRelation)
        {
            return Mapper.Map<SupportStatusRelation, SupportStatusRelationView>(supportStatusRelation);
        }
    }
}
