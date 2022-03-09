using TransactionInformationAPI.Dtos;
using ModelsLib.ContextRepositoryClasses;
using AutoMapper;

namespace TransactionInformationAPI.Profiles
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
