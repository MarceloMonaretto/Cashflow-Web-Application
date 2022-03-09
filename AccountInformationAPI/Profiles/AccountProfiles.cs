using AccountInformationAPI.Dtos;
using ModelsLib.ContextRepositoryClasses;
using AutoMapper;

namespace AccountInformationAPI.Profiles
{
    public class AccountProfiles : Profile
    {
        public AccountProfiles()
        {
            CreateMap<AccountCreateDto, Account>();
            CreateMap<Account, AccountReadDto>();
            CreateMap<AccountUpdateDto, Account>();
        }
    }
}
