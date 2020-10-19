using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;
using log4net;
using NosTale.Configuration;
using NosTale.Configuration.Helper;
using NosTale.Configuration.Utilities;
using OpenNos.Core;
using OpenNos.DAL.EF.Helpers;
using OpenNos.Master.Library.Interface;
using OpenNos.SCS.Communication.Scs.Communication.EndPoints.Tcp;
using OpenNos.SCS.Communication.ScsServices.Service;

namespace OpenNos.Master.Server
{
    internal static class Program
    {
        #region Members

        private static readonly ManualResetEvent _run = new ManualResetEvent(true);

        private static bool _isDebug;

        #endregion

        #region Methods

        public static void Main(string[] args)
        {
            try
            {
#if DEBUG
                _isDebug = true;
#endif
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
                Console.Title = $"OpenNos Master Server{(_isDebug ? " Development Environment" : "")}";

                var ignoreStartupMessages = false;
                var ignoreTelemetry = false;
                foreach (var arg in args)
                {
                    switch (arg)
                    {
                        case "--nomsg":
                            ignoreStartupMessages = true;
                            break;

                        case "--notelemetry":
                            ignoreTelemetry = true;
                            break;
                    }
                }

                // initialize Logger
                Logger.InitializeLogger(LogManager.GetLogger(typeof(Program)));

                ConfigurationHelper.CustomisationRegistration();
                var a = DependencyContainer.Instance.GetInstance<JsonGameConfiguration>();

                var port = a.Server.MasterPort;
                if (!ignoreStartupMessages)
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                    var text = $"MASTER SERVER v{fileVersionInfo.ProductVersion}dev - PORT : {port} by OpenNos Team";
                    var offset = Console.WindowWidth / 2 + text.Length / 2;
                    var separator = new string('=', Console.WindowWidth);
                    Console.WriteLine(separator + string.Format("{0," + offset + "}\n", text) + separator);
                }

                // initialize DB
                if (!DataAccessHelper.Initialize())
                {
                    Console.ReadLine();
                    return;
                }

                Logger.Info(Language.Instance.GetMessageFromKey("CONFIG_LOADED"));

                try
                {
                    // configure Services and Service Host
                    var ipAddress = a.Server.MasterIP;
                    var _server = ScsServiceBuilder.CreateService(new ScsTcpEndPoint(ipAddress, port));

                    _server.AddService<ICommunicationService, CommunicationService>(new CommunicationService());
                    _server.AddService<IConfigurationService, ConfigurationService>(new ConfigurationService());
                    _server.AddService<IMailService, MailService>(new MailService());
                    _server.AddService<IMallService, MallService>(new MallService());
                    _server.AddService<IAuthentificationService, AuthentificationService>(
                        new AuthentificationService());
                    _server.ClientConnected += OnClientConnected;
                    _server.ClientDisconnected += OnClientDisconnected;

                    _server.Start();
                    Logger.Info(Language.Instance.GetMessageFromKey("STARTED"));
                    if (!ignoreTelemetry)
                    {
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("General Error Server", ex);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("General Error", ex);
                Console.ReadKey();
            }
        }

        private static void OnClientConnected(object sender, ServiceClientEventArgs e)
        {
            Logger.Info(Language.Instance.GetMessageFromKey("NEW_CONNECT") + e.Client.ClientId);
        }

        private static void OnClientDisconnected(object sender, ServiceClientEventArgs e)
        {
            Logger.Info(Language.Instance.GetMessageFromKey("DISCONNECT") + e.Client.ClientId);
        }

        #endregion
    }
}