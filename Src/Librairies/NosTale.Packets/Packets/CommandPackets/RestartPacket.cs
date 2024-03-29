﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;
using OpenNos.Domain;

namespace NosTale.Packets.Packets.CommandPackets
{
    [PacketHeader("$Restart", PassNonParseablePacket = true, Authorities = new[] {AuthorityType.Administrator})]
    public class RestartPacket : PacketDefinition
    {
        [PacketIndex(0)] public int Time { get; set; }

        public static string ReturnHelp() => "$Restart <?Delay>";
    }
}