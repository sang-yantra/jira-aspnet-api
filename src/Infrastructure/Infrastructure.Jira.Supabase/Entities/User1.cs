﻿using System;
using System.Collections.Generic;

namespace Infrastructure.Jira.Supabase.Entities
{
    /// <summary>
    /// USER TABLE
    /// </summary>
    public partial class User1
    {
        public User1()
        {
            TaskInfos = new HashSet<TaskInfo>();
            UserRoleTeams = new HashSet<UserRoleTeam>();
            UserRoles = new HashSet<UserRole>();
            UserStories = new HashSet<UserStory>();
        }

        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Lastname { get; set; }
        public string? Alias { get; set; }
        public string? Email { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSuperAdmin { get; set; }

        public virtual ICollection<TaskInfo> TaskInfos { get; set; }
        public virtual ICollection<UserRoleTeam> UserRoleTeams { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserStory> UserStories { get; set; }
    }
}
