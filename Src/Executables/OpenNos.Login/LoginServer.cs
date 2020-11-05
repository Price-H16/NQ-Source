using NosTale.Configuration;
using NosTale.Configuration.Utilities;
using OpenNos.Core;
using OpenNos.Master.Library.Client;
using System;
using System.Linq;

namespace OpenNos.Login
{
    public static class LoginServer
    {
        private static void InitializeMasterCommunication()
        {
            var a = DependencyContainer.Instance.GetInstance<JsonGameConfiguration>().Server;

            if (CommunicationServiceClient.Instance.Authenticate(a.MasterAuthKey))
            {
                Logger.Info(Language.Instance.GetMessageFromKey("API_INITIALIZED"));
            }
        }
        private static void PrintHeader()
        {
            Console.Title = "WingsEmu - Login";
            const string text = @"
 __      __.__                     ___________              
/  \    /  \__| ____    ____  _____\_   _____/ _____  __ __ 
\   \/\/   /  |/    \  / ___\/  ___/|    __)_ /     \|  |  \
 \        /|  |   |  \/ /_/  >___ \ |        \  Y Y  \  |  /
  \__/\  / |__|___|  /\___  /____  >_______  /__|_|  /____/ 
       \/          \//_____/     \/        \/      \/       
            .____                 .__                       
            |    |    ____   ____ |__| ____                 
            |    |   /  _ \ / ___\|  |/    \                
            |    |__(  <_> ) /_/  >  |   |  \               
            |_______ \____/\___  /|__|___|  /               
                    \/    /_____/         \/                
";
            string separator = new string('=', Console.WindowWidth);
            string logo = text.Split('\n').Select(s => string.Format("{0," + (Console.WindowWidth / 2 + s.Length / 2) + "}\n", s))
                .Aggregate("", (current, i) => current + i);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(separator + logo + separator);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

}
