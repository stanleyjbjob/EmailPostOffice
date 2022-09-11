using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EmailPostOffice.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class EmailPostOfficeDbContextFactory : IDesignTimeDbContextFactory<EmailPostOfficeDbContext>
{
    public EmailPostOfficeDbContext CreateDbContext(string[] args)
    {
        EmailPostOfficeEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<EmailPostOfficeDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new EmailPostOfficeDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../EmailPostOffice.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
