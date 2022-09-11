using EmailPostOffice.Localization;
using Volo.Abp.Application.Services;

namespace EmailPostOffice;

/* Inherit your application services from this class.
 */
public abstract class EmailPostOfficeAppService : ApplicationService
{
    protected EmailPostOfficeAppService()
    {
        LocalizationResource = typeof(EmailPostOfficeResource);
    }
}
