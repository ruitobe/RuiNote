//----------------------------------------------------------------------
// <copyright file="LogManager.cs" company="ruitobe">
//   Copyright (c) 2015 ruitobe. All rights reserved. 
// </copyright>
// <summary>
//   The log manager.
// </summary>
// ---------------------------------------------------------------------
using System;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Services;
using RuiNote.Services.Interfaces;

namespace RuiNote.Services
{
    /// <summary>
    /// The log manager.
    /// </summary>
    public class LogManager : ILogManager
    {
        /// <summary>
        /// The network information service.
        /// </summary>
        /// 
        private readonly INetworkInformationService _networkInformationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogManager"/> class.
        /// </summary>
        /// <param name="networkInformationService">The network information service.</param>
        /// 
        public LogManager(INetworkInformationService networkInformationService)
        {
            _networkInformationService = networkInformationService;
        }

        /// <summary>
        /// The start method for initialize the Log system.
        /// </summary>
        /// <param name="current">
        ///  The current application object.
        /// </param>
        /// <param name="rootFrame">
        ///  The root frame object.
        /// </param>
        /// <param name="key">
        /// The key value for initialize the Log system.
        /// </param>
        /// 

        public void Init(string key)
        {
            try
            {
                if (_networkInformationService.IsNetworkAvailable)
                {

                }
            }
            catch (Exception ex)
            {
                // TODO : Init - Send exception using email
            }
        }

        /// <summary>
        /// The log method.
        /// </summary>
        /// <param name="e">
        /// The exception got.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        /// 

        public async Task LogAsync(Exception e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                // TODO : Log - Send exception using email
            }
        }

        /// <summary>
        /// The close async method.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        /// 

        public async Task CloseAsync()
        {
            if (_networkInformationService.IsNetworkAvailable)
            {

            }
        }
    }
}
