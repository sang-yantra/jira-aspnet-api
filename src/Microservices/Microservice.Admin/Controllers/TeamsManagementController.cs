using Admin.DTO;
using Admin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Admin.Teams.Queries;
using Admin.Teams.Commands;
using MediatR;

namespace Microservice.Admin.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("{v:apiVersion}/[controller]/[action]")]
    public class TeamsManagementController : ApiControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsManagementController(ITeamService teamService)
        {
            _teamService = teamService;
        }

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

        [HttpPost]
        [ActionName("team")]
        [ProducesResponseType(201)]
        public async Task<ActionResult> CreateTeam([FromBody] CreateTeamQuery query)
        {
            var url = await Mediator.Send(query);
            return Created(url, "Resouce created");
        }

    }
}
