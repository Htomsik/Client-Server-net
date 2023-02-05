using System.ComponentModel.DataAnnotations;
using Interfaces.Entities;

namespace Domain.Entities;

public class DataSourceInfo : INamedEntity
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public string Description { get; set; }
}