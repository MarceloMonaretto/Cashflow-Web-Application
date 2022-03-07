namespace AccountInformationAPI.Dtos
{
    public class RoleCreateDto
    {
        public string RoleName { get; set; }
        public string MenuAccessPermissions { get; set; }
        public string CommandAccessPermissions { get; set; }
    }
}
