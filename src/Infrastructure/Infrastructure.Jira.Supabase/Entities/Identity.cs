using System;
using System.Collections.Generic;

namespace Infrastructure.Jira.Supabase.Entities
{
    /// <summary>
    /// Auth: Stores identities associated to a user.
    /// </summary>
    public partial class Identity
    {
        public string Id { get; set; } = null!;
        public Guid UserId { get; set; }
        public string IdentityData { get; set; } = null!;
        public string Provider { get; set; } = null!;
        public DateTime? LastSignInAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
