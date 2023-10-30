using ErrandEventAPI.Controllers;
using ErrandEventAPI.Data;
using ErrandEventAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ErrandEventAPI.Services
{
    public class EventService: IEventService
    {
        private readonly EventDbContext _dbContext;
        public EventService(EventDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Event>> GetEventList ()
        {
            return await _dbContext.Events.ToListAsync();                       
        }
        public async Task<Event?> GetEventByID (int id)
        {
            var eventObj = await _dbContext.Events.Where(x => x.EventId == id).FirstOrDefaultAsync();
            if (eventObj == null)
            {
                return null;
            }
            return (Event)eventObj;
        }
        public async Task<Event> AddEvent(Event eventObj)
        {
            var result = await _dbContext.Events.AddAsync(eventObj);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<Event> UpdateEvent (Event eventObj)
        {
            var result = _dbContext.Events.Update(eventObj);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<bool> DeleteEvent(int Id)
        {
            var filteredData = await _dbContext.Events.Where(x => x.EventId == Id).FirstOrDefaultAsync();
            var result = _dbContext.Remove(filteredData);
            await _dbContext.SaveChangesAsync();
            return result != null ? true : false;
        }
        public async Task<bool> AddUserToEvent(UserMessage userMessage)
        {
            var eventId = userMessage.EventId;
            var addedId = userMessage.UserId;
            UserMapping userId = new()
            {
                Id = addedId
            };
            var updatedEvent = await _dbContext.Events.Where(x => x.EventId == eventId).FirstOrDefaultAsync();
            if (updatedEvent != null)
            {
                updatedEvent.Users.Add(userId);
                await _dbContext.SaveChangesAsync();
            }
            return updatedEvent != null ? true : false;
        }
        public async Task<bool> RemoveUserFromEvent(UserMessage userMessage)
        {
            var eventId = userMessage.EventId;
            var addedId = userMessage.UserId;
            UserMapping userId = new()
            {
                Id = addedId
            };
            var updatedEvent = await _dbContext.Events.Where(x => x.EventId == eventId).FirstOrDefaultAsync();
            if (updatedEvent != null)
            {
                updatedEvent.Users.Remove(userId);
                await _dbContext.SaveChangesAsync();
            }
            return updatedEvent != null ? true : false;
        }
    }
}
