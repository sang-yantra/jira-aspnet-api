using Admin.Users;
using Admin.Users.Commands;
using Admin.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.TasksManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/{v:apiVersion}/[controller]/[action]")]
    public class UsersManagementController : ApiControllerBase
    {

        [HttpGet]
        [ActionName("test-update-users")]
        [ProducesResponseType(200, Type = typeof(List<UserDto>))]
        public async Task<ActionResult<string>> TestUpdateUsers()
        {
            try
            {
                return await Mediator.Send(new TestBulkUpdate());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace.ToString());
            }
        }




        /// <summary>
        /// Return all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("users")]
        [ProducesResponseType(200, Type = typeof(List<UserDto>))]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers([FromQuery] GetAllUsersQuery query)
        {
            try
            {

                return await Mediator.Send(query);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace.ToString());
            }
        }


        /// <summary>
        /// Return a user by Id
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("user/{userId}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public async Task<ActionResult<UserDto>> GetUserById([FromRoute] Guid userId)
        {
            return await Mediator.Send(new GetUserByIdQuery() { UserId = userId });
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
