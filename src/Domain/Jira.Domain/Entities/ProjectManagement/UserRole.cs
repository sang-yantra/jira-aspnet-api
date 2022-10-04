using System;
using System.Collections.Generic;

namespace Jira.Domain.Entities.ProjectManagement
{
    /// <summary>
    /// USER AND ROLE ASSOCIATION
    /// </summary>
    public partial class UserRole
    {
        public UserRole()
        {
            UserRoleTeams = new HashSet<UserRoleTeam>();
        }

        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? RoleId { get; set; }
        public string? Description { get; set; }
        public long? RoleOrder { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual Role? Role { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<UserRoleTeam> UserRoleTeams { get; set; }
    }
}
