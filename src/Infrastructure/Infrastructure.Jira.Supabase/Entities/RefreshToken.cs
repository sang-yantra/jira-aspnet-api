using System;
using System.Collections.Generic;

namespace Infrastructure.Jira.Supabase.Entities
{
    /// <summary>
    /// Auth: Store of tokens used to refresh JWT tokens once they expire.
    /// </summary>
    public partial class RefreshToken
    {
        public RefreshToken()
        {
            InverseParentNavigation = new HashSet<RefreshToken>();
        }

        public Guid? InstanceId { get; set; }
        public long Id { get; set; }
        public string Token { get; set; } = null!;
        public string? UserId { get; set; }
        public bool? Revoked { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Parent { get; set; }
        public Guid? SessionId { get; set; }

        public virtual RefreshToken? ParentNavigation { get; set; }
        public virtual Session? Session { get; set; }
        public virtual ICollection<RefreshToken> InverseParentNavigation { get; set; }
    }
}
