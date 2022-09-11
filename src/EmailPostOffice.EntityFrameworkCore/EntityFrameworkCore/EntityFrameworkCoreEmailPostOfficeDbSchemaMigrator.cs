using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EmailPostOffice.Data;
using Volo.Abp.DependencyInjection;

namespace EmailPostOffice.EntityFrameworkCore;

public class EntityFrameworkCoreEmailPostOfficeDbSchemaMigrator
    : IEmailPostOfficeDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreEmailPostOfficeDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the EmailPostOfficeDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<EmailPostOfficeDbContext>()
            .Database
            .MigrateAsync();
    }
}
