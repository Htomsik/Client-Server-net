using API.Data;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.IOC;

internal static partial class IoCRegistration
{
    private const string ConnectionString = "Data Source=";

    public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration) => 
        services
            .AddTransient<IDbInitializer, DataDbInitializer>()
            .AddDbContext<DataDb>(db =>
            {
                var dbConfiguration = configuration.GetSection("Database");
                
                var type = dbConfiguration["Type"];
            
                switch (type)
                {
                    case "MSSQL":
                        db.UseSqlServer($"{ConnectionString}{dbConfiguration.GetConnectionString(type)}", 
                            x => x.MigrationsAssembly(nameof(DAL) + ".SqlServer"));
                        break;
                    
                    case "SQLLite":
                        db.UseSqlite( $"{ConnectionString}{dbConfiguration.GetConnectionString(type)}", 
                            x=>x.MigrationsAssembly(nameof(DAL) + ".SqlLite"));
                        break;

                    case null: 
                        throw new ArgumentNullException($"{nameof(type)} can't be null. Check configuration file/");
                    
                    default: 
                        throw new NotSupportedException($"DataBase {type} doesn't supported");
                }
            });
}