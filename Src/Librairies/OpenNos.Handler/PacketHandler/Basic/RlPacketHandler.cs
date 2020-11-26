﻿using Autofac;
using ChickenAPI.Plugins;
using ChickenAPI.Plugins.Exceptions;
using NosTale.Packets.Packets.ClientPackets;
using OpenNos.Core;
using OpenNos.Domain;
using OpenNos.GameObject;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;
using Plugins.DiscordWebhook;
using System.Collections.Generic;

namespace OpenNos.Handler.PacketHandler.Basic
{
    public class RlPacketHandler : IPacketHandler
    {
        #region Instantiation

        public RlPacketHandler(ClientSession session)
        {
            Session = session;
        }

        #endregion

        #region Properties

        public ClientSession Session { get; }

        #endregion

        #region Methods
        private static IContainer BuildCoreContainer()
        {
            var pluginBuilder = new ContainerBuilder();
            pluginBuilder.RegisterType<DiscordWebhookPlugin>().AsImplementedInterfaces().AsSelf();
            var container = pluginBuilder.Build();

            var coreBuilder = new ContainerBuilder();
            foreach (var plugin in container.Resolve<IEnumerable<ICorePlugin>>())
            {
                try
                {
                    plugin.OnLoad(coreBuilder);
                }
                catch (PluginException e)
                {
                }
            }


            return coreBuilder.Build();
        }
        public void RaidListRegister(RlPacket rlPacket)
        {
            switch (rlPacket.Type)
            {
                case 0: // Show the Raid List
                    if (Session.Character.Group?.IsLeader(Session) == true && Session.Character.Group.GroupType != GroupType.Group && ServerManager.Instance.GroupList.Any(s => s.GroupId == Session.Character.Group.GroupId)) 
                        Session.SendPacket(UserInterfaceHelper.GenerateRl(1));
                    else if (Session.Character.Group != null && Session.Character.Group.GroupType != GroupType.Group && Session.Character.Group.IsLeader(Session))
                        Session.SendPacket(UserInterfaceHelper.GenerateRl(2));
                    else if (Session.Character.Group != null)
                        Session.SendPacket(UserInterfaceHelper.GenerateRl(3));
                    else
                        Session.SendPacket(UserInterfaceHelper.GenerateRl(0));

                    break;

                case 1: // Register a team
                    if (Session.Character.Group != null && Session.Character.Group.GroupType != GroupType.Group && Session.Character.Group.IsLeader(Session) && !ServerManager.Instance.GroupList.Any(s => s.GroupId == Session.Character.Group.GroupId))
                    {
                        var pluginBuilder = new ContainerBuilder();
                        IContainer container = pluginBuilder.Build();
                        using var coreContainer = BuildCoreContainer();
                        ServerManager.Instance.GroupList.Add(Session.Character.Group);
                        Session.SendPacket(UserInterfaceHelper.GenerateRl(1));
                        Session.SendPacket(UserInterfaceHelper.GenerateInfo(Language.Instance.GetMessageFromKey("RAID_REGISTERED")));
                        var ss = coreContainer.Resolve<DiscordWebHookNotifier>();
                        ss.NotifyAllAsync(NotifiableEventType.X_IS_LOOKING_FOR_RAID_MATES, Session.Character.Name, Session.Character.Group.Raid?.Label);
                        ServerManager.Instance.Broadcast(Session, $"qnaml 100 #rl {string.Format(Language.Instance.GetMessageFromKey("SEARCH_TEAM_MEMBERS"), Session.Character.Name, Session.Character.Group.Raid?.Label)}", ReceiverType.AllExceptGroup);
                    }

                    break;

                case 2: // Cancel the team registration
                    if (Session.Character.Group != null
                        && Session.Character.Group.GroupType != GroupType.Group
                        && Session.Character.Group.IsLeader(Session)
                        && ServerManager.Instance.GroupList.Any(s => s.GroupId == Session.Character.Group.GroupId))
                    {
                        ServerManager.Instance.GroupList.Remove(Session.Character.Group);
                        Session.SendPacket(UserInterfaceHelper.GenerateRl(2));
                        Session.SendPacket(
                            UserInterfaceHelper.GenerateInfo(Language.Instance.GetMessageFromKey("RAID_UNREGISTERED")));
                    }

                    break;

                case 3: // Become a team member
                    var targetSession = ServerManager.Instance.GetSessionByCharacterName(rlPacket.CharacterName);

                    if (targetSession?.Character?.Group == null) return;

                    if (targetSession.Character.CharacterId == Session.Character.CharacterId) return;

                    if (!targetSession.Character.Group.IsLeader(targetSession)) return;

                    if (!ServerManager.Instance.GroupList.Any(group =>
                        group.GroupId == targetSession.Character.Group.GroupId)) return;

                    targetSession.Character.GroupSentRequestCharacterIds.Add(Session.Character.CharacterId);

                    new PJoinPacketHandler(Session).GroupJoin(new PJoinPacket
                    {
                        RequestType = GroupRequestType.Accepted,
                        CharacterId = targetSession.Character.CharacterId
                    });

                    break;
            }
        }

        #endregion
    }
}