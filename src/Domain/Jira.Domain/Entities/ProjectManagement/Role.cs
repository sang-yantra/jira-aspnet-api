using System;
using System.Collections.Generic;

namespace Jira.Domain.Entities.ProjectManagement
{
    /// <summary>
    /// ROLE TABLE
    /// </summary>
    public partial class Role
    {
        public Role()
        {
            RolePermissions = new HashSet<RolePermission>();
            RoleTeams = new HashSet<RoleTeam>();
            UserRoles = new HashSet<UserRole>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public virtual ICollection<RoleTeam> RoleTeams { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
