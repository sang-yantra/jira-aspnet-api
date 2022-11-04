using Authentication.Login;
using Microsoft.AspNetCore.Mvc;
using Authentication.Login.Queries;

namespace Microservices.TasksManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/{v:apiVersion}/[controller]/[action]")]
    public class OauthController : ApiControllerBase
    {
        [HttpPost]
        [ActionName("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginPostDto user)
        {
            try
            {
                return await Mediator.Send(new GetAccessToken()
                {
                    Username = user.Username,
                    Password = user.Password
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace.ToString());
            }
        }
    }
}