using Infrastructure.Querying;
using Services.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Employees;

namespace Services.Interfaces
{
    public interface ILocalPhoneStoreService
    {
        GeneralResponse GetLocalPhoneStoresFromAsterisk();

        GetGeneralResponse<IEnumerable<LocalPhoneStoreView>> GetLocalPhoneStores(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort);

        GetGeneralResponse<IEnumerable<LocalPhoneStoreView>> GetUnReservedLocalPhoneStores();
    }
}
