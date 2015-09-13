using System.Threading.Tasks;
using RuiNote.Services.Model;
using System.Collections.ObjectModel;

namespace RuiNote.Services.Interfaces
{
    public interface INoteSessionService
    {
        /// <summary>
        /// The get note async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        Task<ObservableCollection<Note>> GetNoteList();

        /// <summary>
        /// The save note async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        Task SaveNoteDataAsync();

        /// <summary>
        /// The add note async.
        /// </summary>
        /// <param name="note object">
        /// The note object.
        /// </param>
        void AddNote(Note note);

        /// <summary>
        /// The delete note async.
        /// </summary>
        /// <param name="note object">
        /// The note object.
        /// </param>
        void DeleteNote(Note note);

        /// <summary>
        /// The replace note async.
        /// </summary>
        /// <param name="note object">
        /// The note object.
        /// </param>
        void ReplaceNote(Note note);
    }
}
