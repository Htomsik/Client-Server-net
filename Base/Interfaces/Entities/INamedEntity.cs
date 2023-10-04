using System.ComponentModel.DataAnnotations;

namespace Interfaces.Entities;

public interface INamedEntity : IEntity
{
    [Required]
    string Name { get; set; }
}