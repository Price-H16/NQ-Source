// WingsEmu
// 
// Developed by NosWings Team
//Thanks. Price
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using OpenNos.Domain;

namespace OpenNos.GameObject.Configuration
{
    [DataContract]
    public class GameScheduledEventsConfiguration
    {
        [DataMember(EmitDefaultValue = true, IsRequired = true)]
        public List<Schedule> ScheduledEvents { get; set; } = new List<Schedule>
        {
            new Schedule { Event = EventType.INSTANTBATTLE, Time = TimeSpan.Parse("01:55") },
            new Schedule { Event = EventType.INSTANTBATTLE, Time = TimeSpan.Parse("05:55") },
            new Schedule { Event = EventType.INSTANTBATTLE, Time = TimeSpan.Parse("07:55") },
            new Schedule { Event = EventType.INSTANTBATTLE, Time = TimeSpan.Parse("09:55") },
            new Schedule { Event = EventType.INSTANTBATTLE, Time = TimeSpan.Parse("11:55") },
            new Schedule { Event = EventType.INSTANTBATTLE, Time = TimeSpan.Parse("13:55") },
            new Schedule { Event = EventType.INSTANTBATTLE, Time = TimeSpan.Parse("15:55") },
            new Schedule { Event = EventType.INSTANTBATTLE, Time = TimeSpan.Parse("17:55") },
            new Schedule { Event = EventType.INSTANTBATTLE, Time = TimeSpan.Parse("17:55") },
            new Schedule { Event = EventType.INSTANTBATTLE, Time = TimeSpan.Parse("19:55") },
            new Schedule { Event = EventType.INSTANTBATTLE, Time = TimeSpan.Parse("21:55") },
            new Schedule { Event = EventType.INSTANTBATTLE, Time = TimeSpan.Parse("23:55") },
            new Schedule { Event = EventType.METEORITEGAME, Time = TimeSpan.Parse("15:40") },
            new Schedule { Event = EventType.METEORITEGAME, Time = TimeSpan.Parse("17:40") },
            new Schedule { Event = EventType.METEORITEGAME, Time = TimeSpan.Parse("19:15") },
            new Schedule { Event = EventType.RAINBOWBATTLE, Time = TimeSpan.Parse("10:00") },
            new Schedule { Event = EventType.RAINBOWBATTLE, Time = TimeSpan.Parse("13:00") },
            new Schedule { Event = EventType.RAINBOWBATTLE, Time = TimeSpan.Parse("16:00") },
            new Schedule { Event = EventType.RAINBOWBATTLE, Time = TimeSpan.Parse("19:00") },
            new Schedule { Event = EventType.RAINBOWBATTLE, Time = TimeSpan.Parse("22:00") },
            new Schedule { Event = EventType.LOD, Time = TimeSpan.Parse("14:30") },
            new Schedule { Event = EventType.LOD, Time = TimeSpan.Parse("16:30") },
            new Schedule { Event = EventType.LOD, Time = TimeSpan.Parse("18:30") },
            new Schedule { Event = EventType.LOD, Time = TimeSpan.Parse("21:00") },
            new Schedule { Event = EventType.LOD, Time = TimeSpan.Parse("00:00") },
            new Schedule { Event = EventType.AUTOREBOOT, Time = TimeSpan.Parse("4:00") },
            new Schedule { Event = EventType.TALENTARENA, Time = TimeSpan.Parse("15:00") },
            new Schedule { Event = EventType.CALIGOR, Time = TimeSpan.Parse("17:00") },
            new Schedule { Event = EventType.MINILANDREFRESHEVENT, Time = TimeSpan.Parse("01:00") },
            new Schedule { Event = EventType.DAILYMISSIONEXTENSIONREFRESH, Time = TimeSpan.Parse("01:01") },
            new Schedule { Event = EventType.RANKINGREFRESH, Time = TimeSpan.Parse("12:00") },
            new Schedule { Event = EventType.RANKINGREFRESH, Time = TimeSpan.Parse("00:00") },
            new Schedule { Event = EventType.RANKINGREFRESH, Time = TimeSpan.Parse("17:00") },
        };

        [DataMember(EmitDefaultValue = true, IsRequired = true)]
        public TimeSpan TimeBeforeAutoKick { get; set; } = TimeSpan.FromMinutes(2);

        [DataMember(EmitDefaultValue = true, IsRequired = true)]
        public TimeSpan TimeBetweenAct6Refresh { get; set; } = TimeSpan.FromSeconds(5);

        [DataMember(EmitDefaultValue = true, IsRequired = true)]
        public TimeSpan TimeBetweenAct4Refresh { get; set; } = TimeSpan.FromSeconds(5);

        [DataMember(EmitDefaultValue = true, IsRequired = true)]
        public TimeSpan TimeBetweenOutdatedBazaarItemRefresh { get; set; } = TimeSpan.FromMinutes(5);

        [DataMember(EmitDefaultValue = true, IsRequired = true)]
        public TimeSpan TimeBetweenGroupRefresh { get; set; } = TimeSpan.FromSeconds(2);

        [DataMember(EmitDefaultValue = true, IsRequired = true)]
        public TimeSpan TimeBetweenAct4FlowerRespawn { get; set; } = TimeSpan.FromMinutes(1);

        [DataMember(EmitDefaultValue = true, IsRequired = true)]
        public TimeSpan TimeBetweenAntiBotProcess { get; set; } = TimeSpan.FromHours(3);

        [DataMember(EmitDefaultValue = true, IsRequired = true)]
        public TimeSpan TimeBetweenItemValidityRefresh { get; set; } = TimeSpan.FromSeconds(1);

        [DataMember(EmitDefaultValue = true, IsRequired = true)]
        public TimeSpan TimeAutoKickInterval { get; set; } = TimeSpan.FromMinutes(2);
    }
}