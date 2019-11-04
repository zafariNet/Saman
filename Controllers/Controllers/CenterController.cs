#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.CustomerCatalog;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Services.ViewModels.Customers;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Controllers.ViewModels;
using System.Xml;
using Infrastructure.Querying;
#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class CenterController: BaseController
    {

        #region Declares
        private readonly IEmployeeService _employeeService;

        private readonly ICenterService _centerService;
        
        private readonly INetworkCenterService _networkCenterService;
        #endregion

        #region Ctor
        public CenterController(IEmployeeService employeeService, ICenterService centerService,
            INetworkCenterService networkCenterService)
            : base(employeeService)
        {
            this._centerService = centerService;
            this._employeeService = employeeService;
            _networkCenterService = networkCenterService;
        }
        #endregion

        #region Center CRUD
        public JsonResult Centers_Read(int? pageSize, int? pageNumber,string centerName,string sort,IList<FilterData> filter)
        {
            GeneralResponse response = new GeneralResponse();

            //#region Access Check
            //bool hasPermission = GetEmployee().IsGuaranteed("Center_Read");
            //if (!hasPermission)
            //{
            //    ModelState.AddModelError("", "AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}
            //#endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            response = _centerService.GetCenters(PageSize, PageNumber,centerName,ConvertJsonToObject(sort),filter);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Centers_Insert(IEnumerable<AddCenterRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Center_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = this._centerService.AddCenter(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Centers_Update(IEnumerable<EditCenterRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Center_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = this._centerService.EditCenter(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Centers_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Center_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _centerService.DeleteCenter(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region Coverage
        public JsonResult Coverage_Read(Guid CenterID, int? pageSize, int? pageNumber,string sort)
        {
            GeneralResponse response = new GeneralResponse();

            //#region Access Check
            //bool hasPermission = GetEmployee().IsGuaranteed("NetworkCenter_Read");
            //if (!hasPermission)
            //{
            //    ModelState.AddModelError("", "AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}
            //#endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;
              response = _centerService.GetCoverage(CenterID, PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Coverage_Update(IEnumerable<NetworkCenterView> data , Guid CenterID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("NetworkCenter_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _centerService.EditCoverage(data ,CenterID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Old Methods

        #region Ajax
        public ActionResult Center_Read([DataSourceRequest] DataSourceRequest request)
        {
            CenterHomePageView centerHomePageView = new CenterHomePageView();
            centerHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Center_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                return Json(centerHomePageView.CenterViews.ToDataSourceResult(request));
            }
            #endregion
            
            centerHomePageView.CenterViews = this._centerService.GetCenters().CenterViews;

            return Json(centerHomePageView.CenterViews.ToDataSourceResult(request));
        }

        public ActionResult Status_Read(string id, [DataSourceRequest] DataSourceRequest request)
        {
            CenterDetailView centerDetailView = new CenterDetailView();
            centerDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Center_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                //return Json(centerDetailView.CenterView.NetworkCenters.ToDataSourceResult(request));
                return Json(false);
            }
            #endregion
            
            GetRequest getRequest = new GetRequest() { ID = Guid.Parse(id) };
            centerDetailView.CenterView = _centerService.GetCenter(getRequest).CenterView;

            #region DropDownList For Status
            List<DropDownItem> list = new List<DropDownItem>();

            list.Add(new DropDownItem { Value = -1, Text = "مشخص نشده" });
            list.Add(new DropDownItem { Value = 1, Text = "تحت پوشش" });
            list.Add(new DropDownItem { Value = 2, Text = "عدم پوشش" });
            list.Add(new DropDownItem { Value = 3, Text = "عدم امکان موقت" });

            ViewData["statusData"] = list;
            #endregion

            //return Json(centerDetailView.CenterView.NetworkCenters.ToDataSourceResult(request));
            return Json(false);

        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Status_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<NetworkCenterView> networkCenters)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Center_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(ModelState.ToDataSourceResult());
            }
            #endregion

            if (networkCenters != null && ModelState.IsValid)
            {
                foreach (var networkCenter in networkCenters)
                {
                    //var nc = _networkCenterService.GetNetworkCenter(new GetRequest2() { ID1 = networkCenter.NetworkID, ID2 = networkCenter.CenterID });

                    //if (nc != null)
                    {
                        //nc.NetworkCenterView.StatusInt = networkCenter.StatusInt;
                        EditNetworkCenterRequest editReq = new EditNetworkCenterRequest();
                        editReq.CenterID = networkCenter.CenterID;
                        editReq.NetworkID = networkCenter.NetworkID;
                        editReq.Status = networkCenter.Status.Value;

                        response = _networkCenterService.EditNetworkCenter(editReq);
                    }
                }
            }

            return Json(ModelState.ToDataSourceResult());
        }
        #endregion

        #region Index
        public ActionResult Index()
        {
            CenterHomePageView centerHomePageView = new CenterHomePageView();
            centerHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Center_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                return View(centerHomePageView);
            }
            #endregion

            
            centerHomePageView.CenterViews = this._centerService.GetCenters().CenterViews;

            return View(centerHomePageView);
        }
        #endregion

        #region Networks
        public ActionResult Networks(string id)
        {
            CenterDetailView centerDetailView = new CenterDetailView();
            centerDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Network_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(centerDetailView);
            }
            #endregion

            GetRequest getRequest = new GetRequest() { ID = Guid.Parse(id) };
            centerDetailView.CenterView = _centerService.GetCenter(getRequest).CenterView;

            GetNetworkCentersRequest req = new GetNetworkCentersRequest() { CenterID = Guid.Parse(id), NetworkID = Guid.Empty };
            centerDetailView.NetworkCenterViews = _networkCenterService.GetNetworkCenters(req).NetworkCenterViews;

            #region DropDownList For Status
            List<DropDownItem> list = new List<DropDownItem>();

            list.Add(new DropDownItem { Value = -1, Text = "مشخص نشده" });
            list.Add(new DropDownItem { Value = 1, Text = "تحت پوشش" });
            list.Add(new DropDownItem { Value = 2, Text = "عدم پوشش" });
            list.Add(new DropDownItem { Value = 3, Text = "عدم امکان موقت" });

            ViewData["statusData"] = list;

            #endregion

            return View(centerDetailView);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            CenterDetailView centerDetailView = new CenterDetailView();
            centerDetailView.EmployeeView = GetEmployee();
            
            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Center_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                return View(centerDetailView);
            }
            #endregion

            return View(centerDetailView);
        }

        [HttpPost]
        public ActionResult Create(CenterDetailView centerDetailView)
        {
            GeneralResponse response=new GeneralResponse();
            centerDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Center_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return View(centerDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    AddCenterRequest request = new AddCenterRequest();
                    //request.CreateEmployeeID = GetEmployee().ID;
                    request.CenterName = centerDetailView.CenterView.CenterName;
                    request.Note = centerDetailView.CenterView.Note;

                    //response = this._centerService.AddCenter(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(centerDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(centerDetailView);
                }

            return View(centerDetailView);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            CenterDetailView centerDetailView = new CenterDetailView();
            centerDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Center_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(centerDetailView);
            }
            #endregion
            
            centerDetailView.CenterView = this.GetCenterView(id);

            return View(centerDetailView);
        }

        //[HttpPost]
        //public ActionResult Edit(string id, CenterDetailView centerDetailView)
        //{
        //    centerDetailView.EmployeeView = GetEmployee();

        //    #region Access Check
        //    bool hasPermission = GetEmployee().IsGuaranteed("Center_Update");
        //    if (!hasPermission)
        //    {
        //        ModelState.AddModelError("", "AccessDenied");
        //        return View(centerDetailView);
        //    }
        //    #endregion

        //    if (ModelState.IsValid)
        //        try
        //        {
        //            EditCenterRequest request = new EditCenterRequest();

        //            request.ID = Guid.Parse(id);
        //            request.ModifiedEmployeeID = GetEmployee().ID;
        //            request.CenterName = centerDetailView.CenterView.CenterName;
        //            request.Note = centerDetailView.CenterView.Note;
        //            request.RowVersion = centerDetailView.CenterView.RowVersion;

        //            GeneralResponse response = this._centerService.EditCenter(request);

        //            if (response.success)
        //                return RedirectToAction("Index");
        //            else
        //            {
        //                foreach (string error in response.ErrorMessages)
        //                    ModelState.AddModelError("", error);
        //                return View(centerDetailView);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", ex.Message);
        //            return View(centerDetailView);
        //        }

        //    return View(centerDetailView);
        //}
        #endregion

        #region Details
        public ActionResult Details(string id)
        {
            CenterDetailView centerDetailView = new CenterDetailView();
            centerDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Center_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(centerDetailView);
            }
            #endregion

            CenterView centerView = this.GetCenterView(id);

            centerDetailView.CenterView = centerView;
            
            return View(centerDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            CenterDetailView centerDetailView = new CenterDetailView();
            centerDetailView.EmployeeView = GetEmployee();
            /*
            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Center_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(centerDetailView);
            }
            #endregion
            */
            centerDetailView.CenterView = this.GetCenterView(id);
            
            return View(centerDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            CenterDetailView centerDetailView = new CenterDetailView();
            /*
            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Center_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(centerDetailView);
            }
            #endregion
            */
            centerDetailView.CenterView = this.GetCenterView(id);
            centerDetailView.EmployeeView = GetEmployee();

            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = new GeneralResponse(); //this._centerService.DeleteCenter(request);

            if (response.success)
                return RedirectToAction("Index");
                //return Content("Fuck");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(centerDetailView);
            }
        }
        #endregion

        #endregion

        #region Private Members

        private CenterView GetCenterView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetCenterResponse response = this._centerService.GetCenter(request);

            return response.CenterView;
        }

        #endregion

    }
}
