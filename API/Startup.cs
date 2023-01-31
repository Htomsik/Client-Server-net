

using API.Data;

namespace API;

public class Startup
{
    #region Fields
    private readonly IDbInitializer _dbInitializer;
    #endregion

    #region Constructors

    public Startup(IDbInitializer dbInitializer)
    {
        _dbInitializer = dbInitializer;
    }

    #endregion

    #region Methods
    public  void Initialize()
    {
        _dbInitializer.Initialize();
    }
    #endregion
}