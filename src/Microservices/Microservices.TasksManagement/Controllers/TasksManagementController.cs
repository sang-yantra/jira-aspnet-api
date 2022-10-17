using Microsoft.AspNetCore.Mvc;
using Tasks.Tasks;
using Tasks.Tasks.Queries;

namespace Microservices.TasksManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/{v:apiVersion}/[controller]/[action]")]
    public class TasksManagementController : ApiControllerBase
    {
       
        [HttpGet]
        [ActionName("tasksKanban/{teamId}")]
        public async Task<ActionResult<List<TasksKanbanDto>>> GetTasksKanban([FromRoute] Guid teamId)
        {
            teamId = new Guid("159f154d-cb13-445a-a107-f74cd6507beb");
            return await Mediator.Send(new GetTasksQuery() { TeamId = teamId });
        }
        
    }
}
