using Admin.Teams;
using Admin.Teams.Commands.CreateTeam;
using Admin.Teams.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.TasksManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/{v:apiVersion}/[controller]/[action]")]
    public class TeamsManagementController : ApiControllerBase
    {

        /// <summary>
        /// Fetches all teams
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("teams")]
        [ProducesResponseType(200, Type = typeof(List<TeamDto>))]
        public async Task<ActionResult<List<TeamDto>>> GetTeams()
        {
            return await Mediator.Send(new GetTeamsQuery());

        }

        /// <summary>
        /// Fetches all teams
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("team")]
        [ProducesResponseType(200, Type = typeof(List<TeamDto>))]
        public async Task<ActionResult<TeamDto>> GetTeamById([FromQuery] GetTeamByIdQuery query)
        {
            return await Mediator.Send(query);
        }

        /// <summary>
        /// Saving a new team 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("team")]
        [ProducesResponseType(201)]
        public async Task<ActionResult> CreateTeam([FromBody] CreateTeamCommand query)
        {
            var team = await Mediator.Send(query);

            return Created("Uri", team);
        }
    }
}