using Jira.Domain.Entities.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Tasks.Commands
{
    public class CreateTaskCommand: IRequest<TasksInfoDto>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AcceptanceCriteria { get; set; }
        public string? Nfr { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public long? OriginalEstimate { get; set; }
        public string? CreatedBy { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? SprintId { get; set; }
        public Guid? UserStoryId { get; set; }
        public Guid? UserId { get; set; }
    }

    public class CreateTaskCommandHandle : IRequestHandler<CreateTaskCommand, TasksInfoDto>
    {
        private readonly IJiraDbContext _context;

        public CreateTaskCommandHandle(IJiraDbContext context)
        {
            _context = context;
        }
        public async Task<TasksInfoDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {

            TaskInfo task = new TaskInfo()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                AcceptanceCriteria = request.AcceptanceCriteria,
                Nfr = request.Nfr,
                Status = request.Status,
                Priority = request.Priority,
                Completed = 0,
                Remaining = request.OriginalEstimate,
                OriginalEstimate = request.OriginalEstimate,
                CreatedDatetime = DateTime.Now,
                CreatedBy = request.CreatedBy,
                UpdatedDatetime = DateTime.Now,
                UpdatedBy = request.CreatedBy,
                TeamId = request.TeamId,
                SprintId = request.SprintId,
                UserStoryId = request.UserStoryId,
                UserId = request.UserId,
            };

            _context.TaskInfos.Add(task);
            await _context.SaveChangesAsync(cancellationToken);

            return new TasksInfoDto()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                AcceptanceCriteria = task.AcceptanceCriteria,
                Nfr = task.Nfr,
                Status = task.Status,
                Priority = task.Priority,
                Completed = task.Completed,
                Remaining = task.Remaining,
                OriginalEstimate = task.OriginalEstimate,
                CreatedDatetime = ((DateTime)task.CreatedDatetime).ToUniversalTime(),
                CreatedBy = task.CreatedBy,
                UpdatedDatetime = ((DateTime)task.UpdatedDatetime).ToUniversalTime(),
                UpdatedBy = task.UpdatedBy,
                TeamId = task.TeamId,
                SprintId = task.SprintId,
                UserStoryId = task.UserStoryId,
                UserId = task.UserId,
            };           
        }
    }
}
