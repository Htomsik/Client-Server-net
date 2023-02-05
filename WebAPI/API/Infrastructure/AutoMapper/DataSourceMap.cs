using AutoMapper;
using Domain.Entities;
using Models.Data;

namespace API.Infrastructure.AutoMapper;

public class DataSourceMap : Profile
{
    public DataSourceMap() =>
        CreateMap<DataSourceInfo, DataSource>()
            .ReverseMap();
}