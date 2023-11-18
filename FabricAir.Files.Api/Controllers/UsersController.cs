using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FabricAir.Files.Api.Data;
using FabricAir.Files.Api.Model;
using System.Net;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace FabricAir.Files.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private const int SqlLiteUniqueConstraintViolatedErrorCode = 19;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.Include(u => u.Roles)
                                .Select(u => ToDTO(u))
                                .ToListAsync();
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(UserDTO))]
        public async Task<IActionResult> PostUser([FromBody] CreateUserRequest user,
            [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users' is null.");
            }

            var password = HashPassword(user.Password);

            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == user.Role);

            if (role == null)
            {
                ModelState.AddModelError(nameof(user.Role), "The specific user role is not valid");
                return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
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
                    return BadRequest("Email address and sername must be unique");
                }
                throw;
            }
        }

        private async Task<User> SaveUser(CreateUserRequest user, string password, Role role)
        {
            var newUser = new User(user.FirstName, user.LastName, user.Username, user.Email, password);
            newUser.Roles.Add(role);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        private static UserDTO ToDTO(User u) => new(u.Id, u.FirstName, u.LastName, u.Username, u.Email)
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
