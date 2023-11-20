using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Swashbuckle.AspNetCore.Annotations;
using FabricAir.Files.Data.Entities;
using FabricAir.Files.Data.Repositories;
using FabricAir.Files.Api.Model;
using FabricAir.Files.Common;

namespace FabricAir.Files.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IOptions<JwtSettings> _jwtSettings;
        private const int TokenExpirationTimeMinutes = 60;

        public AuthController(UserRepository userRepository, IOptions<JwtSettings> jwtSettings)
        {
            Require.NotNull(userRepository, nameof(userRepository));

            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
        }

        [SwaggerOperation("Authenticate the user")]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO userDto)
        {
            var user = await _userRepository.GetByEmailAsync(userDto.Email);

            if (user == null || !VerifyHashedPassword(user.Password, userDto.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            var token = GenerateToken(user);
            return Ok(new { Token = token });
        }

        private bool VerifyHashedPassword(string passwordHash, string enteredPassword)
        {
            return passwordHash.Equals(enteredPassword);
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Value.SecretKey);
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email.ToString()) };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(TokenExpirationTimeMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
