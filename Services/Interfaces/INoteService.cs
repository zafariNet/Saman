#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Services.ViewModels.Customers;

#endregion

namespace Services.Interfaces
{
    public interface INoteService
    {
        GeneralResponse AddNote(AddNoteRequest request);
        GeneralResponse EditNote(EditNoteRequest request);
        GeneralResponse DeleteNote(DeleteRequest request);
        GetNoteResponse GetNote(GetRequest request);
        GetNotesResponse GetNotes();

        GetGeneralResponse<IEnumerable<NoteView>> GetNotes(Guid customerID, int pageSize, int pageNumber);
    }
}
