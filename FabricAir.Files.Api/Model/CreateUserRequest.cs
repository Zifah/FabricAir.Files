using System.ComponentModel.DataAnnotations;

namespace FabricAir.Files.Api.Model
{
    public record CreateUserRequest
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; init; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; init; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; init; } = string.Empty;
    }
}
