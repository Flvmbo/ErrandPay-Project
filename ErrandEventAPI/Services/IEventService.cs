using ErrandEventAPI.Controllers;
using ErrandEventAPI.Models;
namespace ErrandEventAPI.Services
{
    public interface IEventService
    {
        public Task<List<Event>> GetEventList();
        public Task<Event> GetEventByID(int id);
        public Task<Event> AddEvent (Event eventObj);
        public Task<Event> UpdateEvent(Event eventObj);
        public Task<bool> DeleteEvent(int Id);
        public Task<bool> AddUserToEvent(UserMessage userMessage);
        public Task<bool> RemoveUserFromEvent(UserMessage userMessage);
    }
}
