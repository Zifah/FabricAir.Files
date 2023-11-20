using FabricAir.Files.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FabricAir.Files.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Entities.File> Files { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<FileGroup> FileGroups { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}
