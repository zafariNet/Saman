using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class SmsMapper
    {
        public static IEnumerable<SmsView> ConvertToSmsViews(
            this IEnumerable<Sms> smss)
        {
            return Mapper.Map<IEnumerable<Sms>,
                IEnumerable<SmsView>>(smss);
        }

        public static SmsView ConvertToSmsView(this Sms sms)
        {
            return Mapper.Map<Sms, SmsView>(sms);
        }
    }
}
