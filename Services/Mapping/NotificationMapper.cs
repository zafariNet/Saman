using AutoMapper;
using Model.Employees;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Mapping
{
    public static class NotificationMapper
    {

        public static IEnumerable<NotificationView> ConvertToNotificationViews(
    this IEnumerable<Notification> notifications)
        {
            return Mapper.Map<IEnumerable<Notification>,
                IEnumerable<NotificationView>>(notifications);
        }

        public static NotificationView ConvertToNotificationView(this Notification notification)
        {
            return Mapper.Map<Notification, NotificationView>(notification);
        }
    }
}
