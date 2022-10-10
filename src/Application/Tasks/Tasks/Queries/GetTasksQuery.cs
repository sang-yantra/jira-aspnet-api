using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Tasks.Queries
{
    public class GetTasksQuery: IRequest<List<TasksKanbanDto>>
    {
        public Guid TeamId { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;

    }

    public class GetTasksQueryHandler: IRequestHandler<GetTasksQuery, List<TasksKanbanDto>>
    {
        private readonly IJiraDbContext _context;

        public GetTasksQueryHandler(IJiraDbContext context)
        {
            _context = context;
        }
        public async Task<List<TasksKanbanDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var getTasksKanbanan = await _context.TaskInfos.Where(x => x.TeamId == request.TeamId)
                .Include(x => x.Sprint)
                .Include(x => x.Team)
                .Select(x => new TasksKanbanDto()
                {
                    TaskId = x.Id,
                    TeamId = x.TeamId,
                    UserStoryId = x.UserStoryId,
                    TaskUserId = x.UserId,
                    UserStoryUserId = x.UserStory != null ? x.UserStory.UserId : null,
                    UserStoryTitle = x.UserStory != null ? x.UserStory.Title : null,
                    UserStoryType = x.UserStory != null ? x.UserStory.Type : null,
                    UserStoryStatus = x.UserStory != null ? x.UserStory.Status : null,
                    UserStoryPriority = x.UserStory != null ? x.UserStory.Priority : null,
                    UserStoryEffort = x.UserStory != null ? x.UserStory.Effort : null,
                    TaskTitle = x.Title,
                    TaskStatus = x.Status,
                    TaskPriority = x.Priority,
                    TaskOriginalEstimate = x.OriginalEstimate,
                    TaskCompleted = x.Completed,

                })
                .ToListAsync(cancellationToken);

            var getDistinctTaskUserIds = getTasksKanbanan.Select(x => new { UserId = x.TaskUserId })
                .Distinct()
                .ToList();

            var getDistinctUserStoryUserIds = getTasksKanbanan.Select(x => new { UserId = x.UserStoryUserId })
                .Distinct()
                .ToList();

            getDistinctTaskUserIds.AddRange(getDistinctUserStoryUserIds);
            var distinctUserIds = getDistinctTaskUserIds.Distinct().ToList();


            var getUserDetailsInfo = from usertask in distinctUserIds
                                     join user in _context.Users
                                        on usertask.UserId equals user.Id
                                     select new
                                     { 
                                         UserId = user.Id, 
                                         Username = user.Username
                                     };

            getUserDetailsInfo = getUserDetailsInfo.ToList();

            getTasksKanbanan.ForEach(x =>
            {
                x.TaskUsername = getUserDetailsInfo.FirstOrDefault(y => y.UserId == x.TaskUserId)?.Username;
                x.UserStoryUsername = getUserDetailsInfo.FirstOrDefault(y => y.UserId==x.UserStoryUserId)?.Username;

            });

            return getTasksKanbanan;
        }

        public dynamic? NullPropertCheck(object obj, string propertyName)
        {
            return obj?.GetType()
                .GetProperty(propertyName)
                .GetValue(obj, null);
        }
    }
}
