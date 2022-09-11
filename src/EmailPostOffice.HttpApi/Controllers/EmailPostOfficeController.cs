using EmailPostOffice.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EmailPostOffice.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class EmailPostOfficeController : AbpControllerBase
{
    protected EmailPostOfficeController()
    {
        LocalizationResource = typeof(EmailPostOfficeResource);
    }
}
