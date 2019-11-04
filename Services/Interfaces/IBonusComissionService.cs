using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;
using Services.ViewModels.Reports;
using Services.Messaging.ReportCatalogService;
using Infrastructure.Querying;

namespace Services.Interfaces
{
    public interface IBonusComissionService
    {
        //GeneralResponse AddBonusComission(AddBonusComissionRequest request, Guid CreateEmployeeID);

        GetGeneralResponse<IEnumerable<BonusComissionView>> GetTodayBonusComission();
        GetGeneralResponse<IEnumerable<SlideShowEmployeeView>> GetTodayBonusComissionSimple();
        GetGeneralResponse<IEnumerable<BonusMasterReportView>> GetBonusReport(int pageSize, int pageNumber, IList<FilterData> filter, IList<FilterData> Creditfilter, IList<FilterData> unCreditfilter, IList<FilterData> poductfilter, IList<Sort> sort, bool pro, bool cre, bool unc);
        GetGeneralResponse<IEnumerable<ComissionMasterReportView>> GetComissionReport(int pageSize, int pageNumber, IList<FilterData> filter, IList<FilterData> Creditfilter, IList<FilterData> unCreditfilter, IList<FilterData> poductfilter, IList<Sort> sort,bool pro,bool cre,bool unc);
    }
}
