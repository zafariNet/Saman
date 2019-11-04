using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Mvc;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Controllers.Controllers
{
    public class SupportStatusRelationController : BaseController
    {

        #region declare

        private readonly IEmployeeService _employeeService;
        private readonly ISupportStatusService _supportStatusService;
        private readonly ISupportStatusRelationService _supportStatusRelationService;

        #endregion

        #region ctor

        public SupportStatusRelationController(IEmployeeService employeeService,
            ISupportStatusService supportStatusService,
            ISupportStatusRelationService supportStatusRelationService)
            : base(employeeService)
        {
            _supportStatusService = supportStatusService;
            _supportStatusRelationService = supportStatusRelationService;
        }

        #endregion

        #region Read

        public JsonResult SupportStatusRelation_Read(Guid SupportStatusID)
        {
            GetGeneralResponse<IEnumerable<SupportStatusRelationView>> response=new GetGeneralResponse<IEnumerable<SupportStatusRelationView>>();

            response = _supportStatusRelationService.GetSupportStatuseRelations(SupportStatusID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SupportStatusRelation_ReadAll()
        {
            GetGeneralResponse<IEnumerable<SupportStatusRelationView>> response = new GetGeneralResponse<IEnumerable<SupportStatusRelationView>>();

            response = _supportStatusRelationService.GetSupportStatuseRelations();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read First Statuses

        #endregion

        #region Insert

        public JsonResult SupportStatusRelations_Insert(IEnumerable<SupportStatusRelationView> requests, Guid SupportStatusID)
        {
            GeneralResponse response=new GeneralResponse();

            //IList<AddSupportStatusRelationRequest> d=new List<AddSupportStatusRelationRequest>();
            //d.Add(new AddSupportStatusRelationRequest()
            //{
            //    SupportStatusID = Guid.Parse("08B32DDD-FD72-4C71-B923-9D0AAEEFA233"),
            //    NextSupportStatusID = Guid.Parse("8DA448E5-F947-42DF-840E-A44933D4B5A5")
            //});
            response = _supportStatusRelationService.AddSupportStatusRelation(requests,SupportStatusID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult SupportStatusRelations_Update(IEnumerable<EditSupportStatusRelationRequest> requests,Guid SupportStatusID)
        {
            GeneralResponse response=new GeneralResponse();

            response = _supportStatusRelationService.EditSupportStatusRelation(requests,SupportStatusID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete

        public JsonResult SupportStatusRelations_Delete(IEnumerable<SupportStatusRelationView> requests)
        {
            GeneralResponse response=new GeneralResponse();

            response = _supportStatusRelationService.DeleteSupportStatusRelations(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
