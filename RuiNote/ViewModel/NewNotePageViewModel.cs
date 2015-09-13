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
    public class NewNotePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILogManager _logManager;
        private readonly IMessageBoxService _messageBox;
        private readonly INoteSessionService _noteSessionService;

        private int id;
        private bool _inProgress;
        private Note _note;

        /// <summary>
        /// Gets the add command.
        /// </summary>
        /// <value>
        /// The add command.
        /// </value>
        public ICommand AddCommand { get; private set; }
        
        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        /// <value>
        /// The cancel command.
        /// </value>
        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// Gets the note property.
        /// </summary>
        /// <value>
        /// The note property.
        /// </value>
        public Note Note 
        {
            get { return _note; }
            set { Set(() => Note, ref _note, value); }
        }

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
        /// Initializes a new instance of the <see cref="NewNotePageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">
        /// The navigation service.
        /// </param>
        /// <param name="noteCompositionSessionService">
        /// The note composition session service.
        /// </param>
        /// <param name="messageBox">
        /// The message box.
        /// </param>
        /// <param name="logManager">
        /// The log manager.
        /// </param>

        public NewNotePageViewModel(INavigationService navigationService,
            INoteSessionService noteSessionService,
            IMessageBoxService messageBox,
            ILogManager logManager)
        {
            _navigationService = navigationService;
            _noteSessionService = noteSessionService;
            _messageBox = messageBox;
            _logManager = logManager;
            
            AddCommand = new RelayCommand(
                () =>
                {
                    // here we need to save the new added note, 
                    // including title, cotent and time
                    AddNoteAction();

                });

            CancelCommand = new RelayCommand(
                () =>
                {
                   CancelAction();
                });

            _note = new Note();
            
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                // We need to get the note list count right after the NewNotePage is navigated to
                // We can't get the note list count when back button is pressed
                // Because, we don't get the instance of navigation service is already been destroyed when back button is fired.
                if (message.Notification == "GetNoteListCount")
                {
                    id = (int)_navigationService.CurrentParameter;
                }

                if (message.Notification == "NewNotePageBackButtonPressed")
                {

                    if (_note.NoteTitle == null && _note.NoteContent == null)
                    {
                        // we don't save the empty note.
                        System.Diagnostics.Debug.WriteLine("note is empty we don't save...");
                    }
                    else
                    {
                        AddNoteAction();
                    }
                    
                }
                
            });

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

        private void AddNoteAction()
        {
            Exception exception = null;
            try 
            {
                _note.NoteTime = DateTime.Now;
                _noteSessionService.AddNote(new Note()
                { 
                    NoteTitle = _note.NoteTitle, 
                    NoteContent = _note.NoteContent, 
                    NoteTime = _note.NoteTime,
                    // assign the current note list index as ID
                    ID = id
                });

                Note.NoteTitle = null;
                Note.NoteContent = null;
                // now, since we use back button, we will go to ContentPage automatically:
                //_navigationService.Navigate<ContentPage>();
            }

            catch (Exception ex)
            {
                exception = ex;
            }
        }

        private async void CancelAction()
        {
            int flag = await _messageBox.ShowAsync("Are you sure?",
                        "Cancel Adding New Note",
                        new List<string> {
                        "Yes", "No"});
            if (flag == 0)
            {
                // Yes, I want to cancel the adding new note, then
                // we go to the ContentPage.
                _navigationService.Navigate<ContentPage>();
            }
            else
            {
                // No, I change my mind and want to continue.
                System.Diagnostics.Debug.WriteLine("Stay here.");
            }
        }
    }
}
