﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;

namespace NosTale.Packets.Packets.ClientPackets
{
    [PacketHeader("c_slist")]
    public class CSListPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)] public byte Index { get; set; }


        [PacketIndex(1)] public byte Filter { get; set; }

        #endregion
    }
}