using System.Windows.Input;

namespace Core.Infrastructure.Models.Menu;

public class MenuParamCommandItem: MenuCommandItem
{
    #region Fields

    private readonly Lazy<object> _lazyCommandParam;

    #endregion

    #region Properties

    public object CommandParam => _lazyCommandParam.Value;

    #endregion

    #region Constructors

    public MenuParamCommandItem(string name, ICommand? command,object commandParam) : base(name, command)
        => _lazyCommandParam = new Lazy<object>(()=> commandParam);

    #endregion

    #region Methods

    public static IEnumerable<MenuParamCommandItem> CreateCollectionWithEnumParameter<TEnum>(ICommand command,TEnum[] tEnum) where TEnum : Enum
    {
        foreach (TEnum item in tEnum)
        {
            yield return new MenuParamCommandItem(item.ToString(),command,item);
        }
    }

    #endregion
}