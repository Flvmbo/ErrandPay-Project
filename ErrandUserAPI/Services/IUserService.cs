using ErrandUserAPI.Models;
namespace ErrandUserAPI.Services
{
    public interface IUserService
    {
        public Task<List<User>> GetUserList();
        public Task<User> GetUserByID(int id);
        public Task<User> AddUser (User user);
        public Task<User> UpdateUser(User user);
        public Task<bool> DeleteUser(int Id);
    }
}
