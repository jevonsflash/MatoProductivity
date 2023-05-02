using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace MatoProductivity.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<MatoProductivityDbContext> dbContextOptions, 
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for MatoProductivityDbContext */
            dbContextOptions.UseSqlite(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<MatoProductivityDbContext> builder, DbConnection connection)
        {
            builder.UseSqlite(connection);
        }
    }
}
