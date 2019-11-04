using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Domain
{
    /// <summary>
    /// For retriving TotalCount. If we dont use it, totalCount will equal to PageSize
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        #region Ctor

        public Response(IEnumerable<T> Data, int TotalCount)
        {
            totalCount = TotalCount;
            data = Data;
        }

        public Response()
        {

        }

        #endregion

        public IEnumerable<T> data { get; set; }

        public int totalCount { get; set; }

    }
}
