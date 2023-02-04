using Microsoft.EntityFrameworkCore;
using Models.Data;

namespace DAL.Context;

public class DataDb : DbContext
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

        model.Entity<DataSource>()
            .HasMany<DataValue>()
            .WithOne(x => x.Source)
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}