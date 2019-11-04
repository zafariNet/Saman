using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class EmailMapper
    {
        public static IEnumerable<EmailView> ConvertToEmailViews(
            this IEnumerable<Email> emails)
        {
            return Mapper.Map<IEnumerable<Email>,
                IEnumerable<EmailView>>(emails);
        }

        public static EmailView ConvertToEmailView(this Email email)
        {
            return Mapper.Map<Email, EmailView>(email);
        }
    }
}
