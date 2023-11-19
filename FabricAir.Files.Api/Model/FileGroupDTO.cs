using System.ComponentModel.DataAnnotations;

namespace FabricAir.Files.Api.Model;

public record FileGroupDTO
{
    [Required]
    public string Name { get; init; }
}
