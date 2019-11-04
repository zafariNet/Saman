using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class QueryMapper
    {
        public static IEnumerable<QueryView> ConvertToQueryViews(
            this IEnumerable<Query> querys)
        {
            return Mapper.Map<IEnumerable<Query>,
                IEnumerable<QueryView>>(querys);
        }

        public static QueryView ConvertToQueryView(this Query query)
        {
            return Mapper.Map<Query, QueryView>(query);
        }
    }
}
