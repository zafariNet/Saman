using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Infrastructure.Querying;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Controllers.Controllers
{
    public class SupportController:BaseController
    {

        #region Declare

        private readonly ISupportService _supportService;

        private readonly ICustomerService _customerService;

        private readonly IEmployeeService _employeeService;

        #endregion

        #region ctor

        public SupportController(ISupportService supportService, ICustomerService customerService, IEmployeeService employeeService)
            :base(employeeService)
        {
            _supportService = supportService;
            _customerService = customerService;
            _employeeService = employeeService;
        }

        #endregion

        #region Read

        public JsonResult Supports_Own_Read(int? pageSize, int? pageNumber, string sort, IList<FilterData> filter)
        {
            GetGeneralResponse<IEnumerable<SupportOwnView>> response = new GetGeneralResponse<IEnumerable<SupportOwnView>>();


            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Install_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _supportService.GetOwnSupports(GetEmployee().ID, PageSize, PageNumber, filter, ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Supports_Read()
        {
            GetGeneralResponse<IEnumerable<SupportView>> response=new GetGeneralResponse<IEnumerable<SupportView>>();

            response = _supportService.GetSupports();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Support_ReadAll(int? pageSize, int? pageNumber, string sort, IList<FilterData> filter,int? LastState)
        {
            GetGeneralResponse<IEnumerable<SupportView>> response = new GetGeneralResponse<IEnumerable<SupportView>>();

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int lastState=LastState==null ? 0 : (int) LastState;
            response = _supportService.GetSupports(PageSize, PageNumber, filter, ConvertJsonToObject(sort), lastState);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Support_Read(int? pageSize, int? pageNumber,Guid customerID, string sort)
        {
            GetGeneralResponse<IEnumerable<SupportView>> response = new GetGeneralResponse<IEnumerable<SupportView>>();

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _supportService.GetSupports(PageSize, PageNumber, customerID, ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Insert

        public JsonResult Support_Insert(AddSupportRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            //AddSupportRequest request = new AddSupportRequest();
            //request.Confirmed = true;
            //request.SupportComment = "توضیحات پشتیبانی";
            //request.SupportTitle = "تیتر پشتیبانی";
            //request.CustomerID = Guid.Parse("164D2B54-E194-4705-86D3-067A3AC38B74");

            response = _supportService.AddSupport(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult Support_Update(EditSupportRequest request)
        {
           GeneralResponse response=new GeneralResponse();

            //EditSupportRequest request=new EditSupportRequest();
            //request.ID = Guid.Parse("12823870-1F9F-434C-B8F6-487E618F92EA");
            //request.RowVersion = 0;
            //request.SupportTitle = "این یک تیتر جدید است";
            //request.SupportComment = "این یک عنوان جدید ست برای همین کارها";

            response = _supportService.EditSupport(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        

        #endregion

        #region Delete

        public JsonResult Support_Delete(DeleteRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            response = _supportService.DeleteSupport(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
