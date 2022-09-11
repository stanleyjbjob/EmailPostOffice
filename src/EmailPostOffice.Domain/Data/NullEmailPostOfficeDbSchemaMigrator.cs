using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace EmailPostOffice.Data;

/* This is used if database provider does't define
 * IEmailPostOfficeDbSchemaMigrator implementation.
 */
public class NullEmailPostOfficeDbSchemaMigrator : IEmailPostOfficeDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
