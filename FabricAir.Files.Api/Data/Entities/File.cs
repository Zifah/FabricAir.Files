namespace FabricAir.Files.Api.Data.Entities;

public record File(string Name, int GroupId, string URL, int Id = 0)
{
    public FileGroup Group { get; init; }
}
