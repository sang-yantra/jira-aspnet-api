namespace Jira.Domain.Entities.ProjectManagement
{
    /// <summary>
    /// team table
    /// </summary>
    public partial class Team
    {
        public Team()
        {
            Permissions = new HashSet<Permission>();
            RoleTeams = new HashSet<RoleTeam>();
            Sprints = new HashSet<Sprint>();
            TaskInfos = new HashSet<TaskInfo>();
            UserRoleTeams = new HashSet<UserRoleTeam>();
            UserStories = new HashSet<UserStory>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
        public string? UpdatedBy { get; set; }
        public string? LongDescription { get; set; }
        public bool? IsActive { get; set; }
        public string? ImagePath { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<RoleTeam> RoleTeams { get; set; }
        public virtual ICollection<Sprint> Sprints { get; set; }
        public virtual ICollection<TaskInfo> TaskInfos { get; set; }
        public virtual ICollection<UserRoleTeam> UserRoleTeams { get; set; }
        public virtual ICollection<UserStory> UserStories { get; set; }
    }
}
