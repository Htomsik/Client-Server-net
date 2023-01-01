using System.Windows.Input;
using ReactiveUI;

namespace Core.Infrastructure.Models;

public class MenuCommandItem
{

    #region Fields

    private readonly Lazy<ICommand?> _lazyCommand;
    
    #endregion

    #region Properties

    public string Name { get; }

    public ICommand? Command => _lazyCommand.Value;

    #endregion
    
    public MenuCommandItem(string name, ICommand? command)
    {
        Name = name;

        _lazyCommand = new Lazy<ICommand?>(()=>command);
    }
    
}