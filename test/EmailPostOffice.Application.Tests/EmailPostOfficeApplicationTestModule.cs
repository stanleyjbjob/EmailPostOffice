using Volo.Abp.Modularity;

namespace EmailPostOffice;

[DependsOn(
    typeof(EmailPostOfficeApplicationModule),
    typeof(EmailPostOfficeDomainTestModule)
    )]
public class EmailPostOfficeApplicationTestModule : AbpModule
{

}
