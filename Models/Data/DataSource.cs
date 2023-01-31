using Microsoft.EntityFrameworkCore;
using Models.Base;

namespace Models.Data;

[Index(nameof(Name), IsUnique = true)]
public class DataSource : NamedEntity
{
    public string Description { get; set; }
}