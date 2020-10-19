﻿using NosTale.Packets.Packets.ClientPackets;
using OpenNos.Core;
using OpenNos.GameObject;

namespace OpenNos.GameObject._gameEvent
{
    public enum NotifiableEventType
    {
        INSTANT_BATTLE_STARTS_IN_5_MINUTES,
        INSTANT_BATTLE_STARTS_IN_1_MINUTE,

        ICEBREAKER_STARTS_IN_5_MINUTES,

        FAMILY_X_HAS_BEEN_CREATED_BY_Y,

        X_IS_LOOKING_FOR_RAID_MATES,
        X_TEAM_WON_THE_RAID_Y,
        X_TEAM_LOSE_THE_RAID_Y,

        ACT4_RAID_FIRE_STARTED_ANGEL,
        ACT4_RAID_WATER_STARTED_ANGEL,
        ACT4_RAID_LIGHT_STARTED_ANGEL,
        ACT4_RAID_HATUS_STARTED_ANGEL,
        ACT4_RAID_FIRE_STARTED_DEMON,
        ACT4_RAID_WATER_STARTED_DEMON,
        ACT4_RAID_LIGHT_STARTED_DEMON,
        ACT4_RAID_HATUS_STARTED_DEMON,
    }
}