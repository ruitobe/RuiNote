using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

using RuiNote.Services.Interfaces;
using RuiNote.Services.Model;
using RuiNote.Views;

using Cimbalino.Toolkit.Services;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace RuiNote.ViewModel
{
    public class ContentPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILogManager _logManager;
        private readonly IAuthenticationSessionService _authenticationSessionService;
        private readonly INoteSessionService _noteSessionService;
        //private readonly INotebookSessionService _notebookSessionService;

        public ContentPageViewModel(INavigationService navigationService,
            ILogManager logManager,
            IAuthenticationSessionService authenticationService,
            INoteSessionService noteSessionService)
            // Rui: 2015.07.05: notebook folder to be implemented in the future. I already wrote the source codes in 
            //      Services folder, but I got exception! To investigate it later!
            //INotebookSessionService notebookSessionService)
        {

            _navigationService = navigationService;
            _logManager = logManager;
            _authenticationSessionService = authenticationService;
            _noteSessionService = noteSessionService;
            //_notebookSessionService = notebookSessionService;

            LogoutCommand = new RelayCommand(
                () =>
                {
                    _authenticationSessionService.Logout();

                    // goto MainPage, and remove any saved seesion data

                    _navigationService.Navigate<MainPage>();
                });

            AddNewNoteCommand = new RelayCommand(
                () =>
                {
                    // goto NewNotePage, pass the note list count as ID for the new note!
                    _navigationService.Navigate<NewNotePage>(NoteList.Count);
                });

            DeleteNoteCommand = new RelayCommand<Note>(DeleteNoteAction);

            EditNoteCommand = new RelayCommand<Note>(EditNoteAction);

            AddNewNoteFolderCommand = new RelayCommand(
                () =>
                {
                    // goto NewNoteFolderPage

                    System.Diagnostics.Debug.WriteLine("Here we need to goto NewNoteFolderPage...");
                });

            ViewAllNotesCommand = new RelayCommand(
                () =>
                {
                    // goto NoteListPage
                    _navigationService.Navigate<NoteListPage>();
                });


            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "UpdateNoteList":
                        GetRuiNoteList();
                        break;

                    case "AppBarButtonNewNotebookVisiable":
                        // Rui: using converter for boolean to visibility
                        // in xmal: xmlns:conv="using:RuiNote.Helpers"
                        // create a new help class: BooleanToVisibilityConverter.cs
                        // then group the appBarButtons
                        /*
                         <Page.Resources>
                         <conv:BooleanToVisibilityConverter x:Key="MyConverter"/>
                         </Page.Resources>
                         * 
                         Visibility="{Binding AppBarButtonNewNotebookVisibility, Converter={StaticResource MyConverter}}"/>
                         * */
                        AppBarButtonNewNotebookVisibility = true;
                        AppBarButtonNewNoteVisibility = false;

                        break;

                    case "AppBarButtonNewNoteVisiable":
                        AppBarButtonNewNotebookVisibility = false;
                        AppBarButtonNewNoteVisibility = true;

                        break;

                    default:
                        break;
                }

            });

        }

        private void DeleteNoteAction(Note note)
        {
            _noteSessionService.DeleteNote(note);

            GetRuiNoteList();
        }

        private void EditNoteAction(Note note)
        {

            System.Diagnostics.Debug.WriteLine("Here we need to navigate the select note to NoteEditPage...");
            System.Diagnostics.Debug.WriteLine("the Note ID to NoteEditPage = " + note.ID);
            _navigationService.Navigate<NoteEditPage>(note);
        }

        private async void GetRuiNoteList()
        {
            ObservableCollection<Note> list = await _noteSessionService.GetNoteList();
            NoteList = new ObservableCollection<Note>(list);
        }

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
        /// Gets the about command.
        /// </summary>
        /// <value>
        /// The about command.
        /// </value>

        public ICommand AboutCommand { get; private set; }

        /// <summary>
        /// Gets the logout command.
        /// </summary>
        /// <value>
        /// The logout command.
        /// </value>

        public ICommand LogoutCommand { get; private set; }

        /// <summary>
        /// Adds new note command.
        /// </summary>
        /// <value>
        /// The add new note command.
        /// </value>
        public ICommand AddNewNoteCommand { get; private set; }

        /// <summary>
        /// Deletes note command.
        /// </summary>
        /// <value>
        /// The delete note command.
        /// </value>
        public ICommand DeleteNoteCommand { get; private set; }

        /// <summary>
        /// Gets the edit note command.
        /// </summary>
        /// <value>
        /// The edit note command.
        /// </value>
        public ICommand EditNoteCommand { get; private set; }

        /// <summary>
        /// Adds new note folder command.
        /// </summary>
        /// <value>
        /// The add new note folder command.
        /// </value>
        public ICommand AddNewNoteFolderCommand { get; private set; }

        /// <summary>
        /// Views all notes command.
        /// </summary>
        /// <value>
        /// The view all notes command.
        /// </value>
        public ICommand ViewAllNotesCommand { get; private set; }

        /// <summary>
        /// Gets user info.
        /// </summary>
        /// <value>
        /// The user info.
        /// </value>
       
        public string GetUser
        {
            get
            {
                return _authenticationSessionService.GetAuthenticationSession().UserInfo;
            }
        }

        private ObservableCollection<string> _notebook;

        /// <summary>
        /// Gets the notebook names list property.
        /// </summary>
        /// <value>
        /// The notebook names list property.
        /// </value>
        public ObservableCollection<string> Notebook
        {
            get { return _notebook; }

            set { Set(() => Notebook, ref _notebook, value); }
        }

        /// <summary>
        /// Gets AppBarButton Visibility of new note.
        /// </summary>
        /// <value>
        /// The AppBarButton of new note.
        /// </value>

        private bool _newNoteVisibility = false;

        public bool AppBarButtonNewNoteVisibility
        {
            get { return _newNoteVisibility; }
            set { Set(() => AppBarButtonNewNoteVisibility, ref _newNoteVisibility, value); }
        }

        /// <summary>
        /// Gets AppBarButton Visibility of new notebook.
        /// </summary>
        /// <value>
        /// The AppBarButton of new notebook.
        /// </value>

        private bool _newNotebookVisibility = false;

        public bool AppBarButtonNewNotebookVisibility
        {
            get { return _newNotebookVisibility; }
            set { Set(() => AppBarButtonNewNotebookVisibility, ref _newNotebookVisibility, value); }
        }
    }
}
