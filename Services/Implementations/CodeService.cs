#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.SqlCommand;
using Services.Interfaces;
using Model.Customers.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Model.Customers;
using Services.ViewModels.Customers;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees.Interfaces;
using Infrastructure.Querying;
using Infrastructure.Domain;
#endregion

namespace Services.Implementations
{
    public class CodeService : ICodeService
    {
        #region Declares
        private readonly ICodeRepository _codeRepository;
        private readonly ICenterRepository _centerRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICustomerRepository _customerRepository;
        #endregion

        #region Ctor
        public CodeService(ICodeRepository codeRepository, ICenterRepository centerRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository, ICustomerRepository customerRepository)
        {
            _codeRepository = codeRepository;
            _uow = uow;
            _centerRepository = centerRepository;
            _employeeRepository = employeeRepository;
            _customerRepository = customerRepository;
        }
        #endregion

        #region Old Methods

        #region Add
        public GeneralResponse AddCode(AddCodeRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Code code = new Code();
                code.ID = Guid.NewGuid();
                code.CreateDate = PersianDateTime.Now;
                code.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                code.Center = _centerRepository.FindBy(request.CenterID);
                code.CodeName = request.CodeName;
                code.RowVersion = 1;

                #region If Duplicate, send Error message

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criterion = new Criterion("CodeName", code.CodeName, CriteriaOperator.Equal);
                query.Add(criterion);

                if (_codeRepository.FindBy(query).Count() > 0)
                {
                    
                    Code exitstCode = _codeRepository.FindBy(query).FirstOrDefault();
                    //response.ErrorMessages.Add(String.Format("این پیش شماره قبلاً برای مرکز مخابراتی «{0}» به ثبت رسیده است.", exitstCode.Center.CenterName));

                    response.ErrorMessages.Add("RegisteredKey" + exitstCode.Center.CenterName + "ThisPerfixKey");
                    return response;
                }

                #endregion

                #region Update Center Of Customers if exist any

                IEnumerable<Customer> customers = _customerRepository.FindByPhoneCode(code.CodeName);
                Center center = _centerRepository.FindBy(request.CenterID);

                if (customers.Count() > 0)
                {
                    foreach (Customer customer in customers)
                    {
                        customer.Center = center;
                        _customerRepository.Save(customer);
                    }
                }

                #endregion

                #region Validation
                if (code.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in code.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }
                #endregion

                _codeRepository.Add(code);
                _uow.Commit();

                ////response.success = true;

            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }


        #endregion

        #region Edit
        public GeneralResponse EditCode(EditCodeRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();
            Code code = new Code();
            code = _codeRepository.FindBy(request.ID);

            if (code != null)
            {
                try
                {
                    code.ModifiedDate = PersianDateTime.Now;
                    code.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    code.Center = _centerRepository.FindBy(request.CenterID);
                    //string oldCode = code.CodeName;
                    code.CodeName = request.CodeName;

                    #region RowVersion
                    if (code.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        code.RowVersion += 1;
                    }
                    #endregion

                    #region If Duplicate, send Error message

                    Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                    Criterion criterion = new Criterion("CodeName", code.CodeName, CriteriaOperator.Equal);
                    query.Add(criterion);

                    //if (_codeRepository.FindBy(query).Count() > 0)
                    if (_codeRepository.FindBy(query) != null)
                    {
                        
                        //Code exitstCode = _codeRepository.FindBy(query).FirstOrDefault();
                        //response.ErrorMessages.Add(String.Format("این پیش شماره قبلاً برای مرکز مخابراتی «{0}» به ثبت رسیده است.", exitstCode.Center.CenterName));
                        response.ErrorMessages.Add("DuplicateCode");
                        return response;
                    }

                    #endregion

                    #region Update Center Of Customers if exist any

                    //// customers of New Code
                    //IEnumerable<Customer> customers = _customerRepository.FindBy(code.CodeName);
                    //Center center = _centerRepository.FindBy(request.CenterID);

                    //if (customers != null && customers.Count() > 0)
                    //{
                    //    foreach (Customer customer in customers)
                    //    {
                    //        customer.Center = center;
                    //        _customerRepository.Save(customer);
                    //    }
                    //}

                    //// Customers of Old Code
                    //customers = _customerRepository.FindBy(oldCode);

                    //if (customers != null && customers.Count() > 0)
                    //{
                    //    foreach (Customer customer in customers)
                    //    {
                    //        customer.Center = null;
                    //        _customerRepository.Save(customer);
                    //    }
                    //}

                    #endregion

                    #region Validation

                    if (code.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in code.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _codeRepository.Save(code);
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

        #endregion

        

        #region Get One
        public GetCodeResponse GetCode(GetRequest request)
        {
            GetCodeResponse response = new GetCodeResponse();

            try
            {
                Code code = new Code();
                CodeView codeView = code.ConvertToCodeView();

                code = _codeRepository.FindBy(request.ID);
                if (code != null)
                    codeView = code.ConvertToCodeView();

                response.CodeView = codeView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #endregion

        #region new Metods

        #region Read

        public GetGeneralResponse<IEnumerable<CodeView>> GetCodes(int pageSize, int pageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<CodeView>> response = new GetGeneralResponse<IEnumerable<CodeView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<Code> codesResponse = _codeRepository.FindAllWithSort(index, count,sort);
                response.data = codesResponse.data.ConvertToCodeViews();
                response.totalCount = codesResponse.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }


        public GetGeneralResponse<IEnumerable<CodeView>> GetCodes(Guid centerID, int pageSize, int pageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<CodeView>> response = new GetGeneralResponse<IEnumerable<CodeView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criterion = new Criterion("Center.ID", centerID, CriteriaOperator.Equal);
                query.Add(criterion);

                Response<Code> codesResponse = _codeRepository.FindBy(query, index, count, sort);
                response.data = codesResponse.data.ConvertToCodeViews();
                response.totalCount = codesResponse.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public GetCodesResponse GetCodes(Guid centerID)
        {
            GetCodesResponse response = new GetCodesResponse();

            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                Criterion criterion = new Criterion("Center.ID", centerID, CriteriaOperator.Equal);
                query.Add(criterion);

                IEnumerable<CodeView> codes = _codeRepository.FindBy(query)
                    .ConvertToCodeViews();

                response.CodeViews = codes;

            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion

        #region Insert

        public GeneralResponse AddCode(IEnumerable<AddCodeRequest> requests, Guid CenterID, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            foreach (var request in requests)
            {
                try
                {
                    Code code = new Code();
                    code.ID = Guid.NewGuid();
                    code.CreateDate = PersianDateTime.Now;
                    code.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    code.Center = _centerRepository.FindBy(CenterID);
                    code.CodeName = request.CodeName;
                    code.AddedToSite = false;
                    code.RowVersion = 1;

                    #region If Duplicate, send Error message

                    Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                    Criterion criterion = new Criterion("CodeName", code.CodeName, CriteriaOperator.Equal);
                    query.Add(criterion);

                    IEnumerable<Code> existCode = _codeRepository.FindBy(query);
                    if (existCode != null && existCode.Count() > 0)
                    {
                        Code xcode = _codeRepository.FindBy(query).FirstOrDefault();

                        response.ErrorMessages.Add(" ThisPerfixKey " + xcode.Center.CenterName + " RegisteredKey ");
                        return response;
                    }

                    #endregion

                    #region Update Center Of Customers if exist any

                    IEnumerable<Customer> customers = _customerRepository.FindByPhoneCode(code.CodeName);
                    Center center = _centerRepository.FindBy(CenterID);

                    if (customers.Count() > 0)
                    {
                        foreach (Customer customer in customers)
                        {
                            customer.Center = center;
                            _customerRepository.Save(customer);
                        }
                    }

                    #endregion

                    #region Validation
                    if (code.GetBrokenRules().Count() > 0)
                    {


                        foreach (BusinessRule businessRule in code.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }
                    #endregion

                    _codeRepository.Add(code);
                }
                catch (Exception ex)
                {
                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }
            }

            try
            {
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

        public GeneralResponse EditCode(IEnumerable<EditCodeRequest> requests, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            foreach (var request in requests)
            {
                Code code = new Code();
                code = _codeRepository.FindBy(request.ID);

                if (code != null)
                {
                    try
                    {
                        code.ModifiedDate = PersianDateTime.Now;
                        code.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                        if (request.CenterID != null && request.CenterID != Guid.Empty)
                        {
                            code.Center = _centerRepository.FindBy(request.CenterID);
                        }
                        code.CodeName = request.CodeName;

                        #region RowVersion
                        if (code.RowVersion != request.RowVersion)
                        {

                            response.ErrorMessages.Add("EditConcurrencyKey");
                            return response;
                        }
                        else
                        {
                            code.RowVersion += 1;
                        }
                        #endregion

                        #region If Duplicate, send Error message

                        Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                        Criterion criterion = new Criterion("CodeName", code.CodeName, CriteriaOperator.Equal);
                        query.Add(criterion);

                        IEnumerable<Code> existCodes = _codeRepository.FindBy(query);
                        if (existCodes != null && existCodes.Count() > 0)
                        {
                            response.ErrorMessages.Add("DuplicateCodeKey");
                            return response;
                        }

                        #endregion

                        #region Validation

                        if (code.GetBrokenRules().Count() > 0)
                        {

                            foreach (BusinessRule businessRule in code.GetBrokenRules())
                            {
                                response.ErrorMessages.Add(businessRule.Rule);
                            }

                            return response;
                        }

                        #endregion

                        _codeRepository.Save(code);
                    }
                    catch (Exception ex)
                    {
                        response.ErrorMessages.Add(ex.Message);
                        return response;
                    }
                }
                else
                {
                    response.ErrorMessages.Add("NoItemToEditKey");
                    return response;
                }
            }

            try
            {
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        #endregion

        #region Delete

        public GeneralResponse DeleteCode(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            foreach (var request in requests)
            {
                Code code = new Code();
                code = _codeRepository.FindBy(request.ID);

                #region If used by customers can not be deleted

                IEnumerable<Customer> customers = _customerRepository.FindByPhoneCode(code.CodeName);

                if (customers != null && customers.Count() > 0)
                {
                    //foreach (Customer customer in customers)
                    //{
                    //    customer.Center = null;
                    //    _customerRepository.Add(customer);
                    //}
                    response.ErrorMessages.Add("UsedByCustomersAndCannotDelete");
                    return response;
                }

                #endregion

                if (code != null)
                {
                    try
                    {
                        _codeRepository.Remove(code);
                    }
                    catch (Exception ex)
                    {
                        response.ErrorMessages.Add(ex.Message);
                        return response;
                    }
                }
            }
            try
            {
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return response;
            }

            return response;
        }

        public GeneralResponse DeleteCode(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            Code code = new Code();
            code = _codeRepository.FindBy(request.ID);

            #region If used by customers can not be deleted

            IEnumerable<Customer> customers = _customerRepository.FindByPhoneCode(code.CodeName);

            if (customers != null && customers.Count() > 0)
            {
                //foreach (Customer customer in customers)
                //{
                //    customer.Center = null;
                //    _customerRepository.Add(customer);
                //}
                response.ErrorMessages.Add("UsedByCustomersAndCannotDelete");
                return response;
            }

            #endregion

            if (code != null)
            {
                try
                {
                    _codeRepository.Remove(code);
                    _uow.Commit();
                }
                catch (Exception ex)
                {
                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }
            }

            return response;
        }

        #endregion

        #endregion


        #region add one digit to coude

        public GeneralResponse ChangeCodeName()
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                IEnumerable<Code> codes = _codeRepository.FindAll();
                var res = codes.Select(x => new {ID = x.Center.ID, code = x.CodeName}).ToList();
                foreach (var re in res)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        string c = string.Concat(re.code, i.ToString());
                        Code newCode = new Code();
                        newCode.Center = _centerRepository.FindBy(re.ID);
                        newCode.CreateDate = PersianDateTime.Now;
                        newCode.CreateEmployee =
                            _employeeRepository.FindBy(Guid.Parse("12D942E9-9B2F-42A9-82D5-66D661FAC17D"));
                        newCode.ID = Guid.NewGuid();
                        newCode.ModifiedDate = null;
                        newCode.ModifiedEmployee = null;
                        newCode.RowVersion = 1;
                        newCode.CodeName = c;
                        _codeRepository.Add(newCode);

                    }
                }
                _uow.Commit();
                //IList<Code> TempCode = new List<Code>();
                //foreach (var code in codes)
                //{
                    
                //    for (int i = 0; i < 10; i++)
                //    {
                //        if (code.CodeName == "4408")
                //        {
                //            int a = 1;
                //        }
                //        Code newCode = new Code();
                //        newCode.Center = _centerRepository.FindBy(code.Center.ID);
                //        newCode.CreateDate = code.CreateDate;
                //        newCode.CreateEmployee = code.CreateEmployee;
                //        newCode.ID = Guid.NewGuid();
                //        newCode.ModifiedDate = code.ModifiedDate;
                //        newCode.ModifiedEmployee = code.ModifiedEmployee;
                //        newCode.RowVersion = 1;
                //        newCode.CodeName = string.Concat(code.CodeName, i.ToString());
                //        TempCode.Add(newCode);
                        
                //    }
                //}
                //foreach (var item in TempCode)
                //{
                //    _codeRepository.Add(item);
                //    _uow.Commit();
                //}
                
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException!=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Chage Code Center

        public GeneralResponse ChangeCenter(Guid ID, Guid CenterID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                Code code = _codeRepository.FindBy(ID);

                if (code != null)
                {
                    Guid centerID = code.Center.ID;

                    Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                    Criterion ctr=new Criterion("Center.ID",centerID,CriteriaOperator.Equal);
                    query.Add(ctr);
                    IEnumerable<Customer> customers = _customerRepository.FindBy(query);

                    Center center = _centerRepository.FindBy(CenterID);

                    code.Center = center;
                    _codeRepository.Save(code);
                    _uow.Commit();

                    foreach (var customer in customers)
                    {
                        if(customer.ADSLPhone.Length>5)
                            if (customer.ADSLPhone.Substring(0, 5) == code.CodeName)
                            {
                                customer.Center = center;
                                _customerRepository.Save(customer);
                            }
                    }
                    
                    _uow.Commit();
                }
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
    }
}
