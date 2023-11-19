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

        public FilesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // iv. Files/Users/{{UserId}} (user as parameter) (administrator); 15 minutes
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
            IEnumerable<FileDTO> response = await ToDTOAsync(userFiles);
            return Ok(response);
        }

        private async Task<IEnumerable<FileDTO>> ToDTOAsync(IQueryable<Data.File> userFiles) => 
            await userFiles.Select(f => new FileDTO(f.Name, f.Group.Name, f.URL)).ToListAsync();

        // v. Files/Groups/Users/{{UserId}}(user as parameter) (administrator); 15 minutes(90 minutes)
        [HttpGet("/Groups/Users/{userId}")]
        public async Task<ActionResult<IEnumerable<FileGroupDTO>>> GetUserFileGroups(int userId)
        {
            throw new NotImplementedException();
        }

        // TODO Hafiz: Create this action after implementing authentication
        // iii. Files/Groups (mine) (any user) ; 15 minutes
    }
}
