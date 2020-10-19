using System;
using System.Linq;
using NosTale.Extension.Extension.Packet;
using NosTale.Packets.Packets.ServerPackets;
using OpenNos.Core;
using OpenNos.Domain;
using OpenNos.GameObject;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;

namespace OpenNos.Handler.PacketHandler.Npc
{
    public class BuyPacketHandler : IPacketHandler
    {
        #region Instantiation

        public BuyPacketHandler(ClientSession session)
        {
            Session = session;
        }

        #endregion

        #region Properties

        public ClientSession Session { get; }

        #endregion

        #region Methods

        public void BuyShop(BuyPacket buyPacket)
        {
            if (Session.Character.InExchangeOrTrade) return;

            var amount = buyPacket.Amount;

            switch (buyPacket.Type)
            {
                case BuyShopType.CharacterShop:
                    if (!Session.HasCurrentMapInstance) return;

                    var shop =
                        Session.CurrentMapInstance.UserShops.FirstOrDefault(mapshop =>
                            mapshop.Value.OwnerId.Equals(buyPacket.OwnerId));
                    var item = shop.Value?.Items.Find(i => i.ShopSlot.Equals(buyPacket.Slot));
                    var sess = ServerManager.Instance.GetSessionByCharacterId(shop.Value?.OwnerId ?? 0);
                    if (sess == null || item == null || amount <= 0 || amount > 999) return;

                    Logger.LogUserEvent("ITEM_BUY_PLAYERSHOP", Session.GenerateIdentity(),
                        $"From: {buyPacket.OwnerId} IIId: {item.ItemInstance.Id} ItemVNum: {item.ItemInstance.ItemVNum} Amount: {buyPacket.Amount} PricePer: {item.Price}");

                    if (amount > item.SellAmount) amount = item.SellAmount;

                    if (item.Price * amount
                        + sess.Character.Gold
                        > ServerManager.Instance.Configuration.MaxGold)
                    {
                        Session.SendPacket(UserInterfaceHelper.GenerateShopMemo(3,
                            Language.Instance.GetMessageFromKey("MAX_GOLD")));
                        return;
                    }

                    if (item.Price * amount >= Session.Character.Gold)
                    {
                        Session.SendPacket(UserInterfaceHelper.GenerateShopMemo(3,
                            Language.Instance.GetMessageFromKey("NOT_ENOUGH_MONEY")));
                        return;
                    }

                    // check if the item has been removed successfully from previous owner and remove it
                    if (Session.BuyValidate(Session, shop, buyPacket.Slot, amount))
                    {
                        Session.Character.Gold -= item.Price * amount;
                        Session.SendPacket(Session.Character.GenerateGold());

                        var shop2 =
                            Session.CurrentMapInstance.UserShops.FirstOrDefault(s =>
                                s.Value.OwnerId.Equals(buyPacket.OwnerId));
                        Session.LoadShopItem(buyPacket.OwnerId, shop2);
                    }
                    else
                    {
                        Session.SendPacket(
                            UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("NOT_ENOUGH_PLACE"),
                                0));
                    }

                    break;

                case BuyShopType.ItemShop:
                    if (!Session.HasCurrentMapInstance) return;

                    var npc =
                        Session.CurrentMapInstance.Npcs.Find(n => n.MapNpcId.Equals((int) buyPacket.OwnerId));
                    if (npc != null)
                    {
                        var dist = Map.GetDistance(
                            new MapCell {X = Session.Character.PositionX, Y = Session.Character.PositionY},
                            new MapCell {X = npc.MapX, Y = npc.MapY});
                        if (npc.Shop == null || dist > 5) return;

                        if (npc.Shop.ShopSkills.Count > 0)
                        {
                            if (!npc.Shop.ShopSkills.Exists(s => s.SkillVNum == buyPacket.Slot)) return;

                            // skill shop
                            if (Session.Character.UseSp)
                            {
                                Session.SendPacket(
                                    UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("REMOVE_SP"),
                                        0));
                                return;
                            }

                            if (Session.Character.Skills.Any(s =>
                                s.LastUse.AddMilliseconds(s.Skill.Cooldown * 100) > DateTime.Now))
                            {
                                Session.SendPacket(
                                    UserInterfaceHelper.GenerateMsg(
                                        Language.Instance.GetMessageFromKey("SKILL_NEED_COOLDOWN"), 0));
                                return;
                            }

                            var skillinfo = ServerManager.GetSkill(buyPacket.Slot);
                            if (Session.Character.Skills.Any(s => s.SkillVNum == buyPacket.Slot) ||
                                skillinfo == null) return;

                            Logger.LogUserEvent("SKILL_BUY", Session.GenerateIdentity(),
                                $"SkillVNum: {skillinfo.SkillVNum} Price: {skillinfo.Price}");

                            if (Session.Character.Gold < skillinfo.Price)
                            {
                                Session.SendPacket(
                                    UserInterfaceHelper.GenerateMsg(
                                        Language.Instance.GetMessageFromKey("NOT_ENOUGH_MONEY"), 0));
                            }
                            else if (Session.Character.GetCP() < skillinfo.CPCost)
                            {
                                Session.SendPacket(
                                    UserInterfaceHelper.GenerateMsg(
                                        Language.Instance.GetMessageFromKey("NOT_ENOUGH_CP"), 0));
                            }
                            else
                            {
                                if (skillinfo.SkillVNum < 200)
                                {
                                    var skillMiniumLevel = 0;
                                    if (skillinfo.MinimumSwordmanLevel == 0 && skillinfo.MinimumArcherLevel == 0
                                                                            && skillinfo.MinimumMagicianLevel == 0)
                                        skillMiniumLevel = skillinfo.MinimumAdventurerLevel;
                                    else
                                        switch (Session.Character.Class)
                                        {
                                            case ClassType.Adventurer:
                                                skillMiniumLevel = skillinfo.MinimumAdventurerLevel;
                                                break;

                                            case ClassType.Swordsman:
                                                skillMiniumLevel = skillinfo.MinimumSwordmanLevel;
                                                break;

                                            case ClassType.Archer:
                                                skillMiniumLevel = skillinfo.MinimumArcherLevel;
                                                break;

                                            case ClassType.Magician:
                                                if (skillinfo.MinimumMagicianLevel > 0)
                                                    skillMiniumLevel = skillinfo.MinimumMagicianLevel;
                                                else
                                                    skillMiniumLevel = skillinfo.MinimumAdventurerLevel;
                                                break;
                                        }

                                    if (skillMiniumLevel == 0)
                                    {
                                        Session.SendPacket(UserInterfaceHelper.GenerateMsg(
                                            Language.Instance.GetMessageFromKey("SKILL_CANT_LEARN"), 0));
                                        return;
                                    }

                                    if (Session.Character.Level < skillMiniumLevel)
                                    {
                                        Session.SendPacket(
                                            UserInterfaceHelper.GenerateMsg(
                                                Language.Instance.GetMessageFromKey("LOW_LVL"), 0));
                                        return;
                                    }

                                    foreach (var skill in Session.Character.Skills.GetAllItems())
                                        if (skillinfo.CastId == skill.Skill.CastId && skill.Skill.SkillVNum < 200)
                                            Session.Character.Skills.Remove(skill.SkillVNum);
                                }
                                else
                                {
                                    if ((byte) Session.Character.Class != skillinfo.Class)
                                    {
                                        Session.SendPacket(UserInterfaceHelper.GenerateMsg(
                                            Language.Instance.GetMessageFromKey("SKILL_CANT_LEARN"), 0));
                                        return;
                                    }

                                    if (Session.Character.JobLevel < skillinfo.LevelMinimum)
                                    {
                                        Session.SendPacket(
                                            UserInterfaceHelper.GenerateMsg(
                                                Language.Instance.GetMessageFromKey("LOW_JOB_LVL"), 0));
                                        return;
                                    }

                                    if (skillinfo.UpgradeSkill != 0)
                                    {
                                        var oldupgrade = Session.Character.Skills.FirstOrDefault(s =>
                                            s.Skill.UpgradeSkill == skillinfo.UpgradeSkill
                                            && s.Skill.UpgradeType == skillinfo.UpgradeType &&
                                            s.Skill.UpgradeSkill != 0);
                                        if (oldupgrade != null) Session.Character.Skills.Remove(oldupgrade.SkillVNum);
                                    }
                                }

                                Session.Character.Skills[buyPacket.Slot] = new CharacterSkill
                                {
                                    SkillVNum = buyPacket.Slot,
                                    CharacterId = Session.Character.CharacterId
                                };

                                Session.Character.Gold -= skillinfo.Price;
                                Session.SendPacket(Session.Character.GenerateGold());
                                Session.SendPacket(Session.Character.GenerateSki());
                                Session.SendPackets(Session.Character.GenerateQuicklist());
                                Session.SendPacket(
                                    UserInterfaceHelper.GenerateMsg(
                                        Language.Instance.GetMessageFromKey("SKILL_LEARNED"), 0));
                                Session.SendPacket(Session.Character.GenerateLev());
                                Session.SendPackets(Session.Character.GenerateStatChar());
                            }
                        }
                        else if (npc.Shop.ShopItems.Count > 0)
                        {
                            // npc shop
                            var shopItem = npc.Shop.ShopItems.Find(it => it.Slot == buyPacket.Slot);
                            if (shopItem == null || amount <= 0 || amount > 999) return;

                            var iteminfo = ServerManager.GetItem(shopItem.ItemVNum);
                            var price = iteminfo.Price * amount;
                            var reputprice = iteminfo.ReputPrice * amount;
                            double percent;
                            switch (Session.Character.GetDignityIco())
                            {
                                case 3:
                                    percent = 1.10;
                                    break;

                                case 4:
                                    percent = 1.20;
                                    break;

                                case 5:
                                case 6:
                                    percent = 1.5;
                                    break;

                                default:
                                    percent = 1;
                                    break;
                            }

                            Logger.LogUserEvent("ITEM_BUY_NPCSHOP", Session.GenerateIdentity(),
                                $"From: {npc.MapNpcId} ItemVNum: {iteminfo.VNum} Amount: {buyPacket.Amount} PricePer: {price * percent} ");

                            var rare = shopItem.Rare;
                            if (iteminfo.Type == 0) amount = 1;

                            if (iteminfo.ReputPrice == 0)
                            {
                                if (price < 0 || price * percent > Session.Character.Gold)
                                {
                                    Session.SendPacket(UserInterfaceHelper.GenerateShopMemo(3,
                                        Language.Instance.GetMessageFromKey("NOT_ENOUGH_MONEY")));
                                    return;
                                }
                            }
                            else
                            {
                                if (reputprice <= 0 || reputprice > Session.Character.Reputation)
                                {
                                    Session.SendPacket(UserInterfaceHelper.GenerateShopMemo(3,
                                        Language.Instance.GetMessageFromKey("NOT_ENOUGH_REPUT")));
                                    return;
                                }

                                var ra = (byte) ServerManager.RandomNumber();

                                if (iteminfo.ReputPrice != 0)
                                    for (var i = 0; i < ItemHelper.BuyCraftRareRate.Length; i++)
                                        if (ra <= ItemHelper.BuyCraftRareRate[i])
                                            rare = (sbyte) i;
                            }

                            var newItems = Session.Character.Inventory.AddNewToInventory(
                                shopItem.ItemVNum, amount, Rare: rare, Upgrade: shopItem.Upgrade,
                                Design: shopItem.Color);
                            if (newItems.Count == 0)
                            {
                                Session.SendPacket(UserInterfaceHelper.GenerateShopMemo(3,
                                    Language.Instance.GetMessageFromKey("NOT_ENOUGH_PLACE")));
                                return;
                            }

                            if (newItems.Count > 0)
                            {
                                foreach (var itemInst in newItems)
                                    switch (itemInst.Item.EquipmentSlot)
                                    {
                                        case EquipmentType.Armor:
                                        case EquipmentType.MainWeapon:
                                        case EquipmentType.SecondaryWeapon:
                                            itemInst.SetRarityPoint();
                                            if (iteminfo.ReputPrice > 0)
                                                itemInst.BoundCharacterId = Session.Character.CharacterId;
                                            break;

                                        case EquipmentType.Boots:
                                        case EquipmentType.Gloves:
                                            itemInst.FireResistance =
                                                (short) (itemInst.Item.FireResistance * shopItem.Upgrade);
                                            itemInst.DarkResistance =
                                                (short) (itemInst.Item.DarkResistance * shopItem.Upgrade);
                                            itemInst.LightResistance =
                                                (short) (itemInst.Item.LightResistance * shopItem.Upgrade);
                                            itemInst.WaterResistance =
                                                (short) (itemInst.Item.WaterResistance * shopItem.Upgrade);
                                            break;
                                    }

                                if (iteminfo.ReputPrice == 0)
                                {
                                    Session.SendPacket(UserInterfaceHelper.GenerateShopMemo(1,
                                        string.Format(Language.Instance.GetMessageFromKey("BUY_ITEM_VALID"),
                                            iteminfo.Name, amount)));
                                    Session.Character.Gold -= (long) (price * percent);
                                    Session.SendPacket(Session.Character.GenerateGold());
                                }
                                else
                                {
                                    Session.SendPacket(UserInterfaceHelper.GenerateShopMemo(1,
                                        string.Format(Language.Instance.GetMessageFromKey("BUY_ITEM_VALID"),
                                            iteminfo.Name, amount)));
                                    Session.Character.Reputation -= reputprice;
                                    Session.SendPacket(Session.Character.GenerateFd());
                                    Session.CurrentMapInstance?.Broadcast(Session,
                                        Session.Character.GenerateIn(InEffect: 1), ReceiverType.AllExceptMe);
                                    Session.CurrentMapInstance?.Broadcast(Session, Session.Character.GenerateGidx(),
                                        ReceiverType.AllExceptMe);
                                    Session.SendPacket(
                                        Session.Character.GenerateSay(
                                            string.Format(Language.Instance.GetMessageFromKey("REPUT_DECREASED"),
                                                reputprice), 11));
                                }
                            }
                            else
                            {
                                Session.SendPacket(
                                    UserInterfaceHelper.GenerateMsg(
                                        Language.Instance.GetMessageFromKey("NOT_ENOUGH_PLACE"), 0));
                            }
                        }
                    }

                    break;
            }
        }

        #endregion
    }
}