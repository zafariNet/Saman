using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;

namespace Services.Interfaces
{
    public interface IMainMenuService
    {
        GetMainMenusResponse GetMainMenus(MainMenusGetRequest request);
    }
}
