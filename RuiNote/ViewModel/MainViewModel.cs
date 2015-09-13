using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using RuiNote.Resources;
using RuiNote.Services.Interfaces;
using RuiNote.Views;

using Cimbalino.Toolkit.Services;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Windows.ApplicationModel.Activation;

namespace RuiNote.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ILogManager _logManager;
        private readonly IMessageBoxService _messageBox;
        private readonly INetworkInformationService _networkInformationService;
        private readonly INavigationService _navigationService;
        private readonly IAuthenticationSessionService _authenticationSessionService;
        private bool _inProgress;

        /// <summary>
        /// Gets the login command.
        /// </summary>
        /// <value>
        /// The login command.
        /// </value>
        public ICommand LoginCommand { get; private set; }

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
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">
        /// The navigation service.
        /// </param>
        /// <param name="authenticationSessionService">
        /// The authentication session  service.
        /// </param>
        /// <param name="messageBox">
        /// The message box.
        /// </param>
        /// <param name="networkInformationService">
        /// The network connection.
        /// </param>
        /// <param name="logManager">
        /// The log manager.
        /// </param>
        /// 

        public MainViewModel(INavigationService navigationService,
            IAuthenticationSessionService authenticationSessionService,
            INetworkInformationService networkInformationService,
            IMessageBoxService messageBox, 
            ILogManager logManager)
        {
            _navigationService = navigationService;
            _authenticationSessionService = authenticationSessionService;
            _messageBox = messageBox;
            _networkInformationService = networkInformationService;
            _logManager = logManager;
            LoginCommand = new RelayCommand<string>(LoginAction);
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

        private async void LoginAction(string provider)
        {
            Exception exception = null;
            bool isToShowMessage = false;
            try
            {
                
                if (!_networkInformationService.IsNetworkAvailable)
                {
                    
                    int flag = await _messageBox.ShowAsync("Now, there is no network connection, you can still use this app offline...", "RuiNote", new List<string> { "OK" });
                    if (flag == 0)
                    {
                        // allow user to use the app still, if the user has ever signed in
                        // Jump to the content page here:
                        _navigationService.Navigate<ContentPage>();
                    }
                    else 
                        return;
                }

                InProgress = true;
                var auth = await _authenticationSessionService.LoginAsync(provider);
                
                if (auth == null)
                {
                    return;
                }

                if (!auth.Value)
                {
                    await ShowMessage();
                }
                else
                {
                    // here, the Microsoft authentication is successful, 
                    // we need to navigate to ContentPage. 
                    InProgress = false;
                    
                    _navigationService.Navigate<ContentPage>();
                }
 
            }
            catch (InvalidOperationException e)
            {
                InProgress = false;
                isToShowMessage = true;
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (isToShowMessage)
            {
                await _messageBox.ShowAsync(App.AppResourceLoader.GetString("MainPageViewAuthenticationFailed"),
                    App.AppResourceLoader.GetString("ApplicationTile"),
                    new List<string> { App.AppResourceLoader.GetString("AuthenticationOK") });
            }
            if (exception != null)
            {
                //
            }
        }

        private async Task ShowMessage()
        {
            await _messageBox.ShowAsync(App.AppResourceLoader.GetString("MainPageViewLoginNotAllowedMessage"),
                        App.AppResourceLoader.GetString("AuthenticationMessageBoxTitle"),
                        new List<string> {
                        App.AppResourceLoader.GetString("AuthenticationOK") });
        }
    }
}