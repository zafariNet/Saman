using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class NoteMapper
    {
        public static IEnumerable<NoteView> ConvertToNoteViews(
            this IEnumerable<Note> notes)
        {
            return Mapper.Map<IEnumerable<Note>,
                IEnumerable<NoteView>>(notes);
        }

        public static NoteView ConvertToNoteView(this Note note)
        {
            return Mapper.Map<Note, NoteView>(note);
        }
    }
}
