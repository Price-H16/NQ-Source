﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;

namespace NosTale.Packets.Packets.ClientPackets
{
    [PacketHeader("gjoin")]
    public class JoinFamilyPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)] public byte Type { get; set; }

        [PacketIndex(1)] public long CharacterId { get; set; }

        #endregion
    }
}