using AutoMapper;
using Model.Employees;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Mapping
{
    public static class QueueMapper
    {
        public static IEnumerable<QueueView> ConvertToQueueViews(
        this IEnumerable<Queue> queues)
        {
            return Mapper.Map<IEnumerable<Queue>,
                IEnumerable<QueueView>>(queues);
        }

        public static QueueView ConvertToQueueView(this Queue queue)
        {
            return Mapper.Map<Queue, QueueView>(queue);
        }
    }
}
