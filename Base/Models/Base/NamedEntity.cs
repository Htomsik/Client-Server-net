using System.ComponentModel.DataAnnotations;
using Interfaces.Entities;

namespace Models.Base;

public abstract class NamedEntity : Entity, INamedEntity
{
    [Required]
    public string Name { get; set; }
}