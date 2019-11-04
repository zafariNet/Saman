using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Controllers.Controllers
{
    public class SupportQcController : BaseController
    {
        #region Declare

        private readonly ISupportService _supportService;

        private readonly ICustomerService _customerService;

        private readonly IEmployeeService _employeeService;

        private readonly ISupportQcService _supportQcService;

        #endregion

        #region ctor

        public SupportQcController(ISupportQcService supportQcService,
            ISupportService supportService, ICustomerService customerService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _supportService = supportService;
            _customerService = customerService;
            _employeeService = employeeService;
            _supportQcService = supportQcService;
        }

        #endregion

        #region Read All

        public JsonResult SupportQcs_Read()
        {
            GetGeneralResponse<IEnumerable<SupportQcView>> response=new GetGeneralResponse<IEnumerable<SupportQcView>>();

            response = _supportQcService.GetSupportQcs();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read One

        public JsonResult SupportQc_Read(Guid SupportID)
        {
            GetGeneralResponse<SupportQcView> response = new GetGeneralResponse<SupportQcView>();

            response = _supportQcService.GetSupportQc(SupportID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Insert

        public JsonResult SupportQc_Insert(AddSupportQcRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            //AddSupportQcRequest request=new AddSupportQcRequest();
            //request.Comment = "این یک توضیح است برا ی کیو سی";
            //request.ExpertBehavior = 1;
            //request.ExpertCover = 2;
            //request.InputTime = "12:20";
            //request.InstallerEmployeeID = Guid.Parse("12D942E9-9B2F-42A9-82D5-66D661FAC17D");
            //request.OutputTime="14:50";
            //request.RecivedCost = 230000;
            //request.SaleAndService = 1;
            //request.SendNotificationToCustomer = true;
            //request.SendNotificationToMaster = true;
            //request.SupportID = Guid.Parse("AC57D46B-5139-4F03-B4D4-4A75B747CDCA");
            

            response = _supportQcService.AddSupportQc(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult SupportQc_Update(EditSupportQcRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            response = _supportQcService.EditSupportQc(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult SupportQc_Delete(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            response = _supportQcService.DeleteSupportQc(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
