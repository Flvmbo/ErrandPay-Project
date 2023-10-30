using ErrandUserAPI.Data;
using ErrandUserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ErrandUserAPI.Services
{
    public class UserService: IUserService
    {
        private readonly UserDbContext _dbContext;
        public UserService(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetUserList ()
        {
            return await _dbContext.Users.ToListAsync();                       
        }
        public async Task<User?> GetUserByID (int id)
        {
            var user = await _dbContext.Users.Where(x => x.UserId == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            return (User)user;
        }
        public async Task<User> AddUser(User user)
        {
            var result = await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<User> UpdateUser (User user)
        {
            var result = _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<bool> DeleteUser(int Id)
        {
            var filteredData = await _dbContext.Users.Where(x => x.UserId == Id).FirstOrDefaultAsync();
            var result = _dbContext.Remove(filteredData);
            await _dbContext.SaveChangesAsync();
            return result != null ? true : false;
        }

    }
}
