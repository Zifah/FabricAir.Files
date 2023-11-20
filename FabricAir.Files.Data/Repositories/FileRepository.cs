using FabricAir.Files.Common;
using FabricAir.Files.Data.Entities;
using Microsoft.EntityFrameworkCore;
using File = FabricAir.Files.Data.Entities.File;

namespace FabricAir.Files.Data.Repositories
{
    public class FileRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public FileRepository(IApplicationDbContext dbContext)
        {
            Require.NotNull(dbContext.Files, nameof(dbContext.Files));
            Require.NotNull(dbContext.Users, nameof(dbContext.Users));
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<File>> GetFilesAsync(string userEmailAddress)
        {
            return await _dbContext.Files
                .Include(f => f.Group)
                .Where(f => f.Group
                                .Roles
                                .Any(gr => _dbContext
                                            .Users.Any(u => u.Email == userEmailAddress && u.Roles.Contains(gr))
                                    )
                      )
                .ToArrayAsync();
        }

        public async Task<IEnumerable<FileGroup>> GetFileGroupsAsync(string userEmailAddress)
        {
            return await _dbContext
                .FileGroups
                .Where(g => g.Roles
                                .Any(r => _dbContext
                                            .Users
                                            .Any(u => u.Email == userEmailAddress && u.Roles.Contains(r))
                                     )
                      )
                .ToArrayAsync();
        }
    }
}
