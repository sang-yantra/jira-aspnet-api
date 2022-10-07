using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Jira.Domain.Entities.ProjectManagement;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Admin.DAL.Interfaces;
using System.Reflection;

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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
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


            //// dynamically adding configurations from assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.HasSequence<int>("seq_schema_version", "graphql").IsCyclic();
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
