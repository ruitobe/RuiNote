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
    public class NoteEditPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ILogManager _logManager;
        private readonly IMessageBoxService _messageBox;
        private readonly INoteSessionService _noteSessionService;
        private Note _note;

        public Note TargetNote
        {
            get { return _note;}
            set { Set(() => TargetNote, ref _note, value); }
        }


        /// <summary>
        /// Gets the save command.
        /// </summary>
        /// <value>
        /// The save command.
        /// </value>
        public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteEditPageViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">
        /// The navigation service.
        /// </param>
        /// <param name="noteSessionService">
        /// The note session service.
        /// </param>
        /// <param name="messageBox">
        /// The message box.
        /// </param>
        /// <param name="logManager">
        /// The log manager.
        /// </param>
        public NoteEditPageViewModel(INavigationService navigationService,
            INoteSessionService noteSessionService,
            IMessageBoxService messageBox,
            ILogManager logManager)
        {
            _navigationService = navigationService;
            _noteSessionService = noteSessionService;
            _messageBox = messageBox;
            _logManager = logManager;
            

            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {

                if (message.Notification == "UpdateNavigatedNote")
                {
                    TargetNote = (Note)_navigationService.CurrentParameter;
                }
                
                if (message.Notification == "NoteEditPageBackButtonPressed")
                {

                    if (_note.NoteTitle == null && _note.NoteContent == null)
                    {
                        // we don't save the empty note.
                        System.Diagnostics.Debug.WriteLine("note is empty we don't save...");
                    }
                    else
                    {
                        ReplaceNoteAction();
                    }

                }

            });

        }

        private void ReplaceNoteAction()
        {
            Exception exception = null;
            try
            {
                _note.NoteTime = DateTime.Now;
                _noteSessionService.ReplaceNote(new Note()
                {
                    NoteTitle = TargetNote.NoteTitle,
                    NoteContent = TargetNote.NoteContent,
                    NoteTime = TargetNote.NoteTime,
                    ID = TargetNote.ID
                });

            }

            catch (Exception ex)
            {
                exception = ex;
            }
        }
    
    }
}
