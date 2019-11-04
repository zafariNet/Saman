#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.StoreCatalog;
using System.Web.Mvc;
//using System.Web.UI.WebControls;
using Services.ViewModels.Store;
using Services.Messaging;
using Services.Messaging.StoreCatalogService;

#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class NetworkController: BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly INetworkService _networkService;

        #endregion

        #region Ctor

        public NetworkController(IEmployeeService employeeService, INetworkService networkService)
            : base(employeeService)
        {
            this._networkService = networkService;
            this._employeeService = employeeService;
        }

        #endregion

        #region Old Methods

        public ActionResult Index()
        {
            NetworkHomePageView networkHomePageView = new NetworkHomePageView();
            networkHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Network_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkHomePageView);
            }
            #endregion

            networkHomePageView.NetworkViews = this._networkService.GetNetworks().NetworkViews;

            return View(networkHomePageView);
        }

        public ActionResult Create()
        {
            NetworkDetailView networkDetailView = new NetworkDetailView();
            networkDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Network_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkDetailView);
            }
            #endregion

            
            return View(networkDetailView);
        }

        [HttpPost]
        public ActionResult Create(NetworkDetailView networkDetailView)
        {
            networkDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Network_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    AddNetworkRequestOld request = new AddNetworkRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.DeliverWhenCreditLow = networkDetailView.NetworkView.DeliverWhenCreditLow;
                    request.NetworkName = networkDetailView.NetworkView.NetworkName;
                    request.Note = networkDetailView.NetworkView.Note;

                    GeneralResponse response = this._networkService.AddNetwork(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(networkDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(networkDetailView);
                }

            return View(networkDetailView);
        }

        public ActionResult Edit(string id)
        {
            NetworkDetailView networkDetailView = new NetworkDetailView();
            networkDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Network_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkDetailView);
            }
            #endregion

            networkDetailView.NetworkView = this.GetNetworkView(id);
            
            return View(networkDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, NetworkDetailView networkDetailView)
        {
            networkDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Network_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkDetailView);
            }
            #endregion
            
            if (ModelState.IsValid)
                try
                {
                    EditNetworkRequestOld request = new EditNetworkRequestOld();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.DeliverWhenCreditLow = networkDetailView.NetworkView.DeliverWhenCreditLow;
                    request.NetworkName = networkDetailView.NetworkView.NetworkName;
                    request.Note = networkDetailView.NetworkView.Note;
                    request.RowVersion = networkDetailView.NetworkView.RowVersion;

                    GeneralResponse response = this._networkService.EditNetwork(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(networkDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(networkDetailView);
                }

            return View(networkDetailView);
        }

        public ActionResult Details(string id)
        {
            NetworkDetailView networkDetailView = new NetworkDetailView();
            networkDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Network_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkDetailView);
            }
            #endregion

            NetworkView networkView = this.GetNetworkView(id);

            networkDetailView.NetworkView = networkView;
            
            return View(networkDetailView);
        }

        public ActionResult Delete(string id)
        {
            NetworkDetailView networkDetailView = new NetworkDetailView();
            networkDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Network_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkDetailView);
            }
            #endregion

            
            networkDetailView.NetworkView = this.GetNetworkView(id);
            
            return View(networkDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            NetworkDetailView networkDetailView = new NetworkDetailView();
            networkDetailView.EmployeeView = GetEmployee();


            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Network_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkDetailView);
            }
            #endregion

            networkDetailView.NetworkView = this.GetNetworkView(id);
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._networkService.DeleteNetwork(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(networkDetailView);
            }
        }

        #endregion


        #region New methods

        #region Read

        public JsonResult Networks_Read(int? pageSize, int? pageNumber,string sort)
        {
            GetGeneralResponse<IEnumerable<NetworkSummaryView>>  response=new GetGeneralResponse<IEnumerable<NetworkSummaryView>>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Network_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _networkService.GetNetworks(PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Networks_Read_NoPermission(int? pageSize, int? pageNumber, string sort)
        {
            GetGeneralResponse<IEnumerable<NetworkSummaryView>> response = new GetGeneralResponse<IEnumerable<NetworkSummaryView>>();


            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _networkService.GetNetworks(PageSize, PageNumber, ConvertJsonToObject(sort));
            response.data=response.data.Where(x => x.Discontinued == false).ToList();
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        //public JsonResult Networks_Read(int? pageSize, int? pageNumber)
        //{
        //    GetGeneralResponse<IEnumerable<NetworkSummaryView>> response = new GetGeneralResponse<IEnumerable<NetworkSummaryView>>();

        //    int PageSize = pageSize == null ? -1 : (int)pageSize;
        //    int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

        //    response = _networkService.GetNetworks(PageSize, PageNumber);

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}


        #endregion

        #region Insert

        public JsonResult Networks_Insert(IEnumerable<AddNetworkRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Network_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _networkService.AddNetworks(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Update

        public JsonResult Networks_Update(IEnumerable<EditNetworkRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Network_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _networkService.EditNetworks(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult Networks_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Network_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _networkService.DeleteNetworks(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Moveing

        public JsonResult Network_MoveUp(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _networkService.MoveUp(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Network_MoveDown(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _networkService.MoveDown(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Private Members

        private NetworkView GetNetworkView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetNetworkResponse response = this._networkService.GetNetwork(request);

            return response.NetworkView;
        }

        #endregion

    }
}
