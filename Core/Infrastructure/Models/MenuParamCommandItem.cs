using System.Windows.Input;
using ReactiveUI;

namespace Core.Infrastructure.Models;

public class MenuParamCommandItem: MenuCommandItem
{
    #region Fields

    private readonly Lazy<object> _lazyCommandParam;

    #endregion

    #region Properties

    public object CommandParam => _lazyCommandParam.Value;

    #endregion
    
    public MenuParamCommandItem(string name, ICommand? command,object commandParam) : base(name, command)
        => _lazyCommandParam = new Lazy<object>(()=> commandParam);
}