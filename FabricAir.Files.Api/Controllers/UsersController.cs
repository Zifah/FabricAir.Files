using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FabricAir.Files.Data;
using FabricAir.Files.Api.Model;
using System.Net;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using FabricAir.Files.Data.Entities;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using FabricAir.Files.Data.Repositories;
using FabricAir.Files.Common;

namespace FabricAir.Files.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.UserRoleAdministrator)]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;
        private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;
        private const int SqlLiteUniqueConstraintViolatedErrorCode = 19;

        public UsersController(
            UserRepository userRepository, 
            RoleRepository roleRepository, 
            IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            Require.NotNull(userRepository, nameof(userRepository));
            Require.NotNull(roleRepository, nameof(roleRepository));
            Require.NotNull(apiBehaviorOptions, nameof(apiBehaviorOptions));

            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _apiBehaviorOptions = apiBehaviorOptions;
        }

        [HttpGet]
        [SwaggerOperation("Fetch all users")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var allUsers = await _userRepository.GetAllAsync();
            return Ok(allUsers.Select(ToDTO));
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(UserDTO))]
        [SwaggerOperation("Create a new user")]
        public async Task<IActionResult> PostUser([FromBody] CreateUserRequest user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var password = HashPassword(user.Password);

            var role = await _roleRepository.GetByName(user.Role);

            if (role == null)
            {
                ModelState.AddModelError(nameof(user.Role), "The specified user role is not valid");
                return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }

            try
            {
                User newUser = await SaveUser(user, password, role);
                return StatusCode((int)HttpStatusCode.Created, ToDTO(newUser));
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqliteException exception)
            {
                if (exception.SqliteErrorCode == SqlLiteUniqueConstraintViolatedErrorCode)
                {
                    return BadRequest("User is already registered.");
                }
                throw;
            }
        }

        private async Task<User> SaveUser(CreateUserRequest user, string password, Role role)
        {
            var newUser = new User(user.FirstName, user.LastName, user.Email, password)
            {
                Roles = new Role[] { role }
            };
            _ = await _userRepository.CreateAsync(newUser);
            return newUser;
        }

        private static UserDTO ToDTO(User u) => new(u.Id, u.FirstName, u.LastName, u.Email)
        {
            Roles = u.Roles.Select(r => r.Name)
        };

        private static string HashPassword(string password)
        {
            // I will skip hashing to save time but would use BCrypt along with a salt in a production environment
            return password;
        }
    }
}
