using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Employees;
using Services.ViewModels.Employees;

namespace Services.Mapping
{
    public static class MessageTemplateMapper
    {
        public static IEnumerable<MessageTemplateView> ConvertToMessageTemplateViews(
    this IEnumerable<MessageTemplate> messageTemplate)
        {
            return Mapper.Map<IEnumerable<MessageTemplate>,
                IEnumerable<MessageTemplateView>>(messageTemplate);
        }

        public static MessageTemplateView ConvertToMessageTemplateView(this MessageTemplate messageTemplate)
        {
            return Mapper.Map<MessageTemplate, MessageTemplateView>(messageTemplate);
        }
    }
}
