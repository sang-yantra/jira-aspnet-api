namespace Jira.Domain.Entities.ProjectManagement
{
    /// <summary>
    /// sprint table
    /// </summary>
    public partial class Sprint
    {
        public Sprint()
        {
            TaskInfos = new HashSet<TaskInfo>();
            UserStories = new HashSet<UserStory>();
        }

        public Guid Id { get; set; }
        public Guid? TeamId { get; set; }
        public long? SprintNumber { get; set; }
        public string? Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? Capacity { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual Team? Team { get; set; }
        public virtual ICollection<TaskInfo> TaskInfos { get; set; }
        public virtual ICollection<UserStory> UserStories { get; set; }
    }
}
