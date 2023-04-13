using AutoMapper;
using Domain.Entities;
using Models.Data;

namespace API.Infrastructure.AutoMapper;

internal sealed class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<DataSourceDTO, DataSource>().ReverseMap();
        CreateMap<DataValueDTO, DataValue>().ReverseMap();
    }
        
}