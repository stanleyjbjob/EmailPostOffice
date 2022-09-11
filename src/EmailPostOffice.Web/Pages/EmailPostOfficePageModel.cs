using EmailPostOffice.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EmailPostOffice.Web.Pages;

public abstract class EmailPostOfficePageModel : AbpPageModel
{
    protected EmailPostOfficePageModel()
    {
        LocalizationResourceType = typeof(EmailPostOfficeResource);
    }
}
