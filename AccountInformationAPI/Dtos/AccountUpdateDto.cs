﻿namespace AccountInformationAPI.Dtos
{
    public class AccountUpdateDto
    {
        public int Id { get; set; }
        public string UserCredentialName { get; set; }
        public string UserCredentialPassword { get; set; }
        public string Role { get; set; }
    }
}