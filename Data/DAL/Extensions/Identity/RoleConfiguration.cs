using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Identity;

namespace DAL.Extensions.Identity;

internal class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder) =>
        builder.HasData
        (
            new { Id = 1,Name = Role.Administrators, }, 
            new { Id = 2,Name = Role.Users }
        );
    
}