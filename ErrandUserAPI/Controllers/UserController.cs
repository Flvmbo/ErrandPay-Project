using ErrandUserAPI.RabbitMQ;
using ErrandUserAPI.Services;
using ErrandUserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ErrandUserAPI.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ErrandUserAPI.Controllers
{
    public enum EVENT_TYPE
    {
        WEDDING,
        PARTY,
        CONCERT,
        FESTIVAL,
        MEETING,
        PARADE
    }
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

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IUserProducer _userProducer;
        public UserController(IUserService _userService, IUserProducer userProducer)
        {
            userService = _userService;
            _userProducer = userProducer;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUserList()
        {
            try
            {
                var userList = await userService.GetUserList();
                return Ok(userList);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUserByID(int id)
        {
            try
            {
                var user = await userService.GetUserByID(id);
                return Ok(user);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<UserController>
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
                var userData = await userService.AddUser(user);
                _userProducer.SendUserMessage(userData);
                return Ok(userData);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(User user)
        {
            try
            {
                var userData = await userService.UpdateUser(user);
                return Ok(userData);
            } catch (Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await userService.DeleteUser(id);
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
                _userProducer.SendUserMessage(message);
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
                _userProducer.SendUserMessage(message);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
