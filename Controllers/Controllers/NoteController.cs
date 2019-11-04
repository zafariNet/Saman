#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.CustomerCatalog;
using System.Web.Mvc;
//using System.Web.UI.WebControls;
using Services.ViewModels.Customers;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;

#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class NoteController : BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly INoteService _noteService;

        #endregion

        #region Ctor

        public NoteController(IEmployeeService employeeService, INoteService noteService)
            : base(employeeService)
        {
            this._noteService = noteService;
            this._employeeService = employeeService;
        }

        #endregion

        #region Old Members

        public ActionResult Index()
        {
            NoteHomePageView noteHomePageView = new NoteHomePageView();
            noteHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Note_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(noteHomePageView);
            }
            #endregion

            noteHomePageView.NoteViews = this._noteService.GetNotes().NoteViews;

            return View(noteHomePageView);
        }

        public ActionResult Create()
        {
            NoteDetailView noteDetailView = new NoteDetailView();
            noteDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Note_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(noteDetailView);
            }
            #endregion

            return View(noteDetailView);
        }

        [HttpPost]
        public ActionResult Create(NoteDetailView noteDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Note_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(noteDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    AddNoteRequest request = new AddNoteRequest();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.CustomerID = noteDetailView.NoteView.CustomerID;
                    //request.LevelID = noteDetailView.NoteView.LevelID;
                    request.NoteDescription = noteDetailView.NoteView.NoteDescription;

                    GeneralResponse response = this._noteService.AddNote(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(noteDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(noteDetailView);
                }

            return View(noteDetailView);
        }

        public ActionResult Edit(string id)
        {
            NoteDetailView noteDetailView = new NoteDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Note_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(noteDetailView);
            }
            #endregion
            
            noteDetailView.NoteView = this.GetNoteView(id);
            //noteDetailView.EmployeeView = GetEmployee();

            return View(noteDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, NoteDetailView noteDetailView)
        {

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Note_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(noteDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    EditNoteRequest request = new EditNoteRequest();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.CustomerID = noteDetailView.NoteView.CustomerID;
                    //request.LevelID = noteDetailView.NoteView.LevelID;
                    request.NoteDescription = noteDetailView.NoteView.NoteDescription;
                    request.RowVersion = noteDetailView.NoteView.RowVersion;

                    GeneralResponse response = this._noteService.EditNote(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(noteDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(noteDetailView);
                }

            return View(noteDetailView);
        }

        public ActionResult Details(string id)
        {
            NoteDetailView noteDetailView = new NoteDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Note_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(noteDetailView);
            }
            #endregion

            NoteView noteView = this.GetNoteView(id);
            
            noteDetailView.NoteView = noteView;
            // noteDetailView.EmployeeView = GetEmployee();

            return View(noteDetailView);
        }

        public ActionResult Delete(string id)
        {
            NoteDetailView noteDetailView = new NoteDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Note_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(noteDetailView);
            }
            #endregion

            noteDetailView.NoteView = this.GetNoteView(id);
            //noteDetailView.EmployeeView = GetEmployee();

            return View(noteDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            NoteDetailView noteDetailView = new NoteDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Note_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(noteDetailView);
            }
            #endregion

            noteDetailView.NoteView = this.GetNoteView(id);
            //noteDetailView.EmployeeView = GetEmployee();

            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._noteService.DeleteNote(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(noteDetailView);
            }
        }

        #endregion

        #region Private Members

        private NoteView GetNoteView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetNoteResponse response = this._noteService.GetNote(request);

            return response.NoteView;
        }

        #endregion

    }
}
