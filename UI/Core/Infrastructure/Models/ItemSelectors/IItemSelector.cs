namespace Core.Infrastructure.Models.ItemSelectors;

public interface IItemSelector
{
    public string? Name { get; set; }
   
    public  bool IsAdd { get; set; }
}