using FabricAir.Files.Common;
using FabricAir.Files.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FabricAir.Files.Data.Repositories
{
    public class RoleRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public RoleRepository(IApplicationDbContext dbContext)
        {
            Require.NotNull(dbContext.Roles, nameof(dbContext.Roles));
            _dbContext = dbContext;
        }

        public async Task<Role?> GetByName(string name)
        {
            return await _dbContext.Roles.SingleOrDefaultAsync(r => r.Name == name);
        }
    }
}
