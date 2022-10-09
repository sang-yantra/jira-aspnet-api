using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Infrastructure.Jira.Supabase.Entities;

namespace Infrastructure.Jira.Supabase.Persistence
{
    public partial class JiraTestDbContext : DbContext
    {
        public JiraTestDbContext()
        {
        }

        public JiraTestDbContext(DbContextOptions<JiraTestDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuditLogEntry> AuditLogEntries { get; set; } = null!;
        public virtual DbSet<Bucket> Buckets { get; set; } = null!;
        public virtual DbSet<Identity> Identities { get; set; } = null!;
        public virtual DbSet<Instance> Instances { get; set; } = null!;
        public virtual DbSet<Migration> Migrations { get; set; } = null!;
        public virtual DbSet<Pbi> Pbis { get; set; } = null!;
        public virtual DbSet<Permission> Permissions { get; set; } = null!;
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public virtual DbSet<RoleTeam> RoleTeams { get; set; } = null!;
        public virtual DbSet<SchemaMigration> SchemaMigrations { get; set; } = null!;
        public virtual DbSet<Session> Sessions { get; set; } = null!;
        public virtual DbSet<Sprint> Sprints { get; set; } = null!;
        public virtual DbSet<TaskInfo> TaskInfos { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<User1> Users1 { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;
        public virtual DbSet<UserRoleTeam> UserRoleTeams { get; set; } = null!;
        public virtual DbSet<UserStory> UserStories { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("User Id=postgres;Password=Asupabase123!;Server=db.bhnrieohgidkilckoyyi.supabase.co;Port=5432;Database=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum("graphql", "cardinality", new[] { "ONE", "MANY" })
                .HasPostgresEnum("graphql", "column_order_direction", new[] { "asc", "desc" })
                .HasPostgresEnum("graphql", "comparison_op", new[] { "=", "<", "<=", "<>", ">=", ">", "in" })
                .HasPostgresEnum("graphql", "field_meta_kind", new[] { "Constant", "Query.collection", "Column", "Relationship.toMany", "Relationship.toOne", "OrderBy.Column", "Filter.Column", "Function", "Mutation.insert", "Mutation.delete", "Mutation.update", "UpdateSetArg", "ObjectsArg", "AtMostArg", "Query.heartbeat", "Query.__schema", "Query.__type", "__Typename" })
                .HasPostgresEnum("graphql", "meta_kind", new[] { "__Schema", "__Type", "__TypeKind", "__Field", "__InputValue", "__EnumValue", "__Directive", "__DirectiveLocation", "ID", "Float", "String", "Int", "Boolean", "Date", "Time", "Datetime", "BigInt", "UUID", "JSON", "OrderByDirection", "PageInfo", "Cursor", "Query", "Mutation", "Interface", "Node", "Edge", "Connection", "OrderBy", "FilterEntity", "InsertNode", "UpdateNode", "InsertNodeResponse", "UpdateNodeResponse", "DeleteNodeResponse", "FilterType", "Enum" })
                .HasPostgresEnum("graphql", "type_kind", new[] { "SCALAR", "OBJECT", "INTERFACE", "UNION", "ENUM", "INPUT_OBJECT", "LIST", "NON_NULL" })
                .HasPostgresEnum("pgsodium", "key_status", new[] { "default", "valid", "invalid", "expired" })
                .HasPostgresEnum("pgsodium", "key_type", new[] { "aead-ietf", "aead-det" })
                .HasPostgresExtension("extensions", "pg_graphql")
                .HasPostgresExtension("extensions", "pg_stat_statements")
                .HasPostgresExtension("extensions", "pgcrypto")
                .HasPostgresExtension("extensions", "pgjwt")
                .HasPostgresExtension("extensions", "uuid-ossp")
                .HasPostgresExtension("pgsodium", "pgsodium");

            modelBuilder.Entity<AuditLogEntry>(entity =>
            {
                entity.ToTable("audit_log_entries", "auth");

                entity.HasComment("Auth: Audit trail for user actions.");

                entity.HasIndex(e => e.InstanceId, "audit_logs_instance_id_idx");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.InstanceId).HasColumnName("instance_id");

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(64)
                    .HasColumnName("ip_address")
                    .HasDefaultValueSql("''::character varying");

                entity.Property(e => e.Payload)
                    .HasColumnType("json")
                    .HasColumnName("payload");
            });

            modelBuilder.Entity<Bucket>(entity =>
            {
                entity.ToTable("buckets", "storage");

                entity.HasIndex(e => e.Name, "bname")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Owner).HasColumnName("owner");

                entity.Property(e => e.Public)
                    .HasColumnName("public")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Buckets)
                    .HasForeignKey(d => d.Owner)
                    .HasConstraintName("buckets_owner_fkey");
            });

            modelBuilder.Entity<Identity>(entity =>
            {
                entity.HasKey(e => new { e.Provider, e.Id })
                    .HasName("identities_pkey");

                entity.ToTable("identities", "auth");

                entity.HasComment("Auth: Stores identities associated to a user.");

                entity.HasIndex(e => e.UserId, "identities_user_id_idx");

                entity.Property(e => e.Provider).HasColumnName("provider");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.IdentityData)
                    .HasColumnType("jsonb")
                    .HasColumnName("identity_data");

                entity.Property(e => e.LastSignInAt).HasColumnName("last_sign_in_at");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Identities)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("identities_user_id_fkey");
            });

            modelBuilder.Entity<Instance>(entity =>
            {
                entity.ToTable("instances", "auth");

                entity.HasComment("Auth: Manages users across multiple sites.");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.RawBaseConfig).HasColumnName("raw_base_config");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.Uuid).HasColumnName("uuid");
            });

            modelBuilder.Entity<Migration>(entity =>
            {
                entity.ToTable("migrations", "storage");

                entity.HasIndex(e => e.Name, "migrations_name_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ExecutedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("executed_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Hash)
                    .HasMaxLength(40)
                    .HasColumnName("hash");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });


            modelBuilder.Entity<Pbi>(entity =>
            {
                entity.ToTable("PBI");

                entity.HasComment("PBI TABLE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.AcceptanceCriteria)
                    .HasColumnType("character varying")
                    .HasColumnName("ACCEPTANCE_CRITERIA");

                entity.Property(e => e.AssignedToId).HasColumnName("ASSIGNED_TO_ID");

                entity.Property(e => e.BusinessValue).HasColumnName("BUSINESS_VALUE");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("CREATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.Property(e => e.Description)
                    .HasColumnType("character varying")
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayId).HasColumnName("DISPLAY_ID");

                entity.Property(e => e.Effort).HasColumnName("EFFORT");

                entity.Property(e => e.Nfr)
                    .HasColumnType("character varying")
                    .HasColumnName("NFR");

                entity.Property(e => e.Priority)
                    .HasColumnType("character varying")
                    .HasColumnName("PRIORITY")
                    .HasDefaultValueSql("'LOW'::character varying");

                entity.Property(e => e.Status)
                    .HasColumnType("character varying")
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("'NEW'::character varying");

                entity.Property(e => e.Title)
                    .HasColumnType("character varying")
                    .HasColumnName("TITLE");

                entity.Property(e => e.Type)
                    .HasColumnType("character varying")
                    .HasColumnName("TYPE")
                    .HasDefaultValueSql("'NORMAL'::character varying");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("UPDATED_BY");

                entity.Property(e => e.UpdatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("UPDATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.HasOne(d => d.AssignedTo)
                    .WithMany(p => p.Pbis)
                    .HasForeignKey(d => d.AssignedToId)
                    .HasConstraintName("PBI_ASSIGNED_TO_ID_fkey");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("PERMISSION");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("CREATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.Property(e => e.Description)
                    .HasColumnType("character varying")
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Name)
                    .HasColumnType("character varying")
                    .HasColumnName("NAME");

                entity.Property(e => e.TeamId).HasColumnName("TEAM_ID");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("UPDATED_BY");

                entity.Property(e => e.UpdatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("UPDATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Permissions)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("PERMISSION_TEAM_ID_fkey");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("refresh_tokens", "auth");

                entity.HasComment("Auth: Store of tokens used to refresh JWT tokens once they expire.");

                entity.HasIndex(e => e.InstanceId, "refresh_tokens_instance_id_idx");

                entity.HasIndex(e => new { e.InstanceId, e.UserId }, "refresh_tokens_instance_id_user_id_idx");

                entity.HasIndex(e => e.Parent, "refresh_tokens_parent_idx");

                entity.HasIndex(e => e.Token, "refresh_tokens_token_idx");

                entity.HasIndex(e => e.Token, "refresh_tokens_token_unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.InstanceId).HasColumnName("instance_id");

                entity.Property(e => e.Parent)
                    .HasMaxLength(255)
                    .HasColumnName("parent");

                entity.Property(e => e.Revoked).HasColumnName("revoked");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.Token)
                    .HasMaxLength(255)
                    .HasColumnName("token");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UserId)
                    .HasMaxLength(255)
                    .HasColumnName("user_id");

                entity.HasOne(d => d.ParentNavigation)
                    .WithMany(p => p.InverseParentNavigation)
                    .HasPrincipalKey(p => p.Token)
                    .HasForeignKey(d => d.Parent)
                    .HasConstraintName("refresh_tokens_parent_fkey");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("refresh_tokens_session_id_fkey");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("ROLE");

                entity.HasComment("ROLE TABLE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("CREATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.Property(e => e.Description)
                    .HasColumnType("character varying")
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Name)
                    .HasColumnType("character varying")
                    .HasColumnName("NAME");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("UPDATED_BY");

                entity.Property(e => e.UpdatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("UPDATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("ROLE_PERMISSION");

                entity.HasComment("ROLE AND PERMISSION ASSOCIATION");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("CREATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.Property(e => e.PermissionId).HasColumnName("PERMISSION_ID");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("UPDATED_BY");

                entity.Property(e => e.UpdatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("UPDATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("ROLE_PERMISSION_PERMISSION_ID_fkey");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("ROLE_PERMISSION_ROLE_ID_fkey");
            });

            modelBuilder.Entity<RoleTeam>(entity =>
            {
                entity.ToTable("ROLE_TEAM");

                entity.HasComment("ROLES OF A TEAM");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.Property(e => e.TeamId).HasColumnName("TEAM_ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleTeams)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("ROLE_TEAM_ROLE_ID_fkey");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.RoleTeams)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("ROLE_TEAM_TEAM_ID_fkey");
            });

            modelBuilder.Entity<SchemaMigration>(entity =>
            {
                entity.HasKey(e => e.Version)
                    .HasName("schema_migrations_pkey");

                entity.ToTable("schema_migrations", "auth");

                entity.HasComment("Auth: Manages updates to the auth system.");

                entity.Property(e => e.Version)
                    .HasMaxLength(255)
                    .HasColumnName("version");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("sessions", "auth");

                entity.HasComment("Auth: Stores session data associated to a user.");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("sessions_user_id_fkey");
            });

            modelBuilder.Entity<Sprint>(entity =>
            {
                entity.ToTable("SPRINT");

                entity.HasComment("sprint table");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Capacity).HasColumnName("CAPACITY");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("CREATED_DATETIME")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.EndDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("END_DATE");

                entity.Property(e => e.SprintNumber).HasColumnName("SPRINT_NUMBER");

                entity.Property(e => e.StartDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("START_DATE");

                entity.Property(e => e.TeamId).HasColumnName("TEAM_ID");

                entity.Property(e => e.Title)
                    .HasColumnType("character varying")
                    .HasColumnName("TITLE");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("UPDATED_BY");

                entity.Property(e => e.UpdatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("UPDATED_DATETIME")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Sprints)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("SPRINT_TEAM_ID_fkey");
            });

            modelBuilder.Entity<TaskInfo>(entity =>
            {
                entity.ToTable("TASK_INFO");

                entity.HasComment("Task table");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.AcceptanceCriteria)
                    .HasColumnType("character varying")
                    .HasColumnName("ACCEPTANCE_CRITERIA");

                entity.Property(e => e.Completed).HasColumnName("COMPLETED");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("CREATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.Property(e => e.Description)
                    .HasColumnType("character varying")
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Nfr)
                    .HasColumnType("character varying")
                    .HasColumnName("NFR");

                entity.Property(e => e.OriginalEstimate).HasColumnName("ORIGINAL_ESTIMATE");

                entity.Property(e => e.Priority)
                    .HasColumnType("character varying")
                    .HasColumnName("PRIORITY")
                    .HasDefaultValueSql("'LOW'::character varying");

                entity.Property(e => e.Remaining).HasColumnName("REMAINING");

                entity.Property(e => e.SprintId).HasColumnName("SPRINT_ID");

                entity.Property(e => e.Status)
                    .HasColumnType("character varying")
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("'ACTIVE'::character varying");

                entity.Property(e => e.TeamId).HasColumnName("TEAM_ID");

                entity.Property(e => e.Title)
                    .HasColumnType("character varying")
                    .HasColumnName("TITLE");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("UPDATED_BY");

                entity.Property(e => e.UpdatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("UPDATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.Property(e => e.UserStoryId).HasColumnName("USER_STORY_ID");

                entity.HasOne(d => d.Sprint)
                    .WithMany(p => p.TaskInfos)
                    .HasForeignKey(d => d.SprintId)
                    .HasConstraintName("TASK_INFO_SPRINT_ID_fkey");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TaskInfos)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("TASK_INFO_TEAM_ID_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaskInfos)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("TASK_INFO_USER_ID_fkey");

                entity.HasOne(d => d.UserStory)
                    .WithMany(p => p.TaskInfos)
                    .HasForeignKey(d => d.UserStoryId)
                    .HasConstraintName("TASK_INFO_USER_STORY_ID_fkey");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("TEAM");

                entity.HasComment("team table");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Code)
                    .HasColumnType("character varying")
                    .HasColumnName("CODE");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("CREATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.Property(e => e.Description)
                    .HasColumnType("character varying")
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.ImagePath)
                    .HasColumnType("character varying")
                    .HasColumnName("IMAGE_PATH");

                entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE");

                entity.Property(e => e.LongDescription).HasColumnName("LONG_DESCRIPTION");

                entity.Property(e => e.Name)
                    .HasColumnType("character varying")
                    .HasColumnName("NAME");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("UPDATED_BY");

                entity.Property(e => e.UpdatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("UPDATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users", "auth");

                entity.HasComment("Auth: Stores user login data within a secure schema.");

                entity.HasIndex(e => e.ConfirmationToken, "confirmation_token_idx")
                    .IsUnique()
                    .HasFilter("((confirmation_token)::text !~ '^[0-9 ]*$'::text)");

                entity.HasIndex(e => e.EmailChangeTokenCurrent, "email_change_token_current_idx")
                    .IsUnique()
                    .HasFilter("((email_change_token_current)::text !~ '^[0-9 ]*$'::text)");

                entity.HasIndex(e => e.EmailChangeTokenNew, "email_change_token_new_idx")
                    .IsUnique()
                    .HasFilter("((email_change_token_new)::text !~ '^[0-9 ]*$'::text)");

                entity.HasIndex(e => e.ReauthenticationToken, "reauthentication_token_idx")
                    .IsUnique()
                    .HasFilter("((reauthentication_token)::text !~ '^[0-9 ]*$'::text)");

                entity.HasIndex(e => e.RecoveryToken, "recovery_token_idx")
                    .IsUnique()
                    .HasFilter("((recovery_token)::text !~ '^[0-9 ]*$'::text)");

                entity.HasIndex(e => e.Email, "users_email_key")
                    .IsUnique();

                entity.HasIndex(e => e.InstanceId, "users_instance_id_idx");

                entity.HasIndex(e => e.Phone, "users_phone_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Aud)
                    .HasMaxLength(255)
                    .HasColumnName("aud");

                entity.Property(e => e.BannedUntil).HasColumnName("banned_until");

                entity.Property(e => e.ConfirmationSentAt).HasColumnName("confirmation_sent_at");

                entity.Property(e => e.ConfirmationToken)
                    .HasMaxLength(255)
                    .HasColumnName("confirmation_token");

                entity.Property(e => e.ConfirmedAt)
                    .HasColumnName("confirmed_at")
                    .HasComputedColumnSql("LEAST(email_confirmed_at, phone_confirmed_at)", true);

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.EmailChange)
                    .HasMaxLength(255)
                    .HasColumnName("email_change");

                entity.Property(e => e.EmailChangeConfirmStatus)
                    .HasColumnName("email_change_confirm_status")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.EmailChangeSentAt).HasColumnName("email_change_sent_at");

                entity.Property(e => e.EmailChangeTokenCurrent)
                    .HasMaxLength(255)
                    .HasColumnName("email_change_token_current")
                    .HasDefaultValueSql("''::character varying");

                entity.Property(e => e.EmailChangeTokenNew)
                    .HasMaxLength(255)
                    .HasColumnName("email_change_token_new");

                entity.Property(e => e.EmailConfirmedAt).HasColumnName("email_confirmed_at");

                entity.Property(e => e.EncryptedPassword)
                    .HasMaxLength(255)
                    .HasColumnName("encrypted_password");

                entity.Property(e => e.InstanceId).HasColumnName("instance_id");

                entity.Property(e => e.InvitedAt).HasColumnName("invited_at");

                entity.Property(e => e.IsSuperAdmin).HasColumnName("is_super_admin");

                entity.Property(e => e.LastSignInAt).HasColumnName("last_sign_in_at");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .HasColumnName("phone")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.PhoneChange)
                    .HasMaxLength(15)
                    .HasColumnName("phone_change")
                    .HasDefaultValueSql("''::character varying");

                entity.Property(e => e.PhoneChangeSentAt).HasColumnName("phone_change_sent_at");

                entity.Property(e => e.PhoneChangeToken)
                    .HasMaxLength(255)
                    .HasColumnName("phone_change_token")
                    .HasDefaultValueSql("''::character varying");

                entity.Property(e => e.PhoneConfirmedAt).HasColumnName("phone_confirmed_at");

                entity.Property(e => e.RawAppMetaData)
                    .HasColumnType("jsonb")
                    .HasColumnName("raw_app_meta_data");

                entity.Property(e => e.RawUserMetaData)
                    .HasColumnType("jsonb")
                    .HasColumnName("raw_user_meta_data");

                entity.Property(e => e.ReauthenticationSentAt).HasColumnName("reauthentication_sent_at");

                entity.Property(e => e.ReauthenticationToken)
                    .HasMaxLength(255)
                    .HasColumnName("reauthentication_token")
                    .HasDefaultValueSql("''::character varying");

                entity.Property(e => e.RecoverySentAt).HasColumnName("recovery_sent_at");

                entity.Property(e => e.RecoveryToken)
                    .HasMaxLength(255)
                    .HasColumnName("recovery_token");

                entity.Property(e => e.Role)
                    .HasMaxLength(255)
                    .HasColumnName("role");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<User1>(entity =>
            {
                entity.ToTable("USER");

                entity.HasComment("USER TABLE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Alias)
                    .HasColumnType("character varying")
                    .HasColumnName("ALIAS");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("CREATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.Property(e => e.Email)
                    .HasColumnType("character varying")
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Firstname)
                    .HasColumnType("character varying")
                    .HasColumnName("FIRSTNAME");

                entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE");

                entity.Property(e => e.IsSuperAdmin).HasColumnName("IS_SUPER_ADMIN");

                entity.Property(e => e.Lastname)
                    .HasColumnType("character varying")
                    .HasColumnName("LASTNAME");

                entity.Property(e => e.Middlename)
                    .HasColumnType("character varying")
                    .HasColumnName("MIDDLENAME");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("UPDATED_BY");

                entity.Property(e => e.UpdatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("UPDATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.Property(e => e.Username)
                    .HasColumnType("character varying")
                    .HasColumnName("USERNAME");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("USER_ROLE");

                entity.HasComment("USER AND ROLE ASSOCIATION");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("CREATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.Property(e => e.Description)
                    .HasColumnType("character varying")
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.Property(e => e.RoleOrder).HasColumnName("ROLE_ORDER");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("UPDATED_BY");

                entity.Property(e => e.UpdatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("UPDATED_DATETIME")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("USER_ROLE_ROLE_ID_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("USER_ROLE_USER_ID_fkey");
            });

            modelBuilder.Entity<UserRoleTeam>(entity =>
            {
                entity.ToTable("USER_ROLE_TEAM");

                entity.HasComment("USER ROLE TEAM ASSOCIATION");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.TeamId).HasColumnName("TEAM_ID");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.Property(e => e.UserRoleId).HasColumnName("USER_ROLE_ID");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.UserRoleTeams)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("USER_ROLE_TEAM_TEAM_ID_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoleTeams)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("USER_ROLE_TEAM_USER_ID_fkey");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.UserRoleTeams)
                    .HasForeignKey(d => d.UserRoleId)
                    .HasConstraintName("USER_ROLE_TEAM_USER_ROLE_ID_fkey");
            });

            modelBuilder.Entity<UserStory>(entity =>
            {
                entity.ToTable("USER_STORY");

                entity.HasComment("user stories of a sprint");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.AcceptanceCriteria)
                    .HasColumnType("character varying")
                    .HasColumnName("ACCEPTANCE_CRITERIA");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("CREATED_BY")
                    .HasDefaultValueSql("'testUser1'::character varying");

                entity.Property(e => e.CreatedDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("CREATED_DATETIME")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnType("character varying")
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Effort).HasColumnName("EFFORT");

                entity.Property(e => e.Nfr)
                    .HasColumnType("character varying")
                    .HasColumnName("NFR");

                entity.Property(e => e.Priority)
                    .HasColumnType("character varying")
                    .HasColumnName("PRIORITY");

                entity.Property(e => e.SprintId).HasColumnName("SPRINT_ID");

                entity.Property(e => e.Status)
                    .HasColumnType("character varying")
                    .HasColumnName("STATUS");

                entity.Property(e => e.TeamId).HasColumnName("TEAM_ID");

                entity.Property(e => e.Title)
                    .HasColumnType("character varying")
                    .HasColumnName("TITLE");

                entity.Property(e => e.Type)
                    .HasColumnType("character varying")
                    .HasColumnName("TYPE");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("UPDATED_BY")
                    .HasDefaultValueSql("'testUser1'::character varying");

                entity.Property(e => e.UpdaterdDatetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("UPDATERD_DATETIME")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.HasOne(d => d.Sprint)
                    .WithMany(p => p.UserStories)
                    .HasForeignKey(d => d.SprintId)
                    .HasConstraintName("USER_STORY_SPRINT_ID_fkey");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.UserStories)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("USER_STORY_TEAM_ID_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserStories)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("USER_STORY_USER_ID_fkey");
            });

            modelBuilder.HasSequence<int>("seq_schema_version", "graphql").IsCyclic();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
