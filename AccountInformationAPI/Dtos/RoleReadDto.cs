namespace AccountInformationAPI.Dtos
{
    public class RoleReadDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string MenuAccessPermissions { get; set; }
        public string CommandAccessPermissions { get; set; }
    }
}
