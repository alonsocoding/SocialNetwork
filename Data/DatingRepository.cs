/*
    Name: DatingRepository
    This class is made to apply the Repository Pattern to controll the interface of 
    the methods to Save, Update, Remove and Get Information from the Database.
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;
        }
        // Add a new entity to the database
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        // Delete a new entity to the database
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        // Get a User based on specific id 
        public async Task<User> GetUser(int id)
        {
            return await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
        }
        // Get all the Users in the database
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.Include(p => p.Photos).ToListAsync();

        }
        // Save all the entities in the database
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}