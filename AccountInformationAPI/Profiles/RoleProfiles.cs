using AccountInformationAPI.Dtos;
using ModelsLib.ContextRepositoryClasses;
using AutoMapper;

namespace AccountInformationAPI.Profiles
{
    public class RoleProfiles : Profile
    {
        public RoleProfiles()
        {
            CreateMap<RoleCreateDto, Role>();
            CreateMap<Role, RoleReadDto>();
            CreateMap<RoleUpdateDto, Role>();
        }
    }
}
