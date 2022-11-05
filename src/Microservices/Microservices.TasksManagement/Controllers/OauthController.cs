using Authentication.Login;
using Authentication.Login.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.TasksManagement.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class OauthController : ApiControllerBase
    {
        [HttpPost]
        [ActionName("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginPostDto user)
        {
            return await Mediator.Send(new GetAccessToken()
            {
                Username = user.Username,
                Password = user.Password
            });
        }
    }
}