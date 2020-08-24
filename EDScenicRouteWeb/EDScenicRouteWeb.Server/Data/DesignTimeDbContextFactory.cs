using System.IO;
using EDScenicRouteWeb.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace EDScenicRouteWeb.Server.Repositories
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
            new NpgsqlConnectionStringBuilder { Host = "localhost", Port = 5432, Database = "elite2", Username = "keir", Password = "apple2banana"};
        
        builder.UseNpgsql(connectionStringBuilder.ToString());

        return new GalacticSystemContext(builder.Options);
    }
}
}
