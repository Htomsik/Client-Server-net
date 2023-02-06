using DAL.Extensions.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Identity;

namespace DAL.Context;

public class DataDb : IdentityDbContext<User,Role,int>
{
    #region Properties

    public DbSet<DataValue> Values { get; set; }
    
    public DbSet<DataSource> Sources { get; set; }

    #endregion
    
    #region Constructors

    public DataDb(DbContextOptions<DataDb> options) : base(options) { }

    #endregion

    #region Methods

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);

        model.ApplyConfiguration(new RoleConfiguration());
        
        model.Entity<DataSource>()
            .HasMany<DataValue>()
            .WithOne(x => x.Source)
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}