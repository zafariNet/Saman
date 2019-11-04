#region Usings
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Controllers.ViewModels;
using Controllers.ViewModels.EmployeeCatalog;
using Infrastructure.Querying;
using Kendo.Mvc.UI;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;
using Kendo.Mvc.Extensions;
using System.Web;
using System.IO;
using Services.Messaging.CustomerCatalogService;
//Added By Zafari
using Model.Employees;
using System.Configuration;
#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class EmployeeController : BaseController
    {
        #region Declares
        private readonly IEmployeeService _employeeService;

        private readonly IGroupService _groupService;

        private readonly IPermissionService _permissionService;

        private readonly ILocalPhoneService _localPhoneService;
        #endregion

        #region Ctor
        public EmployeeController(IEmployeeService employeeService, IGroupService groupService
            , IPermissionService permissionService, ILocalPhoneService localPhoneService)
            : base(employeeService)
        {
            this._groupService = groupService;
            this._employeeService = employeeService;
            _permissionService = permissionService;
            _localPhoneService = localPhoneService;
        }
        #endregion

        #region Ajax
        public ActionResult Employee_Read([DataSourceRequest] DataSourceRequest request)
        {
            EmployeeHomePageView employeeHomePageView = new EmployeeHomePageView();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Employee_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(employeeHomePageView, JsonRequestBehavior.AllowGet);
            }
            #endregion

            employeeHomePageView.EmployeeView = GetEmployee();
            employeeHomePageView.EmployeeViews = this._employeeService.GetEmployees().EmployeeViews;

            return Json(employeeHomePageView.EmployeeViews.ToDataSourceResult(request));
        }

        

        public ActionResult Permission_Read(string id, [DataSourceRequest] DataSourceRequest request)
        {
            EmployeeDetailView employeeDetailView = new EmployeeDetailView();
            employeeDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Permission_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(employeeDetailView.PermissionViews.ToDataSourceResult(request));
            }
            #endregion

            GetPermissionsRequest permissionsRequest = new GetPermissionsRequest() { EmployeeID = Guid.Parse(id) };
            employeeDetailView.PermissionViews = _permissionService.GetPermissions(permissionsRequest).PermissionViews;

            return Json(employeeDetailView.PermissionViews.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Permission_Edit(string id, [DataSourceRequest] DataSourceRequest request, PermissionView permission)
        {
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Permission_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(ModelState.ToDataSourceResult());
            }
            #endregion
            
            EmployeeDetailView employeeDetailView = new EmployeeDetailView();
            employeeDetailView.EmployeeMainView = this.GetEmployeeView(id);
            
            if (permission != null && ModelState.IsValid)
            {
                try
                {
                    EditPermissionRequest editPermissionRequest = new EditPermissionRequest()
                    {
                        PermitKey = permission.Key,
                        EmployeeID = Guid.Parse(id),
                        Guaranteed = permission.Guaranteed
                    };

                    GeneralResponse response = _employeeService.EditEmployee(editPermissionRequest);


                    if (response.hasError)
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(employeeDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return Json(ModelState.ToDataSourceResult());
                }

            }

            return Json(ModelState.ToDataSourceResult());
        }

        #endregion

        #region Permissions
        public ActionResult Permissions(string id)
        {
            EmployeeDetailView employeeDetailView = new EmployeeDetailView();
            employeeDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Permission_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(employeeDetailView);
            }
            #endregion

            employeeDetailView.EmployeeMainView = this.GetEmployeeView(id);
            GetPermissionsRequest permissionsRequest = new GetPermissionsRequest() { EmployeeID = Guid.Parse(id) };
            employeeDetailView.PermissionViews = _permissionService.GetPermissions(permissionsRequest).PermissionViews;

            return View(employeeDetailView);
        }
        #endregion

        #region Queue
        public class test
        {
            public string Name { get; set; }
            public string LastStatus { get; set; }
        }

        public JsonResult MySql()
        {
            var con = ConfigurationManager.ConnectionStrings["Asterisk"].ToString();
            IList<test> test = new List<test>();
         
            using (MySqlConnection myConnection = new MySqlConnection(con))
            {
                string oString =
                    "Select * from sip";
                MySqlCommand oCmd = new MySqlCommand(oString, myConnection);
                //oCmd.Parameters.AddWithValue("@ADSLPhone", ADSLPhone);
                myConnection.Open();
                using (MySqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        test.Add(new test() { 
                            LastStatus=oReader["last_status"].ToString(),
                            Name = oReader["name"].ToString()

                        });
                    }
                    myConnection.Close();
                }
            }
            return Json(test, JsonRequestBehavior.AllowGet);
        }
        #endregion
        //Added By Zafari
        #region Json Methods

        #region Read
        public JsonResult Employees_Read(Guid? GroupID, int? pageSize, int? pageNumber,string sort,List<FilterData> filter,string FirstName,string LastName)
        {
            GetGeneralResponse<IEnumerable<EmployeeView>> response = new GetGeneralResponse<IEnumerable<EmployeeView>>();

            #region Preparing Filter

            IList<FilterData> Filter=new List<FilterData>();

            if (!string.IsNullOrEmpty(FirstName))
            {
                Filter.Add(new FilterData()
                {
                    data = new data()
                    {
                      comparison  = "eq",
                      type = "string",
                      value = new[]{FirstName}
                    },
                    field = "FirstName"
                });
            }

            if (!string.IsNullOrEmpty(LastName))
            {
                Filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "eq",
                        type = "string",
                        value = new[] { LastName }
                    },
                    field = "FirstName"
                });
            }

            if(filter!=null)
                foreach(var item in filter)
                    Filter.Add(item);

            #endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _employeeService.GetEmployees(GroupID, PageSize, PageNumber, Filter.ToList(),ConvertJsonToObject(sort).ToList());

            //خالی کردن پرمیشن ها
            foreach (EmployeeView employeeView in response.data)
            {
                employeeView.Permissions = null;
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }



        public JsonResult Permissions_Read_ByEmployee(Guid EmployeeID, int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<PermissionViewJ>> response = new GetGeneralResponse<IEnumerable<PermissionViewJ>>();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Employee_Read");

            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            EmployeeView employeeView = _employeeService.GetEmployee(new GetRequest() { ID = EmployeeID }).EmployeeView;

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            int index = (PageNumber - 1) * PageSize;
            int count = PageSize;

            IEnumerable<IGrouping<string, PermitView>> groupedPermissions = count != -1 ?
                employeeView.Permissions.GroupBy(g => g.Permission.Group).Skip(index).Take(count) :
                employeeView.Permissions.GroupBy(g => g.Permission.Group);

            // تبدیل گروهها به مدل مورد نیاز یوآی
            List<PermissionViewJ> permissionViews = new List<PermissionViewJ>();
            foreach (var permitCollection in groupedPermissions)
            {
                PermissionViewJ permissionView = new PermissionViewJ();
                permissionView.Group = permitCollection.First().Permission.Group;

                if (permitCollection.Where(w => w.Permission.Title == "Read").Count() > 0)
                {
                    permissionView.ReadID = permitCollection.Where(w => w.Permission.Title == "Read").First().ID;
                    permissionView.ReadGuaranteed = permitCollection.Where(w => w.Permission.Title == "Read").First().Guaranteed;
                    permissionView.ReadKey = permitCollection.Where(w => w.Permission.Title == "Read").First().PermitKey;
                }
                if (permitCollection.Where(w => w.Permission.Title == "Update").Count() > 0)
                {
                    permissionView.UpdateID = permitCollection.Where(w => w.Permission.Title == "Update").First().ID;
                    permissionView.UpdateGuaranteed = permitCollection.Where(w => w.Permission.Title == "Update").First().Guaranteed;
                    permissionView.UpdateKey = permitCollection.Where(w => w.Permission.Title == "Update").First().PermitKey;
                }
                if (permitCollection.Where(w => w.Permission.Title == "Insert").Count() > 0)
                {
                    permissionView.InsertID = permitCollection.Where(w => w.Permission.Title == "Insert").First().ID;
                    permissionView.InsertGuaranteed = permitCollection.Where(w => w.Permission.Title == "Insert").First().Guaranteed;
                    permissionView.InsertKey = permitCollection.Where(w => w.Permission.Title == "Insert").First().PermitKey;
                }
                if (permitCollection.Where(w => w.Permission.Title == "Delete").Count() > 0)
                {
                    permissionView.DeleteID = permitCollection.Where(w => w.Permission.Title == "Delete").First().ID;
                    permissionView.DeleteGuaranteed = permitCollection.Where(w => w.Permission.Title == "Delete").First().Guaranteed;
                    permissionView.DeleteKey = permitCollection.Where(w => w.Permission.Title == "Delete").First().PermitKey;
                }
                permissionViews.Add(permissionView);
            }

            response.data = permissionViews;
            response.totalCount = permissionViews.Count();

            return Json(response, JsonRequestBehavior.AllowGet);


        }
        #endregion

        #region Update
        /// <summary>
        /// ویرایش یک کارمند
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult Employee_Update(EditEmployeeRequest request, HttpPostedFileBase file)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {


                #region Check Access
                bool hasPermission = GetEmployee().IsGuaranteed("Employee_Update");
                if (!hasPermission)
                {
                    response.ErrorMessages.Add("AccessDenied");
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                #endregion

                #region Upload file

                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {
                    // extract the extention
                    var fileExtention = Path.GetExtension(file.FileName);
                    // create filename
                    //string fileName = response.ID + "." + fileExtention;
                    // fileName = Path.GetFileName(file.FileName);

                    // Create a unique file name
                    string fileName = request.LoginName;

                    // Gettin current Year and Month
                    PersianCalendar pc = new PersianCalendar();
                    int year = pc.GetYear(DateTime.Now);
                    int month = pc.GetMonth(DateTime.Now);

                    // Create File Path
                    string path = Path.Combine(Server.MapPath("~/data/EmployeePicture"), fileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    // Create reqired directried if not exist
                    new FileInfo(path).Directory.Create();

                    // Uploading
                    using (var fs = new FileStream(path, FileMode.Create))
                    {
                        var buffer = new byte[file.InputStream.Length];
                        file.InputStream.Read(buffer, 0, buffer.Length);

                        fs.Write(buffer, 0, buffer.Length);
                    }
                    request.Picture = path;
                }

                #endregion
            }
            catch (Exception ex)
            {
                //response.success = false;
                response.ErrorMessages.Add("در آپلود کردن فایل خطایی به وجود آمده است.");
                return Json(response, JsonRequestBehavior.AllowGet);
            }


            response = _employeeService.EditEmployee(request,GetEmployee().ID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Employee_Permissions_Update(IEnumerable<PermissionViewJ> requests, Guid EmployeeID)
        {

            GeneralResponse response = new GeneralResponse();



            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Employee_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion


            List<EditPermissionRequestG> editPermissionRequests = new List<EditPermissionRequestG>();

            foreach (var request in requests)
            {
                EditPermissionRequestG insertRequest = new EditPermissionRequestG();
                EditPermissionRequestG updateRequest = new EditPermissionRequestG();
                EditPermissionRequestG deleteRequest = new EditPermissionRequestG();
                EditPermissionRequestG readRequest = new EditPermissionRequestG();

                if (request.InsertKey != null)
                {
                    insertRequest.Key = request.InsertKey;
                    insertRequest.Guaranteed = request.InsertGuaranteed;
                    editPermissionRequests.Add(insertRequest);

                }

                if (request.UpdateKey != null)
                {
                    updateRequest.Key = request.UpdateKey;
                    updateRequest.Guaranteed = request.UpdateGuaranteed;
                    editPermissionRequests.Add(updateRequest);
                }

                if (request.ReadKey != null)
                {
                    readRequest.Key = request.ReadKey;
                    readRequest.Guaranteed = request.ReadGuaranteed;
                    editPermissionRequests.Add(readRequest);
                }

                if (request.DeleteKey != null)
                {
                    deleteRequest.Key = request.DeleteKey;
                    deleteRequest.Guaranteed = request.DeleteGuaranteed;
                    editPermissionRequests.Add(deleteRequest);
                }
            }

            response = _employeeService.EditEmployeePermission(editPermissionRequests, EmployeeID, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ویرایش گروهی کارمندان
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        public JsonResult Employese_Update(IEnumerable<EditEmployeeRequest> Employees)
        {
            GeneralResponse response = new GeneralResponse();


            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Employee_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _employeeService.EditEmployees(Employees,GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult Employees_Simple_Update(IEnumerable<EditSimpleEmployeeRequest> Employees)
        {
            GeneralResponse response = new GeneralResponse();
            response = _employeeService.EditSimpleEmployees(Employees);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Employee_UpdatePassword(string CurrentPassword, string NewPassword, string ConfirmPassword)
        {
            GeneralResponse response = new GeneralResponse();

            response = _employeeService.ChangePassword(CurrentPassword, NewPassword, ConfirmPassword, GetEmployee().ID);

            return Json(response,JsonRequestBehavior.AllowGet);
        }

        #region Insert
        // Added By Hojjat:
        public JsonResult Employee_Insert(AddEmployeeRequest request)
        {
            GeneralResponse response = new GeneralResponse();
        
            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Employee_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            try
            {
                
            }
            catch (Exception ex)
            {
                //response.success = false;
                response.ErrorMessages.Add("در آپلود کردن فایل خطایی به وجود آمده است.");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            request.Discontinued = false;

            response = _employeeService.AddEmployee(request,GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Employee_Picture_Insert(Guid EmployeeID, HttpPostedFileBase file)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                #region Upload file

                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {
                    // extract the extention
                    var fileExtention = Path.GetExtension(file.FileName);
                    // create filename
                    //string fileName = response.ID + "." + fileExtention;
                    // fileName = Path.GetFileName(file.FileName);

                    // Create a unique file name
                    string fileName = _employeeService.GetEmployee(new GetRequest() { ID = EmployeeID }).EmployeeView.ID.ToString();
                    fileName = Path.GetFileName(file.FileName);

                    // Gettin current Year and Month
                    PersianCalendar pc = new PersianCalendar();
                    int year = pc.GetYear(DateTime.Now);
                    int month = pc.GetMonth(DateTime.Now);

                    // Create File Path
                    string path = Path.Combine(Server.MapPath("~/data/EmployeePicture"), fileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    // Create reqired directried if not exist
                    new FileInfo(path).Directory.Create();

                    // Uploading
                    using (var fs = new FileStream(path, FileMode.Create))
                    {
                        var buffer = new byte[file.InputStream.Length];
                        file.InputStream.Read(buffer, 0, buffer.Length);

                        fs.Write(buffer, 0, buffer.Length);
                    }
                    response = _employeeService.InsertPicture(EmployeeID, path);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return Json(response, JsonRequestBehavior.AllowGet) ;

            #endregion
        }
        #endregion

        #region Delete
        ///// <summary>
        ///// حذف تکی یک کارمند  //Added By Zafari
        ///// </summary>
        ///// <param name="EmployeeID"></param>
        ///// <returns></returns>
        //public JsonResult Employee_Delete(Guid EmployeeID)
        //{
        //    GeneralResponse response = new GeneralResponse();
        //    EmployeeDetailView employeeDetailView = new EmployeeDetailView();
        //    employeeDetailView.EmployeeView = GetEmployee();

        //    #region Check Access
        //    bool hasPermission = GetEmployee().IsGuaranteed("Employee_Delete");
        //    if (!hasPermission)
        //    {
        //        response.ErrorMessages.Add("AccessDenied");
        //        return Json(response, JsonRequestBehavior.AllowGet);
        //    }
        //    #endregion

        //    employeeDetailView.EmployeeMainView = this.GetEmployeeView(EmployeeID.ToString());

        //    DeleteRequest request = new DeleteRequest() { ID = EmployeeID };

        //    response = this._employeeService.DeleteEmployee(request);

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// حذف گروهی کارمندان  //Added By Zafari
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        public JsonResult Employees_Delete(IEnumerable<EmployeeView> Employees)
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Employee_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            IList<DeleteRequest> request = new List<DeleteRequest>();

            foreach (EmployeeView employeeView in Employees)
            {
                request.Add(new DeleteRequest() { ID = employeeView.ID });
            }
            response = _employeeService.DeleteEmployees(request);

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #endregion

        public JsonResult LocalPhones_Read(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<LocalPhoneView>> response=new GetGeneralResponse<IEnumerable<LocalPhoneView>>();

            

            Guid EmployeeID = GetEmployee().ID;

            response = _localPhoneService.GetLocalPhonesByEmployee(EmployeeID);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public JsonResult RecreateSaPermissions()
        {
            GeneralResponse response=new GeneralResponse();

            #region Check Access
            //bool hasPermission = GetEmployee().IsGuaranteed("Permission_Insert");
            //if (!hasPermission)
            //{
            //    ModelState.AddModelError("", "AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}
            #endregion

            response = _employeeService.RecreateSaPermissions();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInstallerExpert()
        {
            GetGeneralResponse<IEnumerable<EmployeeView>> response = new GetGeneralResponse<IEnumerable<EmployeeView>>();

            #region Check Access
            //bool hasPermission = GetEmployee().IsGuaranteed("Permission_Insert");
            //if (!hasPermission)
            //{
            //    ModelState.AddModelError("", "AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}
            #endregion

            response = _employeeService.GetEmployeesInstaller();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #region Private Members

        private EmployeeView GetEmployeeView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetEmployeeResponse response = this._employeeService.GetEmployee(request);

            return response.EmployeeView;
        }
        #endregion

        #region Get All Child of Employee

        public JsonResult Employees_GetAllChilds(Guid EmployeeID)
        {
            GetGeneralResponse<IEnumerable<EmployeeView>> response=new GetGeneralResponse<IEnumerable<EmployeeView>>();
            response = _employeeService.GetAllChilOfAnEmployee(EmployeeID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }


}
