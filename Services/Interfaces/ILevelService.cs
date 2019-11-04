#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Services.ViewModels.Customers;
using Infrastructure.Querying;

#endregion

namespace Services.Interfaces
{
    public interface ILevelService
    {
        GeneralResponse AddLevel(AddLevelRequest request);

        GeneralResponse EditLevel(EditLevelRequest request);

        GeneralResponse DeleteLevel(DeleteRequest request);

        GetGeneralResponse<LevelView> GetLevel(GetRequest request);

        GetGeneralResponse<IEnumerable<LevelView>> GetLevels(int pageSize, int pageNumber);

        GetGeneralResponse<IEnumerable<LevelView>> GetNextLevels(AjaxGetRequest request);

        GetGeneralResponse<IEnumerable<LevelView>> GetLevelsByLevelTypeID(Guid LevelTypeID, int pageSize, int pageNumber,IList<Sort> sort);

        GetGeneralResponse<IEnumerable<LevelView>> GetLevelsByLevelID(Guid LevelID, int pageSize, int pageNumber);

        GetGeneralResponse<IEnumerable<ConditionView>> GetConditions(Guid LevelID);

        GeneralResponse EditGraphicalProperties(Guid LevelID, int X, int Y, int Width, int Height, bool EnableDragging);

        IEnumerable<LevelLevelView3> GetRelations(Guid levelTypeID);

        GeneralResponse EditLevel_Email(Guid LevelID, string EmailText, Guid EmployeeID, int RowVersion);

        GeneralResponse EditLevel_Sms(Guid LevelID, string SmsText, Guid EmployeeID, int RowVersion);

        GeneralResponse EditLevel_Options(Guid LevelID, Guid EmployeeID, int RowVersion, LevelOptionsView levelOptionsView);

        MoveResponse MoveUp(MoveRequest request);
        MoveResponse MoveDown(MoveRequest request);

    }
}
