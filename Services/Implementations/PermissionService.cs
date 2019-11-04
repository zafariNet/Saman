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

namespace Services.Implementations
{
    public class PermissionService : IPermissionService
    {
        #region Declares
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IGroupRepository _groupRepository;
        #endregion

        #region Ctor
        public PermissionService(IPermissionRepository permissionRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository, IGroupRepository groupRepository)
        {
            _permissionRepository = permissionRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
            _groupRepository = groupRepository;
        }
        #endregion

        #region Add
        public GeneralResponse AddPermission(AddPermissionRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Permission permission = new Permission();
                permission.ID = Guid.NewGuid();
                permission.CreateDate = PersianDateTime.Now;
                permission.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                permission.Group = request.Group;
                permission.Key = request.Key;
                permission.Title = request.Title;
                permission.RowVersion = 1;

                _permissionRepository.Add(permission);
                _uow.Commit();

                // Validation
                if (permission.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in permission.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        #endregion

        #region Edit
        public GeneralResponse EditPermission(EditPermissionRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            //Permission permission = new Permission();
            //permission = _permissionRepository.FindBy(request.ID);

            //if (permission != null)
            //{
            //    try
            //    {
            //        permission.ModifiedDate = PersianDateTime.Now;
            //        permission.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
            //        if (request.Key != null)
            //            permission.Key = request.Key;
            //        if (request.Title != null)
            //            permission.Title = request.Title;

            //        if (permission.RowVersion != request.RowVersion)
            //        {
            //            response.hasCenter = false;
            //            response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
            //            return response;
            //        }
            //        else
            //        {
            //            permission.RowVersion += 1;
            //        }

            //        if (permission.GetBrokenRules().Count() > 0)
            //        {
            //            response.hasCenter = false;
            //            foreach (BusinessRule businessRule in permission.GetBrokenRules())
            //            {
            //                response.ErrorMessages.Add(businessRule.Rule);
            //            }

            //            return response;
            //        }

            //        _permissionRepository.Save(permission);
            //        _uow.Commit();

            //        response.hasCenter = true;
            //    }
            //    catch (Exception ex)
            //    {
            //        response.hasCenter = false;
            //        response.ErrorMessages.Add(ex.Message);
            //    }
            //}
            //else
            //{
            //    response.hasCenter = false;
            //    response.ErrorMessages.Add("هیچ موردی جهت ویرایش وجود ندارد.");
            //}
            return response;
        }
        #endregion

        #region Delete
        public GeneralResponse DeletePermission(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            Permission permission = new Permission();
            permission = _permissionRepository.FindBy(request.ID);

            if (permission != null)
            {
                try
                {
                    _permissionRepository.Remove(permission);
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
        public GetPermissionResponse GetPermission(GetRequest request)
        {
            GetPermissionResponse response = new GetPermissionResponse();

            try
            {
                Permission permission = new Permission();
                PermissionView permissionView = permission.ConvertToPermissionView();

                permission = _permissionRepository.FindBy(request.ID);
                if (permission != null)
                    permissionView = permission.ConvertToPermissionView();

                response.PermissionView = permissionView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get All
        public GetPermissionsResponse GetPermissions()
        {
            GetPermissionsResponse response = new GetPermissionsResponse();

            try
            {
                IEnumerable<PermissionView> permissions = _permissionRepository.FindAll()
                    .ConvertToPermissionViews();

                response.PermissionViews = permissions;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get Permissions of One Employee
        public GetPermissionsResponse GetPermissions(GetPermissionsRequest request)
        {
            GetPermissionsResponse response = new GetPermissionsResponse();

            EmployeeView employeeView = new EmployeeView();
            employeeView = _employeeRepository.FindBy(request.EmployeeID).ConvertToEmployeeView();

            try
            {
                IEnumerable<PermissionView> permissions = _permissionRepository.FindAll()
                    .ConvertToPermissionViews();

                response.PermissionViews = permissions;
                foreach (PermissionView permissionView in response.PermissionViews)
                {
                    bool guaranteed = (from p in employeeView.Permissions
                                       where p.PermitKey == permissionView.Key
                                       select p.Guaranteed).FirstOrDefault();
                    // بخاطر اینکه بصورت آجاکسی بتوان ویرایش کرد ناچاریم آیدی کارمند را به جای آیدی پرمیشن جا بزنیم
                    permissionView.ID = employeeView.ID;
                    permissionView.Guaranteed = guaranteed;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
        #endregion

        #region Get Permissions of One Group
        public GetPermissionsResponse GetPermissions(Guid GroupID)
        {
            GetPermissionsResponse response = new GetPermissionsResponse();
            
            GroupView groupView = new GroupView();
            groupView = _groupRepository.FindBy(GroupID).ConvertToGroupView();

            try
            {
                IEnumerable<PermissionView> permissions = _permissionRepository.FindAll()
                    .ConvertToPermissionViews();

                response.PermissionViews = permissions;
                foreach (PermissionView permissionView in response.PermissionViews)
                {
                    bool guaranteed = (from p in groupView.Permissions
                                       where p.PermitKey == permissionView.Key
                                       select p.Guaranteed).FirstOrDefault();
                    // بخاطر اینکه بصورت آجاکسی بتوان ویرایش کرد ناچاریم آیدی گروه را به جای آیدی پرمیشن جا بزنیم
                    permissionView.ID = groupView.ID;
                    permissionView.Guaranteed = guaranteed;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<PermissionView>> GetPermissions(Guid GroupID, int pageSize, int pageNumber)
        {
            //GetPermissionsResponse response = new GetPermissionsResponse();
            GetGeneralResponse<IEnumerable<PermissionView>> response = new GetGeneralResponse<IEnumerable<PermissionView>>();
            GroupView groupView = new GroupView();
            groupView = _groupRepository.FindBy(GroupID).ConvertToGroupView();

            try
            {

                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<Permission> permissions = _permissionRepository.FindAll(index, count);
                response.data = permissions.data.ConvertToPermissionViews();


                foreach (PermissionView permissionView in response.data)
                {
                    bool guaranteed = (from p in groupView.Permissions
                                       where p.PermitKey == permissionView.Key
                                       select p.Guaranteed).FirstOrDefault();
                    // بخاطر اینکه بصورت آجاکسی بتوان ویرایش کرد ناچاریم آیدی گروه را به جای آیدی پرمیشن جا بزنیم
                    permissionView.ID = groupView.ID;
                    permissionView.Guaranteed = guaranteed;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
        #endregion

    }
}
