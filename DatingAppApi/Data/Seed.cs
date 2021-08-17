using DatingAppApi.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppApi.Data
{
    public class Seed
    {
        private readonly DataContext _Context;

        public Seed(DataContext context)
        {
            _Context = context;
        }

        public void SeedUsers()
        {
            _Context.Users.RemoveRange(_Context.Users); //remove any previous data in table
            _Context.SaveChanges();

            // see users table
            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");

            // Deserialized userData into an Object
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            // loop thru users 
            foreach (var user in users)
            {
                //create the password hash
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash("password", out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();

                _Context.Users.Add(user);
            }

            _Context.SaveChanges();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
