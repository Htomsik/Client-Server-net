namespace Core.Infrastructure.Models.ItemSelectors;

public interface IItemSelector
{
    public string? Name { get; }
   
    public  bool IsAdd { get; set; }
}