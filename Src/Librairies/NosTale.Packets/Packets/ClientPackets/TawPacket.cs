﻿//// <auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;

namespace NosTale.Packets.Packets.ClientPackets
{
    [PacketHeader("taw")]
    public class TawPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)] public string Username { get; set; }

        #endregion
    }
}