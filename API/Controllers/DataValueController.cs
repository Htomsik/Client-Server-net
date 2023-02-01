using API.Controllers.Base;
using Interfaces.Repositories;
using Models.Data;

namespace API.Controllers;

public sealed class DataValueController : EntityController<DataValue>
{
    public DataValueController(IRepository<DataValue> repository) : base(repository)
    {
    }
}