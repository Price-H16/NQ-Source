using Autofac;
using ChickenAPI.Plugins;
using Discord.Webhook;

namespace Plugins.DiscordWebhook
{
    public class DiscordWebhookPlugin : ICorePlugin
    {
        public string Name { get; } = nameof(DiscordWebhookPlugin);

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
        }

        public void OnLoad(ContainerBuilder builder)
        {
            var webhook = "https://discordapp.com/api/webhooks/676941880247975973/uMI0T85omzo5OBcCNCVVryObx1btUbibX9jvaXjziwCUUAdMQvvZ5lG50Lto0Q_EBcWZ";
            builder.Register(s => new DiscordWebhookClient(webhook));
            builder.Register(s => new DiscordWebHookNotifier(new DiscordWebhookNotifierConfig(), null, s.Resolve<DiscordWebhookClient>()));
        }
    }
}