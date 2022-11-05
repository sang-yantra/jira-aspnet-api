namespace Tasks.Tasks
{
    public class TasksKanbanDto
    {
        public Guid? TaskId { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? UserStoryId { get; set; }
        public Guid? UserStoryUserId { get; set; }
        public Guid? TaskUserId { get; set; }
        public string? UserStoryTitle { get; set; }
        public string? UserStoryType { get; set; }
        public string? UserStoryUsername { get; set; }
        public string? UserStoryStatus { get; set; }
        public string? UserStoryPriority { get; set; }
        public short? UserStoryEffort { get; set; }
        public string? TaskTitle { get; set; }
        public string? TaskStatus { get; set; }
        public string? TaskPriority { get; set; }
        public long? TaskOriginalEstimate { get; set; }
        public long? TaskCompleted { get; set; }
        public string? TaskUsername { get; set; }
    }
}
