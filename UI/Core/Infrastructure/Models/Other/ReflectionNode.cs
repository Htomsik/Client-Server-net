using System.Reflection;
using ReactiveUI;

namespace Core.Infrastructure.Models.Other;

public class ReflectionNode : ReactiveObject
{
    #region Node properties

    public string Name => _nodeType.Name;

    public IEnumerable<ReflectionNode> Nodes => GetNodes(_nodeType);
    
    public IEnumerable<PropertyInfo> Properties => _nodeType.GetProperties();
    
    public IEnumerable<MethodInfo> Methods => _nodeType.GetMethods();

    public IEnumerable<Type> Interfaces => _nodeType.GetInterfaces();
    
    #endregion
    
    #region Fields

    private readonly Type _nodeType;

    #endregion
    
    #region Constructors
    
    public ReflectionNode(Type reflectionType) =>
        _nodeType = reflectionType;
    
    #endregion
    
    #region Methods

    private IEnumerable<ReflectionNode> GetNodes(Type type)
    {
        if(type.BaseType == null) return type.GetInterfaces().Select(x=> new ReflectionNode(x));

        return Enumerable.Repeat(type.BaseType, 1).Select(x=>new ReflectionNode(x));
    }

    #endregion
}