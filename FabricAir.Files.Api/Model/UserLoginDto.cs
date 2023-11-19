using System.ComponentModel.DataAnnotations;

namespace FabricAir.Files.Api.Model
{
    public record UserLoginDTO
    {
        public UserLoginDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}