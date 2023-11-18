namespace FabricAir.Files.Api.Data;
public record UserRole(int UserId, int RoleId)
{
    // Navigation properties to represent relationships
    public User? User { get; init; }
    public Role? Role { get; init; }
}