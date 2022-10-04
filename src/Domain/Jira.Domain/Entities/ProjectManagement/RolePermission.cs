using System;
using System.Collections.Generic;

namespace Jira.Domain.Entities.ProjectManagement
{
    /// <summary>
    /// ROLE AND PERMISSION ASSOCIATION
    /// </summary>
    public partial class RolePermission
    {
        public Guid Id { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? PermissionId { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual Permission? Permission { get; set; }
        public virtual Role? Role { get; set; }
    }
}
