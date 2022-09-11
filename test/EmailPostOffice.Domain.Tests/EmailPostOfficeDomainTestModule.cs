using EmailPostOffice.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EmailPostOffice;

[DependsOn(
    typeof(EmailPostOfficeEntityFrameworkCoreTestModule)
    )]
public class EmailPostOfficeDomainTestModule : AbpModule
{

}
