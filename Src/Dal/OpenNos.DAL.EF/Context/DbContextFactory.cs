// WingsEmu
// 
// Developed by NosWings Team

using Microsoft.EntityFrameworkCore;
using OpenNos.DAL.EF.Configuration;
using OpenNos.DAL.EF.Helpers;

namespace OpenNos.DAL.EF
{
    public class DbContextFactory : IOpenNosContextFactory
    {
        private readonly DatabaseConfiguration _conf;

        public DbContextFactory(DatabaseConfiguration conf) => _conf = conf;

        public OpenNosContext CreateContext()
        {
            DbContextOptionsBuilder<OpenNosContext> optionsBuilder = new DbContextOptionsBuilder<OpenNosContext>();
            optionsBuilder.UseSqlServer(_conf.ToString());
            return new OpenNosContext(optionsBuilder.Options);
        }
    }
}