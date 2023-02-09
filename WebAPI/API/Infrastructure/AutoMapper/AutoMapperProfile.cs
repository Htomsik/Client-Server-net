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
    }
        
}