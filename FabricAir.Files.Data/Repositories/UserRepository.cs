using FabricAir.Files.Common;
using FabricAir.Files.Common.Exceptions;
using FabricAir.Files.Data.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FabricAir.Files.Data.Repositories
{
    public class UserRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private const int SqlLiteUniqueConstraintViolatedErrorCode = 19;

        public UserRepository(IApplicationDbContext dbContext)
        {
            Require.NotNull(dbContext.Users, nameof(dbContext.Users));
            _dbContext = dbContext;
        }

        public async Task<User?> GetByEmailAsync(string emailAddress)
        {
            return await _dbContext.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Email == emailAddress);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.Include(u => u.Roles).ToArrayAsync();
        }

        public async Task<int> CreateAsync(User newUser)
        {
            try
            {
                _dbContext.Users.Add(newUser);
                return await _dbContext.SaveChangesAsync(default);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqliteException exception)
            {
                if (exception.SqliteErrorCode == SqlLiteUniqueConstraintViolatedErrorCode)
                {
                    throw new EntityAlreadyExistsException("User is already registered.");
                }
                throw;
            }
        }
    }
}
