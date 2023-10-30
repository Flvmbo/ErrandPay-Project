using ErrandEventAPI.RabbitMQ;
using ErrandEventAPI.Services;
using ErrandEventAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ErrandEventAPI.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ErrandEventAPI.Controllers
{
    
    public enum MESSAGE_ACTION
    {
        ADD,
        REMOVE
    }
    public class UserMessage
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public EVENT_TYPE EventType { get; set; }
        public MESSAGE_ACTION Action { get; set; }
    }
    public class EventMessage
    {
        public int EventId { get; set; }
        public EVENT_TYPE EventType { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public List<int> UserIds { get; set; }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService eventService;
        private readonly IEventProducer _eventProducer;
        public EventController(IEventService _eventService, IEventProducer eventProducer)
        {
            eventService = _eventService;
            _eventProducer = eventProducer;
        }

        [HttpGet("GetEvent")]
        public async Task<IActionResult> GetEventList()
        {
            try
            {
                var eventList = await eventService.GetEventList();
                return Ok(eventList);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }

        [HttpGet("GetEvent/{id}")]
        public async Task<IActionResult> GetEventByID(int id)
        {
            try
            {
                var eventObj = await eventService.GetEventByID(id);
                return Ok(eventObj);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<UserController>
        [HttpPost("AddEvent")]
        public async Task<IActionResult> AddEvent(Event eventObj)
        {
            try
            {
                var eventData = await eventService.AddEvent(eventObj);
                _eventProducer.SendUserMessage(eventData);
                return Ok(eventData);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent(Event eventObj)
        {
            try
            {
                var eventData = await eventService.UpdateEvent(eventObj);
                return Ok(eventData);
            } catch (Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("DeleteEvent")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var result = await eventService.DeleteEvent(id);
                return result ? Ok(result) : BadRequest(result);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Event/Add")]
        public IActionResult AddUserToEvent(int UserId, EVENT_TYPE EventType, int EventId)
        {
            try
            {
                UserMessage message = new()
                {
                    UserId = UserId,
                    EventType = EventType,
                    EventId = EventId,
                    Action = MESSAGE_ACTION.ADD
                };
                _eventProducer.SendUserMessage(message);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Event/Remove")]
        public IActionResult RemoveUserFromEvent(int UserId, EVENT_TYPE EventType, int EventId)
        {
            try
            {
                UserMessage message = new()
                {
                    UserId = UserId,
                    EventType = EventType,
                    EventId = EventId,
                    Action = MESSAGE_ACTION.REMOVE
                };
                _eventProducer.SendUserMessage(message);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
