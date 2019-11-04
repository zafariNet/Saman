using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Leads;
using Services.ViewModels.Leads;

namespace Services.Mapping
{
    public static class NegotiationMapper
    {
        public static IEnumerable<NegotiationView> ConvertToNegotiationViews(
            this IEnumerable<Negotiation> negotiations)
        {
            return Mapper.Map<IEnumerable<Negotiation>,
                IEnumerable<NegotiationView>>(negotiations);
        }

        public static NegotiationView ConvertToNegotiationView(this Negotiation negotiation)
        {
            return Mapper.Map<Negotiation, NegotiationView>(negotiation);
        }
    }
}
