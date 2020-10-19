﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;
using OpenNos.Domain;

namespace NosTale.Packets.Packets.ClientPackets
{
    [PacketHeader("put")]
    public class PutPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)] public InventoryType InventoryType { get; set; }

        [PacketIndex(1)] public byte Slot { get; set; }

        [PacketIndex(2)] public short Amount { get; set; }

        #endregion
    }
}