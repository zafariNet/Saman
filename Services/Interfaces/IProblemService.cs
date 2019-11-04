using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.SupportCatalogService;
using Services.Messaging;
using Services.ViewModels.Support;
using Infrastructure.Querying;

namespace Services.Interfaces
{
    public interface IProblemService
    {
        GeneralResponse AddProblem(AddProblemRequest request);
        GeneralResponse EditProblem(EditProblemRequest request);
        GeneralResponse DeleteProblem(DeleteRequest request);
        GetProblemResponse GetProblem(GetRequest request);
        GetProblemsResponse GetProblems();

        GetGeneralResponse<IEnumerable<ProblemView>> GetProblems(Guid customerID, int PageSize, int PageNumber,IList<Sort> sort);
    }
}
