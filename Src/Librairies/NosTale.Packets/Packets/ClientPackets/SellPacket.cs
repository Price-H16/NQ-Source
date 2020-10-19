﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;

namespace NosTale.Packets.Packets.ClientPackets
{
    [PacketHeader("sell")]
    public class SellPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(2)] public short Data { get; set; }

        [PacketIndex(3)] public byte? Slot { get; set; }

        [PacketIndex(4)] public short? Amount { get; set; }

        #endregion
    }
}