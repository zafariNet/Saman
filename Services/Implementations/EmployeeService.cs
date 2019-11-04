#region Usings

using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Sales;
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
using System.Web.Security;
using Infrastructure.Querying;
using Infrastructure.Domain;
using Infrastructure.Encrypting;

#endregion

namespace Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        #region Declares
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISimpleEmployeeRepository _simpleEmployeeRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _uow;
        private readonly IPermitRepository _permitRepository;
        #endregion

        #region Ctor
        public EmployeeService(IEmployeeRepository employeeRepository, IUnitOfWork uow)
        {
            _employeeRepository = employeeRepository;
            _uow = uow;
        }

        public EmployeeService(IEmployeeRepository employeeRepository, IGroupRepository groupRepository, IUnitOfWork uow
            , IPermissionRepository permissionRepository
            , IPermitRepository permitRepository, ISimpleEmployeeRepository simpleEmployeeRepository)
            : this(employeeRepository, uow)
        {
            this._groupRepository = groupRepository;
            _permissionRepository = permissionRepository;
            _permitRepository = permitRepository;
            _simpleEmployeeRepository = simpleEmployeeRepository;
        }
        #endregion

        #region Add
        public GeneralResponse AddEmployee(AddEmployeeRequest request, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Employee employee = new Employee();
                employee.ID = Guid.NewGuid();
                employee.CreateDate = PersianDateTime.Now;
                employee.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                employee.BirthDate = request.BirthDate;
                employee.FirstName = request.FirstName;
                Group group = _groupRepository.FindBy(request.GroupID);
                employee.Group = group;
                employee.HireDate = request.HireDate;
                employee.InstallExpert = request.InstallExpert;
                employee.LastName = request.LastName;
                employee.LoginName = request.LoginName;
                employee.ParentEmployee = this._employeeRepository.FindBy(request.ParentEmployeeID);
                employee.Password = request.Password;
                employee.Phone = request.Phone;
                employee.Mobile = request.Mobile;
                employee.Address = request.Address;
                employee.Note = request.Note;
                employee.Discontinued = request.Discontinued;
                employee.Picture = request.Picture;
                
                IList<Permit> permits = group.Permissions.ToList();
                //foreach (var permit in permits)
                //{
                //    permit.Group = null;
                //}

                employee.Permissions = GetNewPermitList(group.Permissions.ToList());

                employee.RowVersion = 1;

                #region Validation

                if (employee.GetBrokenRules().Count() > 0)
                {
                    foreach (BusinessRule businessRule in employee.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                _employeeRepository.Add(employee);
                _uow.Commit();

            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //    if (ex.InnerException.Message == "Violation of UNIQUE KEY constraint 'IX_Employee'. Cannot insert duplicate key in object 'Emp.Employee'. The duplicate key value is (a).")
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        #endregion

        #region Edit
        public GeneralResponse EditEmployeePermission(IEnumerable<EditPermissionRequestG> requests, Guid EmployeeID, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            Employee employee = new Employee();

            employee = _employeeRepository.FindBy(EmployeeID);

            if (employee != null)
            {
                try
                {
                    foreach (EditPermissionRequestG request in requests)
                    {
                        if (employee.Permissions.Where(s => s.PermitKey == request.Key).FirstOrDefault() != null)
                        {
                            employee.Permissions.Where(s => s.PermitKey == request.Key).FirstOrDefault().Guaranteed = request.Guaranteed;
                        }
                        else
                        {
                            IList<Permit> permits = employee.Permissions.ToList();
                            IEnumerable<Permission> permissions = _permissionRepository.FindAll();

                            permits.Add(new Permit(permissions.Where(s => s.Key == request.Key).FirstOrDefault()) { Guaranteed = request.Guaranteed });
                            employee.Permissions = permits;
                        }

                        #region Update Permissions of Employees

                        Employee editedEmployees = UpdateEmployeesPermissions(employee, request.Key, request.Guaranteed);

                        #endregion

                        _employeeRepository.Save(employee);
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
        #region private
        private Employee UpdateEmployeesPermissions(Employee employee, string permitKey, bool guaranteed)
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


            return employee;
        }
        #endregion
        public GeneralResponse EditEmployee(EditEmployeeRequest request,Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            Employee employee = new Employee();
            employee = _employeeRepository.FindBy(request.ID);

            if (employee != null)
            {
                try
                {
                    employee.ModifiedDate = PersianDateTime.Now;
                    employee.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                    if (request.BirthDate != null)
                        employee.BirthDate = request.BirthDate;
                    if (request.FirstName != null)
                        employee.FirstName = request.FirstName;
                    if (request.GroupID != employee.Group.ID)
                        employee.Group = this._groupRepository.FindBy(request.GroupID);
                    if (request.HireDate != null)
                        employee.HireDate = request.HireDate;
                        employee.InstallExpert = request.InstallExpert;
                    if (request.LastName != null)
                        employee.LastName = request.LastName;
                    if (request.LoginName != null)
                        employee.LoginName = request.LoginName;
                    if (request.ParentEmployeeID != null)
                        employee.ParentEmployee = this._employeeRepository.FindBy(request.ParentEmployeeID);
                    if (request.Password != null)
                        employee.Password = request.Password;
                    if (request.Phone != null)
                        employee.Phone = request.Phone;
                    if (request.Mobile != null)
                        employee.Mobile = request.Mobile;
                    if (request.Address != null)
                        employee.Address = request.Address;
                    if (request.Note != null)
                        employee.Note = request.Note;
                        employee.Discontinued = request.Discontinued;

                    if (employee.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        employee.RowVersion += 1;
                    }

                    if (employee.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in employee.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _employeeRepository.Save(employee);
                    _uow.Commit();

                    ////response.success = true;
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
        /// ویرایش مجموعه ای از کارمندان
        /// </summary>
        /// Added By Zafari
        /// <param name="request"></param>
        /// <returns></returns>
        public GeneralResponse EditEmployees(IEnumerable<EditEmployeeRequest> requests , Guid ModifiedEmployeeId)
        {
            GeneralResponse response = new GeneralResponse();


            if (requests != null)
            {
                try
                {
                    foreach(EditEmployeeRequest request in requests)
                    {
                        Employee employee = new Employee();
                        employee.ID = request.ID;
                        employee.ModifiedDate = PersianDateTime.Now;
                        employee.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeId);
                        if (request.BirthDate != null)
                            employee.BirthDate = request.BirthDate;
                        if (request.FirstName != null)
                            employee.FirstName = request.FirstName;
                        if (request.GroupID != employee.Group.ID)
                            employee.Group = this._groupRepository.FindBy(request.GroupID);
                        if (request.HireDate != null)
                            employee.HireDate = request.HireDate;
                        employee.InstallExpert = request.InstallExpert;
                        if (request.LastName != null)
                            employee.LastName = request.LastName;
                        if (request.LoginName != null)
                            employee.LoginName = request.LoginName;
                        //if (request.ParentEmployeeID != null)
                        //    employee.ParentEmployee = this._employeeRepository.FindBy(request.ParentEmployeeID);
                        if (request.Password != null)
                            employee.Password = request.Password;
                        if (request.Phone != null)
                            employee.Phone = request.Phone;
                        if (request.Mobile != null)
                            employee.Mobile = request.Mobile;
                        if (request.Address != null)
                            employee.Address = request.Address;
                        if (request.Note != null)
                            employee.Note = request.Note;
                        employee.Discontinued = request.Discontinued;

                        employee.Permissions = request.Permissions;
                        
                        if (employee.RowVersion != request.RowVersion)
                        {

                            response.ErrorMessages.Add("EditConcurrencyKey");
                            return response;
                        }
                        else
                        {
                            employee.RowVersion += 1;
                        }
                        
                        if (employee.GetBrokenRules().Count() > 0)
                        {

                            foreach (BusinessRule businessRule in employee.GetBrokenRules())
                            {
                                response.ErrorMessages.Add(businessRule.Rule);
                            }

                            return response;
                        }

                        _employeeRepository.Save(employee);

                        

                        ////response.success = true;
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

                response.ErrorMessages.Add("NoItemToEditKey");
            }

            return response;
        }
        /// <summary>
        /// فعال یا غیر فعال کردن یک کارمند
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        public GeneralResponse EditSimpleEmployees(IEnumerable<EditSimpleEmployeeRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            foreach (EditSimpleEmployeeRequest request in requests)
            {
                Employee employee = _employeeRepository.FindBy(request.ID);
                employee.Discontinued = request.Discontinued;
                _employeeRepository.Save(employee);
            }
            _uow.Commit();

            return response;
        }

        /// <summary>
        /// ویرایش دسترسیهای یک کارمند
        /// </summary>
        public GeneralResponse EditEmployee(EditPermissionRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            Employee employee = new Employee();
            employee = _employeeRepository.FindBy(request.EmployeeID);

            if (employee != null)
            {
                try
                {
                    if (employee.Permissions.Where(s => s.PermitKey == request.PermitKey).FirstOrDefault() != null)
                    {
                        employee.Permissions.Where(s => s.PermitKey == request.PermitKey).FirstOrDefault().Guaranteed = request.Guaranteed;
                    }
                    else
                    {
                        IList<Permit> permits = employee.Permissions.ToList();
                        IEnumerable<Permission> permissions = _permissionRepository.FindAll();

                        permits.Add(new Permit(permissions.Where(s => s.Key == request.PermitKey).FirstOrDefault()) { Guaranteed = request.Guaranteed });
                        employee.Permissions = permits;
                    }
                    _employeeRepository.Save(employee);
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

        public GeneralResponse ChangePassword(string CurrentPassword,string NewPassword, string ConfirmPassword, Guid EmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            Employee employee = _employeeRepository.FindBy(EmployeeID);
            if(employee.Password != CurrentPassword)
            {
                response.ErrorMessages.Add("رمز وارد شده با رمز عبور ذخیره شده تطابق ندارد");
                return response;
            }
            else if (NewPassword != ConfirmPassword)
            {
                response.ErrorMessages.Add("رمز عبور وارد شده در قسمت تایید رمز اشتباه است");
                return response;
            }

            else
            {
                employee.Password = NewPassword;
                employee.ModifiedEmployee = _employeeRepository.FindBy(EmployeeID);
                employee.ModifiedDate = PersianDateTime.Now;
                _employeeRepository.Save(employee);
                _uow.Commit();
            }

            return response;
        }
        #endregion

        #region Delete
        public GeneralResponse DeleteEmployee(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            Employee employee = new Employee();
            employee = _employeeRepository.FindBy(request.ID);

            if (employee != null)
            {
                try
                {
                    _employeeRepository.Remove(employee);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);

                    if (ex.Message.Substring(0, 40) == "Cannot insert the value NULL into column"
                        || ex.InnerException.Message.Substring(0, 40) == "Cannot insert the value NULL into column")
                    {
                        response.ErrorMessages.Clear();
                        response.ErrorMessages.Add("CanNotDeleteEmployeeKey");
                    }
                    else
                    {
                        if (ex.InnerException != null)
                        {
                            response.ErrorMessages.Add("FIRST INNER EXPCEPTION: " + ex.InnerException.Message);
                            if (ex.InnerException.InnerException != null)
                                response.ErrorMessages.Add("SECOND INNER EXPCEPTION: " + ex.InnerException.InnerException.Message);
                        }
                    }
                }
            }
            return response;
        }
        //Added By Zafari
        /// <summary>
        /// حذف گروهی کارمندان
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        public GeneralResponse DeleteEmployees(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            foreach (DeleteRequest request in requests)
            {
                Employee employee = new Employee();
                employee = _employeeRepository.FindBy(request.ID);

                if (employee != null)
                {
                    try
                    {
                        _employeeRepository.Remove(employee);
                        _uow.Commit();

                        ////response.success = true;
                    }
                    catch (Exception ex)
                    {

                        response.ErrorMessages.Add(ex.Message);

                        if (ex.Message.Substring(0, 40) == "Cannot insert the value NULL into column"
                            || ex.InnerException.Message.Substring(0, 40) == "Cannot insert the value NULL into column")
                        {
                            response.ErrorMessages.Clear();
                            response.ErrorMessages.Add("CanNotDeleteEmployeeKey");
                        }
                        else
                        {
                            if (ex.InnerException != null)
                            {
                                response.ErrorMessages.Add("FIRST INNER EXPCEPTION: " + ex.InnerException.Message);
                                if (ex.InnerException.InnerException != null)
                                    response.ErrorMessages.Add("SECOND INNER EXPCEPTION: " + ex.InnerException.InnerException.Message);
                            }
                        }
                    }
                }
            }
            
            return response;
        }

        #endregion

        #region Get One
        public GetEmployeeResponse GetEmployee(GetRequest request)
        {
            GetEmployeeResponse response = new GetEmployeeResponse();

            try
            {
                Employee employee = new Employee();
                EmployeeView employeeView = new EmployeeView();

                employee = _employeeRepository.FindBy(request.ID);
                if (employee != null)
                    employeeView = employee.ConvertToEmployeeView();

                IList<PermitView> permits = new List<PermitView>();
                if (employeeView.LoginName == "sa")
                {
                    foreach (var permit in employeeView.Permissions)
                    {
                        permit.Guaranteed = true;
                        permits.Add(permit);
                    }

                    employeeView.Permissions = permits;
                }

                response.EmployeeView = employeeView;
                response.Employee = employee;
            }
            catch (Exception ex)
            {
               
            }

            return response;
        }

        public GetEmployeeResponse GetSimpleEmployee(GetRequest request)
        {
            GetEmployeeResponse response = new GetEmployeeResponse();

            try
            {
                Employee employee = _employeeRepository.FindBy(request.ID);
                EmployeeView employeeView = employee.ConvertToEmployeeView();

                response.EmployeeView = employeeView;
                response.Employee = employee;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }


        #endregion

        #region Get Some
        public GetEmployeesResponse GetEmployees()
        {
            GetEmployeesResponse response = new GetEmployeesResponse();

            try
            {
                IEnumerable<EmployeeView> employees = _employeeRepository.FindAll()
                    .ConvertToEmployeeViews();

                response.EmployeeViews = employees;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
        public GetGeneralResponse<IEnumerable<SimpleEmployeeView>> _GetEmployees()
        {
            GetGeneralResponse<IEnumerable<SimpleEmployeeView>> response = new GetGeneralResponse<IEnumerable<SimpleEmployeeView>>();
            try
            {
                IEnumerable<SimpleEmployeeView> item = _simpleEmployeeRepository.FindAll().ConvertToSimpleEmployeeViews().ToList();
                response.data = item;
                response.totalCount = item.Count();
            }
            catch (Exception ex)
            {
                
            }
            return response;
        }

        public GetGeneralResponse<IEnumerable<EmployeeView>> GetEmployeesInstaller()
        {
            GetGeneralResponse<IEnumerable<EmployeeView>> response = new GetGeneralResponse<IEnumerable<EmployeeView>>();
            try
            {
                Query query = new Query();
                Criterion criteria = new Criterion("InstallExpert", true, CriteriaOperator.Equal);
                query.Add(criteria);
                IEnumerable<Employee> simpleEmployees = _employeeRepository.FindBy(query);
                IEnumerable<EmployeeView> item = simpleEmployees.ConvertToEmployeeViews();
                var simpleEmployeeViews = item as EmployeeView[] ?? item.ToArray();
                foreach (var d in simpleEmployeeViews)
                    d.Permissions = null;
                response.data = simpleEmployeeViews;
                response.totalCount = simpleEmployeeViews.Count();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }
            return response;
        }
        public GetGeneralResponse<IEnumerable<EmployeeView>> GetEmployees(Guid? groupID, int pageSize, int pageNumber,List<FilterData> filter,List<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<EmployeeView>> response = new GetGeneralResponse<IEnumerable<EmployeeView>>();
            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<Employee> employees = new Response<Employee>();
                                                     
                if (groupID != null)
                {
                    Query query = new Query();
                    Criterion criteria = new Criterion("Group.ID", groupID, CriteriaOperator.Equal);
                    query.Add(criteria);

                    employees = _employeeRepository.FindBy(query, index, count);
                }
                else
                {
                    string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "Employee", sort);
                    if (count == -1)
                        employees = _employeeRepository.FindAll(query);
                    else
                        employees = _employeeRepository.FindAll(query, index, count);
                }
                foreach (var item in employees.data)
                    item.Permissions = null;
                response.data = employees.data.ConvertToEmployeeViews();
                
                response.totalCount = employees.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }
            return response;
        }

        //Added By Zafari
        public GetGeneralResponse<IEnumerable<EmployeeView>> GetEmployeesByGroup(Guid GroupID)
        {
            GetGeneralResponse<IEnumerable<EmployeeView>> response = new GetGeneralResponse<IEnumerable<EmployeeView>>();

            try
            {
                // By Hojjat
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("Group.ID", GroupID, CriteriaOperator.Equal);
                query.Add(criteria);
                //

                Response<Employee> employees = _employeeRepository.FindBy(query, -1, -1);

                response.data = employees.data.ConvertToEmployeeViews();
                response.totalCount = employees.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<EmployeeView>> GetEmployeesByGroup(Guid GroupID, int pageSize,
            int pageNumber)
        {
            GetGeneralResponse<IEnumerable<EmployeeView>> response = new GetGeneralResponse<IEnumerable<EmployeeView>>();
            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;
                // By Hojjat
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("Group.ID", GroupID, CriteriaOperator.Equal);
                query.Add(criteria);
                //
                
                Response<Employee> employees = _employeeRepository.FindBy(query, index, count);

                response.data = employees.data.ConvertToEmployeeViews();
                response.totalCount = employees.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<EmployeeView>> GetAllChilOfAnEmployee(Guid EmployeeID)
        {
            GetGeneralResponse<IEnumerable<EmployeeView>> response = new GetGeneralResponse<IEnumerable<EmployeeView>>();
            try
            {
                Employee employee = _employeeRepository.FindBy(EmployeeID);
                foreach (var item in employee.GetAllChild())
                    item.Permissions = null;
                var list=employee.GetAllChild().ConvertToEmployeeViews();

                response.data = list;
                response.totalCount = list.Count();

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public GetEmployeesResponse GetInstallExprets()
        {
            GetEmployeesResponse response = new GetEmployeesResponse();

            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criterion = new Criterion("InstallExpert", true, CriteriaOperator.Equal);
                query.Add(criterion);

                IEnumerable<EmployeeView> employees = _employeeRepository.FindBy(query)
                    .ConvertToEmployeeViews();
                foreach (var item in employees)
                {
                    item.Permissions = null;
                    item.Queues = null;
                    
                }

                response.EmployeeViews = employees;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion

        #region Add Picture To Employee

        public GeneralResponse InsertPicture(Guid EmployeeID, string Path)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                var employee = _employeeRepository.FindBy(EmployeeID);
                employee.Picture = Path;
                _employeeRepository.Save(employee);
                _uow.Commit();
            }

            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;

        }

        #endregion

        #region Autheticate

        public string Authenticate(string userName, string password, bool persistentLogin)
        {
            Employee employee = _employeeRepository.VerifyLogin(userName);

            var f = employee.Permissions.Where(x => x.PermitKey == "Note_Read");

            if (employee != null)
            {
                if (!employee.Discontinued)
                {
                    if (employee.Password == password)
                    {
                        FormsAuthentication.SetAuthCookie(employee.ID.ToString(), persistentLogin);
                        return "OK";
                    }
                    else
                        return "PasswordInvalid";
                }
                else
                    return "Discontinued";
            }
            else
                return "UserNameInvalid";
        }

        #endregion

        #region Other Methods

        public GeneralResponse ChangeGroup(IEnumerable<Guid> EmployeeIDs, Guid GroupID)
        {
            GeneralResponse response = new GeneralResponse();
            IList<Employee> employees =new  List<Employee>();
            try
            {
                foreach (Guid employeeID in EmployeeIDs)
                {
                    Employee employee = _employeeRepository.FindBy(employeeID);
                    if (employee != null)
                    {
                        Group group= _groupRepository.FindBy(GroupID);
                        employee.Group = group;
                        List<Permit> permits = group.Permissions.ToList();

                        _permitRepository.Remove(employee.Permissions);

                        employee.Permissions = GetNewPermitList(group.Permissions.ToList());
                        
                        _employeeRepository.Save(employee);
                    }
                }
                _uow.Commit();
            }
            catch(Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;

        }

        private List<Permit> GetNewPermitList(List<Permit> permits)
        {
            List<Permit> newPermits = new List<Permit>();
            foreach (var permit in permits)
            {
                Permit per = new Permit();
                per.ID = Guid.NewGuid();
                per.Guaranteed = permit.Guaranteed;
                per.Permission = permit.Permission;
                per.Group = null;

                newPermits.Add(per);
            }

            return newPermits;
        }
        
        #endregion

        #region Recreate Permissions
        public GeneralResponse RecreateSaPermissions()
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                //Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                //Criterion criterion = new Criterion("LoginName", "sa", CriteriaOperator.Equal);
                //query.Add(criterion);

                //Employee employee = _employeeRepository.FindBy(query).FirstOrDefault();

                //if (employee == null)
                //    return response;

                IEnumerable<Permission> permissions = _permissionRepository.FindAll();
                foreach (var employee in _employeeRepository.FindAll())
                {
                    //if (employee.Permissions != null)
                    {
                        IList<Permit> permits = employee.Permissions.ToList();
                        foreach (var permission in permissions)
                        {
                            if (employee.Permissions.Where(s => s.PermitKey == permission.Key).FirstOrDefault() != null)
                            {
                                employee.Permissions.Where(s => s.PermitKey == permission.Key).FirstOrDefault().Guaranteed = true;
                            }
                            else
                            {
                                permits.Add(new Permit(permission) { Guaranteed = true });
                            }
                        }

                        employee.Permissions = permits;
                    }

                    _employeeRepository.Save(employee);
                    _uow.Commit();
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                {
                    response.ErrorMessages.Add(ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null)
                        response.ErrorMessages.Add(ex.InnerException.InnerException.Message);
                }

                return response;
            }

            return response;
        }

        public GeneralResponse RecreateGroupPermissions(Guid GroupID)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                // Group group = _groupRepository.FindBy(GroupID);
                foreach (var group in _groupRepository.FindAll())
                {
                    if (group == null)
                        return response;

                    IEnumerable<Permission> permissions = _permissionRepository.FindAll();

                    if (group.Permissions != null)
                    {
                        IList<Permit> permits = group.Permissions.ToList();
                        foreach (var permission in permissions)
                        {
                            if (group.Permissions.Where(s => s.PermitKey == permission.Key).FirstOrDefault() != null)
                            {
                                group.Permissions.Where(s => s.PermitKey == permission.Key).FirstOrDefault().Guaranteed = true;
                            }
                            else
                            {
                                permits.Add(new Permit(permission) { Guaranteed = true });
                            }
                        }

                        group.Permissions = permits;
                    }

                    _groupRepository.Save(group);
                    _uow.Commit();
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                {
                    response.ErrorMessages.Add(ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null)
                        response.ErrorMessages.Add(ex.InnerException.InnerException.Message);
                }

                return response;
            }

            return response;
        }

    	#endregion    
    }

}
