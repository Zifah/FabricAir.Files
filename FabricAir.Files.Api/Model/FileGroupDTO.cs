using System.ComponentModel.DataAnnotations;

namespace FabricAir.Files.Api.Model;

public record FileGroupDTO
{
    public FileGroupDTO(string name)
    {
        Name = name;
    }

    [Required]
    public string Name { get; init; }
}
