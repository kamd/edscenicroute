using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseSqlite(connectionString);

        return new GalacticSystemContext(builder.Options);
    }
}
}
