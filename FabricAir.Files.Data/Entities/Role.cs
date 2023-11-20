namespace FabricAir.Files.Data.Entities;
public record Role(string Name, string Description, int Id = 0)
{
    public ICollection<User> Users { get; init; } = new List<User>();
    public ICollection<FileGroup> FileGroups { get; init; } = new List<FileGroup>();
}