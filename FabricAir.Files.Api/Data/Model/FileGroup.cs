using Microsoft.AspNetCore.Authorization.Infrastructure;
using static FabricAir.Files.Api.Data.ApplicationDbContext;

namespace FabricAir.Files.Api.Data;

public record FileGroup(int Id, string Name)
{
    public ICollection<File> Files { get; init; } = new List<File>();
    public ICollection<Role> Roles { get; init; } = new List<Role>();
}