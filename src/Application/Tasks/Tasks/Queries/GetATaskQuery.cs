using Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Tasks.Queries
{
    public class GetATaskQuery: IRequest<TasksInfoDto>
    {
        public Guid Id { get; set; }
    }

    public class GetATaskQueryHandle : IRequestHandler<GetATaskQuery, TasksInfoDto>
    {
        private readonly IJiraDbContext _context;

        public GetATaskQueryHandle(IJiraDbContext context)
        {
            _context = context;
        }

        public async Task<TasksInfoDto> Handle(GetATaskQuery request, CancellationToken cancellationToken)
        {
            TasksInfoDto tasksInfoDto = null;

            var task = await _context.TaskInfos.Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if(task == null)
            {
                throw new DataNotFoundException("Invalid TaskId" + request.Id);
            }

            tasksInfoDto = new TasksInfoDto()
            {
                Id = request.Id,
                Title = task.Title,
                Description = task.Description,
                AcceptanceCriteria = task.AcceptanceCriteria,
                Nfr = task.Nfr,
                Status = task.Status,
                Priority = task.Priority,
                OriginalEstimate = task.OriginalEstimate,
                Completed = task.Completed,
                Remaining = task.Remaining,
                CreatedDatetime = task.CreatedDatetime,
                CreatedBy = task.CreatedBy,
                UpdatedDatetime = task.UpdatedDatetime,
                UpdatedBy = task.UpdatedBy,
                TeamId = task.TeamId,
                SprintId = task.SprintId,
                UserStoryId = task.UserStoryId,
                UserId = task.UserId,
            };

            return tasksInfoDto;
        }
    }
}
