﻿using System;
using OpenNos.Core.Networking.Communication.Scs.Communication.Channels;
using OpenNos.Core.Networking.Communication.Scs.Communication.Protocols;

namespace OpenNos.Core.Networking.Communication.Scs.Server
{
    /// <summary>
    ///     This class provides base functionality for server Classs.
    /// </summary>
    public abstract class ScsServerBase : IScsServer, IDisposable
    {
        #region Instantiation

        /// <summary>
        ///     Constructor.
        /// </summary>
        protected ScsServerBase()
        {
            Clients = new ThreadSafeSortedList<long, IScsServerClient>();
            WireProtocolFactory = WireProtocolManager.GetDefaultWireProtocolFactory();
        }

        #endregion

        #region Members

        /// <summary>
        ///     This object is used to listen incoming connections.
        /// </summary>
        private IConnectionListener _connectionListener;

        private bool _disposed;

        #endregion

        #region Events

        /// <summary>
        ///     This event is raised when a new client is connected.
        /// </summary>
        public event EventHandler<ServerClientEventArgs> ClientConnected;

        /// <summary>
        ///     This event is raised when a client disconnected from the server.
        /// </summary>
        public event EventHandler<ServerClientEventArgs> ClientDisconnected;

        #endregion

        #region Properties

        /// <summary>
        ///     A collection of clients that are connected to the server.
        /// </summary>
        public ThreadSafeSortedList<long, IScsServerClient> Clients { get; }

        /// <summary>
        ///     Gets/sets wire protocol that is used while reading and writing messages.
        /// </summary>
        public IScsWireProtocolFactory WireProtocolFactory { get; set; }

        #endregion

        #region Methods

        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
                _disposed = true;
            }
        }

        /// <summary>
        ///     Starts the server.
        /// </summary>
        public virtual void Start()
        {
            _connectionListener = CreateConnectionListener();
            _connectionListener.CommunicationChannelConnected += ConnectionListener_CommunicationChannelConnected;
            _connectionListener.Start();
        }

        /// <summary>
        ///     Stops the server.
        /// </summary>
        public virtual void Stop()
        {
            _connectionListener?.Stop();
            foreach (var client in Clients.GetAllItems()) client.Disconnect();
        }

        /// <summary>
        ///     This method is implemented by derived Classs to create appropriate connection listener to
        ///     listen incoming connection requets.
        /// </summary>
        /// <returns></returns>
        protected abstract IConnectionListener CreateConnectionListener();

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) Clients.Dispose();
        }

        /// <summary>
        ///     Raises ClientConnected event.
        /// </summary>
        /// <param name="client">Connected client</param>
        protected virtual void OnClientConnected(IScsServerClient client)
        {
            ClientConnected?.Invoke(this, new ServerClientEventArgs(client));
        }

        /// <summary>
        ///     Raises ClientDisconnected event.
        /// </summary>
        /// <param name="client">Disconnected client</param>
        protected virtual void OnClientDisconnected(IScsServerClient client)
        {
            ClientDisconnected?.Invoke(this, new ServerClientEventArgs(client));
        }

        /// <summary>
        ///     Handles Disconnected events of all connected clients.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void Client_Disconnected(object sender, EventArgs e)
        {
            var client = (IScsServerClient) sender;
            Clients.Remove(client.ClientId);
            OnClientDisconnected(client);
        }

        /// <summary>
        ///     Handles CommunicationChannelConnected event of _connectionListener object.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void ConnectionListener_CommunicationChannelConnected(object sender, CommunicationChannelEventArgs e)
        {
            var client = new NetworkClient(e.Channel)
            {
                ClientId = ScsServerManager.GetClientId(),
                WireProtocol = WireProtocolFactory.CreateWireProtocol()
            };

            client.Disconnected += Client_Disconnected;
            Clients[client.ClientId] = client;
            OnClientConnected(client);
            e.Channel.Start();
        }

        #endregion
    }
}