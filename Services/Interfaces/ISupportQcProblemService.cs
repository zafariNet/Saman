using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging;
using Services.ViewModels.Support;

namespace Services.Interfaces
{
    public interface ISupportQcProblemService
    {
        GetGeneralResponse<IEnumerable<SupportQcProblemView>> GetSupportQcproblems();
    }
}
