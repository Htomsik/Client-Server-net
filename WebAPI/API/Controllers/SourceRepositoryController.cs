using API.Controllers.Base;
using AutoMapper;
using Domain.Entities;
using Interfaces.Repositories;
using Models.Data;

namespace API.Controllers;

[Authorize]
public class SourceRepositoryController : MappedEntityController<DataSourceDTO, DataSource>
{
    public SourceRepositoryController(INamedRepository<DataSource> repository, IMapper mapper) : base(repository, mapper)
    {
    }
}