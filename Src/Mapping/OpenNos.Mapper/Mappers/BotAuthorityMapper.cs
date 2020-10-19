using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public static class BotAuthorityMapper
    {
        #region Methods

        public static bool ToAccount(BotAuthorityDTO input, BotAuthority output)
        {
            if (input == null) return false;

            output.DiscordId = input.DiscordId;
            output.Authority = input.Authority;

            return true;
        }

        public static bool ToAccountDTO(BotAuthority input, BotAuthorityDTO output)
        {
            if (input == null) return false;

            output.DiscordId = input.DiscordId;
            output.Authority = input.Authority;

            return true;
        }

        #endregion
    }
}