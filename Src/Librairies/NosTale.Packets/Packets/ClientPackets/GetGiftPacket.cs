﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;

namespace NosTale.Packets.Packets.ClientPackets
{
    [PacketHeader("pcl")]
    public class GetGiftPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)] public byte Type { get; set; }

        [PacketIndex(1)] public int GiftId { get; set; }

        #endregion
    }
}