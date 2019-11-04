using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;
using Model.Support;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Controllers.Controllers
{
    public class SupportExpertDispatchController : BaseController
    {
        #region Declare

        private readonly ISupportService _supportService;

        private readonly ICustomerService _customerService;

        private readonly IEmployeeService _employeeService;

        private readonly ISupportExpertDispatchService _supportExpertDispatchService;

        #endregion

        #region ctor

        public SupportExpertDispatchController(ISupportExpertDispatchService supportExpertDispatchService,
            ISupportService supportService, ICustomerService customerService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _supportService = supportService;
            _customerService = customerService;
            _employeeService = employeeService;
            _supportExpertDispatchService = supportExpertDispatchService;
        }

        #endregion

        #region Read All

        public JsonResult SupportExpertDispatches_Read()
        {
            GetGeneralResponse<IEnumerable<SupportExpertDispatchView>> response=new GetGeneralResponse<IEnumerable<SupportExpertDispatchView>>();

            response = _supportExpertDispatchService.GetSupportExpertDispaches();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read One

        public JsonResult SupportExpertDispatch_Read(Guid SupportID)
        {
            GetGeneralResponse<SupportExpertDispatchView> response=new GetGeneralResponse<SupportExpertDispatchView>();

            response = _supportExpertDispatchService.GetSupportExpertDispach(SupportID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult SupportExpertDispatch_Insert(AddSupportExpertDispatchRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            response = _supportExpertDispatchService.AddSupportExpertDispatch(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult SupportExpertDispatch_Update(EditSupportExpertDispatchRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            response = _supportExpertDispatchService.EditSupportExpertDispatch(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delte

        public JsonResult SupportExpertDispatch_Delete(DeleteRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            response = _supportExpertDispatchService.DeleteSupportExpertDispatch(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}
