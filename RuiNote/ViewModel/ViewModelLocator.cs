//----------------------------------------------------------------------
// <copyright file="ViewModelLocator.cs" company="ruitobe">
//   Copyright (c) 2015 ruitobe. All rights reserved. 
// </copyright>
// <summary>
//   This class contains static references to all the view models in the
//   application and provides an entry point for the bindings.
// </summary>
// ---------------------------------------------------------------------

using RuiNote.Services;
using RuiNote.Services.Interfaces;
using Cimbalino.Toolkit.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace RuiNote.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
            }

            SimpleIoc.Default.Register<IApplicationDataService, ApplicationDataService>();
            SimpleIoc.Default.Register<IStorageService, StorageService>();
            SimpleIoc.Default.Register<IMessageBoxService, MessageBoxService>();
            SimpleIoc.Default.Register<IAuthenticationSessionService, AuthenticationSessionService>();
            SimpleIoc.Default.Register<IMicrosoftAuthenticationService, MicrosoftAuthenticationService>();
            SimpleIoc.Default.Register<INetworkInformationService, NetworkInformationService>();
            SimpleIoc.Default.Register<IApplicationManifestService, ApplicationManifestService>();
            SimpleIoc.Default.Register<ILogManager, LogManager>();
            SimpleIoc.Default.Register<IEmailComposeService, EmailComposeService>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<INoteSessionService, NoteSessionService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ContentPageViewModel>();
            SimpleIoc.Default.Register<NewNotePageViewModel>();
            SimpleIoc.Default.Register<NoteListPageViewModel>();
            SimpleIoc.Default.Register<NoteViewPageViewModel>();
            SimpleIoc.Default.Register<NoteEditPageViewModel>();
        }

        /// <summary>
        /// Gets the main page view model.
        /// </summary>
        /// <value>
        /// The main page view model.
        /// </value>

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Gets the content page view model.
        /// </summary>
        /// <value>
        /// The content page view model.
        /// </value>
        
        public ContentPageViewModel ContentPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ContentPageViewModel>();
            }
        }

        /// <summary>
        /// Gets the new note page view model.
        /// </summary>
        /// <value>
        /// The new note page view model.
        /// </value>

        public NewNotePageViewModel NewNotePage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NewNotePageViewModel>();
            }
        }

        /// <summary>
        /// Gets the note list page view model.
        /// </summary>
        /// <value>
        /// The note list page view model.
        /// </value>

        public NoteListPageViewModel NoteListPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NoteListPageViewModel>();
            }
        }

        /// <summary>
        /// Gets the note view page view model.
        /// </summary>
        /// <value>
        /// The note view page view model.
        /// </value>

        public NoteViewPageViewModel NoteViewPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NoteViewPageViewModel>();
            }
        }

        /// <summary>
        /// Gets the note edit page view model.
        /// </summary>
        /// <value>
        /// The note edit page view model.
        /// </value>

        public NoteEditPageViewModel NoteEditPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NoteEditPageViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}