#region Usings

using Infrastructure.Querying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace Services.Messaging
{
    public class QuickSearchRequest
    {
        /// <summary>
        /// عبارت قابل سرچ
        /// </summary>
        public string query { get; set; }
        /// <summary>
        /// شناسه نمایی که قرار است در آن جستجو شود
        /// </summary>
        public Guid queryID { get; set; }

        public int pageSize { get; set; }

        public int pageNumber { get; set; }

        public Guid? customerID { get; set; }

        public IList<Sort> sort { get; set; }
    }
}
 