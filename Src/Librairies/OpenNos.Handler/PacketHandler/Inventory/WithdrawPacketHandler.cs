using System;
using NosTale.Packets.Packets.ClientPackets;
using OpenNos.Core;
using OpenNos.Domain;
using OpenNos.GameObject;
using OpenNos.GameObject.Helpers;

namespace OpenNos.Handler.PacketHandler.Inventory
{
    public class WithdrawPacketHandler : IPacketHandler
    {
        #region Instantiation

        public WithdrawPacketHandler(ClientSession session)
        {
            Session = session;
        }

        #endregion

        #region Properties

        public ClientSession Session { get; }

        #endregion

        #region Methods

        public void Withdraw(WithdrawPacket withdrawPacket)
        {
            if (withdrawPacket == null)
            {
                return;
            }
            
            if (withdrawPacket.Amount < 1)
            {
                return;
            }
            
            if (Session.Character.InExchangeOrTrade)
            {
                return;
            }

            if (Session.Character.HasShopOpened)
            {
                return;
            }

            ItemInstance previousInventory = Session.Character.Inventory.LoadBySlotAndType(withdrawPacket.Slot, withdrawPacket.PetBackpack ? InventoryType.PetWarehouse : InventoryType.Warehouse);

            if (previousInventory == null)
            {
                return;
            }

            if (withdrawPacket.Amount > previousInventory.Amount)
            {
                return;
            }

            if (!Session.Character.Inventory.CanAddItem(previousInventory.ItemVNum))
            {
                return;
            }

            var item2 = previousInventory.DeepCopy();
            item2.Id = Guid.NewGuid();
            item2.Amount = withdrawPacket.Amount;
            
            Logger.LogUserEvent("STASH_WITHDRAW", Session.GenerateIdentity(),
                $"[Withdraw]OldIIId: {previousInventory.Id} NewIIId: {item2.Id} Amount: {withdrawPacket.Amount} PartnerBackpack: {withdrawPacket.PetBackpack}");

            Session.Character.Inventory.RemoveItemFromInventory(previousInventory.Id, withdrawPacket.Amount);
            
            Session.Character.Inventory.AddToInventory(item2, item2.Item.Type);
            Session.Character.Inventory.LoadBySlotAndType(withdrawPacket.Slot, withdrawPacket.PetBackpack ? InventoryType.PetWarehouse : InventoryType.Warehouse);

            if (previousInventory.Amount > 0)
            {
                Session.SendPacket(withdrawPacket.PetBackpack
                    ? previousInventory.GeneratePStash()
                    : previousInventory.GenerateStash());
            }
            else
            {
                Session.SendPacket(withdrawPacket.PetBackpack
                    ? UserInterfaceHelper.Instance.GeneratePStashRemove(withdrawPacket.Slot)
                    : UserInterfaceHelper.Instance.GenerateStashRemove(withdrawPacket.Slot));
            }
        }

        #endregion
    }
}