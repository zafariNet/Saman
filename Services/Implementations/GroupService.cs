#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Employees.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Model.Employees;
using Services.ViewModels.Employees;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Infrastructure.Domain;
using Infrastructure.Querying;

#endregion

namespace Services.Implementations
{
    public class GroupService : IGroupService
    {
        #region Declares
        private readonly IGroupRepository _groupRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermitRepository _permitRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _uow;
        #endregion

        #region Ctor
        public GroupService(IGroupRepository groupRepository, IUnitOfWork uow)
        {
            _groupRepository = groupRepository;
            _uow = uow;
        }

        public GroupService(IGroupRepository groupRepository, IEmployeeRepository employeeRepository, 
            IPermissionRepository permissionRepository,
            IPermitRepository permitRepository,
            IUnitOfWork uow)
            : this(groupRepository, uow)
        {
            this._employeeRepository = employeeRepository;
            _permissionRepository = permissionRepository;
            _permitRepository = permitRepository;
        }
        #endregion

        #region Add
        public GeneralResponse AddGroup(AddGroupRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Group group = new Group();
                group.ID = Guid.NewGuid();
                group.CreateDate = PersianDateTime.Now;
                group.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                group.GroupStaff = _employeeRepository.FindBy(request.GroupStaffID);
                group.GroupName = request.GroupName;
                group.ParentGroup = _groupRepository.FindBy(request.ParentGroupID);
                group.RowVersion = 1;

                IEnumerable<Permission> permissions = _permissionRepository.FindAll();
                IList<Permit> permits = new List<Permit>();
                foreach (var permission in permissions)
                {
                    permits.Add(new Permit(permission) { Guaranteed = false });
                }

                group.Permissions = permits;

                // Validation
                if (group.GetBrokenRules().Count() > 0)
                {
                    foreach (BusinessRule businessRule in group.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                _groupRepository.Add(group);
                _uow.Commit();

            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        #endregion

        #region Edit

        public GeneralResponse EditGroupPermissions(EditGroupPermissionsRequest request, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            //Group group = new Group();
            //group = _groupRepository.FindBy(request.GroupID);

            
            //    Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

            //    Criterion criteriaGroupID = new Criterion("Group.ID", request.GroupID, CriteriaOperator.Equal);
            //    query.Add(criteriaGroupID);

            //    IEnumerable<Permission> FinalPermit = _permissionRepository.FindBy(query);

            //    group.Permissions = FinalPermit;
            //group.ModifiedDate = PersianDateTime.Now;
            //group.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);

            //_groupRepository.Save(group);
            //_uow.Commit();

            return response;
        }

        public GeneralResponse ChangeGroupParent(Guid GroupID, Guid ParentID, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                    Group group = _groupRepository.FindBy(GroupID);
                    if (group.ParentGroup.ID != null)
                    {
                        group.ParentGroup = _groupRepository.FindBy(ParentID);
                        group.ModifiedDate = PersianDateTime.Now;
                        group.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);

                        _groupRepository.Save(group);
                        _uow.Commit();
                    }

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        public GeneralResponse EditGroup(EditGroupRequest request, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            Group group = new Group();
            group = _groupRepository.FindBy(request.ID);

            if (group != null)
            {
                try
                {
                    group.ModifiedDate = PersianDateTime.Now;
                    if (request.ParentGroupID != null)
                        group.ParentGroup = _groupRepository.FindBy(request.ParentGroupID);
                    group.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                    if (request.GroupStaffID != null)
                        group.GroupStaff = _employeeRepository.FindBy(request.GroupStaffID);
                    if (request.Permissions != null)
                        group.Permissions = request.Permissions;

                    if (request.GroupName != null)
                        group.GroupName = request.GroupName;

                    if (group.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        group.RowVersion ++;
                    }

                    if (group.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in group.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _groupRepository.Save(group);
                    _uow.Commit();

                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                }
            }
            else
            {

                response.ErrorMessages.Add("NoItemToEditKey");
            }
            return response;
        }
        /// <summary>
        /// ویرایش دسترسیهای یک گروه
        /// </summary>
        public GeneralResponse EditGroup(IEnumerable<EditPermissionRequestG> requests , Guid GroupID,Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            Group group = new Group();
             
            group = _groupRepository.FindBy(GroupID);
            
            

            if (group != null)
            {
                try
                {
                    foreach (EditPermissionRequestG request in requests)
                    {

                        
                        if (group.Permissions.Where(s => s.PermitKey == request.Key).FirstOrDefault() != null)
                        {
                            group.Permissions.Where(s => s.PermitKey == request.Key).FirstOrDefault().Guaranteed = request.Guaranteed;
                        }
                        else
                        {
                            IList<Permit> permits = group.Permissions.ToList();
                            IEnumerable<Permission> permissions = _permissionRepository.FindAll();

                            permits.Add(new Permit(permissions.Where(s => s.Key == request.Key).FirstOrDefault()) { Guaranteed = request.Guaranteed });
                            group.Permissions = permits;
                        }

                        #region Update Permissions of Employees

                        IEnumerable<Employee> editedEmployees = UpdateEmployeesPermissions(group.Employees, request.Key, request.Guaranteed);

                        #endregion

                        _groupRepository.Save(group);
                    }
                    _uow.Commit();
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                }
            }
            else
            {

                response.ErrorMessages.Add("هیچ موردی جهت ویرایش وجود ندارد.");
            }

            return response;
        }

        private IEnumerable<Employee> UpdateEmployeesPermissions(IEnumerable<Employee> employees, string permitKey, bool guaranteed)
        {
            foreach (var employee in employees)
            {
                try
                {
                    if (employee.Permissions.Where(s => s.PermitKey == permitKey).FirstOrDefault() != null)
                    {
                        employee.Permissions.Where(s => s.PermitKey == permitKey).FirstOrDefault().Guaranteed = guaranteed;
                    }
                    else
                    {
                        IList<Permit> permits = employee.Permissions.ToList();
                        IEnumerable<Permission> permissions = _permissionRepository.FindAll();

                        permits.Add(new Permit(permissions.Where(s => s.Key == permitKey).FirstOrDefault()) { Guaranteed = guaranteed });
                        employee.Permissions = permits;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return employees;
        }

        #endregion

        #region Delete
        public GeneralResponse DeleteGroup(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            Group group = new Group();
            group = _groupRepository.FindBy(request.ID);

            if (group.Employees == null)
            {
                
                response.ErrorMessages.Add("چند کارمند عضو این گروه هستند، بنابراین حذف آن غیر ممکن است.");
                return response;
            }

            if (group != null)
            {
                try
                {
                    _groupRepository.Remove(group);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                }
            }

            return response;
        }
        #endregion

        #region Get One
        public GetGroupResponse GetGroup(GetRequest request)
        {
            GetGroupResponse response = new GetGroupResponse();

            try
            {
                Group group = new Group();

                GroupView groupView = group.ConvertToGroupView();

                group = _groupRepository.FindBy(request.ID);
                if (group != null)
                    groupView = group.ConvertToGroupView();

                response.GroupView = groupView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get All
        public GetGeneralResponse<IEnumerable<GroupView>> GetGroups(int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<GroupView>> response = new GetGeneralResponse<IEnumerable<GroupView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<Group> groups = _groupRepository.FindAll(index, count);
                response.data = groups.data.ConvertToGroupViews();
                response.totalCount = groups.totalCount;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<GroupView>> GetChildGroups(Guid GroupID)
        {
            GetGeneralResponse<IEnumerable<GroupView>> response = new GetGeneralResponse<IEnumerable<GroupView>>();

            Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
            Criterion ParentGroupID = new Criterion("ParentGroup.ID", GroupID, CriteriaOperator.Equal);

            Response<Group> groupViews = _groupRepository.FindBy(query, -1, -1);

            response.data = groupViews.data.ConvertToGroupViews().Where(x=>x.ParentGroupID==GroupID);
            response.totalCount = groupViews.totalCount;

            return response;
        }

        public GetGeneralResponse<IEnumerable<SimpleGroupView>> GetSimpleGroup()
        {
            GetGeneralResponse<IEnumerable<SimpleGroupView>> response = new GetGeneralResponse<IEnumerable<SimpleGroupView>>();

            IEnumerable<Group> groups = _groupRepository.FindAll();
            var _groups = new List<SimpleGroupView>();
            foreach (var item in groups)
            {
                SimpleGroupView simple = new SimpleGroupView();
                simple.GroupName = item.GroupName;
                simple.ID = item.ID;
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion employeectr = new Criterion("Group.ID", item.ID, CriteriaOperator.Equal);
                query.Add(employeectr);

                IEnumerable<Employee> employee = _employeeRepository.FindBy(query);
                var simpleEmployee = new List<SimpleEmployeeView>();
                foreach (var _item in employee)
                {
                    simpleEmployee.Add(new SimpleEmployeeView() { 
                        EmployeeID=_item.ID,
                        ID=_item.ID,
                        Name = _item.FirstName +" "+ _item.LastName,
                        FirstName = _item.LastName,
                        LastName = _item.LastName,
                        Discontinued=_item.Discontinued
                    });
                }
                simple.Employees = simpleEmployee;
                _groups.Add(simple);
            }

            response.data = _groups;
            response.totalCount = _groups.Count();

            return response;

        }
        #endregion

        #region Add New Permission To Groups And Users

        public GeneralResponse AddNewPermissionsToGroupAndUsers(Guid PermissionID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                Permission permission=new Permission();

                permission = _permissionRepository.FindBy(PermissionID);

                IEnumerable<Employee> employees = _employeeRepository.FindAll();
                IEnumerable<Group> groups = _groupRepository.FindAll();

                foreach (var employee in employees)
                {
                    
                    if ( employee.Permissions.Where(x => x.ID == permission.ID).Count()== 0)
                    {
                        Permit permit = new Permit();
                        permit.ID = Guid.NewGuid();
                        permit.Employee = employee;
                        permit.CreateDate = PersianDateTime.Now;
                        permit.CreateEmployee = employee;
                        permit.Permission = permission;
                        permit.Guaranteed = false;

                        _permitRepository.Add(permit);
                    }
                }
                foreach (var group in groups)
                {
                    if (group.Permissions.Where(x => x.ID == permission.ID).Count() == 0)
                    {
                        Permit permit = new Permit(); 
                        permit.ID = Guid.NewGuid();
                        permit.Group = group;
                        permit.Employee = null;
                        permit.CreateDate = PersianDateTime.Now;
                        permit.CreateEmployee = null;
                        permit.Permission = permission;
                        permit.Guaranteed = false;

                        _permitRepository.Add(permit);
                    }
                }
                _uow.Commit();
            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        #endregion

    }
}
