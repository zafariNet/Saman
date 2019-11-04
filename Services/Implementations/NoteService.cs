#region Usings

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
using Infrastructure.Querying;
using Infrastructure.Domain;

#endregion

namespace Services.Implementations
{
    public class NoteService : INoteService
    {
        #region Declares

        private readonly INoteRepository _noteRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILevelRepository _levelRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;

        #endregion

        #region Ctor

        public NoteService(INoteRepository noteRepository, IUnitOfWork uow)
        {
            _noteRepository = noteRepository;
            _uow = uow;
        }

        public NoteService(INoteRepository noteRepository, ICustomerRepository customerRepository, ILevelRepository levelRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository)
            : this(noteRepository, uow)
        {
            this._customerRepository = customerRepository;
            this._levelRepository = levelRepository;
            _employeeRepository = employeeRepository;
        }

        #endregion

        #region Add

        public GeneralResponse AddNote(AddNoteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Note note = new Note();
                note.ID = Guid.NewGuid();
                note.CreateDate = PersianDateTime.Now;
                note.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                note.Customer = this._customerRepository.FindBy(request.CustomerID);
                note.Level = this._levelRepository.FindBy(note.Customer.Level.ID);
                note.NoteDescription = request.NoteDescription;
                note.RowVersion = 1;

                #region Validation

                if (note.GetBrokenRules().Count() > 0)
                {


                    foreach (BusinessRule businessRule in note.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                _noteRepository.Add(note);
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

        public GeneralResponse EditNote(EditNoteRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            Note note = new Note();
            note = _noteRepository.FindBy(request.ID);

            if (note != null)
            {
                try
                {
                    note.ModifiedDate = PersianDateTime.Now;
                    note.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    note.NoteDescription = request.NoteDescription;

                    if (note.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        note.RowVersion += 1;
                    }

                    if (note.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in note.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _noteRepository.Save(note);
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

        #region Delete

        public GeneralResponse DeleteNote(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();


                try
                {
                    _noteRepository.RemoveById(request.ID);
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

        #region Get One

        public GetNoteResponse GetNote(GetRequest request)
        {
            GetNoteResponse response = new GetNoteResponse();

            try
            {
                Note note = new Note();
                NoteView noteView = note.ConvertToNoteView();

                note = _noteRepository.FindBy(request.ID);
                if (note != null)
                    noteView = note.ConvertToNoteView();

                response.NoteView = noteView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        #endregion

        #region Get Some

        public GetNotesResponse GetNotes()
        {
            GetNotesResponse response = new GetNotesResponse();

            try
            {
                IEnumerable<NoteView> notes = _noteRepository.FindAll()
                    .ConvertToNoteViews();

                response.NoteViews = notes;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<NoteView>> GetNotes(Guid customerID, int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<NoteView>> response = new GetGeneralResponse<IEnumerable<NoteView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("Customer.ID", customerID, CriteriaOperator.Equal);

                query.Add(criteria);

                Response<Note> notes = _noteRepository.FindBy(query, index, count);

                response.data = notes.data.ConvertToNoteViews();
                response.totalCount = notes.totalCount;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        #endregion
    }
}
