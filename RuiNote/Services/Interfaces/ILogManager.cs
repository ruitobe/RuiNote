//----------------------------------------------------------------------
// <copyright file="AuthenticationSessionService.cs" company="ruitobe">
//   Copyright (c) 2015 ruitobe. All rights reserved. 
// </copyright>
// <summary>
//   Defines the ILogManager type.
// </summary>
// ---------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace RuiNote.Services.Interfaces
{
    public interface ILogManager
    {
        /// <summary>
        /// The init.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        void Init(string key);

        /// <summary>
        /// The log method.
        /// </summary>
        /// <param name="e">
        /// The exception got.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        Task LogAsync(Exception e);

        /// <summary>
        /// The close async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> object.
        /// </returns>
        Task CloseAsync();
    }
}
