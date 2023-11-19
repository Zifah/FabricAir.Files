using System.ComponentModel.DataAnnotations;

namespace FabricAir.Files.Api.Model;

public record FileDTO
{
    public FileDTO(string name, string fileGroup, string uRL)
    {
        Name = name;
        FileGroup = fileGroup;
        URL = uRL;
    }

    [Required]
    public string Name { get; init; }
    [Required]
    public string FileGroup { get; init; }
    [Required]
    public string URL { get; init; }
}
