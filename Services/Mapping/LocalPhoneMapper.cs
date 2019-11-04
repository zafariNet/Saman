using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using Services.ViewModels.Employees;
using AutoMapper;

namespace Services.Mapping
{
    public static class LocalPhoneMapper
    {
        public static IEnumerable<LocalPhoneView> ConvertToLocalPhoneViews(
            this IEnumerable<LocalPhone> localPhones)
        {
            List<LocalPhoneView> returnLocalPhones = new List<LocalPhoneView>();

            foreach (LocalPhone employee in localPhones)
            {
                returnLocalPhones.Add(employee.ConvertToLocalPhoneView());
            }

            return returnLocalPhones;
        }

        public static LocalPhoneView ConvertToLocalPhoneView(this LocalPhone localPhone)
        {
            return Mapper.Map<LocalPhone, LocalPhoneView>(localPhone);
        }
    }
}
