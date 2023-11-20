using FabricAir.Files.Common;
using FabricAir.Files.Data.Entities;
using Microsoft.EntityFrameworkCore;
using File = FabricAir.Files.Data.Entities.File;

namespace FabricAir.Files.Data.Repositories
{
    public class FileRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FileRepository(ApplicationDbContext dbContext)
        {
            Require.NotNull(dbContext.Files, nameof(dbContext.Files));
            Require.NotNull(dbContext.Users, nameof(dbContext.Users));
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<File>> GetFilesAsync(string userEmailAddress)
        {
            var user = await _dbContext.Users.Include(u => u.Roles)
                .SingleOrDefaultAsync(u => u.Email == userEmailAddress);

            if (user == null)
            {
                return Array.Empty<File>();
            }

            return _dbContext.Files.Where(f => f.Group.Roles.Any(r => user.Roles.Contains(r))).Include(f => f.Group);
        }

        public async Task<IEnumerable<FileGroup>> GetFileGroupsAsync(string userEmailAddress)
        {
            var user = await _dbContext.Users
                .Include(u => u.Roles)
                .SingleOrDefaultAsync(x => x.Email == userEmailAddress);

            if (user == null)
            {
                return Array.Empty<FileGroup>();
            }

            return _dbContext.FileGroups.Where(g => g.Roles.Any(r => user.Roles.Contains(r)));
        }
    }
}
