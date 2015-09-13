using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using System.Windows.Input;
using RuiNote.Services.Interfaces;
using RuiNote.Services.Model;
using RuiNote.Views;

using Cimbalino.Toolkit.Services;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Windows.ApplicationModel.Activation;


namespace RuiNote.ViewModel
{
    public class NoteListPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILogManager _logManager;
        private readonly IMessageBoxService _messageBox;
        private readonly INoteSessionService _noteSessionService;
        private bool _inProgress;
        private int _noteListCount;
        private ObservableCollection<Note> _noteList;

        /// <summary>
        /// Gets the note list property.
        /// </summary>
        /// <value>
        /// The note list property.
        /// </value>
        public ObservableCollection<Note> NoteList
        {
            get { return _noteList; }
           
            set { Set(() => NoteList, ref _noteList, value); }
        }

        /// <summary>
        /// Gets the delete command.
        /// </summary>
        /// <value>
        /// The delete command.
        /// </value>
        public ICommand DeleteCommand { get; private set; }

        /// <summary>
        /// Gets the view selected note command.
        /// </summary>
        /// <value>
        /// The view selected note command.
        /// </value>
        public ICommand ViewCommand { get; private set; }

        /// <summary>
        /// Gets the pin note command.
        /// </summary>
        /// <value>
        /// The pin note command.
        /// </value>

        public ICommand PinCommand { get; private set; }

        /// <summary>
        /// Gets the email note command.
        /// </summary>
        /// <value>
        /// The email note command.
        /// </value>

        public ICommand EmailCommand { get; private set; }

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        /// <value>
        /// The navigation service.
        /// </value>
        public INavigationService NavigationService
        {
            get { return _navigationService; }
        }

        /// <summary>
        /// Gets the note list count.
        /// </summary>
        /// <value>
        /// The note list count property.
        /// </value>
        public int NoteListCount
        {
            get { return _noteListCount; }

            set { Set(() => NoteListCount, ref _noteListCount, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteListPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">
        /// The navigation service.
        /// </param>
        /// <param name="noteListViewSessionService">
        /// The note list view session service.
        /// </param>
        /// <param name="messageBox">
        /// The message box.
        /// </param>
        /// <param name="logManager">
        /// The log manager.
        /// </param>

        public NoteListPageViewModel(INavigationService navigationService,
            INoteSessionService noteSessionService,
            IMessageBoxService messageBox,
            ILogManager logManager)
        {
             
            _navigationService = navigationService;
            _noteSessionService = noteSessionService;
            _messageBox = messageBox;
            _logManager = logManager;
            ViewCommand = new RelayCommand<Note>(ViewAction);
            DeleteCommand = new RelayCommand<Note>(DeleteAction);
            EmailCommand = new RelayCommand<Note>(EmailAction);
            PinCommand = new RelayCommand(
                () =>
                {
                    System.Diagnostics.Debug.WriteLine("Here we need to pin the select note...");
                });
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                if (message.Notification == "UpdateNoteList")
                {
                    GetRuiNoteList();
                }
            });
        }

        private async void GetRuiNoteList()
        {
             ObservableCollection<Note> list = await _noteSessionService.GetNoteList();
             NoteList = new ObservableCollection<Note>(list);
             NoteListCount = list.Count;
        }

        private void DeleteAction(Note note)
        {
            
            _noteSessionService.DeleteNote(note);

            GetRuiNoteList();
        }

        private void ViewAction(Note note)
        {
            System.Diagnostics.Debug.WriteLine("Here we need to pass select note to view page...");
            System.Diagnostics.Debug.WriteLine("Passed note = " + note.NoteTitle);
            System.Diagnostics.Debug.WriteLine("ID = " + note.ID);
            _navigationService.Navigate<NoteViewPage>(note);
            
        }


        private void EmailAction(Note note)
        {
            System.Diagnostics.Debug.WriteLine("Here we need to pass select note to send email...");
        }

        /// <summary>
        /// Gets or sets a value indicating whether in progress.
        /// </summary>
        /// <value>
        /// The in progress.
        /// </value>
        public bool InProgress
        {
            get { return _inProgress; }
            set { Set(() => InProgress, ref _inProgress, value); }
        }

         
    }
}
