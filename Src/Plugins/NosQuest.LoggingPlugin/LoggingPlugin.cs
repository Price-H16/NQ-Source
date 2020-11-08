using Autofac;
using ChickenAPI.Core.Logging;
using ChickenAPI.Plugins;

namespace NosQuest.Plugins.Logging
{
    public class LoggingPlugin : ICorePlugin
    {
        public string Name => nameof(LoggingPlugin);

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
        }

        public void OnLoad(ContainerBuilder builder)
        {
            builder.RegisterType<SerilogLogger>().As<ILogger>();
        }
    }
}