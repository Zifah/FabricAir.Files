using static FabricAir.Files.Api.Data.ApplicationDbContext;

namespace FabricAir.Files.Api.Data;

public record FileGroup(int Id, string Name)
{
    public ICollection<RoleFileGroup> RoleFileGroups { get; init; } = new List<RoleFileGroup>();
}