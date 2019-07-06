﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql;

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
            new NpgsqlConnectionStringBuilder { Host = "localhost", Port = 5432, Database = "", Username = "", Password = ""};
        builder.UseNpgsql(connectionStringBuilder.ToString());

        return new GalacticSystemContext(builder.Options);
    }
}
}
