﻿using System;

namespace OpenNos.Core.Networking.Communication.Scs.Server
{
    /// <summary>
    ///     Stores client information to be used by an event.
    /// </summary>
    public class ServerClientEventArgs : EventArgs
    {
        #region Instantiation

        /// <summary>
        ///     Creates a new ServerClientEventArgs object.
        /// </summary>
        /// <param name="client">Client that is associated with this event</param>
        public ServerClientEventArgs(IScsServerClient client) => Client = client;

        #endregion

        #region Properties

        /// <summary>
        ///     Client that is associated with this event.
        /// </summary>
        public IScsServerClient Client { get; }

        #endregion
    }
}