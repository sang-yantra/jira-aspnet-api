using System;
using System.Collections.Generic;

namespace Infrastructure.Jira.Supabase.Entities
{
    public partial class Bucket
    {
        public Bucket()
        {
            Objects = new HashSet<Object>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public Guid? Owner { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? Public { get; set; }

        public virtual User? OwnerNavigation { get; set; }
        public virtual ICollection<Object> Objects { get; set; }
    }
}
