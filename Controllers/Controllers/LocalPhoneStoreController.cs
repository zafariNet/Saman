using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Infrastructure.Querying;
using Services.Interfaces;
using Services.Messaging;
using Services.ViewModels.Employees;

namespace Controllers.Controllers
{
    public class LocalPhoneStoreController:BaseController
    {

        #region Declare

        private readonly IEmployeeService _employeeService;

        private readonly ILocalPhoneStoreService _localPhoneStoreService;

        #endregion

        #region Ctor

        public LocalPhoneStoreController(IEmployeeService employeeService, ILocalPhoneStoreService localPhoneStoreService)
            : base(employeeService)
        {
            _localPhoneStoreService = localPhoneStoreService;
            _employeeService = employeeService;
        }

        #endregion

        #region update Local Phone Store

        public JsonResult LocalPhoneStore_Update()
        {
            GeneralResponse response=new GeneralResponse();

            response = _localPhoneStoreService.GetLocalPhoneStoresFromAsterisk();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read All

        public JsonResult LocalPhoneStores_Read(int? pageSize, int? pageNumber, IList<FilterData> filter, string sort)
        {
            var response = new GetGeneralResponse<IEnumerable<LocalPhoneStoreView>>();


            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _localPhoneStoreService.GetLocalPhoneStores(PageSize, PageNumber, filter,
                ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UnReserved_LocalPhoneStores_Read()
        {
            var response = new GetGeneralResponse<IEnumerable<LocalPhoneStoreView>>();



            response = _localPhoneStoreService.GetUnReservedLocalPhoneStores();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
