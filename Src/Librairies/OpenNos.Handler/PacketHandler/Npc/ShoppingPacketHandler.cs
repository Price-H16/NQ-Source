﻿using System.Linq;
using NosTale.Packets.Packets.ClientPackets;
using OpenNos.Core;
using OpenNos.Data;
using OpenNos.Domain;
using OpenNos.GameObject;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;

namespace OpenNos.Handler.PacketHandler.Npc
{
    public class ShoppingPacketHandler : IPacketHandler
    {
        #region Instantiation

        public ShoppingPacketHandler(ClientSession session)
        {
            Session = session;
        }

        #endregion

        #region Properties

        public ClientSession Session { get; }

        #endregion

        #region Methods

        /// <summary>
        /// shopping packet
        /// </summary>
        /// <param name="shoppingPacket"></param>
        public void Shopping(ShoppingPacket shoppingPacket)
        {
            byte type = shoppingPacket.Type, typeshop = 0;
            int npcId = shoppingPacket.NpcId;
            if (Session.Character.IsShopping || !Session.HasCurrentMapInstance)
            {
                return;
            }

            MapNpc mapnpc = Session.CurrentMapInstance.Npcs.Find(n => n.MapNpcId.Equals(npcId));
            if (mapnpc?.Shop == null)
            {
                return;
            }

            string shoplist = "";
            foreach (ShopItemDTO item in mapnpc.Shop.ShopItems.Where(s => s.Type.Equals(type)))
            {
                Item iteminfo = ServerManager.GetItem(item.ItemVNum);
                typeshop = 100;
                double percent = 1;
                switch (Session.Character.GetDignityIco())
                {
                    case 3:
                        percent = 1.1;
                        typeshop = 110;
                        break;

                    case 4:
                        percent = 1.2;
                        typeshop = 120;
                        break;

                    case 5:
                        percent = 1.5;
                        typeshop = 150;
                        break;

                    case 6:
                        percent = 1.5;
                        typeshop = 150;
                        break;

                    default:
                        if (Session.CurrentMapInstance.Map.MapTypes.Any(s => s.MapTypeId == (short)MapTypeEnum.Act4))
                        {
                            percent *= 1.5;
                            typeshop = 150;
                        }

                        break;
                }

                if (Session.CurrentMapInstance.Map.MapTypes.Any(s =>
                    s.MapTypeId == (short)MapTypeEnum.Act4 && Session.Character.GetDignityIco() == 3))
                {
                    percent = 1.6;
                    typeshop = 160;
                }
                else if (Session.CurrentMapInstance.Map.MapTypes.Any(s =>
                    s.MapTypeId == (short)MapTypeEnum.Act4 && Session.Character.GetDignityIco() == 4))
                {
                    percent = 1.7;
                    typeshop = 170;
                }
                else if (Session.CurrentMapInstance.Map.MapTypes.Any(s =>
                    s.MapTypeId == (short)MapTypeEnum.Act4 && Session.Character.GetDignityIco() == 5))
                {
                    percent = 2;
                    typeshop = 200;
                }
                else if
                (Session.CurrentMapInstance.Map.MapTypes.Any(s =>
                    s.MapTypeId == (short)MapTypeEnum.Act4 && Session.Character.GetDignityIco() == 6))
                {
                    percent = 2;
                    typeshop = 200;
                }
                //EDIT THIS #FAMILYEXTENSIONS
                if (mapnpc.Shop.ShopType == 48)
                {
                    var mv = FamilySystemHelper.GetMissValues(item.ItemVNum);
                    if (mv == null) continue;

                    shoplist += $" {item.ItemVNum}|{(mv[1] > Session.Character.Family?.FamilyLevel ? 2 : Session.Character.Family?.FamilySkillMissions.Any(s => s.ItemVNum == item.ItemVNum) ?? false ? 1 : 0)}|{item.Slot}";
                }
                else if (iteminfo.ReputPrice > 0 && iteminfo.Type == 0)
                {
                    shoplist +=
                        $" {(byte)iteminfo.Type}.{item.Slot}.{item.ItemVNum}.{item.Rare}.{(iteminfo.IsColored ? item.Color : item.Upgrade)}.{iteminfo.ReputPrice}";
                }
                else if (iteminfo.ReputPrice > 0 && iteminfo.Type != 0)
                {
                    shoplist += $" {(byte)iteminfo.Type}.{item.Slot}.{item.ItemVNum}.-1.{iteminfo.ReputPrice}";
                }
                else if (iteminfo.Type != 0)
                {
                    shoplist += $" {(byte)iteminfo.Type}.{item.Slot}.{item.ItemVNum}.-1.{iteminfo.Price * percent}";
                }
                else
                {
                    shoplist +=
                        $" {(byte)iteminfo.Type}.{item.Slot}.{item.ItemVNum}.{item.Rare}.{(iteminfo.IsColored ? item.Color : item.Upgrade)}.{iteminfo.Price * percent}";
                }
            }

            foreach (ShopSkillDTO skill in mapnpc.Shop.ShopSkills.Where(s => s.Type.Equals(type)))
            {
                Skill skillinfo = ServerManager.GetSkill(skill.SkillVNum);

                if (skill.Type != 0)
                {
                    typeshop = 1;
                    if (skillinfo.Class == (byte)Session.Character.Class)
                    {
                        shoplist += $" {skillinfo.SkillVNum}";
                    }
                }
                else
                {
                    shoplist += $" {skillinfo.SkillVNum}";
                }
            }
            if (mapnpc.Shop.ShopType == 48)
            {
                Session.SendPacket($"n_inv 2 {mapnpc.MapNpcId} 0 {shoplist}");
            }
            else
                Session.SendPacket($"n_inv 2 {mapnpc.MapNpcId} 0 {typeshop}{shoplist}");
        }

        #endregion
    }
}