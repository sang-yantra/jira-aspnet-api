using Jira.Domain.Entities.ChatManagement;
using Jira.Domain.Entities.ProjectManagement;
using Microsoft.EntityFrameworkCore;

namespace Common.Interfaces
{
    public interface IJiraDbContext
    {
        DbSet<Permission> Permissions { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<RolePermission> RolePermissions { get; set; }
        DbSet<RoleTeam> RoleTeams { get; set; }
        DbSet<Sprint> Sprints { get; set; }
        DbSet<TaskInfo> TaskInfos { get; set; }
        DbSet<Team> Teams { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<UserRoleTeam> UserRoleTeams { get; set; }
        DbSet<UserStory> UserStories { get; set; }
        DbSet<Chat> Chats { get; set; }
        DbSet<ChatRoom> ChatRooms { get; set; }
        DbSet<ChatRoomUser> ChatRoomUsers { get; set; }
        DbSet<UserToken> UserTokens { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
