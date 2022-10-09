using System;
using System.Collections.Generic;

namespace Infrastructure.Jira.Supabase.Entities
{
    /// <summary>
    /// USER ROLE TEAM ASSOCIATION
    /// </summary>
    public partial class UserRoleTeam
    {
        public UserRoleTeam()
        {
            Pbis = new HashSet<Pbi>();
        }

        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? UserRoleId { get; set; }

        public virtual Team? Team { get; set; }
        public virtual User1? User { get; set; }
        public virtual UserRole? UserRole { get; set; }
        public virtual ICollection<Pbi> Pbis { get; set; }
    }
}
