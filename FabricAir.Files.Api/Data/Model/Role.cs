using static FabricAir.Files.Api.Data.ApplicationDbContext;

namespace FabricAir.Files.Api.Data;
public record Role(int Id, string Name, string Description)
{
    public ICollection<UserRole> UserRoles { get; init; } = new List<UserRole>();
    public ICollection<RoleFileGroup> RoleFileGroups { get; init; } = new List<RoleFileGroup>();
}