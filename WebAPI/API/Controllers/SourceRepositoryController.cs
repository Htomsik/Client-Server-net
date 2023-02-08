using API.Controllers.Base;
using AutoMapper;
using Domain.Entities;
using Interfaces.Repositories;
using Models.Data;

namespace API.Controllers;

public class SourceRepositoryController : MappedEntityController<DataSourceDTO, DataSource>
{
    public SourceRepositoryController(IRepository<DataSource> repository, IMapper mapper) : base(repository, mapper)
    {
    }
}