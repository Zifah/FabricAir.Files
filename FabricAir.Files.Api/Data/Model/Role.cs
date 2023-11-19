using static FabricAir.Files.Api.Data.ApplicationDbContext;

namespace FabricAir.Files.Api.Data;
public record Role(string Name, string Description, int Id = 0)
{
    public ICollection<User> Users { get; init; } = new List<User>();
    public ICollection<FileGroup> FileGroups { get; init; } = new List<FileGroup>();
}