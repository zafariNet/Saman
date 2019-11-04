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
    public class SupportPhoneInstallationController : BaseController
    {
        #region Declare

        private readonly ISupportService _supportService;

        private readonly ICustomerService _customerService;

        private readonly IEmployeeService _employeeService;

        private readonly ISupportPhoneInstallationService _supportPhoneInstallationService;



        #endregion

        #region ctor

        public SupportPhoneInstallationController(ISupportPhoneInstallationService supportPhoneInstallationService,
            ISupportService supportService, ICustomerService customerService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _supportService = supportService;
            _customerService = customerService;
            _employeeService = employeeService;
            _supportPhoneInstallationService = supportPhoneInstallationService;
        }

        #endregion

        #region Read All

        public JsonResult SupportPhoneInstallations_Read()
        {
            GetGeneralResponse<IEnumerable<SupportPhoneInstallationView>> response = new GetGeneralResponse<IEnumerable<SupportPhoneInstallationView>>();

            response = _supportPhoneInstallationService.GetSupportPhoneInstallations();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read One

        public JsonResult SupportPhoneInstallation_Read(Guid SupportID)
        {
            GetGeneralResponse<SupportPhoneInstallationView> response=new GetGeneralResponse<SupportPhoneInstallationView>();

            response = _supportPhoneInstallationService.GetSupportPhoneInstallation(SupportID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult SupportPhoneInstallation_Insert(AddSupportPhoneInstallationRequst request)
        {
            GeneralResponse response=new GeneralResponse();

            response = _supportPhoneInstallationService.AddSupportPhoneInstallation(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult SupportPhoneInstallation_Update(EditSupportPhoneInstallationRequst request)
        {
           GeneralResponse response=new GeneralResponse();

            response = _supportPhoneInstallationService.EditSupportPhoneInstalltion(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        #endregion 

        #region Delete

        public JsonResult SupportPhonInstallation_Delete(DeleteRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            response = _supportPhoneInstallationService.DeleteSupportPhonInstallation(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        

        #endregion
    }
}
