using System;
using System.Collections.Generic;

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
            UserRoleTeams = new HashSet<UserRoleTeam>();
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
        public virtual ICollection<UserRoleTeam> UserRoleTeams { get; set; }
    }
}
