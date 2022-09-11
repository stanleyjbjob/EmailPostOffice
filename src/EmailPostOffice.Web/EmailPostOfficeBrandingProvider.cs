using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace EmailPostOffice.Web;

[Dependency(ReplaceServices = true)]
public class EmailPostOfficeBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "EmailPostOffice";
}
