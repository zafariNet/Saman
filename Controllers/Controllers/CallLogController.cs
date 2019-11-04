using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Infrastructure.Querying;
using Services.Interfaces;
using Services.Messaging;
using Services.ViewModels.Customers;

namespace Controllers.Controllers
{
    public class CallLogController:BaseController
    {
        #region Delcare

        private readonly IEmployeeService _employeeService;
        private readonly ICallLogService _callLogService;

        #endregion

        #region Ctor

        public CallLogController(IEmployeeService employeeService,ICallLogService callLogService):base(employeeService)
        {
            _employeeService = employeeService;
            _callLogService = callLogService;
        }

        #endregion

        #region Read

        public JsonResult CallLogs_ReadAll(int? pageSize, int? pageNumber, IList<FilterData> filter, string sort)
        {
            var response=new GetGeneralResponse<IEnumerable<CallLogView>>();


            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _callLogService.GetAllCallLog(PageSize, PageNumber, filter, ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CallLogs_ReadOwn(int? pageSize, int? pageNumber, IList<FilterData> filter, string sort)
        {
            var response = new GetGeneralResponse<IEnumerable<CallLogView>>();


            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _callLogService.GetOwnCallLog(PageSize, PageNumber, filter, ConvertJsonToObject(sort),GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }



        public JsonResult CallLogs_Customer(int? pageSize, int? pageNumber, IList<FilterData> filter, string sort,Guid CustomerID)
        {
            var response = new GetGeneralResponse<IEnumerable<CallLogView>>();


            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _callLogService.GetOwnCallLog(PageSize, PageNumber, filter, ConvertJsonToObject(sort), CustomerID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Set Result

        public JsonResult CallLog_SetResult(Guid CallLogID, Guid CustomerContactTemplateID, string Description)
        {
            GeneralResponse response = new GeneralResponse();

            response = _callLogService.SetResult(CallLogID, CustomerContactTemplateID, Description, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);

            
         // declare @Phone nvarchar(11)
         // declare @LocalPhone nvarchar(4)
         // set @Phone='44001554'
         // set @LocalPhone='7779'
         // declare @EmployeeID uniqueidentifier
         // set @EmployeeID=(select t1.EmployeeID from emp.Employee t1 inner join emp.LocalPhoneStoreEmployee t2 on t1.EmployeeID=t2.OwnerEmployeeID inner join 
         // emp.LocalPhoneStore t4 on t2.LocalPhoneStoreID=t4.LocalPhoneStoreID
         // where t4.LocalPhoneStoreNumber=@LocalPhone)
         // select @EmployeeID
         // declare @CustomerID uniqueidentifier
         // set @CustomerID=(select CustomerID from Cus.Customer where ADSLPhone=@Phone or Mobile1=@Phone)
         // select @CustomerID

        }

        #endregion
    }
}
