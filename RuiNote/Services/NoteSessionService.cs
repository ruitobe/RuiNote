using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Cimbalino.Toolkit.Services;

using RuiNote.Resources;
using RuiNote.Services.Interfaces;
using RuiNote.Services.Model;

using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;
using Windows.Storage;
using System.IO;

namespace RuiNote.Services.Interfaces
{
    /// <summary>
    /// The note service session.
    /// </summary>
    /// 
    public class NoteSessionService : INoteSessionService
    {
        private readonly IApplicationDataService _applicationSettings;
        private readonly ILogManager _logManager;
        private ObservableCollection<Note> _notes = new ObservableCollection<Note>();
        const string fileName = "notes.json";

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteSessionService" /> class.
        /// </summary>
        /// <param name="applicationSettings">The application settings.</param>
        /// <param name="logManager">The log manager.</param>
        
        public NoteSessionService(IApplicationDataService applicationSettings, ILogManager logManager)
        {
            _applicationSettings = applicationSettings;
            _logManager = logManager;

        }

        /// <summary>
        /// The add note async.
        /// </summary>
        /// <param name="note object">
        /// The note object.
        /// </param>

        public async void AddNote(Note note)
        {
            _notes.Add(note);
            await SaveNoteDataAsync();

            for (int i = 0; i < _notes.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine("_notes" + i + "Title = " + _notes[i].NoteTitle);
                System.Diagnostics.Debug.WriteLine("_notes" + i + "Content = " + _notes[i].NoteContent);
            }

        }

        /// <summary>
        /// The delete note async.
        /// </summary>
        /// <param name="note object">
        /// The note object.
        /// </param>

        public async void DeleteNote(Note note)
        {
            _notes.Remove(note);
            for (int i = 0; i < _notes.Count; i++)
            {
                _notes[i].ID = i;
            }
            await SaveNoteDataAsync();
        }

        /// <summary>
        /// The replace note async.
        /// </summary>
        /// <param name="note object">
        /// The note object.
        /// </param>

        public async void ReplaceNote(Note note)
        {

            _notes[note.ID] = note;
            await SaveNoteDataAsync();
        }

        /// <summary>
        /// The save note data async.
        /// </summary>

        public async Task SaveNoteDataAsync()
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<Note>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(fileName,
                CreationCollisionOption.ReplaceExisting))
            {
                jsonSerializer.WriteObject(stream, _notes);
            }
        }

        /// <summary>
        /// The get note list data async.
        /// </summary>

        public async Task<ObservableCollection<Note>> GetNoteList()
        {
            await EnsureDataLoaded();
            return _notes;
        }

        /// <summary>
        /// The ensure note list data loaded.
        /// </summary>

        private async Task EnsureDataLoaded()
        {
            if (_notes.Count == 0)
                await GetNoteListDataAsync();

            return;
        }

        /// <summary>
        /// The get note list data.
        /// </summary>

        private async Task GetNoteListDataAsync()
        {
            if (_notes.Count != 0)
                return;

            var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<Note>));

            try
            {
                // Add a using System.IO;

                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(fileName))
                {
                    _notes = (ObservableCollection<Note>)jsonSerializer.ReadObject(stream);
                }
            }
            catch
            {
                _notes = new ObservableCollection<Note>();
            }
        }
    }
}
