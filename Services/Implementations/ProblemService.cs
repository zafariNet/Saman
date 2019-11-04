#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Support.Interfaces;
using Model.Customers.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Model.Support;
using Services.ViewModels.Support;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees.Interfaces;
using Infrastructure.Querying;
using Infrastructure.Domain;
using Model.Customers;

#endregion

namespace Services.Implementations
{
    public class ProblemService : IProblemService
    {
        #region Declares
        
        private readonly IProblemRepository _problemRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILevelRepository _levelRepository;
   
        #endregion

        #region Ctor
        
        public ProblemService(IProblemRepository problemRepository, IUnitOfWork uow)
        {
            _problemRepository = problemRepository;
            _uow = uow;
        }

        public ProblemService(IProblemRepository problemRepository, ICustomerRepository customerRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository,
            ILevelRepository levelRepository)
            : this(problemRepository, uow)
        {
            this._customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _levelRepository = levelRepository;
        }
        
        #endregion

        #region Old Methods
        
        public GeneralResponse AddProblem(AddProblemRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                // چک کردن اینکه آیا در این مرحله امکان افزودن مشکل وجود دارد یا خیر
                Customer customer = this._customerRepository.FindBy(request.CustomerID);
                Level level = _levelRepository.FindBy(customer.Level.ID);
                if (level.Options == null || !level.Options.CanAddProblem)
                {
                    response.ErrorMessages.Add("ProblemIsNotPermitedInThisLevel");
                    return response;
                }

                Problem problem = new Problem();
                problem.ID = Guid.NewGuid();
                problem.CreateDate = PersianDateTime.Now;
                problem.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                problem.Customer = this._customerRepository.FindBy(request.CustomerID);
                problem.Priority = request.Priority;
                problem.ProblemDescription = request.ProblemDescription;
                problem.ProblemTitle = request.ProblemTitle;
                problem.State = request.State;
                problem.RowVersion = 1;

                _problemRepository.Add(problem);
                _uow.Commit();

                ////response.success = true;

                // Validation
                if (problem.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in problem.GetBrokenRules())
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

        public GeneralResponse EditProblem(EditProblemRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            Problem problem = new Problem();
            problem = _problemRepository.FindBy(request.ID);

            if (problem != null)
            {
                try
                {
                    problem.ModifiedDate = PersianDateTime.Now;
                    problem.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    problem.Priority = request.Priority;
                    problem.ProblemDescription = request.ProblemDescription;
                    problem.ProblemTitle = request.ProblemTitle;
                    problem.State = request.State;

                    #region RowVersion

                    if (problem.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        problem.RowVersion += 1;
                    }

                    #endregion

                    #region Validation

                    if (problem.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in problem.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _problemRepository.Save(problem);
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

        public GeneralResponse DeleteProblem(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            Problem problem = new Problem();
            problem = _problemRepository.FindBy(request.ID);

            if (problem != null)
            {
                try
                {
                    _problemRepository.Remove(problem);
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

        public GetProblemResponse GetProblem(GetRequest request)
        {
            GetProblemResponse response = new GetProblemResponse();

            try
            {
                Problem problem = new Problem();
                ProblemView problemView = problem.ConvertToProblemView();

                problem = _problemRepository.FindBy(request.ID);
                if (problem != null)
                    problemView = problem.ConvertToProblemView();

                response.ProblemView = problemView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetProblemsResponse GetProblems()
        {
            GetProblemsResponse response = new GetProblemsResponse();

            try
            {
                IEnumerable<ProblemView> problems = _problemRepository.FindAll()
                    .ConvertToProblemViews();

                response.ProblemViews = problems;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        
        #endregion

        public GetGeneralResponse<IEnumerable<ProblemView>> GetProblems(Guid customerID, int pageSize, int pageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<ProblemView>> response = new GetGeneralResponse<IEnumerable<ProblemView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("Customer.ID", customerID, CriteriaOperator.Equal);

                query.Add(criteria);

                Response<Problem> problems = _problemRepository.FindBy(query, index, count,sort);

                response.data = problems.data.ConvertToProblemViews();
                response.totalCount = problems.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }


            return response;
        }

    }
}
