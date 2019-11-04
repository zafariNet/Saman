#region Usings

using Infrastructure.Querying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace Services.Messaging
{
    public class AdvancedSearchRequest
    {
        public IEnumerable<SortRequest> sorts { get; set; }

        public IEnumerable<SearchPhrase> SearchPhrases{get;set;}

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public IList<Sort> sort { get; set; }
    }
}
