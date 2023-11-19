using System.ComponentModel.DataAnnotations;

public record JwtSettings
{
    [Required]
    public string SecretKey { get; set; }
}