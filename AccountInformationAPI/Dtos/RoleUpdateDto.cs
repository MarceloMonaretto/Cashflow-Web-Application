namespace AccountInformationAPI.Dtos
{
    public class RoleUpdateDto
    {
        public string RoleName { get; set; }
        public string MenuAccessPermissions { get; set; }
        public string CommandAccessPermissions { get; set; }
    }
}
