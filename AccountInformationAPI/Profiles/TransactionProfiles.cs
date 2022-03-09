using AccountInformationAPI.Dtos;
using ModelsLib.ContextRepositoryClasses;
using AutoMapper;

namespace AccountInformationAPI.Profiles
{
    public class TransactionProfiles : Profile
    {
        public TransactionProfiles()
        {
            CreateMap<TransactionCreateDto, Transaction>();
            CreateMap<Transaction, TransactionReadDto>();
            CreateMap<TransactionUpdateDto, Transaction>();
        }
    }
}
