namespace FabricAir.Files.Api.Data.Entities;

public record FileGroup(string Name, int Id = 0)
{
    public ICollection<File> Files { get; init; } = new List<File>();
    public ICollection<Role> Roles { get; init; } = new List<Role>();
}