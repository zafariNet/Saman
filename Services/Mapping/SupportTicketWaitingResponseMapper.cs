using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Support;
using Services.ViewModels.Support;

namespace Services.Mapping
{
    public static class SupportTicketWaitingResponseMapper
    {
        public static IEnumerable<SupportTicketWaitingResponseView> ConvertToSupportTicketWaitingResponseViews(
            this IEnumerable<SupportTicketWaitingResponse> supportTicketWaitingResponses)
        {
            return Mapper.Map<IEnumerable<SupportTicketWaitingResponse>,
                IEnumerable<SupportTicketWaitingResponseView>>(supportTicketWaitingResponses);
        }

        public static SupportTicketWaitingResponseView ConvertToSupportTicketWaitingResponseView(
            this SupportTicketWaitingResponse supportTicketWaitingResponse)
        {
            return
                Mapper.Map<SupportTicketWaitingResponse, SupportTicketWaitingResponseView>(supportTicketWaitingResponse);
        }
    }
}
