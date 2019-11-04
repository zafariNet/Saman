using System.Collections.Generic;
using AutoMapper;
using Model.Customers;
using Services.ViewModels.Customers;

namespace Services.Mapping
{
    public static class CallLogMapper
    {
        public static IEnumerable<CallLogView> ConvertToCallLogViews(
    this IEnumerable<CallLog> callLogs)
        {
            return Mapper.Map<IEnumerable<CallLog>,
                IEnumerable<CallLogView>>(callLogs);
        }

        public static CallLogView ConvertToCallLogView(this CallLog callLog)
        {
            return Mapper.Map<CallLog, CallLogView>(callLog);
        }
    }
}
