﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;

namespace NosTale.Packets.Packets.ClientPackets
{
    [PacketHeader("fdel")]
    public class FDelPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)] public long CharacterId { get; set; }

        #endregion
    }
}