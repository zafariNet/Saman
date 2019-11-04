#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.CustomerCatalog;
using System.Web.Mvc;
//using System.Web.UI.WebControls;
using Services.ViewModels.Customers;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class SuctionModeDetailController:BaseController
    {

        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly ISuctionModeDetailService _suctionModeDetailService;

        #endregion

        #region Ctor

        public SuctionModeDetailController(IEmployeeService employeeService, ISuctionModeDetailService suctionModeDetailService)
            : base(employeeService)
        {
            this._suctionModeDetailService = suctionModeDetailService;
            this._employeeService = employeeService;
        }

        #endregion

        #region New Methods

        #region Read
        //1392/11/06 تست شد
        public JsonResult SuctionModeDetails_Read(Guid? SuctionModeID , int? pageSize,int? pageNumber,string sort)
        {
            GetGeneralResponse<IEnumerable<SuctionModeDetailview>> response = new GetGeneralResponse<IEnumerable<SuctionModeDetailview>>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion
            
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            Guid suctionModeID = SuctionModeID == null ? Guid.Empty : (Guid)SuctionModeID;
            response = _suctionModeDetailService.GetSuctionModeDetailBySuctionMode(suctionModeID, PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SuctionModeDetails_Read_NoPermission(Guid? SuctionModeID, int? pageSize, int? pageNumber, string sort)
        {
            GetGeneralResponse<IEnumerable<SuctionModeDetailview>> response = new GetGeneralResponse<IEnumerable<SuctionModeDetailview>>();

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            Guid suctionModeID = SuctionModeID == null ? Guid.Empty : (Guid)SuctionModeID;
            response = _suctionModeDetailService.GetSuctionModeDetailBySuctionModeAll(suctionModeID, PageSize, PageNumber, ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert
        //1392/11/06 تست شد
        public JsonResult SuctionModeDetails_Insert(IEnumerable<AddSuctionModeDetailRequest> requests,Guid SuctionModeID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion
            #region بررسی اینکه آیا غیر فعا نال میآید یا خیر
            IList<AddSuctionModeDetailRequest> finalRequest =new List<AddSuctionModeDetailRequest>();

            foreach (AddSuctionModeDetailRequest request in requests)
            {
                if(request.Discontinued == null)
                    request.Discontinued=false;
                finalRequest.Add(request);
            }
            #endregion

            response = _suctionModeDetailService.AddSuctionModeDetails(finalRequest, SuctionModeID , GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Update
        //1392/11/06 تست شد
        public JsonResult SuctionModeDetails_Update(IEnumerable<EditSuctionModeDetailRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _suctionModeDetailService.EditSuctionModeDetails(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete
        //1392/11/06 تست شد
        public JsonResult SuctionModeDetails_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();
            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("SuctionMode_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _suctionModeDetailService.DeleteSuctionModeDetails(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Moveing

        public JsonResult SuctionModeDetail_MoveUp(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _suctionModeDetailService.MoveUp(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SuctionModeDetail_MoveDown(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _suctionModeDetailService.MoveDown(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion
    }
}
