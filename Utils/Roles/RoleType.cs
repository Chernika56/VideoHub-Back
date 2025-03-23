namespace BackEnd.Utils.Roles
{
    public static class RoleType
    {
        public static string Administrator = "Admin";
        public static string User = "User";
        public static string OrganizationAdmin = "OrganizationAdmin";
        public static string[] AllRoles = [Administrator, User, OrganizationAdmin];
        public static string[] Administrators = [Administrator, OrganizationAdmin];
    }
}
