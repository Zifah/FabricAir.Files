using Microsoft.EntityFrameworkCore;

namespace FabricAir.Files.Api.Data;
// Define your DbContext
public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<File> Files { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<FileGroup> FileGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(u => u.Name).IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasMany(u => u.Roles).WithMany(r => r.Users).UsingEntity(j => j.ToTable("UserRoles"));
        });

        modelBuilder.Entity<FileGroup>(entity =>
        {
            entity.HasIndex(fg => fg.Name).IsUnique();
            entity.HasMany(fg => fg.Roles).WithMany(r => r.FileGroups).UsingEntity(j => j.ToTable("RoleFileGroups"));
        });

        // Define the relationship between RoleFileGroups and Role & FileGroup entities
        modelBuilder.Entity<File>(entity =>
        {
            entity.HasIndex(f => f.Name).IsUnique();
            entity.HasOne(f => f.Group).WithMany(g => g.Files).HasForeignKey(f => f.GroupId);
        });
    }
}
