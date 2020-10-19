using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenNos.Domain;

namespace OpenNos.DAL.EF
{
    public class BotAuthority
    {
        #region Properties

        public DiscordAuthorityType Authority { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long DiscordId { get; set; }

        #endregion
    }
}