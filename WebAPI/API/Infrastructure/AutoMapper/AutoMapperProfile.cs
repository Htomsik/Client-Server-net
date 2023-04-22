using AutoMapper;
using Domain.Entities;
using Domain.identity;
using Models.Data;
using Models.Identity;

namespace API.Infrastructure.AutoMapper;

internal sealed class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<DataSourceDTO, DataSource>().ReverseMap();
        
        CreateMap<DataValueDTO, DataValue>().ReverseMap();
        
        CreateMap<LoginUserDTO, User>().ForMember(
            dest => dest.UserName, 
            opt => opt.MapFrom(src => src.Name))
            .ReverseMap();
        
        CreateMap<RegistratonUserDTO, User>().ForMember(
                dest => dest.UserName, 
                opt => opt.MapFrom(src => src.Name))
            .ReverseMap();
        
        CreateMap<UserDTO, User>()
            .ForMember( 
                dest => dest.UserName, 
                opt=> opt.MapFrom(src=>src.Name))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src=>src.Email)).ReverseMap();


        CreateMap<TokensDTO,Tokens>().ReverseMap();
    }
        
}