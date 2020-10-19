using System;
using System.Data.Entity;
using System.Linq;
using OpenNos.Core;
using OpenNos.DAL.EF;
using OpenNos.DAL.EF.Helpers;
using OpenNos.DAL.Interface;
using OpenNos.Data;
using OpenNos.Data.Enums;
using OpenNos.Mapper.Mappers;

namespace OpenNos.DAL.DAO
{
    public class BotAuthorityDAO : IBotAuthorityDAO
    {
        #region Methods

        public DeleteResult Delete(long discordId)
        {
            try
            {
                using (var context = DataAccessHelper.CreateContext())
                {
                    var account = context.BotAuthority.FirstOrDefault(c => c.DiscordId.Equals(discordId));

                    if (account != null)
                    {
                        context.BotAuthority.Remove(account);
                        context.SaveChanges();
                    }

                    return DeleteResult.Deleted;
                }
            }
            catch (Exception e)
            {
                Logger.Error(
                    string.Format(Language.Instance.GetMessageFromKey("DELETE_ACCOUNT_ERROR"), discordId, e.Message),
                    e);
                return DeleteResult.Error;
            }
        }

        public SaveResult InsertOrUpdate(ref BotAuthorityDTO discord)
        {
            try
            {
                using (var context = DataAccessHelper.CreateContext())
                {
                    var accountId = discord.DiscordId;
                    var entity = context.BotAuthority.FirstOrDefault(c => c.DiscordId.Equals(accountId));

                    if (entity == null)
                    {
                        discord = insert(discord, context);
                        return SaveResult.Inserted;
                    }

                    discord = update(entity, discord, context);
                    return SaveResult.Updated;
                }
            }
            catch (Exception e)
            {
                Logger.Error(
                    string.Format(Language.Instance.GetMessageFromKey("UPDATE_ACCOUNT_ERROR"), discord.DiscordId,
                        e.Message), e);
                return SaveResult.Error;
            }
        }

        public BotAuthorityDTO LoadById(long discordId)
        {
            try
            {
                using (var context = DataAccessHelper.CreateContext())
                {
                    var account = context.BotAuthority.FirstOrDefault(a => a.DiscordId.Equals(discordId));
                    if (account != null)
                    {
                        var accountDTO = new BotAuthorityDTO();
                        if (BotAuthorityMapper.ToAccountDTO(account, accountDTO)) return accountDTO;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            return null;
        }

        private static BotAuthorityDTO insert(BotAuthorityDTO account, OpenNosContext context)
        {
            var entity = new BotAuthority();
            BotAuthorityMapper.ToAccount(account, entity);
            context.BotAuthority.Add(entity);
            context.SaveChanges();
            BotAuthorityMapper.ToAccountDTO(entity, account);
            return account;
        }

        private static BotAuthorityDTO update(BotAuthority entity, BotAuthorityDTO account, OpenNosContext context)
        {
            if (entity != null)
            {
                BotAuthorityMapper.ToAccount(account, entity);
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }

            if (BotAuthorityMapper.ToAccountDTO(entity, account)) return account;

            return null;
        }

        #endregion
    }
}