using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Support;
using Services.ViewModels.Support;

namespace Services.Mapping
{
    public static class SupportTicketWaitingMapper
    {
        public static IEnumerable<SupportTicketWaitingView> ConvertToSupportTicketWaitingViews(
            this IEnumerable<SupportTicketWaiting> supportTicketWaitings)
        {
            return Mapper.Map<IEnumerable<SupportTicketWaiting>,
                IEnumerable<SupportTicketWaitingView>>(supportTicketWaitings);
        }

        public static SupportTicketWaitingView ConvertToSupportTicketWaitingView(
            this SupportTicketWaiting supportTicketWaiting)
        {
            return Mapper.Map<SupportTicketWaiting, SupportTicketWaitingView>(supportTicketWaiting);
        }
    }
}
