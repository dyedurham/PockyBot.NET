using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    internal class PockyUserRepository : IPockyUserRepository
    {
        private readonly DatabaseContext _context;

        public PockyUserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public PockyUser GetUser(string userId)
        {
            return _context.PockyUsers
                .Where(x => x.UserId == userId)
                .Include(x => x.PegsGiven)
                    .ThenInclude(x => x.Receiver)
                .Include(x => x.PegsReceived)
                .Include(x => x.Location)
                .Include(x => x.Roles)
                .SingleOrDefault();
        }

        public List<PockyUser> GetUsersByUsername(string username)
        {
            return _context.PockyUsers
                .Where(x => x.Username == username)
                .Include(x => x.PegsGiven)
                    .ThenInclude(x => x.Receiver)
                .Include(x => x.PegsReceived)
                .Include(x => x.Location)
                .Include(x => x.Roles)
                .ToList();
        }

        public async Task<PockyUser> AddOrUpdateUserAsync(string userId, string username)
        {
            var existingUser = _context.PockyUsers
                .Include(x => x.PegsGiven)
                .Include(x => x.Roles)
                .SingleOrDefault(x => x.UserId == userId);

            if (existingUser != null)
            {
                existingUser.Username = username;
                await _context.SaveChangesAsync();
                return existingUser;
            }

            PockyUser newPockyUser = new PockyUser
            {
                UserId = userId,
                Username = username
            };
            _context.Add(newPockyUser);
            await _context.SaveChangesAsync();

            return newPockyUser;
        }

        public List<PockyUser> GetAllUsersWithPegs()
        {
            return _context.PockyUsers
                .Include(x => x.PegsGiven)
                    .ThenInclude(x => x.Receiver)
                        .ThenInclude(x => x.Location)
                .Include(x => x.PegsReceived)
                    .ThenInclude(x => x.Sender)
                        .ThenInclude(x => x.Location)
                .Include(x => x.Location)
                .Include(x => x.Roles)
                .Where(x => x.PegsGiven.Any() || x.PegsReceived.Any()).ToList();
        }

        public List<PockyUser> GetAllUsersLocations()
        {
            return _context.PockyUsers.Include(x => x.Location).ToList();
        }

        public async Task RemoveUserAsync(PockyUser user)
        {
            var userLocations = _context.UserLocations.Where(x => x.UserId == user.UserId);
            _context.UserLocations.RemoveRange(userLocations);

            _context.PockyUsers.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PockyUser>> GetAllUserRolesAsync()
        {
            return await _context.PockyUsers
                .Include(x => x.Roles)
                .Where(x => x.Roles.Count > 0)
                .ToListAsync();
        }

        public async Task AddRoleAsync(string userId, Role role)
        {
            var user = await _context.PockyUsers
                .Where(x => x.UserId == userId)
                .Include(x => x.Roles)
                .SingleAsync();

            user.Roles.Add(new UserRole
            {
                UserId = userId,
                Role = role
            });

            await _context.SaveChangesAsync();
        }

        public async Task RemoveRoleAsync(string userId, Role role)
        {
            var existingRole = await _context.Roles
                .Where(x => x.UserId == userId && x.Role == role)
                .SingleAsync();

            _context.Roles.Remove(existingRole);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUsernameAsync(string userId, string username)
        {
            var existingUser = await _context.PockyUsers.Where(x => x.UserId == userId)
                .SingleAsync();
            existingUser.Username = username;
            await _context.SaveChangesAsync();
        }
    }
}
