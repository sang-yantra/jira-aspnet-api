using System;
using System.Collections.Generic;

namespace Infrastructure.Jira.Supabase.Entities
{
    /// <summary>
    /// user stories of a sprint
    /// </summary>
    public partial class UserStory
    {
        public UserStory()
        {
            TaskInfos = new HashSet<TaskInfo>();
        }

        public Guid Id { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? SprintId { get; set; }
        public Guid? UserId { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AcceptanceCriteria { get; set; }
        public string? Nfr { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public short? Effort { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? UpdaterdDatetime { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual Sprint? Sprint { get; set; }
        public virtual Team? Team { get; set; }
        public virtual User1? User { get; set; }
        public virtual ICollection<TaskInfo> TaskInfos { get; set; }
    }
}
