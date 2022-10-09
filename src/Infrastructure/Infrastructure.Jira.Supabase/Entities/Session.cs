using System;
using System.Collections.Generic;

namespace Infrastructure.Jira.Supabase.Entities
{
    /// <summary>
    /// Auth: Stores session data associated to a user.
    /// </summary>
    public partial class Session
    {
        public Session()
        {
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
