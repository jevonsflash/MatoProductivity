using System;
using System.Diagnostics;
using System.IO;
using MatoProductivity.Core;
using MatoProductivity.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MatoProductivity.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class MatoProductivityDbContextFactory : IDesignTimeDbContextFactory<MatoProductivityDbContext>
    {
        public MatoProductivityDbContext CreateDbContext(string[] args)
        {
            var sqliteFilename = "mato.db";
            string documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), sqliteFilename);
            var builder = new DbContextOptionsBuilder<MatoProductivityDbContext>();
            var hostFolder = Path.Combine(Environment.CurrentDirectory, "bin", "Debug", "net7.0");

            var configuration = AppConfigurations.Get(hostFolder);
            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(MatoProductivityConsts.ConnectionStringName)
            );

            return new MatoProductivityDbContext(builder.Options);

        }
    }
}