using System.Threading.Tasks;

namespace EmailPostOffice.Data;

public interface IEmailPostOfficeDbSchemaMigrator
{
    Task MigrateAsync();
}
