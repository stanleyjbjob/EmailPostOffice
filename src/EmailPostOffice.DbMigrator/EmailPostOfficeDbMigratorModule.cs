using EmailPostOffice.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace EmailPostOffice.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(EmailPostOfficeEntityFrameworkCoreModule),
    typeof(EmailPostOfficeApplicationContractsModule)
)]
public class EmailPostOfficeDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options =>
        {
            options.IsJobExecutionEnabled = false;
        });
    }
}
