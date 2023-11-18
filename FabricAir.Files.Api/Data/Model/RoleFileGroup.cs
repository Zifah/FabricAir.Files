namespace FabricAir.Files.Api.Data;

public record RoleFileGroup(int RoleId, int FileGroupId)
{
    public Role? Role { get; init; }
    public FileGroup? FileGroup { get; init; }
}