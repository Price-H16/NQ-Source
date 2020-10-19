using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using log4net;
using NosTale.Configuration;
using NosTale.Configuration.Helper;
using NosTale.Configuration.Utilities;
using NosTale.Packets.Packets.ClientPackets;
using OpenNos.Core;
using OpenNos.DAL.EF.Helpers;
using OpenNos.GameObject;
using OpenNos.Handler.BasicPacket.Login;
using OpenNos.Master.Library.Client;

namespace OpenNos.Login
{
    public static class Program
    {
        #region Members

        private static bool _isDebug;

        private static int _port;

        #endregion

        #region Methods

        public static void Main(string[] args)
        {
            checked
            {
                try
                {
#if DEBUG
                    _isDebug = true;
#endif
                    CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
                    Console.Title = $"OpenNos Login Server{(_isDebug ? " Development Environment" : "")}";

                    var ignoreStartupMessages = false;
                    foreach (var arg in args)
                    {
                        ignoreStartupMessages |= arg == "--nomsg";
                    }

                    // initialize Logger
                    Logger.InitializeLogger(LogManager.GetLogger(typeof(Program)));
                    ConfigurationHelper.CustomisationRegistration();
                    var a = DependencyContainer.Instance.GetInstance<JsonGameConfiguration>().Server;

                    var port = a.LoginPort;
                    var portArgIndex = Array.FindIndex(args, s => s == "--port");
                    if (portArgIndex != -1
                        && args.Length >= portArgIndex + 1
                        && int.TryParse(args[portArgIndex + 1], out port))
                    {
                        Console.WriteLine("Port override: " + port);
                    }

                    _port = port;
                    if (!ignoreStartupMessages)
                    {
                        var assembly = Assembly.GetExecutingAssembly();
                        var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                        var text = $"LOGIN SERVER v{fileVersionInfo.ProductVersion}dev - PORT : {port} by OpenNos Team";
                        var offset = Console.WindowWidth / 2 + text.Length / 2;
                        var separator = new string('=', Console.WindowWidth);
                        Console.WriteLine(separator + string.Format("{0," + offset + "}\n", text) + separator);
                    }

                    // initialize api
                    if (CommunicationServiceClient.Instance.Authenticate(a.MasterAuthKey))
                    {
                        Logger.Info(Language.Instance.GetMessageFromKey("API_INITIALIZED"));
                    }

                    // initialize DB
                    if (!DataAccessHelper.Initialize())
                    {
                        Console.ReadKey();
                        return;
                    }

                    Logger.Info(Language.Instance.GetMessageFromKey("CONFIG_LOADED"));

                    try
                    {
                        AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("General Error", ex);
                    }

                    try
                    {
                        // initialize PacketSerialization
                        PacketFactory.Initialize<WalkPacket>();

                        var networkManager = new NetworkManager<LoginCryptography>(a.IPAddress, port,
                            typeof(LoginPacketHandler), typeof(LoginCryptography), false);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogEventError("INITIALIZATION_EXCEPTION", "General Error Server", ex);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogEventError("INITIALIZATION_EXCEPTION", "General Error", ex);
                    Console.ReadKey();
                }
            }
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Error((Exception) e.ExceptionObject);
            try
            {
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            Logger.Debug("Login Server crashed! Rebooting gracefully...");
            Process.Start("OpenNos.Login.exe", $"--nomsg --port {_port}");
            Environment.Exit(1);
        }

        #endregion
    }
}