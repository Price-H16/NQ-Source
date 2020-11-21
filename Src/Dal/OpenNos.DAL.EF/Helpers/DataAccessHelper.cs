using System;
using System.Data;
using System.Data.Common;
using OpenNos.Core;

namespace OpenNos.DAL.EF.Helpers
{
    public interface IOpenNosContextFactory
    {
        OpenNosContext CreateContext();
    }

    public static class DataAccessHelper
    {
        private static IOpenNosContextFactory _contextFactory;


        /// <summary>
        ///     Creates new instance of database context.
        /// </summary>
        public static OpenNosContext CreateContext() => _contextFactory.CreateContext();


        public static bool Initialize(IOpenNosContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            using (OpenNosContext context = CreateContext())
            {
                try
                {
                    context.Database.EnsureCreated();
                    Logger.Info(Language.Instance.GetMessageFromKey("DATABASE_INITIALIZED"));
                }
                catch (Exception ex)
                {
                    Logger.Error(Language.Instance.GetMessageFromKey("DATABASE_NOT_UPTODATE"), ex);
                    return false;
                }

                return true;
            }
        }
    }
}