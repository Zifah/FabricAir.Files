namespace FabricAir.Files.Data.Entities;

public record User(string FirstName, string LastName, string Email, string Password, int Id = 0)
{
    public ICollection<Role> Roles { get; init; } = new List<Role>();
}