using Microsoft.EntityFrameworkCore;

namespace FabricAir.Files.Api.Data;
// Define your DbContext
public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<File> Files { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<FileGroup> FileGroups { get; set; }
    public DbSet<RoleFileGroup> RoleFileGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        // Define the relationship between UserRoles and User & Role entities
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<RoleFileGroup>()
            .HasKey(rfg => new { rfg.RoleId, rfg.FileGroupId });

        // Define the relationship between RoleFileGroups and Role & FileGroup entities
        modelBuilder.Entity<RoleFileGroup>()
            .HasOne(rfg => rfg.Role)
            .WithMany(r => r.RoleFileGroups)
            .HasForeignKey(rfg => rfg.RoleId);

        modelBuilder.Entity<RoleFileGroup>()
            .HasOne(rfg => rfg.FileGroup)
            .WithMany(fg => fg.RoleFileGroups)
            .HasForeignKey(rfg => rfg.FileGroupId);
    }
}
