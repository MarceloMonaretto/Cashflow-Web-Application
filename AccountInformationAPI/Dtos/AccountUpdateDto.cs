namespace AccountInformationAPI.Dtos
{
    public class AccountUpdateDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserRole { get; set; }
    }
}
