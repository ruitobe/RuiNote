//----------------------------------------------------------------------
// <copyright file="AuthenticationSessionService.cs" company="ruitobe">
//   Copyright (c) 2015 ruitobe. All rights reserved. 
// </copyright>
// <summary>
//   Defines the AuthenticationServiceSession type.
// </summary>
// ---------------------------------------------------------------------

using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Cimbalino.Toolkit.Services;

using RuiNote.Resources;
using RuiNote.Services.Interfaces;
using RuiNote.Services.Model;

namespace RuiNote.Services
{
    /// <summary>
    /// The authentication service session.
    /// </summary>
    
    public class AuthenticationSessionService : IAuthenticationSessionService
    {
        private readonly IApplicationDataService _applicationSettings;
        private readonly IMicrosoftAuthenticationService _microsoftAuthenticationService;
        private readonly ILogManager _logManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationSessionService" /> class.
        /// </summary>
        /// <param name="applicationSettings">The application settings.</param>
        /// <param name="microsoftAuthenticationService">The microsoft authentication service.</param>
        /// <param name="logManager">The log manager.</param>
        
        public AuthenticationSessionService(IApplicationDataService applicationSettings,
            IMicrosoftAuthenticationService microsoftAuthenticationService, ILogManager logManager)
        {
            _applicationSettings = applicationSettings;
            _microsoftAuthenticationService = microsoftAuthenticationService;
            _logManager = logManager;
        }

        /// <summary>
        /// Get the authentication session.
        /// </summary>
        /// <returns>The session object.</returns>
        /// 
        public AuthenticationSession GetAuthenticationSession()
        {
            var expiryValue = DateTime.MinValue;
            string expiryTicks = LoadEncryptedSettingValue("session_expiredate");

            if (!string.IsNullOrWhiteSpace(expiryTicks))
            {
                long expiryTicksValue;
                if (long.TryParse(expiryTicks, out expiryTicksValue))
                {
                    expiryValue = new DateTime(expiryTicksValue);
                }
            }

            var authenticationSession = new AuthenticationSession
            {
                AccessToken = LoadEncryptedSettingValue("session_token"),
                UserInfo = LoadEncryptedSettingValue("session_user"),
                Id = LoadEncryptedSettingValue("session_id"),
                ExpireDate = expiryValue,
                Provider = LoadEncryptedSettingValue("session_provider")
            };

            _applicationSettings.LocalSettings[Constants.LoginToken] = true;
            return authenticationSession;
        }

        /// <summary>
        /// The authentication save session.
        /// </summary>
        /// <param name="session">
        /// The authentication session.
        /// </param>
        //private void Save(AuthenticationSession session)
        public void SaveAuthenticationSession(AuthenticationSession session)
        {
            SaveEncryptedSettingValue("session_user", session.UserInfo);
            SaveEncryptedSettingValue("session_token", session.AccessToken);
            SaveEncryptedSettingValue("session_id", session.Id);
            SaveEncryptedSettingValue("session_expiredate", session.ExpireDate.Ticks.ToString(CultureInfo.InvariantCulture));
            SaveEncryptedSettingValue("session_provider", session.Provider);
        }

        /// <summary>
        /// The authentication clean session.
        /// </summary>
        private void CleanAuthenticationSession()
        {
            _applicationSettings.LocalSettings.Remove("session_user");
            _applicationSettings.LocalSettings.Remove("session_token");
            _applicationSettings.LocalSettings.Remove("session_id");
            _applicationSettings.LocalSettings.Remove("session_expiredate");
            _applicationSettings.LocalSettings.Remove("session_provider");
            _applicationSettings.LocalSettings.Remove(Constants.LoginToken);

        }

        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        /// <value>
        /// The provider.
        /// </value>
        public string Provider { get; set; }


        /// <summary>
        /// The login async.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        public async Task<bool?> LoginAsync(string provider)
        {
            Provider = provider;
            Exception exception;

            try
            {
                AuthenticationSession session = null;
                switch (provider)
                {
                    case Constants.MicrosoftProvider:
                        session = await _microsoftAuthenticationService.LoginAsync();
                        break;
                    default:
                        break;
                }

                if (session == null)
                {
                    return null;
                }
                // if the session is OK, we need to save it
                // so the user doesn't need to sign in again, when
                // starts the app.
                SaveAuthenticationSession(session);
                return true;
            }

            catch (Exception ex)
            {
                exception = ex;
            }

            await _logManager.LogAsync(exception);

            return false;
        }

        public async void Logout()
        {
            Exception exception = null;
            try
            {
                var session = GetAuthenticationSession();
                switch (session.Provider)
                {
                    case Constants.MicrosoftProvider:
                        _microsoftAuthenticationService.Logout();
                        break;

                    default:
                        break;
                }

                // Rui: here we clean all session data:
                CleanAuthenticationSession();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            if (exception != null)
            {
                await _logManager.LogAsync(exception);
            }
        }

        /// <summary>
        /// Loads an encrypted setting value for a given key.
        /// </summary>
        /// <param name="key">
        /// The key to load.
        /// </param>
        /// <returns>
        /// The value of the key.
        /// </returns>
        private string LoadEncryptedSettingValue(string key)
        {
            string value = null;

            var protectedBytes = _applicationSettings.LocalSettings[key];
            if (protectedBytes != null)
            {
                value = protectedBytes.ToString();
            }

            return value;
        }

        /// <summary>
        /// Saves a setting value against a given key, encrypted.
        /// </summary>
        /// <param name="key">
        /// The key to save against.
        /// </param>
        /// <param name="value">
        /// The value to save against.
        /// </param>
        /// 
        private void SaveEncryptedSettingValue(string key, string value)
        {

            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                _applicationSettings.LocalSettings[key] = value;
            }
        }
    }
}
