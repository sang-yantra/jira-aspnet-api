using System;
using System.Collections.Generic;

namespace Jira.Domain.Entities.ProjectManagement
{
    /// <summary>
    /// USER ROLE TEAM ASSOCIATION
    /// </summary>
    public partial class UserRoleTeam
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? UserRoleId { get; set; }

        public virtual Team? Team { get; set; }
        public virtual User? User { get; set; }
        public virtual UserRole? UserRole { get; set; }
    }
}
