using API.Data;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.DI;

internal static partial class DiRegistration
{
    private const string ConnectionString = "Data Source=";

    public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration) => 
        services
            .AddTransient<IDbInitializer, DataDbInitializer>()
            .AddDbContext<DataDb>(db =>
        {
            var type = configuration["Database:Type"];
            
            switch (type)
            {
                case "MSSQL":
                    db.UseSqlServer($"{ConnectionString}{configuration.GetConnectionString(type)}", 
                        x => x.MigrationsAssembly(nameof(DAL) + ".SqlServer"));
                    break;
                
                case "SQLLite":
                    db.UseSqlite( $"{ConnectionString}{configuration.GetConnectionString(type)}", 
                        x=>x.MigrationsAssembly(nameof(DAL) + ".SqlLite"));
                    break;

                case null: 
                    throw new ArgumentNullException($"{nameof(type)} can't be null. Check configuration file/");
                
                default: 
                    throw new NotSupportedException($"DataBase {type} doesn't supported");
            }
        });
}