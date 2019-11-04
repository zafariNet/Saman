#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.CustomerCatalog;
using System.Web.Mvc;
using Services.ViewModels.Customers;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Controllers.ViewModels;
#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class NetworkCenterController : BaseController
    {
        #region Declares
        private readonly IEmployeeService _employeeService;

        private readonly INetworkCenterService _networkCenterService;
        #endregion

        #region Ctor
        public NetworkCenterController(IEmployeeService employeeService, INetworkCenterService networkCenterService)
            : base(employeeService)
        {
            this._networkCenterService = networkCenterService;
            this._employeeService = employeeService;
        }
        #endregion

        #region Old Methods

        #region Index
        public ActionResult Index()
        {
            NetworkCenterHomePageView networkCenterHomePageView = new NetworkCenterHomePageView();
            networkCenterHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("NetworkCenter_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkCenterHomePageView);
            }
            #endregion
            
            GetNetworkCentersRequest getNetworkCenterRequest = new GetNetworkCentersRequest();
            networkCenterHomePageView.NetworkCenterViews = this._networkCenterService.GetNetworkCenters(getNetworkCenterRequest).NetworkCenterViews;

            return View(networkCenterHomePageView);
        }
        #endregion

        #region Networks
        public ActionResult Networks(string id)
        {
            NetworkCenterHomePageView networkCenterHomePageView = new NetworkCenterHomePageView();
            networkCenterHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("NetworkCenter_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkCenterHomePageView);
            }
            #endregion

            GetNetworkCentersRequest getNetworkCenterRequest = new GetNetworkCentersRequest();
            getNetworkCenterRequest.CenterID = Guid.Parse(id);
            networkCenterHomePageView.NetworkCenterViews = this._networkCenterService.GetNetworkCenters(getNetworkCenterRequest).NetworkCenterViews;

            return View(networkCenterHomePageView);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id, string Cid)
        {
            NetworkCenterDetailView networkCenterDetailView = new NetworkCenterDetailView();
            networkCenterDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("NetworkCenter_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkCenterDetailView);
            }
            #endregion
            
            networkCenterDetailView.NetworkCenterView = this.GetNetworkCenterView(id, Cid);
            
            #region DropDownList For Status
            List<DropDownItem> list = new List<DropDownItem>();

            list.Add(new DropDownItem { Value = -1, Text = "مشخص نشده" });
            list.Add(new DropDownItem { Value = 1, Text = "تحت پوشش" });
            list.Add(new DropDownItem { Value = 2, Text = "عدم پوشش" });
            list.Add(new DropDownItem { Value = 3, Text = "عدم امکان موقت" });

            var selectList = new SelectList(list, "Value", "Text");
            ViewData["Status"] = selectList;
            #endregion

            return View(networkCenterDetailView);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, string Cid, NetworkCenterDetailView networkCenterDetailView)
        {

            networkCenterDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("NetworkCenter_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(networkCenterDetailView);
            }
            #endregion

            #region DropDownList For Status
            List<DropDownItem> list = new List<DropDownItem>();

            list.Add(new DropDownItem { Value = -1, Text = "مشخص نشده" });
            list.Add(new DropDownItem { Value = 1, Text = "تحت پوشش" });
            list.Add(new DropDownItem { Value = 2, Text = "عدم پوشش" });
            list.Add(new DropDownItem { Value = 3, Text = "عدم امکان موقت" });

            var selectList = new SelectList(list, "Value", "Text");
            ViewData["Status"] = selectList;
            #endregion


            if (ModelState.IsValid)
                try
                {
                    string networkID = id;
                    string centerID = Cid;

                    EditNetworkCenterRequest request = new EditNetworkCenterRequest();

                    request.NetworkID = Guid.Parse(networkID);
                    request.CenterID = Guid.Parse(centerID);
                    request.ModifiedEmployeeID = GetEmployee().ID; 
                    request.Status = networkCenterDetailView.NetworkCenterView.Status.Value;
                    request.RowVersion = networkCenterDetailView.NetworkCenterView.RowVersion;

                    GeneralResponse response = _networkCenterService.EditNetworkCenter(request);

                    if (response.success)
                        return RedirectToAction("Networks/" + centerID, "Center");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(networkCenterDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(networkCenterDetailView);
                }

            return View(networkCenterDetailView);
        }
        #endregion

        #endregion

        #region New Methods


        #endregion

        #region Private Members

        private NetworkCenterView GetNetworkCenterView(string Nid, string Cid)
        {
            GetRequest2 request = new GetRequest2();
            request.ID1 = Guid.Parse(Nid);
            request.ID2 = Guid.Parse(Cid);

            GetNetworkCenterResponse response = _networkCenterService.GetNetworkCenter(request);

            return response.NetworkCenterView;
        }

        #endregion

    }
}
