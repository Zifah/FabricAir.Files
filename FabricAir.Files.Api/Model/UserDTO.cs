namespace FabricAir.Files.Api.Model
{
    public record UserDTO(int Id, string FirstName, string LastName, string Username, string Email)
    {
        public IEnumerable<string> Roles { get; init; } = new HashSet<string>();
    }
}