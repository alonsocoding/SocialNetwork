using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;
        public AuthRepository(DataContext context)
        {
            this.context = context;
        }
        public async Task<User> Login(string username, string password)
        {
            // Use EntityFramework (ORM) to find the user or return null and not an Exception
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Username == username);
            
            // In case the user doesn't exist return null
            if(user == null) return null; 

            // If it exist Verify if the password is correct
            return VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) ? user : null;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) {

                // Create the same hash to see if both the password and hash are equal
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int hash_index = 0; hash_index < computedHash.Length; hash_index++) {

                    // If any character of the hash is different return false
                    if(computedHash[hash_index] != passwordHash[hash_index]) return false;
                }
            }
            // If all the characters are equal then return true
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            // Take the password and create both the hash and the salt
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            // Set the properties to the user
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Await until the User make the Insert to the table
            await this.context.Users.AddAsync(user);

            // Await until the changes are made
            await this.context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512()) {

                // Create a salt using the HMACSHA512 to build a Key
                passwordSalt = hmac.Key;

                // Create a hash using the method ComputeHash
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            // If the user exist return true and if it doesn't return false
            return (await this.context.Users.AnyAsync(x => x.Username == username)) ? true : false;
        }
    }
}