using Admin.DTO;
using Admin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("{v:apiVersion}/[controller]/[action]")]
    public class TeamsManagementController : Controller
    {
        private readonly ITeamService _teamService;

        public TeamsManagementController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet]
        [ActionName("Teams")]
        [ProducesResponseType(200, Type = typeof(List<TeamDto>))]
        public async Task<ActionResult<List<TeamDto>>> GetAllTeams()
        {
            try
            {
                var teams = await _teamService.GetAllTeamsService();
                return Ok(teams);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
