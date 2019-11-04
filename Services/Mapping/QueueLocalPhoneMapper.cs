using AutoMapper;
using Model.Employees;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Mapping
{
    public static class QueueLocalPhoneMapper
    {
        public static IEnumerable<QueueLocalPhoneStoreView> ConvertToQueueLocalPhoneViews(
        this IEnumerable<QueueLocalPhoneStore> queueLocalPhones)
        {
            return Mapper.Map<IEnumerable<QueueLocalPhoneStore>,
                IEnumerable<QueueLocalPhoneStoreView>>(queueLocalPhones);
        }

        public static QueueLocalPhoneStoreView ConvertToQueueLocalPhoneView(this QueueLocalPhoneStore queueLocalPhone)
        {
            return Mapper.Map<QueueLocalPhoneStore, QueueLocalPhoneStoreView>(queueLocalPhone);
        }
    }
}
