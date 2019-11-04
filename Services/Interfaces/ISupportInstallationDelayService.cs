using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Interfaces
{
    public interface ISupportInstallationDelayService
    {
        GetGeneralResponse<IEnumerable<SupportInstallationDelayView>> GetSupportInstallationDelays();

        GetGeneralResponse<SupportInstallationDelayView> GetSupportInstallationDelay(Guid SupportID);

        GeneralResponse AddSupportInstallationDelay(AddSupportInstallationDelayRequest request,
            Guid CreateEmployeeID);

        GeneralResponse EditSpportInstallationDelay(EditSupportInstallationDelayRequest request, Guid ModifiedEmployeeID);

        GeneralResponse DeleteSupportInstallationDelay(DeleteRequest request);
    }
}
