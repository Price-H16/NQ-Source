using System;


namespace OpenNos.DAL.EF.Configuration
{
    public class DatabaseConfiguration
    {
        public DatabaseConfiguration()
        {
            Ip = Environment.GetEnvironmentVariable("DATABASE_IP") ?? "localhost";
            Username = Environment.GetEnvironmentVariable("DATABASE_USER") ?? "sa";
            Password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? "NosEngine19@";
            Database = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "nosquest";
            if (!ushort.TryParse(Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "1433", out ushort port))
            {
                port = 1433;
            }

            Port = port;
        }

        public string Ip { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public ushort Port { get; set; }

        public override string ToString() => $"Server={Ip},{Port};User id={Username};Password={Password};Initial Catalog={Database};";
    }
}