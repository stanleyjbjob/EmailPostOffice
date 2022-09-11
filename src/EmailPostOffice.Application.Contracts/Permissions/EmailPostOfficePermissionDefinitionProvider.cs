using EmailPostOffice.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace EmailPostOffice.Permissions;

public class EmailPostOfficePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(EmailPostOfficePermissions.GroupName);

        myGroup.AddPermission(EmailPostOfficePermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        myGroup.AddPermission(EmailPostOfficePermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(EmailPostOfficePermissions.MyPermission1, L("Permission:MyPermission1"));

        var mailQueuePermission = myGroup.AddPermission(EmailPostOfficePermissions.MailQueues.Default, L("Permission:MailQueues"));
        mailQueuePermission.AddChild(EmailPostOfficePermissions.MailQueues.Create, L("Permission:Create"));
        mailQueuePermission.AddChild(EmailPostOfficePermissions.MailQueues.Edit, L("Permission:Edit"));
        mailQueuePermission.AddChild(EmailPostOfficePermissions.MailQueues.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EmailPostOfficeResource>(name);
    }
}