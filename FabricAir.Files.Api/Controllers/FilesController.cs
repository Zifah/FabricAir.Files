using Microsoft.AspNetCore.Mvc;
using FabricAir.Files.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using FabricAir.Files.Data;
using FabricAir.Files.Data.Entities;
using File = FabricAir.Files.Data.Entities.File;

namespace FabricAir.Files.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

        public FilesController(ApplicationDbContext dbContext, IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            _dbContext = dbContext;
            _apiBehaviorOptions = apiBehaviorOptions;
        }

        [HttpGet("Users/{emailAddress}")]
        [SwaggerOperation("Fetch all files that a user has access to")]
        [Authorize(Roles = Constants.UserRoleAdministrator)]
        [ProducesDefaultResponseType(typeof(IEnumerable<FileDTO>))]
        public async Task<IActionResult> GetUserFiles(string emailAddress,
            [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            var user = _dbContext.Users.Include(u => u.Roles).SingleOrDefault(x => x.Email == emailAddress);

            if (user == null)
            {
                ModelState.AddModelError(nameof(emailAddress), "Invalid email address");
                return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }

            var userFiles = _dbContext.Files.Where(f => f.Group.Roles.Any(r => user.Roles.Contains(r)));
            var response = await ToDTOAsync(userFiles);
            return Ok(response);
        }

        [HttpGet("Groups/Users/{emailAddress}")]
        [SwaggerOperation("Fetch all file-groups that a user has access to")]
        [Authorize(Roles = Constants.UserRoleAdministrator)]
        [ProducesDefaultResponseType(typeof(IEnumerable<FileGroupDTO>))]
        public async Task<IActionResult> GetUserFileGroups(string emailAddress)
        {
            var user = _dbContext.Users.Include(u => u.Roles).SingleOrDefault(x => x.Email == emailAddress);

            if (user == null)
            {
                ModelState.AddModelError(nameof(emailAddress), "Invalid email address");
                return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }

            var userFileGroups = _dbContext.FileGroups.Where(g => g.Roles.Any(r => user.Roles.Contains(r)));
            var response = await ToDTOAsync(userFileGroups);
            return Ok(response);
        }


        // TODO Hafiz: Create this action after implementing authentication
        // iii. Files/Groups (mine) (any user) ; 15 minutes
        [HttpGet]
        [SwaggerOperation("Fetch all files that the authenticated user has access to")]
        [ProducesDefaultResponseType(typeof(IEnumerable<FileDTO>))]
        public async Task<IActionResult> GetFiles()
        {
            var user = _dbContext.Users.Include(u => u.Roles).SingleOrDefault(x => x.Email == GetUserEmail());

            if (user == null)
            {
                return Problem("Something went really wrong!");
            }

            var userFiles = _dbContext.Files.Where(f => f.Group.Roles.Any(r => user.Roles.Contains(r)));
            var response = await ToDTOAsync(userFiles);
            return Ok(response);
        }

        [HttpGet("Groups")]
        [SwaggerOperation("Fetch all file-groups that the authenticated user has access to")]
        [ProducesDefaultResponseType(typeof(IEnumerable<FileGroupDTO>))]
        public async Task<IActionResult> GetFileGroups()
        {
            var user = _dbContext.Users.Include(u => u.Roles).SingleOrDefault(x => x.Email == GetUserEmail());

            if (user == null)
            {
                return Problem("Something went really wrong!");
            }

            var userFileGroups = _dbContext.FileGroups.Where(g => g.Roles.Any(r => user.Roles.Contains(r)));
            var response = await ToDTOAsync(userFileGroups);
            return Ok(response);
        }
        private async Task<IEnumerable<FileGroupDTO>> ToDTOAsync(IQueryable<FileGroup> userFileGroups) =>
            await userFileGroups.Select(fg => new FileGroupDTO(fg.Name)).ToListAsync();
        private string GetUserEmail()
        {
            return User!.Identity!.Name!;
        }

        private static async Task<IEnumerable<FileDTO>> ToDTOAsync(IQueryable<File> userFiles) =>
            await userFiles.Select(f => new FileDTO(f.Name, f.Group.Name, f.URL)).ToListAsync();
    }
}
