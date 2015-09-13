//----------------------------------------------------------------------
// <copyright file="MicrosoftAuthenticationService.cs" company="ruitobe">
//   Copyright (c) 2015 ruitobe. All rights reserved. 
// </copyright>
// <summary>
//   Defines the Microsoft Authentication Service type.
// </summary>
// ---------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using RuiNote.Services.Interfaces;
using RuiNote.Services.Model;
using RuiNote.Resources;

using Microsoft.Live;

namespace RuiNote.Services
{

    public class MicrosoftAuthenticationService : IMicrosoftAuthenticationService
    {
        private readonly ILogManager _logManager;
        private LiveAuthClient _authClient;
        private LiveConnectSession _liveSession;

        /// <summary>
        /// Defines the scopes the application needs.
        /// </summary>
        /// 
        private List<string> _scopes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftAuthenticationService"/> class.
        /// </summary>
        /// <param name="logManager">
        /// The log manager.
        /// </param>
        public MicrosoftAuthenticationService(ILogManager logManager)
        {
            _scopes = new List<string> { "wl.signin", "wl.skydrive", "wl.skydrive_update" };
            _logManager = logManager;
        }

        /// <summary>
        /// The login async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        /// 
        public async Task<AuthenticationSession> LoginAsync()
        {
            Exception rException = null;
            try
            {
                _authClient = new LiveAuthClient();
                // Rui: https://msdn.microsoft.com/en-us/library/hh694244.aspx
                //var loginResult = await _authClient.InitializeAsync(_scopes);
                
                // Rui: https://msdn.microsoft.com/en-us/library/hh533677.aspx
                var result = await _authClient.LoginAsync(_scopes);

                //if (loginResult.Status == LiveConnectSessionStatus.Connected)
                if (result.Status == LiveConnectSessionStatus.Connected)
                {
                    
                    //_liveSession = loginResult.Session;
                    _liveSession = result.Session;
                    var meResult = await GetUserInfo();
                    var session = new AuthenticationSession
                    {
                        AccessToken = result.Session.AccessToken,
                        //AccessToken = loginResult.Session.AccessToken,
                        Provider = Constants.MicrosoftProvider,
                        UserInfo = meResult["name"].ToString()
                    };
                    return session;
                }

            }

            catch (LiveAuthException authExp)
            {
                System.Diagnostics.Debug.WriteLine("LiveAuthException = " + authExp.ToString());
            }

            catch (LiveConnectException connExp)
            {
                System.Diagnostics.Debug.WriteLine("LiveConnectException = " + connExp.ToString());
            }

            catch (Exception ex)
            {
                rException = ex;
                System.Diagnostics.Debug.WriteLine("rException = " + rException.ToString());
            }
            await _logManager.LogAsync(rException);

            return null;
        }

        /// <summary>
        /// The get user info.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        /// 

        public async Task<IDictionary<string, object>> GetUserInfo()
        {
            Exception exception = null;

            try
            {
                var liveClient = new LiveConnectClient(_liveSession);
                LiveOperationResult operationResult = await liveClient.GetAsync("me");

                return operationResult.Result;
            }

            catch (LiveConnectException e)
            {
                exception = e;
            }

            await _logManager.LogAsync(exception);

            return null;
        }

        /// <summary>
        /// The logout.
        /// </summary>
        public async void Logout()
        {
            if (_authClient == null)
            {
                _authClient = new LiveAuthClient();
                var loginResult = await _authClient.InitializeAsync(_scopes);
            }

            if (_authClient.CanLogout)
            _authClient.Logout();

        }


    }
}
