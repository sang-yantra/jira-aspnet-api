using Admin.Users;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Admin.Users.Queries;
using Admin.Users.Commands;

namespace Microservice.Admin.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("{v:apiVersion}/[controller]/[action]")]
    public class UsersManagementController : ApiControllerBase
    {
        /// <summary>
        /// Return all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("users")]
        [ProducesResponseType(200, Type = typeof(List<UserDto>))]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers([FromQuery] GetAllUsersQuery query)
        {
            return await Mediator.Send(query);
        }


        /// <summary>
        /// Return a user by Id
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("user/{userId}")]
        [ProducesResponseType(200, Type =typeof(UserDto))]
        public async Task<ActionResult<UserDto>> GetUserById([FromRoute] Guid userId)
        {
            return await Mediator.Send(new GetUserByIdQuery() { UserId = userId});
        }

        

        /// <summary>
        /// Create a user 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("user")]
        [ProducesResponseType(201, Type = typeof(UserDto))]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserCommandQuery command)
        {
            var response = await Mediator.Send(command);
            return Created("uri", response);
        }
    }
}
