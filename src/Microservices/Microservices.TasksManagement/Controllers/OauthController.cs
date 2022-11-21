using Authentication.Login;
using Authentication.Login.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.TasksManagement.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class OauthController : ApiControllerBase
    {
        /// <summary>
        /// get logged in user details 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("login")]
        [ProducesResponseType(200, Type = typeof(LoggedInUserDto))]
        public async Task<ActionResult<LoggedInUserDto>> GetLoggedInUser([FromBody] LoginPostDto user)
        {
            return await Mediator.Send(new GetLoggedInUser()
            {
                Username = user.Username,
                Password = user.Password
            });
        }

        /// <summary>
        /// get access token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("oauth/access_token")]
        [ProducesResponseType(200, Type = typeof(TokenResponseDto))]
        public async Task<ActionResult<TokenResponseDto>> GetAccessToken([FromBody] LoginPostDto user)
        {
            return await Mediator.Send(new GetAccessToken()
            {
                Username = user.Username,
                Password = user.Password
            });
        }
    }
}