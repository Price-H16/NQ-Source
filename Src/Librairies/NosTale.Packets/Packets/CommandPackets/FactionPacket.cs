﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;
using OpenNos.Domain;

namespace NosTale.Packets.Packets.CommandPackets
{
    [PacketHeader("$Faction", PassNonParseablePacket = true, Authorities = new[] {AuthorityType.Administrator})]
    public class FactionPacket : PacketDefinition
    {
        public static string ReturnHelp() => "$Faction";
    }
}