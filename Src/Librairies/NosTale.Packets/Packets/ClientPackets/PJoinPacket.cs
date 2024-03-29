﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;
using OpenNos.Domain;

namespace NosTale.Packets.Packets.ClientPackets
{
    [PacketHeader("pjoin")]
    public class PJoinPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)] public GroupRequestType RequestType { get; set; }

        [PacketIndex(1)] public long CharacterId { get; set; }

        #endregion
    }
}