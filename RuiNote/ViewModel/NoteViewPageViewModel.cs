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
    public class NoteViewPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILogManager _logManager;
        private readonly IMessageBoxService _messageBox;
        private readonly INoteSessionService _noteSessionService;

        public Note SelectedNote
        {
            get { return (Note)_navigationService.CurrentParameter; }
        }

        /// <summary>
        /// Gets the edit command.
        /// </summary>
        /// <value>
        /// The edit command.
        /// </value>
        public ICommand EditCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteViewPageViewModel"/> class.
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
        public NoteViewPageViewModel(INavigationService navigationService,
            INoteSessionService noteSessionService,
            IMessageBoxService messageBox,
            ILogManager logManager)
        {
            _navigationService = navigationService;
            _noteSessionService = noteSessionService;
            _messageBox = messageBox;
            _logManager = logManager;

            EditCommand = new RelayCommand<Note>(EditAction);
        }


        private void EditAction(Note note)
        {

            System.Diagnostics.Debug.WriteLine("Here we need to navigate the select note to NoteEditPage...");
            System.Diagnostics.Debug.WriteLine("the Note ID to NoteEditPage = " + note.ID);
            _navigationService.Navigate<NoteEditPage>(note);          
        }
    }
}
