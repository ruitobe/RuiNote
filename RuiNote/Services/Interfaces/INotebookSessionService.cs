using System.Threading.Tasks;
using RuiNote.Services.Model;
using System.Collections.ObjectModel;

namespace RuiNote.Services.Interfaces
{
    public interface INotebookSessionService
    {
        /// <summary>
        /// The get notebook names list async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        Task<ObservableCollection<string>> GetNotebooks();

        /// <summary>
        /// The save notebook name async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        Task SaveNotebooksAsync();

        /// <summary>
        /// The add notebook name async.
        /// </summary>
        /// <param name="notebook name string object">
        /// The notebook name string object.
        /// </param>
        void AddNotebook(string notebook);

        /// <summary>
        /// The delete notebook name async.
        /// </summary>
        /// <param name="note object">
        /// The notebook name string object.
        /// </param>
        void DeleteNotebook(string notebook);

        /// <summary>
        /// The replace notebook name async.
        /// </summary>
        /// <param name="note object">
        /// The notebook name string object.
        /// </param>
        void ReplaceNotebook(string notebook);
    }
}
