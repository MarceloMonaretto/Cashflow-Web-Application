using AccountInformationAPI.Dtos;
using AccountModelsLib.Models;
using AutoMapper;

namespace AccountInformationAPI.Profiles
{
    public class AccountProfiles : Profile
    {
        public AccountProfiles()
        {
            CreateMap<AccountCreateDto, AccountModel>();
            CreateMap<AccountModel, AccountReadDto>();
            CreateMap<AccountUpdateDto, AccountModel>();
        }
    }
}
