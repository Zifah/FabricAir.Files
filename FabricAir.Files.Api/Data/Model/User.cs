using static FabricAir.Files.Api.Data.ApplicationDbContext;

namespace FabricAir.Files.Api.Data;

public record User(int Id, string FirstName, string LastName, string Username, string Email)
{
    public ICollection<UserRole> UserRoles { get; init; } = new List<UserRole>();
}