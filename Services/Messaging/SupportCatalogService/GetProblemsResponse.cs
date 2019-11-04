using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Support;

namespace Services.Messaging.SupportCatalogService
{
    public class GetProblemsResponse
    {
        public IEnumerable<ProblemView> ProblemViews { get; set; }
    }
}
