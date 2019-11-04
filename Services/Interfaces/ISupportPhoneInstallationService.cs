using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Interfaces
{
    public interface ISupportPhoneInstallationService
    {
        GetGeneralResponse<IEnumerable<SupportPhoneInstallationView>> GetSupportPhoneInstallations();

        GetGeneralResponse<SupportPhoneInstallationView> GetSupportPhoneInstallation(Guid SupportID);

        GeneralResponse AddSupportPhoneInstallation(AddSupportPhoneInstallationRequst request, Guid CreateemployeeID);

        GeneralResponse EditSupportPhoneInstalltion(EditSupportPhoneInstallationRequst request, Guid ModifiedEployeeID);

        GeneralResponse DeleteSupportPhonInstallation(DeleteRequest request);
    }
}
