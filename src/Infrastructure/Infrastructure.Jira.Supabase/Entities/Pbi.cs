using System;
using System.Collections.Generic;

namespace Infrastructure.Jira.Supabase.Entities
{
    /// <summary>
    /// PBI TABLE
    /// </summary>
    public partial class Pbi
    {
        public Guid Id { get; set; }
        public long? DisplayId { get; set; }
        public string Type { get; set; } = null!;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? AcceptanceCriteria { get; set; }
        public string? Nfr { get; set; }
        public long? BusinessValue { get; set; }
        public long? Effort { get; set; }
        public string? Priority { get; set; }
        public Guid? AssignedToId { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual UserRoleTeam? AssignedTo { get; set; }
    }
}
