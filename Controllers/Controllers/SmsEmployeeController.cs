using Infrastructure.Querying;
using Services.Interfaces;
using Services.Messaging;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Controllers.Controllers
{
    public class SmsEmployeeController:BaseController
    {
        #region Declare

        private readonly IEmployeeService _employeeService;
        private readonly ISmsEmployeeService _smsEmployeeService;

        #endregion

        #region Ctor

        public SmsEmployeeController(IEmployeeService employeeService,ISmsEmployeeService smsEmployeeService):base(employeeService)
        {
            _employeeService = employeeService;
            _smsEmployeeService = smsEmployeeService;
        }

        #endregion

        #region Get By Employee

        public JsonResult SmsEmployee_ByEmployee(int? pageSize,int? pageNumber)
        {

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            GetGeneralResponse<IEnumerable<SmsEmployeeView>> response = new GetGeneralResponse<IEnumerable<SmsEmployeeView>>();

            response = _smsEmployeeService.GetSmsEmployeeByOwner(GetEmployee().ID, PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read All

        public JsonResult SmsEmployee_ReadAll(IList<FilterData> filter,int? pageSize, int?pageNumber,string sort)
        {
            GetGeneralResponse<IEnumerable<SmsEmployeeView>> response = new GetGeneralResponse<IEnumerable<SmsEmployeeView>>();

                        int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _smsEmployeeService.GetSmsEmployees(filter, ConvertJsonToObject(sort), PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    
        #endregion
    }
}
