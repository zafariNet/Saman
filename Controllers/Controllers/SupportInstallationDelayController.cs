using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Controllers.Controllers
{
    public class SupportInstallationDelayController:BaseController
    {
        #region Declare

        private readonly ISupportService _supportService;

        private readonly ICustomerService _customerService;

        private readonly IEmployeeService _employeeService;

        private readonly ISupportInstallationDelayService _supportInstallationDelayService;

        #endregion

        #region ctor

        public SupportInstallationDelayController(ISupportInstallationDelayService supportInstallationDelayService,
            ISupportService supportService, ICustomerService customerService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _supportService = supportService;
            _customerService = customerService;
            _employeeService = employeeService;
            _supportInstallationDelayService = supportInstallationDelayService;
        }

        #endregion

        #region Read

        #region All

        public JsonResult SupportInstallationDelays_Read()
        {
            GetGeneralResponse<IEnumerable<SupportInstallationDelayView>> response=new GetGeneralResponse<IEnumerable<SupportInstallationDelayView>>();

            response = _supportInstallationDelayService.GetSupportInstallationDelays();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region One

        public JsonResult SupportInstallationDelay_Read(Guid SupportID)
        {
            GetGeneralResponse<SupportInstallationDelayView> response=new GetGeneralResponse<SupportInstallationDelayView>();

            response = _supportInstallationDelayService.GetSupportInstallationDelay(SupportID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Insert

        public JsonResult SupportInstallationDelay_Insert(AddSupportInstallationDelayRequest request)
        {
            GeneralResponse response=new GeneralResponse();


            //AddSupportInstallationDelayRequest request=new AddSupportInstallationDelayRequest();
            //request.Comment = "توضیحات تاخیر در نصب";
            //request.InstallDate = "1393/10/10";
            //request.NextCallDate = "1393/10/11";
            //response.ID = Guid.NewGuid();
            //request.SendNotificationToCustomer = true;
            //request.SupportID = Guid.Parse("AC57D46B-5139-4F03-B4D4-4A75B747CDCA");


            response = _supportInstallationDelayService.AddSupportInstallationDelay(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult SupportInstallationDelay_Update(EditSupportInstallationDelayRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            response = _supportInstallationDelayService.EditSpportInstallationDelay(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Delete

        public JsonResult SupportInstallationDelay_Delete(DeleteRequest request)
        {
            GeneralResponse response =new  GeneralResponse();

            response = _supportInstallationDelayService.DeleteSupportInstallationDelay(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
