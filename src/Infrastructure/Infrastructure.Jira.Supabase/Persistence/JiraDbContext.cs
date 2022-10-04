using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Jira.Domain.Entities.ProjectManagement;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Admin.DAL.Interfaces;

namespace Microservice.Admin.Persistence
{
    public partial class JiraDbContext : DbContext, IJiraDbContext
    {
        public JiraDbContext()
        {
        }

        public JiraDbContext(DbContextOptions<JiraDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Permission> Permissions { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public virtual DbSet<RoleTeam> RoleTeams { get; set; } = null!;
        public virtual DbSet<TaskInfo> TaskInfos { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;
        public virtual DbSet<UserRoleTeam> UserRoleTeams { get; set; } = null!;


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

                entity.Property(e => e.Status)
                    .HasColumnType("character varying")
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("'ACTIVE'::character varying");

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

            modelBuilder.HasSequence<int>("seq_schema_version", "graphql").IsCyclic();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
