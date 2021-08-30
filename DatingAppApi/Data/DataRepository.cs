using DatingAppApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppApi.Data
{
    // generic concrete class
    public class DataRepository : IDataRepository
    {
        private readonly DataContext _Context;

        public DataRepository(DataContext context)
        {
            _Context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _Context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _Context.Remove(entity);
        }

        public Task<Photo> GetPhoto(int id)
        {
            var photo = _Context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _Context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _Context.Users.Include(p => p.Photos).ToListAsync();

            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _Context.SaveChangesAsync() > 0;
        }
    }
}
