using FabricAir.Files.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using FabricAir.Files.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FabricAir.Files.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

        public FilesController(ApplicationDbContext dbContext, IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            _dbContext = dbContext;
            _apiBehaviorOptions = apiBehaviorOptions;
        }

        [HttpGet("/Users/{userId}")]
        [ProducesDefaultResponseType(typeof(IEnumerable<FileDTO>))]
        public async Task<IActionResult> GetUserFiles(int userId,
            [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            var user = _dbContext.Users.Include(u => u.Roles).SingleOrDefault(x => x.Id == userId);

            if (user == null)
            {
                ModelState.AddModelError(nameof(userId), "The specific userId is not valid");
                return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }

            var userFiles = _dbContext.Files.Where(f => f.Group.Roles.Any(r => user.Roles.Contains(r)));
            var response = await ToDTOAsync(userFiles);
            return Ok(response);
        }

        [HttpGet("/Groups/Users/{userId}")]
        [ProducesDefaultResponseType(typeof(IEnumerable<FileGroupDTO>))]
        public async Task<IActionResult> GetUserFileGroups(int userId)
        {
            var user = _dbContext.Users.Include(u => u.Roles).SingleOrDefault(x => x.Id == userId);

            if (user == null)
            {
                ModelState.AddModelError(nameof(userId), "The specific userId is not valid");
                return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }

            var userFileGroups = _dbContext.FileGroups.Where(g => g.Roles.Any(r => user.Roles.Contains(r)));
            var response = await ToDTOAsync(userFileGroups);
            return Ok(response);
        }

        private async Task<IEnumerable<FileGroupDTO>> ToDTOAsync(IQueryable<FileGroup> userFileGroups) =>
            await userFileGroups.Select(fg => new FileGroupDTO(fg.Name)).ToListAsync();

        // TODO Hafiz: Create this action after implementing authentication
        // iii. Files/Groups (mine) (any user) ; 15 minutes

        private async Task<IEnumerable<FileDTO>> ToDTOAsync(IQueryable<Data.File> userFiles) =>
            await userFiles.Select(f => new FileDTO(f.Name, f.Group.Name, f.URL)).ToListAsync();
    }
}
