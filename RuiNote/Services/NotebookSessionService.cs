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

namespace RuiNote.Services
{
    /// <summary>
    /// The notebook service session.
    /// </summary>
    /// 

    public class NotebookSessionService : INotebookSessionService
    {
        private readonly IApplicationDataService _applicationSettings;
        private readonly ILogManager _logManager;
        private ObservableCollection<string> _notebooks = new ObservableCollection<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NotebookSessionService" /> class.
        /// </summary>
        /// <param name="applicationSettings">The application settings.</param>
        /// <param name="logManager">The log manager.</param>
        /// 
        public NotebookSessionService(IApplicationDataService applicationSettings, ILogManager logManager)
        {
            _applicationSettings = applicationSettings;
            _logManager = logManager;

        }

        /// <summary>
        /// The add notebook name async.
        /// </summary>
        /// <param name="notebook name string object">
        /// The notebook name string object.
        /// </param>
        /// 
        public async void AddNotebook(string notebook)
        {
            _notebooks.Add(notebook);
            await SaveNotebooksAsync();
        }

        /// <summary>
        /// The delete notebook name async.
        /// </summary>
        /// <param name="note object">
        /// The notebook name string object.
        /// </param>
        /// 
        public async void DeleteNotebook(string notebook)
        {
            _notebooks.Remove(notebook);
            await SaveNotebooksAsync();
        }

        /// <summary>
        /// The replace notebook name async.
        /// </summary>
        /// <param name="note object">
        /// The notebook name string object.
        /// </param>
        /// 
        public async void ReplaceNotebook(string notebook)
        {

            // replace the string object
            await SaveNotebooksAsync();
        }

        /// <summary>
        /// The save notebook name async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        /// 
        public async Task SaveNotebooksAsync()
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<string>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync("notebooks.json",
                CreationCollisionOption.ReplaceExisting))
            {
                jsonSerializer.WriteObject(stream, _notebooks);
            }
        }

        /// <summary>
        /// The get notebook names list async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        public async Task<ObservableCollection<string>> GetNotebooks()
        {
            await EnsureDataLoaded();
            return _notebooks;
        }


        /// <summary>
        /// The ensure note list data loaded.
        /// </summary>

        private async Task EnsureDataLoaded()
        {
            if (_notebooks.Count == 0)
                await GetNotebooksDataAsync();

            return;
        }

        /// <summary>
        /// The get note list data.
        /// </summary>

        private async Task GetNotebooksDataAsync()
        {
            if (_notebooks.Count != 0)
                return;

            var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<string>));

            try
            {
                // Add a using System.IO;

                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("notebooks.json"))
                {
                    _notebooks = (ObservableCollection<string>)jsonSerializer.ReadObject(stream);
                }
            }
            catch
            {
                _notebooks = new ObservableCollection<string>();
            }
        }
    }
}
