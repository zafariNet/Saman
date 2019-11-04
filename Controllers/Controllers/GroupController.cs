using System;
using System.Collections.Generic;
using System.Linq;
using Services.Interfaces;
using System.Web.Mvc;
using Services.ViewModels.Employees;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Model.Employees;

namespace Controllers.Controllers
{
    [Authorize]
    public class GroupController : BaseController
    {
        #region Declares
        private readonly IEmployeeService _employeeService;

        private readonly IGroupService _groupService;

        private readonly IPermissionService _permissionService;

        

        #endregion

        #region Ctor
        public GroupController(IEmployeeService employeeService, IGroupService groupService
            , IPermissionService permissionService)
            : base(employeeService)
        {
            this._groupService = groupService;
            this._employeeService = employeeService;
            _permissionService = permissionService;
        }
        #endregion

        #region New Methods 

        #region Read

        public JsonResult SimpleGroup_Read()
        {
            GetGeneralResponse<IEnumerable<SimpleGroupView>> response = new GetGeneralResponse<IEnumerable<SimpleGroupView>>();
            response = _groupService.GetSimpleGroup();

            return Json(response,JsonRequestBehavior.AllowGet);
        }

        public JsonResult Groups_Read(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<GroupView>> response = new GetGeneralResponse<IEnumerable<GroupView>>();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Group_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _groupService.GetGroups(PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// لیست گروه ها بدون دسترسی ها
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public JsonResult Just_Groups_Read(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<GroupView>> response = new GetGeneralResponse<IEnumerable<GroupView>>();

            

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _groupService.GetGroups(PageSize, PageNumber);
            IList<GroupView> list = new List<GroupView>();
            foreach (GroupView group in response.data)
            {
                group.Permissions = null;
                //group.Employees = null;
                list.Add(group);
            }
            response.data = list;
            response.totalCount = list.Count();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //Added By Zafari
        //برگرداند کارمندان عضو یک گروه
        public JsonResult Employee_Read_ByGroup(IEnumerable<Guid> GroupIDs)
        {
            GetGeneralResponse<IEnumerable<SimpleEmployeeView>> response = new GetGeneralResponse<IEnumerable<SimpleEmployeeView>>();
            GetGeneralResponse<IEnumerable<EmployeeView>> request = new GetGeneralResponse<IEnumerable<EmployeeView>>();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Employee_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            IList<SimpleEmployeeView> list = new List<SimpleEmployeeView>();
            foreach (Guid GroupID in GroupIDs)
            {
                request = _employeeService.GetEmployeesByGroup(GroupID);

                foreach (EmployeeView employeeView in request.data)
                {
                    if(!employeeView.Discontinued)
                    list.Add(new SimpleEmployeeView()
                    {
                        ID = employeeView.ID,
                        EmployeeID = employeeView.ID,
                        Name = employeeView.Name,
                        GroupID = employeeView.GroupID,
                        GroupName = employeeView.GroupName,
                    });
                }
            }
            response.data = list;
            response.totalCount = request.totalCount;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        

        public JsonResult Employees_Read_ByGroup(Guid GroupID, int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<SimpleEmployeeView>> response = new GetGeneralResponse<IEnumerable<SimpleEmployeeView>>();
            GetGeneralResponse<IEnumerable<EmployeeView>> request = new GetGeneralResponse<IEnumerable<EmployeeView>>();
            IList<SimpleEmployeeView> list = new List<SimpleEmployeeView>();
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            request = _employeeService.GetEmployeesByGroup(GroupID, PageSize, PageNumber);
            foreach (EmployeeView employeeView in request.data)
            {
                if (!employeeView.Discontinued)
                    list.Add(new SimpleEmployeeView()
                    {
                        ID = employeeView.ID,
                        EmployeeID = employeeView.ID,
                        Name = employeeView.Name,
                        GroupID = employeeView.GroupID,
                        GroupName = employeeView.GroupName,
                    });
            }
            response.data = list;
            response.totalCount = request.totalCount;

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        //Added By Zafari
        // برگرداند دسترسی های یک گروه
        public JsonResult Permissions_Test(Guid GroupID, int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<Permit>> response = new GetGeneralResponse<IEnumerable<Permit>>();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("GroupPermission_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            int index = (PageNumber - 1) * PageSize;
            int count = PageSize;

            GroupView groupView = _groupService.GetGroup(new GetRequest() { ID = GroupID }).GroupView;
            
            response.data = count != -1 ? groupView.Permissions.Skip(index).Take(count) : groupView.Permissions;
            response.totalCount = groupView.Permissions.Count();

            //.GroupBy(g => g.Permission.Group)
            return Json(response.data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Permissions_Read_ByGroup (Guid GroupID, int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<PermissionViewJ>> response = new GetGeneralResponse<IEnumerable<PermissionViewJ>>();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Group_Read");
            
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            GroupView groupView = _groupService.GetGroup(new GetRequest() { ID = GroupID }).GroupView;

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            int index = (PageNumber - 1) * PageSize;
            int count = PageSize;

            IEnumerable<IGrouping<string, Permit>> groupedPermissions = count != -1 ?
                groupView.Permissions.GroupBy(g => g.Permission.Group).Skip(index).Take(count) :
                groupView.Permissions.GroupBy(g => g.Permission.Group);

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

        //Added By Zafari
        // خواند گروه ها
        public JsonResult GroupTree_Read(Guid? GroupID)
        {

            GetGeneralResponse<IEnumerable<GroupTreeView>> response = new GetGeneralResponse<IEnumerable<GroupTreeView>>();
            GetGeneralResponse<IEnumerable<GroupView>> groupViews = new GetGeneralResponse<IEnumerable<GroupView>>();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Group_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion
            
            groupViews = _groupService.GetGroups(-1, -1);
            var treeView = new GetGeneralResponse<IEnumerable<GroupView>>();
            if (GroupID != null)
            {
                treeView.data = groupViews.data.Where(x => x.ParentGroupID == GroupID).ToList();
            }
            else
            {
                treeView.data = groupViews.data.Where(x => x.ParentGroupID == Guid.Empty).ToList();
            }

            IList<GroupTreeView> groupTreeViews = new List<GroupTreeView>();
            
            foreach (GroupView group in treeView.data)
            {
                groupTreeViews.Add(new GroupTreeView() { 
                GroupName=group.GroupName,
                GroupID=group.ID,
                ParentGroupID=group.ParentGroupID,
                GroupStaffID=group.GroupStaffID==null ? Guid.Empty : (Guid)group.GroupStaffID,
                RowVersion=group.RowVersion,
                TotalEmployees=group.EmployeeCount
                });
            }

            response.data = groupTreeViews;
            response.totalCount = groupTreeViews.Count();
            
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert
        public JsonResult Group_Insert(string GroupName, Guid? ParentGroupID, Guid? GroupStaffID)
        {
            // تست شده؟ اگر شده در فایل اکسل هم جواب دریافتی از سرور نوشته شود
            GeneralResponse response = new GeneralResponse();
            AddGroupRequest request = new AddGroupRequest();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Group_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            request.CreateEmployeeID = GetEmployee().ID;
            request.GroupName = GroupName;
            request.GroupStaffID = GroupStaffID == null ? Guid.Empty : (Guid) GroupStaffID;
            request.ParentGroupID = ParentGroupID == null ? Guid.Empty : (Guid) ParentGroupID;
                       
            response = _groupService.AddGroup(request);

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Update
        public JsonResult Group_Update(EditGroupRequest request)
        {
            
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Group_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            //request.ID = request.ID; ;
            //request.GroupName = request.GroupName;
            //request.GroupStaaffID = request.GroupStaaffID == null ? Guid.Empty : (Guid)request.GroupStaaffID;
            //request.ParentGroupID = request.ParentGroupID == null ? Guid.Empty : (Guid)request.ParentGroupID;

            response = _groupService.EditGroup(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Group_Permissions_Update(IEnumerable<PermissionViewJ> requests, Guid GroupID)
        {

            GeneralResponse response = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("GroupPermission_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
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

            response = _groupService.EditGroup(editPermissionRequests, GroupID, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// تغییر سر گروه یک گروه
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="ParentGroupID"></param>
        /// <returns></returns>
        public JsonResult Group_Parent_Update(Guid GroupID , Guid ParentGroupID)
        {
            
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Group_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion


            response = _groupService.ChangeGroupParent(GroupID, ParentGroupID,GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ویرایش و افزودن کارمندان  یگ گروه
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public JsonResult Group_Employee_Update(IEnumerable<SimpleEmployeeView>  Employees, Guid GroupID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Group_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _employeeService.ChangeGroup(Employees.Select(x => x.EmployeeID), GroupID);
            
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Delete
        public JsonResult Group_Delete(Guid GroupID , Guid ReplaceGroupID)
        {
            GetGeneralResponse<IEnumerable<EmployeeView>> employees = _employeeService.GetEmployeesByGroup(GroupID);
            GetGeneralResponse<IEnumerable<GroupView>> ChildGroups = _groupService.GetChildGroups(GroupID);
            GeneralResponse employeeResponse = new GeneralResponse();
            GeneralResponse groupResponse = new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Group_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(groupResponse, JsonRequestBehavior.AllowGet);
            }
            #endregion

            IList<Guid> EmployeeIDs = new List<Guid>();
            foreach(EmployeeView employeeView in employees.data)
            {
                EmployeeIDs.Add(employeeView.ID);
            }

            employeeResponse = _employeeService.ChangeGroup(EmployeeIDs, ReplaceGroupID);

            foreach (var item in ChildGroups.data)
            {
                _groupService.ChangeGroupParent(item.ID, ReplaceGroupID,GetEmployee().ID);
            }

            if(employeeResponse.hasError != true)
            {
                groupResponse = _groupService.DeleteGroup(new DeleteRequest() { ID = GroupID });
            }

            return Json(groupResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Add New Permission To Groups And Users

        public JsonResult AddNewPermissionsToGroupAndUsers(Guid PermissionID)
        {
            GeneralResponse response=new GeneralResponse();
            response = _groupService.AddNewPermissionsToGroupAndUsers(PermissionID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Private Members

        private GroupView GetGroupView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetGroupResponse response = this._groupService.GetGroup(request);

            return response.GroupView;
        }
        #endregion
    }

    public class PermissionViewJ
    {
        public string Group { get; set; }
        // Read
        public Guid ReadID { get; set; }
        public bool ReadGuaranteed { get; set; }
        public string ReadKey { get; set; }
        // Update
        public Guid UpdateID { get; set; }
        public bool UpdateGuaranteed { get; set; }
        public string UpdateKey { get; set; }
        // Insert
        public Guid InsertID { get; set; }
        public bool InsertGuaranteed { get; set; }
        public string InsertKey { get; set; }
        // Delete
        public Guid DeleteID { get; set; }
        public bool DeleteGuaranteed { get; set; }
        public string DeleteKey { get; set; }


    }

}