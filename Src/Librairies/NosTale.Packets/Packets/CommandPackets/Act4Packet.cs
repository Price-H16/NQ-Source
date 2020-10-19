﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;
using OpenNos.Domain;

namespace NosTale.Packets.Packets.CommandPackets
{
    [PacketHeader("$Act4", PassNonParseablePacket = true,
        Authorities = new[] {AuthorityType.DSGM, AuthorityType.Administrator})]
    public class Act4Packet : PacketDefinition
    {
        #region Properties

        public static string ReturnHelp() => "$Act4";

        #endregion
    }
}