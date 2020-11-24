using System.Collections.Generic;
using Discord;
using OpenNos.Domain;

namespace Plugins.DiscordWebhook
{
     public class DiscordWebhookNotifierConfig
    {
        public Dictionary<NotifiableEventType, string> Messages = new Dictionary<NotifiableEventType, string>
        {
            {
                NotifiableEventType.ACT4_RAID_FIRE_STARTED_ANGEL, @"/!\ {0} raid has started for {1} ! /!\"
            },
            {
                NotifiableEventType.ACT4_RAID_WATER_STARTED_DEMON, @"/!\ {0} raid has started for {1}! /!\"
            },
            { 
                NotifiableEventType.FAMILY_X_HAS_BEEN_CREATED_BY_Y, "Family {0} has been created  by {1}!" 
            },
            {
                NotifiableEventType.X_IS_LOOKING_FOR_RAID_MATES, "Player {0} is looking members for the Raid {1}."
            },
            {
                NotifiableEventType.X_TEAM_WON_THE_RAID_Y, "{0}'s team completed succesfully the raid {1}."
            },
            {
                NotifiableEventType.ICEBREAKER_STARTS_IN_5_MINUTES, "Icebreaker will begin in 5 minute/s!."
            },
            {
                NotifiableEventType.INSTANT_BATTLE_STARTS_IN_5_MINUTES, "The Instant Combat will start in {0} minutes."
            },
            {
                NotifiableEventType.INSTANT_BATTLE_STARTS_IN_1_MINUTE, "The Instant Combat will start in {0} minute."
            },
            {
                NotifiableEventType.CHANNEL_ONLINE, "New Channel {0} has been Started!"
            },
        };

        public Dictionary<NotifiableEventType, string> Thumbnails = new Dictionary<NotifiableEventType, string>
        {
            { NotifiableEventType.ACT4_RAID_FIRE_STARTED_ANGEL, "https://cdn.discordapp.com/attachments/605815407093481472/607209589330673694/alewe.png" },
            { NotifiableEventType.ACT4_RAID_WATER_STARTED_DEMON, "https://cdn.discordapp.com/attachments/605815407093481472/607209587925450788/dlewe1.png" },
            { NotifiableEventType.FAMILY_X_HAS_BEEN_CREATED_BY_Y, "https://i.ytimg.com/vi/5sRaVQuhYY4/maxresdefault.jpg" }
        };

        public Dictionary<NotifiableEventType, string> Images = new Dictionary<NotifiableEventType, string>
        {
            { NotifiableEventType.ACT4_RAID_FIRE_STARTED_ANGEL, "https://cdn.discordapp.com/attachments/605815407093481472/607205615143485441/unknown.png" },
            { NotifiableEventType.ACT4_RAID_WATER_STARTED_DEMON, "https://cdn.discordapp.com/attachments/605815407093481472/607212101622038528/calvinas.png" },
            { NotifiableEventType.FAMILY_X_HAS_BEEN_CREATED_BY_Y, "https://i.ytimg.com/vi/5sRaVQuhYY4/maxresdefault.jpg" }
        };
        
        public Dictionary<NotifiableEventType, string> Title = new Dictionary<NotifiableEventType, string>
        {
            { NotifiableEventType.ACT4_RAID_FIRE_STARTED_ANGEL, "ACT 4 RAID Started" },
            { NotifiableEventType.ACT4_RAID_WATER_STARTED_DEMON, "ACT 4 RAID Started" },
            { NotifiableEventType.FAMILY_X_HAS_BEEN_CREATED_BY_Y, "A Family As Been Created" },
            { NotifiableEventType.CHANNEL_ONLINE, "Channel Online" },
            { NotifiableEventType.X_IS_LOOKING_FOR_RAID_MATES, "Someone Is Looking For You !" },
            { NotifiableEventType.X_TEAM_WON_THE_RAID_Y, "Raid Won" },
            { NotifiableEventType.ICEBREAKER_STARTS_IN_5_MINUTES, "IceBreaker" },
            { NotifiableEventType.INSTANT_BATTLE_STARTS_IN_5_MINUTES, "InstantBattle" },
            { NotifiableEventType.INSTANT_BATTLE_STARTS_IN_1_MINUTE, "InstanBattle" },
        };

        public string IconUrl { get; set; } = "https://cdn.discordapp.com/attachments/605815407093481472/607205615143485441/unknown.png";

        public Dictionary<NotifiableEventType, Color> Colors { get; set; } = new Dictionary<NotifiableEventType, Color>
        {
            { NotifiableEventType.ACT4_RAID_FIRE_STARTED_ANGEL, new Color(0xdd5522) },
            { NotifiableEventType.ACT4_RAID_FIRE_STARTED_DEMON, new Color(0xdd5522) },
            { NotifiableEventType.ACT4_RAID_WATER_STARTED_ANGEL, new Color(0x33aadd) },
            { NotifiableEventType.ACT4_RAID_WATER_STARTED_DEMON, new Color(0x33aadd) },
            { NotifiableEventType.FAMILY_X_HAS_BEEN_CREATED_BY_Y, new Color(0x33aadd) },
        };
    }
}