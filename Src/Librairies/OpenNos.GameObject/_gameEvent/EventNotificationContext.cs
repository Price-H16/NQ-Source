using NosTale.Packets.Packets.ClientPackets;
using OpenNos.Core;
using OpenNos.GameObject;

namespace OpenNos.GameObject._gameEvent
{
    public struct EventNotificationContext
    {
        /// <summary>
        /// Servers from where the event is happening
        /// </summary>
        //public WorldServerDTO Server { get; set; }

        public NotifiableEventType NotifiableEventType { get; set; }
    }
}