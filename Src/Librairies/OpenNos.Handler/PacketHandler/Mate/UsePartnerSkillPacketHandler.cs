using NosTale.Extension.Extension.Packet;
using NosTale.Packets.Packets.ClientPackets;
using OpenNos.Core;
using OpenNos.Domain;
using OpenNos.GameObject;
using OpenNos.GameObject.Battle;
using OpenNos.GameObject.Helpers;

namespace OpenNos.Handler.PacketHandler.Mate
{
    public class UsePartnerSkillPacketHandler : IPacketHandler
    {
        #region Instantiation

        public UsePartnerSkillPacketHandler(ClientSession session)
        {
            Session = session;
        }

        #endregion

        #region Properties

        public ClientSession Session { get; }

        #endregion

        #region Methods

        public void UseSkill(UsePartnerSkillPacket usePartnerSkillPacket)
        {
            #region Invalid packet

            if (usePartnerSkillPacket == null) return;

            if (Session.Character.MapInstance == null) return;

            #endregion

            #region Mate not found (or invalid)

            var mate = Session.Character.Mates.Find(x => x.MateTransportId == usePartnerSkillPacket.TransportId
                                                         && x.MateType == MateType.Partner && x.IsTeamMember);
            if (mate == null) return;

            #endregion

            #region Not using PSP

            if (mate.Sp == null || !mate.IsUsingSp) return;

            #endregion

            #region Skill not found

            var partnerSkill = mate.Sp.GetSkill(usePartnerSkillPacket.CastId);

            if (partnerSkill == null) return;

            #endregion

            #region Convert PartnerSkill to Skill

            var skill = PartnerSkillHelper.ConvertToNormalSkill(partnerSkill);

            #endregion

            #region Battle entities

            // Re-constructing entity to avoid infinite damage bugs.
            var battleEntityAttacker = new BattleEntity(mate);
            BattleEntity battleEntityDefender = null;

            // Same with defense entity
            switch (usePartnerSkillPacket.TargetType)
            {
                case UserType.Player:
                    {
                        var target = Session.Character.MapInstance?.GetCharacterById(usePartnerSkillPacket.TargetId);
                        battleEntityDefender = new BattleEntity(target, null);
                    }
                    break;

                case UserType.Npc:
                    {
                        var target = Session.Character.MapInstance?.GetMate(usePartnerSkillPacket.TargetId);
                        battleEntityDefender = new BattleEntity(target);
                    }
                    break;

                case UserType.Monster:
                    {
                        var target = Session.Character.MapInstance?.GetMonsterById(usePartnerSkillPacket.TargetId);
                        battleEntityDefender = new BattleEntity(target);
                    }
                    break;
            }

            #endregion

            #region Attack

            battleEntityAttacker.PartnerSkillTargetHit(battleEntityDefender, skill);

            #endregion
        }

        #endregion
    }
}