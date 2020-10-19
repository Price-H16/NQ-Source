using OpenNos.Data;
using OpenNos.Data.Enums;

namespace OpenNos.DAL.Interface
{
    public interface IBotAuthorityDAO
    {
        #region Methods

        DeleteResult Delete(long discordId);

        SaveResult InsertOrUpdate(ref BotAuthorityDTO discord);

        BotAuthorityDTO LoadById(long discordId);

        #endregion
    }
}