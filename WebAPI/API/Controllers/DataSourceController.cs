using API.Controllers.Base;
using Interfaces.Repositories;
using Models.Data;

namespace API.Controllers;

public sealed class DataSourceController : EntityController<DataSource>
{
    public DataSourceController(INamedRepository<DataSource> repository) : base(repository) { }
}