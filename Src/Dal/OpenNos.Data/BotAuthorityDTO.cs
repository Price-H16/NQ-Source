using System;
using OpenNos.Domain;

namespace OpenNos.Data
{
    [Serializable]
    public class BotAuthorityDTO
    {
        #region Properties

        public DiscordAuthorityType Authority { get; set; }

        public long DiscordId { get; set; }

        #endregion
    }
}