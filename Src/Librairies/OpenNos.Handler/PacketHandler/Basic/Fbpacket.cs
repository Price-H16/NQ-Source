//using NosTale.Packets.Packets.ClientPackets;
//using OpenNos.Core;
//using OpenNos.Domain;
//using OpenNos.GameObject;
//using OpenNos.GameObject.Helpers;
//using OpenNos.GameObject.Networking;
//using System.Linq;

//namespace OpenNos.Handler.PacketHandler.Basic
//{
//    public class FbPacket : IPacketHandler
//    {
//        #region Instantiation

//        public FbPacket(ClientSession session)
//        {
//            Session = session;
//        }

//        #endregion

//        #region Properties

//        public ClientSession Session { get; }

//        #endregion

//        #region Methods

//        public void RainbowBattleManage(FbPacket fbPacket)
//        {
//            Group grp;
//            switch (fbPacket.Type)
//            {
//                // Join BaTeam
//                case 1:
//                    if (Session.CurrentMapInstance.MapInstanceType == MapInstanceType.RainbowBattle)
//                    {
//                        return;
//                    }

//                    ClientSession target = ServerManager.Instance.GetSessionByCharacterId(fbPacket.CharacterId);
//                    if (fbPacket.Parameter == null && target?.Character?.Group == null
//                        && Session.Character.Group.IsLeader(Session))
//                    {
//                        GroupJoin(new PJoinPacket
//                        {
//                            RequestType = GroupRequestType.Invited,
//                            CharacterId = fbPacket.CharacterId
//                        });
//                    }
//                    else if (Session.Character.Group == null)
//                    {
//                        GroupJoin(new PJoinPacket
//                        {
//                            RequestType = GroupRequestType.Accepted,
//                            CharacterId = fbPacket.CharacterId
//                        });
//                    }

//                    break;

//                // Leave BaTeam
//                case 2:
//                    ClientSession sender = ServerManager.Instance.GetSessionByCharacterId(fbPacket.CharacterId);
//                    if (sender?.Character?.Group == null)
//                    {
//                        return;
//                    }

//                    Session.SendPacket(
//                        UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("LEFT_TEAM")),
//                            0));
//                    if (Session?.CurrentMapInstance?.MapInstanceType == MapInstanceType.RainbowBattleInstance)
//                    {
//                        ServerManager.Instance.ChangeMap(Session.Character.CharacterId, Session.Character.MapId,
//                            Session.Character.MapX, Session.Character.MapY);
//                    }

//                    grp = sender.Character?.Group;
//                    Session.SendPacket(Session.Character.GenerateFbt(1, true));
//                    Session.SendPacket(Session.Character.GenerateFbt(2, true));

//                    grp.Sessions.ForEach(s =>
//                    {
//                        s.SendPacket(grp.GenerateFblst());
//                        //s.SendPacket(grp.GeneraterRaidmbf(s));
//                        s.SendPacket(s.Character.GenerateFbt(0));
//                    });

//                    RainbowBattleMember rainbowbattleTeamMember = ServerManager.Instance.RainbowBattleMembers.Where(s => s.Session == Session).First();
//                    if (rainbowbattleTeamMember != null)
//                    {
//                        ServerManager.Instance.RainbowBattleMembers.Remove(rainbowbattleTeamMember);
//                    }


//                    RainbowBattleMember rainbowbattleTeamMemberRegistered = ServerManager.Instance.RainbowBattleMembersRegistered.Where(s => s.Session == Session).First();
//                    if (rainbowbattleTeamMemberRegistered != null)
//                    {
//                        ServerManager.Instance.RainbowBattleMembersRegistered.Remove(rainbowbattleTeamMember);
//                    }
//                    break;

//                // Kick from BaTeam
//                case 3:
//                    if (Session.CurrentMapInstance.MapInstanceType == MapInstanceType.RainbowBattle)
//                    {
//                        return;
//                    }

//                    if (Session.Character.Group?.IsLeader(Session) == true)
//                    {
//                        ClientSession chartokick = ServerManager.Instance.GetSessionByCharacterId(fbPacket.CharacterId);
//                        if (chartokick.Character?.Group == null)
//                        {
//                            return;
//                        }

//                        RainbowBattleMember rainbowbattleTeamMemberRegisteredd = ServerManager.Instance.RainbowBattleMembersRegistered.Where(s => s.Session == chartokick).First();
//                        if (rainbowbattleTeamMemberRegisteredd != null)
//                        {
//                            Session.SendPacket(
//                                UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("CANNOT_KICK_BA"), 0));
//                            return;
//                        }

//                        chartokick.SendPacket(
//                            UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("KICK_BA"), 0));
//                        grp = chartokick.Character?.Group;
//                        chartokick.SendPacket(chartokick.Character?.GenerateFbt(1, true));
//                        chartokick.SendPacket(chartokick.Character?.GenerateFbt(2, true));
//                        grp?.LeaveGroup(chartokick);
//                        grp?.Sessions.ForEach(s =>
//                        {
//                            s.SendPacket(grp.GenerateRdlst());
//                            s.SendPacket(s.Character.GenerateFbt(0));
//                        });
//                        RainbowBattleMember rainbowbattleTeamMemberr = ServerManager.Instance.RainbowBattleMembers.Where(s => s.Session == chartokick).First();
//                        if (rainbowbattleTeamMemberr != null)
//                        {
//                            ServerManager.Instance.RainbowBattleMembers.Remove(rainbowbattleTeamMemberr);
//                        }
//                    }

//                    break;

//                // Disolve BaTeam
//                case 4:
//                    if (Session.CurrentMapInstance.MapInstanceType == MapInstanceType.RainbowBattle)
//                    {
//                        return;
//                    }

//                    if (Session.Character.Group?.IsLeader(Session) == true)
//                    {
//                        RainbowBattleMember rainbowbattleTeamMemberRegistereddd = ServerManager.Instance.RainbowBattleMembersRegistered?.Where(s => s.Session == Session).First();
//                        if (rainbowbattleTeamMemberRegistereddd != null)
//                        {
//                            Session.SendPacket(
//                                UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("CANNOT_DISSOLVE_BA"), 0));
//                            return;
//                        }

//                        grp = Session.Character.Group;

//                        ClientSession[] grpmembers = new ClientSession[40];
//                        grp.Sessions.CopyTo(grpmembers);
//                        foreach (ClientSession targetSession in grpmembers)
//                        {
//                            if (targetSession != null)
//                            {
//                                targetSession.SendPacket(targetSession.Character.GenerateFbt(1, true));
//                                targetSession.SendPacket(targetSession.Character.GenerateFbt(2, true));
//                                targetSession.SendPacket(
//                                    UserInterfaceHelper.GenerateMsg(
//                                        Language.Instance.GetMessageFromKey("BA_DISOLVED"), 0));
//                                grp.LeaveGroup(targetSession);


//                                RainbowBattleMember rainbowbattleTeamMemberrr = ServerManager.Instance.RainbowBattleMembers.Where(s => s.Session == targetSession).First();
//                                if (rainbowbattleTeamMemberrr != null)
//                                {
//                                    ServerManager.Instance.RainbowBattleMembers.Remove(rainbowbattleTeamMemberrr);
//                                }
//                            }
//                        }

//                        ServerManager.Instance.GroupList.RemoveAll(s => s.GroupId == grp.GroupId);
//                        ServerManager.Instance.ThreadSafeGroupList.Remove(grp.GroupId);
//                    }

//                    break;
//            }
//        }

//        #endregion
//    }
//}