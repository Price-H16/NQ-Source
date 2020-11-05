using System;
using System.Collections.Concurrent;
using System.Linq;
using NosTale.Packets.Packets.ClientPackets;
using OpenNos.Core;
using OpenNos.Data;
using OpenNos.Domain;
using OpenNos.GameObject;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;

namespace OpenNos.Handler.PacketHandler.Basic
{
    public class SayPacketHandler : IPacketHandler
    {
        #region Instantiation

        public SayPacketHandler(ClientSession session) => Session = session;

        #endregion

        #region Properties

        public ClientSession Session { get; }

        #endregion

        #region Methods

        public void Say(SayPacket sayPacket)
        {
            if (string.IsNullOrEmpty(sayPacket.Message))
            {
                return;
            }

            var penalty = Session.Account.PenaltyLogs.OrderByDescending(s => s.DateEnd).FirstOrDefault();
            var message = sayPacket.Message;
            if (Session.Character.IsMuted() && penalty != null)
            {
                if (Session.Character.Gender == GenderType.Female)
                {
                    var member = ServerManager.Instance.ArenaTeams.ToList()
                        .FirstOrDefault(s => s.Any(e => e.Session == Session));
                    if (Session.CurrentMapInstance.MapInstanceType == MapInstanceType.TalentArenaMapInstance &&
                        member != null)
                    {
                        var member2 = member.FirstOrDefault(o => o.Session == Session);
                        member.Replace(s => member2 != null && s.ArenaTeamType == member2.ArenaTeamType && s != member2)
                            .Replace(s =>
                                s.ArenaTeamType == member.FirstOrDefault(o => o.Session == Session)?.ArenaTeamType)
                            .ToList().ForEach(o =>
                                o.Session.SendPacket(
                                    Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("MUTED_FEMALE"),
                                        1)));
                    }
                    else
                    {
                        Session.CurrentMapInstance?.Broadcast(Session,
                            Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("MUTED_FEMALE"), 1));
                    }

                    Session.SendPacket(Session.Character.GenerateSay(
                        string.Format(Language.Instance.GetMessageFromKey("MUTE_TIME"),
                            (penalty.DateEnd - DateTime.Now).ToString(@"hh\:mm\:ss")), 11));
                    Session.SendPacket(Session.Character.GenerateSay(
                        string.Format(Language.Instance.GetMessageFromKey("MUTE_TIME"),
                            (penalty.DateEnd - DateTime.Now).ToString(@"hh\:mm\:ss")), 12));
                }
                else
                {
                    var member = ServerManager.Instance.ArenaTeams.ToList()
                        .FirstOrDefault(s => s.Any(e => e.Session == Session));
                    if (Session.CurrentMapInstance.MapInstanceType == MapInstanceType.TalentArenaMapInstance &&
                        member != null)
                    {
                        var member2 = member.FirstOrDefault(o => o.Session == Session);
                        member.Replace(s => member2 != null && s.ArenaTeamType == member2.ArenaTeamType && s != member2)
                            .Replace(s =>
                                s.ArenaTeamType == member.FirstOrDefault(o => o.Session == Session)?.ArenaTeamType)
                            .ToList().ForEach(o =>
                                o.Session.SendPacket(
                                    Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("MUTED_MALE"),
                                        1)));
                    }
                    else
                    {
                        Session.CurrentMapInstance?.Broadcast(Session,
                            Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("MUTED_MALE"), 1));
                    }

                    Session.SendPacket(Session.Character.GenerateSay(
                        string.Format(Language.Instance.GetMessageFromKey("MUTE_TIME"),
                            (penalty.DateEnd - DateTime.Now).ToString(@"hh\:mm\:ss")), 11));
                    Session.SendPacket(Session.Character.GenerateSay(
                        string.Format(Language.Instance.GetMessageFromKey("MUTE_TIME"),
                            (penalty.DateEnd - DateTime.Now).ToString(@"hh\:mm\:ss")), 12));
                }
            }
            else
            {
                ServerManager.Instance.ChatLogs.Add(new ChatLogDTO
                {
                    AccountId = (int)Session.Account.AccountId,
                    CharacterId = (int)Session.Character.CharacterId,
                    CharacterName = Session.Character.Name,
                    DateTime = DateTime.Now,
                    Message = message,
                    Type = DialogType.Normal
                });

                var type = CharacterHelper.AuthorityChatColor(Session.Character.Authority);

                ConcurrentBag<ArenaTeamMember> member = null;
                lock (ServerManager.Instance.ArenaTeams)
                {
                    member = ServerManager.Instance.ArenaTeams.ToList()
                        .FirstOrDefault(s => s.Any(e => e.Session == Session));
                }

                var rbb = ServerManager.Instance.RainbowBattleMembers.Find(s => s.Session.Contains(Session));

                type = CharacterHelper.AuthorityChatColor(Session.Character.Authority);

                Session.CurrentMapInstance?.Broadcast(Session, Session.Character.GenerateSay(message.Trim(), 1), ReceiverType.AllExceptMe);
                if (Session.CurrentMapInstance.MapInstanceType == MapInstanceType.RainbowBattleInstance && rbb != null)
                {
                    foreach (var ses in rbb.Session)
                    {
                        if (ses == Session)
                        {
                            continue;
                        }
                        ses.SendPacket(Session.Character.GenerateSay(message.Trim(), type, Session.Account.Authority >= AuthorityType.User));
                    }
                }
                if (Session.Character.Authority >= AuthorityType.Supporter)
                {
                    type = CharacterHelper.AuthorityChatColor(Session.Character.Authority);
                    if (Session.CurrentMapInstance.MapInstanceType == MapInstanceType.TalentArenaMapInstance &&
                        member != null)
                    {
                        var member2 = member.FirstOrDefault(o => o.Session == Session);
                        member.Replace(s => member2 != null && s.ArenaTeamType == member2.ArenaTeamType && s != member2)
                            .Replace(s =>
                                s.ArenaTeamType == member.FirstOrDefault(o => o.Session == Session)?.ArenaTeamType)
                            .ToList().ForEach(o =>
                                o.Session.SendPacket(Session.Character.GenerateSay(message.Trim(), 1)));
                    }
                    else
                    {
                        Session.CurrentMapInstance?.Broadcast(Session, Session.Character.GenerateSay(message.Trim(), 1),
                            ReceiverType.AllExceptMe);
                    }

                    message = $"[{Session.Character.Authority} {Session.Character.Name}]: " + message;
                }

                if (Session.CurrentMapInstance.MapInstanceType == MapInstanceType.TalentArenaMapInstance &&
                    member != null)
                {
                    var member2 = member.FirstOrDefault(o => o.Session == Session);
                    member.Where(s => s.ArenaTeamType == member2?.ArenaTeamType && s != member2).ToList().ForEach(o =>
                        o.Session.SendPacket(Session.Character.GenerateSay(message.Trim(), type,
                            Session.Account.Authority >= AuthorityType.Supporter)));
                }
                else if (ServerManager.Instance.ChannelId == 51 && Session.Account.Authority < AuthorityType.DSGM)
                {
                    Session.CurrentMapInstance?.Broadcast(Session, Session.Character.GenerateSay(message.Trim(), type),
                        ReceiverType.AllExceptMeAct4);
                }
                else
                {
                    Session.CurrentMapInstance?.Broadcast(Session,
                        Session.Character.GenerateSay(message.Trim(), type,
                            Session.Character.Authority >= AuthorityType.Supporter), ReceiverType.AllExceptMe);
                }
            }
        }

        #endregion
    }
}