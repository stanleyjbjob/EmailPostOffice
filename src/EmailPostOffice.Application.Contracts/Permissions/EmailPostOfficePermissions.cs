namespace EmailPostOffice.Permissions;

public static class EmailPostOfficePermissions
{
    public const string GroupName = "EmailPostOffice";

    public static class Dashboard
    {
        public const string DashboardGroup = GroupName + ".Dashboard";
        public const string Host = DashboardGroup + ".Host";
        public const string Tenant = DashboardGroup + ".Tenant";
    }

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public class MailQueues
    {
        public const string Default = GroupName + ".MailQueues";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}