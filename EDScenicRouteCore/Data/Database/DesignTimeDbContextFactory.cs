using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EDScenicRouteCore.Data
{

	
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<GalacticSystemContext>
{
    public GalacticSystemContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("hostparams.json")
            .Build();

        var builder = new DbContextOptionsBuilder<GalacticSystemContext>();

        var connectionStringBuilder =
            new SqliteConnectionStringBuilder { DataSource = configuration.GetConnectionString("DefaultConnection") };
        builder.UseSqlite(connectionStringBuilder.ToString());

        return new GalacticSystemContext(builder.Options);
    }
}
}
