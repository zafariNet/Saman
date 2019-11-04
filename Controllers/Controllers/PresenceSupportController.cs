#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.SupportCatalog;
using System.Web.Mvc;
using Services.ViewModels.Support;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;

#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class PersenceSupportController : BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly IPersenceSupportService _persenceSupportService;

        #endregion

        #region Ctor

        public PersenceSupportController(IEmployeeService employeeService, IPersenceSupportService persenceSupportService)
            : base(employeeService)
        {
            this._persenceSupportService = persenceSupportService;
            this._employeeService = employeeService;
        }

        #endregion

        #region Json
        #region Read
        //public JsonResult PersenceSupports_Read(Guid CustomerID, int? PageSize, int? PageNumber)
        //{
        //    GetGeneralResponse<IEnumerable<PersenceSupportView>> response = new GetGeneralResponse<IEnumerable<PersenceSupportView>>();

        //    #region Access Check
        //    bool hasPermission = GetEmployee().IsGuaranteed("PersenceSupports_Read");
        //    if (!hasPermission)
        //    {
        //        response.ErrorMessages.Add("AccessDenied");
        //        return Json(response, JsonRequestBehavior.AllowGet);
        //    }
        //    #endregion

        //    int _PageSize = PageSize == null ? -1 : (int)PageSize;
        //    int _PageNumber = PageNumber == null ? -1 : (int)PageNumber;

        //    response = _persenceSupportService.GetPersenceSupports(CustomerID, _PageSize, _PageNumber);

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// خواندن تمام پشتیبانی های حضوری با قابلیت فیلتر سازی
        /// </summary>
        /// <param name="CreateEmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="SupportType"></param>
        /// <param name="StartCreateDate"></param>
        /// <param name="EndCreateDate"></param>
        /// <param name="StartPlanDate"></param>
        /// <param name="EndPlanDate"></param>
        /// <param name="Installer"></param>
        /// <param name="Deliverd"></param>
        /// <param name="StartDeliverDate"></param>
        /// <param name="EndDeliverDate"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageNumber"></param>
        /// <returns></returns>
        public JsonResult PersenceSupports_Read(Guid? CustomerID, Guid? CreateEmployeeID, int? SupportType,
            string StartCreateDate, string EndCreateDate, string StartPlanDate, string EndPlanDate, Guid? Installer,
            bool? Deliverd, string StartDeliverDate, string EndDeliverDate, int? PageSize, int? PageNumber,string sort)
        {
            GetGeneralResponse<IEnumerable<PersenceSupportView>> response = new GetGeneralResponse<IEnumerable<PersenceSupportView>>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("PersenceSupports_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            ///ابتدای مقدار دهی به مقادیر ورودی
            CreateEmployeeID = CreateEmployeeID == null ? Guid.Empty : CreateEmployeeID;
            CustomerID = CustomerID == null ? Guid.Empty : CustomerID;
            SupportType = SupportType == null ? 0 : SupportType;
            Installer = Installer == null ? Guid.Empty : Installer;

            
            //انتهای مقدار دهی به مقادیر ورودی

            int _PageSize = PageSize == null ? -1 : (int)PageSize;
            int _PageNumber = PageNumber == null ? -1 : (int)PageNumber;

            response = _persenceSupportService.GetCustomizedPersenceSupports(CustomerID, CreateEmployeeID, SupportType, StartCreateDate,
                EndCreateDate, StartPlanDate, EndPlanDate, Installer, Deliverd, StartDeliverDate, EndDeliverDate, _PageSize, _PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public JsonResult PersenceSupport_Delete(Guid PersenceSupportID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("PersenceSupport_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _persenceSupportService.DeletePersenceSupport(new DeleteRequest() { ID = PersenceSupportID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PersenceSupport_Insert(Guid CustomerID, short SupportType, string Problem, string PlanDate,
            string PlanTimeFrom, string PlanTimeTo, string PlanNote)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("PersenceSupport_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            AddPersenceSupportRequest request = new AddPersenceSupportRequest();
            request.CreateEmployeeID = GetEmployee().ID;
            request.CustomerID = CustomerID;
            request.SupportType = SupportType;
            request.Problem = Problem;
            request.PlanDate = PlanDate;
            request.PlanTimeFrom = PlanTimeFrom;
            request.PlanTimeTo = PlanTimeTo;
            request.PlanNote = PlanNote;

            response = this._persenceSupportService.AddPersenceSupport(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PersenceSupport_Update(Guid PersenceSupportID, short SupportType, string Problem, string PlanDate,
            string PlanTimeFrom, string PlanTimeTo, string PlanNote, int RowVersion)
        {
              GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("PersenceSupport_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            EditPersenceSupportRequest request = new EditPersenceSupportRequest();
            request.ID = PersenceSupportID;
            request.RowVersion = RowVersion;
            request.ModifiedEmployeeID = GetEmployee().ID;
            request.SupportType = SupportType;
            request.Problem = Problem;
            request.PlanDate = PlanDate;
            request.PlanTimeFrom = PlanTimeFrom;
            request.PlanTimeTo = PlanTimeTo;
            request.PlanNote = PlanNote;

            response = this._persenceSupportService.EditPersenceSupport(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PersenceSupport_Deliver(Guid PersenceSupportID, Guid InstallerID, bool Delivered,
            string DeliverDate, string DeliverTime, string DeliverNote, long ReceivedCost, int RowVersion)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("PersenceSupport_Deliver");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            Delivered = Delivered == null ? false : true;

            response = this._persenceSupportService.DeliverPersenceSupport(PersenceSupportID, InstallerID, Delivered, DeliverDate,
                DeliverNote, DeliverTime, ReceivedCost, RowVersion);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #region New Methods Added By Zafari
        
        #endregion
        #endregion

        #region Private Members

        private PersenceSupportView GetPersenceSupportView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetPersenceSupportResponse response = this._persenceSupportService.GetPersenceSupport(request);

            return response.PersenceSupportView;
        }

        #endregion

    }
}
