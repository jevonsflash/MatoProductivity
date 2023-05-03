using System;
using System.Diagnostics;
using System.IO;
using MatoProductivity.Core;
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
            var builder = new DbContextOptionsBuilder<MatoProductivityDbContext>();
            DbContextOptionsConfigurer.Configure(
                builder,
                "Data Source=file:mato.db;"
            );

            return new MatoProductivityDbContext(builder.Options);

        }
    }
}