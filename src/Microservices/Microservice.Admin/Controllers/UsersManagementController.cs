using Admin.Users;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Admin.Users.Queries;

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
        [ActionName("user")]
        [ProducesResponseType(200, Type =typeof(UserDto))]
        public async Task<ActionResult<UserDto>> GetUserById([FromQuery] GetUserByIdQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
