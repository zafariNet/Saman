using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Infrastructure.Domain;
using Infrastructure.Querying;

namespace Services.Implementations
{
    public class SpecialNumberService : ISpecialNumberService
    {
        #region Declares

        private readonly ISpecialNumberRepository _specialNumberRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;

        #endregion

        #region Ctor

        public SpecialNumberService(ISpecialNumberRepository specialNumberRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository)
        {
            _specialNumberRepository = specialNumberRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
        }

        #endregion

        #region Old Methods

        public GeneralResponse AddSpecialNumber(AddSpecialNumberRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                SpecialNumber specialNumber = new SpecialNumber();
                specialNumber.ID = Guid.NewGuid();
                specialNumber.CreateDate = PersianDateTime.Now;
                specialNumber.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                specialNumber.FromNumber = request.FromNumber;
                specialNumber.Note = request.Note;
                specialNumber.ToNumber = request.ToNumber;
                specialNumber.RowVersion = 1;

                _specialNumberRepository.Add(specialNumber);
                _uow.Commit();

                ////response.success = true;

                // Validation
                if (specialNumber.GetBrokenRules().Count() > 0)
                {
                    ////response.success = false;

                    foreach (BusinessRule businessRule in specialNumber.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                ////response.success = false;
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public GeneralResponse EditSpecialNumber(EditSpecialNumberRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();
            SpecialNumber specialNumber = new SpecialNumber();
            specialNumber = _specialNumberRepository.FindBy(request.ID);

            if (specialNumber != null)
            {
                try
                {
                    specialNumber.ModifiedDate = PersianDateTime.Now;
                    specialNumber.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                        specialNumber.FromNumber = request.FromNumber;
                    if (request.Note != null)
                        specialNumber.Note = request.Note;
                        specialNumber.ToNumber = request.ToNumber;

                    if (specialNumber.RowVersion != request.RowVersion)
                    {
                        ////response.success = false;
                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        specialNumber.RowVersion += 1;
                    }

                    if (specialNumber.GetBrokenRules().Count() > 0)
                    {
                        ////response.success = false;
                        foreach (BusinessRule businessRule in specialNumber.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _specialNumberRepository.Save(specialNumber);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    ////response.success = false;
                    response.ErrorMessages.Add(ex.Message);
                }
            }
            else
            {
                ////response.success = false;
                response.ErrorMessages.Add("هیچ موردی جهت ویرایش وجود ندارد.");
            }
            return response;
        }

        public GeneralResponse DeleteSpecialNumber(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            SpecialNumber specialNumber = new SpecialNumber();
            specialNumber = _specialNumberRepository.FindBy(request.ID);

            if (specialNumber != null)
            {
                try
                {
                    _specialNumberRepository.Remove(specialNumber);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    ////response.success = false;
                    response.ErrorMessages.Add(ex.Message);
                }
            }

            return response;
        }

        public GetSpecialNumberResponse GetSpecialNumber(GetRequest request)
        {
            GetSpecialNumberResponse response = new GetSpecialNumberResponse();

            try
            {
                SpecialNumber specialNumber = new SpecialNumber();
                SpecialNumberView specialNumberView = specialNumber.ConvertToSpecialNumberView();

                specialNumber = _specialNumberRepository.FindBy(request.ID);
                if (specialNumber != null)
                    specialNumberView = specialNumber.ConvertToSpecialNumberView();

                response.SpecialNumberView = specialNumberView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetSpecialNumbersResponse GetSpecialNumbers()
        {
            GetSpecialNumbersResponse response = new GetSpecialNumbersResponse();

            try
            {
                IEnumerable<SpecialNumberView> specialNumbers = _specialNumberRepository.FindAll()
                    .ConvertToSpecialNumberViews();

                response.SpecialNumberViews = specialNumbers;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region New Methods

        #region Read

        public GetGeneralResponse<IEnumerable<SpecialNumberView>> GetSpecialNumbers(int pageSize, int pageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<SpecialNumberView>> response = new GetGeneralResponse<IEnumerable<SpecialNumberView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<SpecialNumber> numbers = _specialNumberRepository.FindAllWithSort(index, count,sort);
                response.data = numbers.data.ConvertToSpecialNumberViews();
                response.totalCount = numbers.totalCount;
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

        #region Insert

        public GeneralResponse AddSpecialNumbers(IEnumerable<AddSpecialNumberRequest> requests, Guid CreateEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach(AddSpecialNumberRequest request in requests)
                {
                    SpecialNumber specialNumber = new SpecialNumber();
                    specialNumber.ID = Guid.NewGuid();
                    specialNumber.CreateDate = PersianDateTime.Now;
                    specialNumber.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    specialNumber.FromNumber = request.FromNumber;
                    specialNumber.Note = request.Note;
                    specialNumber.ToNumber = request.ToNumber;
                    specialNumber.RowVersion = 1;

                    _specialNumberRepository.Add(specialNumber);
                }
                _uow.Commit();
            }
            catch(Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException != null)
                   response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }
        

        #endregion

        #region Edit

        public GeneralResponse EditSpecialNumbers(IEnumerable<EditSpecialNumberRequest> requests, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (EditSpecialNumberRequest request in requests)
                {
                    SpecialNumber specialNumber = new SpecialNumber();

                    specialNumber.ModifiedDate = PersianDateTime.Now;
                    specialNumber.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                    specialNumber.FromNumber = request.FromNumber;
                    if (request.Note != null)
                        specialNumber.Note = request.Note;
                    specialNumber.ToNumber = request.ToNumber;

                    if (specialNumber.RowVersion != request.RowVersion)
                    {
                        ////response.success = false;
                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        specialNumber.RowVersion += 1;
                    }

                    if (specialNumber.GetBrokenRules().Count() > 0)
                    {
                        ////response.success = false;
                        foreach (BusinessRule businessRule in specialNumber.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _specialNumberRepository.Save(specialNumber);
                }
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

        #region Delete

        public GeneralResponse DeleteSpecialNumbers(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (DeleteRequest request in requests)
                {
                    SpecialNumber specialNumber = _specialNumberRepository.FindBy(request.ID);
                    _specialNumberRepository.Remove(specialNumber);
                }
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

        #endregion
    }
}
