// WingsEmu
// 
// Developed by NosWings Team

using Autofac;
using OpenNos.Core;
using ChickenAPI.Core.Utils;
using ChickenAPI.Plugins;
using OpenNos.DAL;
using OpenNos.DAL.DAO;
using OpenNos.DAL.EF;
using WingsEmu.Plugins.DB.MSSQL.Mapping;
using OpenNos.DAL.Interface;
using OpenNos.DAL.EF.Configuration;

namespace WingsEmu.Plugins.DB.MSSQL
{
    public class DatabasePlugin : ICorePlugin
    {
        public string Name => nameof(DatabasePlugin);

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
        }

        public void OnLoad(ContainerBuilder builder)
        {
            Logger.Log.InfoFormat("Registering DAL.EF objects");
            builder.RegisterTypes(typeof(AccountDAO).Assembly.GetTypesImplementingInterface<IMappingBaseDAO>()).AsImplementedInterfaces().AsSelf();
            builder.RegisterType<DbContextFactory>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<DatabaseConfiguration>().AsImplementedInterfaces().AsSelf();
            Logger.Log.InfoFormat("Registering DAL objects");
            builder.RegisterType(typeof(DAOFactory)).AsSelf();
            Logger.Log.InfoFormat("Registering Mapping objects");
            builder.Register(_ => new NosQuestItemInstanceMappingType()).As<ItemInstanceDAO.IItemInstanceMappingTypes>();
            Logger.Log.InfoFormat("Registering DAL.EF.DAO objects");
            builder.RegisterTypes(typeof(OpenNosContext).Assembly.GetTypes()).AsSelf().AsImplementedInterfaces().SingleInstance();
        }
    }
}