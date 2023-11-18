namespace FabricAir.Files.Api.Data;

public record User(string FirstName, string LastName, string Username, string Email, string Password, int Id = 0)
{
    public ICollection<Role> Roles { get; init; } = new List<Role>();
}