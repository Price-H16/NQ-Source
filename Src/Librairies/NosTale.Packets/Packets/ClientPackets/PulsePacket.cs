﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;

namespace NosTale.Packets.Packets.ClientPackets
{
    [PacketHeader("pulse")]
    public class PulsePacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)] public int Tick { get; set; }

        #endregion
    }
}