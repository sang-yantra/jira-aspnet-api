using System;
using System.Collections.Generic;

namespace Jira.Domain.Entities.ProjectManagement
{
    /// <summary>
    /// ROLES OF A TEAM
    /// </summary>
    public partial class RoleTeam
    {
        public Guid Id { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual Team? Team { get; set; }
    }
}
