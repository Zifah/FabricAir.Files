using static FabricAir.Files.Api.Data.ApplicationDbContext;

namespace FabricAir.Files.Api.Data;
public record Role(int Id, string Name, string Description)
{
    public ICollection<User> Users { get; init; } = new List<User>();
    public ICollection<FileGroup> FileGroups { get; init; } = new List<FileGroup>();
}