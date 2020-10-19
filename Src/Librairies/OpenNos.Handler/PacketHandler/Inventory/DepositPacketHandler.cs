using System.Linq;
using NosTale.Packets.Packets.ClientPackets;
using OpenNos.Core;
using OpenNos.Domain;
using OpenNos.GameObject;
using OpenNos.GameObject.Helpers;

namespace OpenNos.Handler.PacketHandler.Inventory
{
    public class DepositPacketHandler : IPacketHandler
    {
        #region Instantiation

        public DepositPacketHandler(ClientSession session) => Session = session;

        #endregion

        #region Properties

        public ClientSession Session { get; }

        #endregion

        #region Methods

        public void Deposit(DepositPacket depositPacket)
        {
            if (depositPacket == null)
            {
                return;
            }

            if (depositPacket.Inventory != InventoryType.Equipment &&
                depositPacket.Inventory != InventoryType.Main &&
                depositPacket.Inventory != InventoryType.Etc)
            {
                return;
            }

            ItemInstance originalItem = Session.Character.Inventory.LoadBySlotAndType(depositPacket.Slot, depositPacket.Inventory);

            if (originalItem == null)
            {
                return;
            }

            if (originalItem.Item.ItemType == ItemType.Quest1 || originalItem.Item.ItemType == ItemType.Quest2)
            {
                return;
            }

            if (depositPacket.Amount > originalItem.Amount)
            {
                return;
            }

            ItemInstance anotherItem = Session.Character.Inventory.LoadBySlotAndType(depositPacket.NewSlot, depositPacket.PartnerBackpack ? InventoryType.PetWarehouse : InventoryType.Warehouse);

            if (anotherItem != null)
            {
                return;
            }

            // check if the destination slot is out of range
            if (depositPacket.NewSlot >= (depositPacket.PartnerBackpack ? 
                Session.Character.StaticBonusList.Any(s => s.StaticBonusType == StaticBonusType.PetBackPack) ? 50 : 0 : Session.Character.WareHouseSize))
            {
                return;
            }

            // check if the character is allowed to move the item
            if (Session.Character.InExchangeOrTrade)
            {
                return;
            }

            if (Session.Character.HasShopOpened)
            {
                return;
            }

            if (depositPacket.PartnerBackpack)
            {
                AddToPartnerBackpack(originalItem, depositPacket, ref anotherItem);
                return;
            }

            // actually move the item from source to destination
            Session.Character.Inventory.MoveItem(depositPacket.Inventory, InventoryType.Warehouse, depositPacket.Slot,
                depositPacket.Amount, depositPacket.NewSlot, out originalItem, out anotherItem);

            if (anotherItem == null)
            {
                return;
            }
            
            Session.SendPacket(UserInterfaceHelper.Instance.GenerateInventoryRemove(depositPacket.Inventory, depositPacket.Slot));
            Session.SendPacket(anotherItem.GenerateStash());
        }

        private void AddToPartnerBackpack(ItemInstance originalItem, DepositPacket packet, ref ItemInstance partnerItem)
        {
            Session.Character.Inventory.MoveItem(originalItem.Type, InventoryType.PetWarehouse, originalItem.Slot, 
                packet.Amount, packet.NewSlot, out originalItem, out partnerItem);

            if (partnerItem == null)
            {
                return;
            }
            
            Session.SendPacket(UserInterfaceHelper.Instance.GenerateInventoryRemove(packet.Inventory, packet.Slot));
            Session.SendPacket(partnerItem.GeneratePStash());
        }

        #endregion
    }
}