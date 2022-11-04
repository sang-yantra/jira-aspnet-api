using Authentication.Login;
using Microsoft.AspNetCore.Mvc;
using Authentication.Login.Queries;

namespace Microservices.TasksManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
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