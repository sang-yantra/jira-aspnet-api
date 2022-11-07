using Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Tasks.Commands
{
    public class UpdateTaskCommand: IRequest<TasksInfoDto>
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AcceptanceCriteria { get; set; }
        public string? Nfr { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public long? Completed { get; set; }
        public long? Remaining { get; set; }
        public long? OriginalEstimate { get; set; }
        public string? UpdatedBy { get; set; }
        public Guid? UserStoryId { get; set; }
    }

    public class UpdateTaskCommandHandle : IRequestHandler<UpdateTaskCommand, TasksInfoDto>
    {
        private readonly IJiraDbContext _context;

        public UpdateTaskCommandHandle(IJiraDbContext context)
        {
            _context = context;
        }
        public async Task<TasksInfoDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.TaskInfos.FirstOrDefaultAsync(x => x.Id == request.Id);

            if(task == null)
            {
                throw new DataNotFoundException("Invalid TaskId" + request.Id);
            }

            var properties = request.GetType().GetProperties()
                .Where(x => x.Name != nameof(request.Id)
                && x.GetValue(request) != null
                );

            foreach(var property in properties)
            {
                var task_prop = task.GetType().GetProperty(property.Name);
                task_prop.SetValue(task, property.GetValue(request));
            }

            _context.TaskInfos.Update(task);
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
