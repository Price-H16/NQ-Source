using System;
using NosTale.Packets.Packets.ClientPackets;
using OpenNos.Core;
using OpenNos.DAL;
using OpenNos.Domain;
using OpenNos.GameObject;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;
using OpenNos.Master.Library.Client;
using OpenNos.Master.Library.Data;

namespace OpenNos.Handler.PacketHandler.Bazaar
{
    public class CBuyPacketHandler : IPacketHandler
    {
        #region Instantiation

        public CBuyPacketHandler(ClientSession session)
        {
            Session = session;
        }

        #endregion

        #region Properties

        public ClientSession Session { get; }

        #endregion

        #region Methods

        public void BuyBazaar(CBuyPacket cBuyPacket)
        {
            if (Session == null || Session.Character == null) return;
            
            if (Session.Account.IsLimited)
            {
                Session.SendPacket(
                    UserInterfaceHelper.GenerateInfo(Language.Instance.GetMessageFromKey("LIMITED_ACCOUNT")));
                return;
            }

            
            var bz = DAOFactory.BazaarItemDAO.LoadById(cBuyPacket.BazaarId);
            if (bz != null && cBuyPacket.Amount > 0)
            {
                var price = cBuyPacket.Amount * bz.Price;
                if (Session.Character.Gold >= price)
                {
                    var bzcree = new BazaarItemLink {BazaarItem = bz};
                    if (DAOFactory.CharacterDAO.LoadById(bz.SellerId) != null)
                    {
                        bzcree.Owner = DAOFactory.CharacterDAO.LoadById(bz.SellerId)?.Name;
                        bzcree.Item = new ItemInstance(DAOFactory.ItemInstanceDAO.LoadById(bz.ItemInstanceId));
                    }
                    else
                    {
                        return;
                    }

                    if (cBuyPacket.Amount <= bzcree.Item.Amount)
                    {
                        if (!Session.Character.Inventory.CanAddItem(bzcree.Item.ItemVNum))
                        {
                            Session.SendPacket(
                                UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("NOT_ENOUGH_PLACE"),
                                    0));
                            return;
                        }
                        
                        if (Session.Character.LastBazaarBuy.AddSeconds(5) > DateTime.Now) return;

                        if (bzcree.Item != null)
                        {
                            if (bz.IsPackage && cBuyPacket.Amount != bz.Amount) return;

                            var bzitemdto =
                                DAOFactory.ItemInstanceDAO.LoadById(bzcree.BazaarItem.ItemInstanceId);
                            if (bzitemdto.Amount < cBuyPacket.Amount) return;

                            // Edit this soo we dont generate new guid every single time we take
                            // something out.
                            var newBz = bzcree.Item.DeepCopy();
                            newBz.Id = Guid.NewGuid();
                            newBz.Amount = cBuyPacket.Amount;
                            newBz.Type = newBz.Item.Type;
                            var newInv = Session.Character.Inventory.AddToInventory(newBz);

                            if (newInv.Count > 0)
                            {
                                bzitemdto.Amount -= cBuyPacket.Amount;
                                Session.Character.Gold -= price;
                                Session.SendPacket(Session.Character.GenerateGold());
                                DAOFactory.ItemInstanceDAO.InsertOrUpdate(bzitemdto);
                                ServerManager.Instance.BazaarRefresh(bzcree.BazaarItem.BazaarItemId);
                                Session.SendPacket(
                                    $"rc_buy 1 {bzcree.Item.Item.VNum} {bzcree.Owner} {cBuyPacket.Amount} {cBuyPacket.Price} 0 0 0");

                                Session.SendPacket(Session.Character.GenerateSay(
                                    $"{Language.Instance.GetMessageFromKey("ITEM_ACQUIRED")}: {bzcree.Item.Item.Name} x {cBuyPacket.Amount}",
                                    10));

                                CommunicationServiceClient.Instance.SendMessageToCharacter(new SCSCharacterMessage
                                {
                                    DestinationCharacterId = bz.SellerId,
                                    SourceWorldId = ServerManager.Instance.WorldId,
                                    Message = StaticPacketHelper.Say(1, bz.SellerId, 12,
                                        string.Format(Language.Instance.GetMessageFromKey("BAZAAR_ITEM_SOLD"),
                                            cBuyPacket.Amount, bzcree.Item.Item.Name)),
                                    Type = MessageType.Other
                                });
                                
                                Session.Character.LastBazaarBuy = DateTime.Now;
                                
                                Logger.LogUserEvent("BAZAAR_BUY", Session.GenerateIdentity(),
                                    $"BazaarId: {cBuyPacket.BazaarId} VNum: {cBuyPacket.VNum} Amount: {cBuyPacket.Amount} Price: {cBuyPacket.Price}");
                            }
                        }
                    }
                    else
                    {
                        Session.SendPacket(
                            UserInterfaceHelper.GenerateModal(Language.Instance.GetMessageFromKey("STATE_CHANGED"), 1));
                    }
                }
                else
                {
                    Session.SendPacket(
                        Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("NOT_ENOUGH_MONEY"), 10));
                    Session.SendPacket(
                        UserInterfaceHelper.GenerateModal(Language.Instance.GetMessageFromKey("NOT_ENOUGH_MONEY"), 1));
                }
            }
            else
            {
                Session.SendPacket(
                    UserInterfaceHelper.GenerateModal(Language.Instance.GetMessageFromKey("STATE_CHANGED"), 1));
            }
        }

        #endregion
    }
}