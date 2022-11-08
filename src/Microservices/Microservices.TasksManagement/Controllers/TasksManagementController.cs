using Microsoft.AspNetCore.Mvc;
using Tasks.Tasks;
using Tasks.Tasks.Commands;
using Tasks.Tasks.Queries;

namespace Microservices.TasksManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/{v:apiVersion}/[controller]/[action]")]
    public class TasksManagementController : ApiControllerBase
    {
        /// <summary>
        /// Get all tasks of a team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet] 
        [ActionName("{teamId}/tasks")]
        [ProducesResponseType(200, Type=typeof(List<TasksKanbanDto>))]
        public async Task<ActionResult<List<TasksKanbanDto>>> GetTasksKanban([FromRoute] Guid teamId)
        {
            teamId = new Guid("159f154d-cb13-445a-a107-f74cd6507beb");
            return await Mediator.Send(new GetTasksQuery() { TeamId = teamId });
        }

        /// <summary>
        /// Create a task
        /// </summary>
        /// <param name="createTask"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("task")]
        [ProducesResponseType(200, Type = typeof(TasksInfoDto))]
        public async Task<ActionResult<TasksInfoDto>> CreateTask([FromBody] CreateTaskCommand createTask)
        {
            return await Mediator.Send(createTask);
        }

        /// <summary>
        /// Get a task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("tasks/{taskId}")]
        [ProducesResponseType(200, Type = typeof(TasksInfoDto))]
        public async Task<ActionResult<TasksInfoDto>> GetTask([FromRoute] Guid taskId)
        {
            return await Mediator.Send(new GetATaskQuery() { Id = taskId });
        }

        /// <summary>
        /// Update a task
        /// </summary>
        /// <param name="updateTaskCommand"></param>
        /// <returns></returns>
        [HttpPut]
        [ActionName("task")]
        [ProducesResponseType(200, Type = typeof(TasksInfoDto))]
        public async Task<ActionResult<TasksInfoDto>> UpdateTask([FromBody] UpdateTaskCommand updateTaskCommand)
        {
            return await Mediator.Send(updateTaskCommand);
        }

        /// <summary>
        /// update the status of a task
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPatch]
        [ActionName("task")]
        [ProducesResponseType(200, Type = typeof(TasksInfoDto))]
        public async Task<ActionResult> UpdateStatusTask([FromQuery] Guid id, [FromQuery] string status)
        {
            await Mediator.Send(new UpdateTaskStatus() { Id = id, Status = status });
            return NoContent();
        }


        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("tasks/{taskId}")]
        [ProducesResponseType(200, Type = typeof(TasksInfoDto))]
        public async Task<ActionResult> DeleteTask([FromRoute] Guid taskId)
        {
            await Mediator.Send(new DeleteTaskCommand() { Id = taskId });
            return Ok("Task deleted");
        }
    }
}
