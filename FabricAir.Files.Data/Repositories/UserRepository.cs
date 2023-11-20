﻿using FabricAir.Files.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FabricAir.Files.Data.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            // TODO Hafiz: Validates that Users collection is not null
        }

        public async Task<User?> GetByEmailAsync(string emailAddress)
        {

            return await _dbContext.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Email == emailAddress);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.Include(u => u.Roles).ToArrayAsync();
        }

        public async Task<int> Create(User newUser)
        {
            _dbContext.Users.Add(newUser);
            return await _dbContext.SaveChangesAsync();
        }
    }
}