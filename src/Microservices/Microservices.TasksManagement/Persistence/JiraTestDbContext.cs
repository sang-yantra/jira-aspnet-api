using Microservices.TasksManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microservices.TasksManagement.Persistence
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

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserToken> UserTokens { get; set; } = null!;

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
            modelBuilder.HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
                .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
                .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn" })
                .HasPostgresEnum("graphql", "cardinality", new[] { "ONE", "MANY" })
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

                entity.Property(e => e.AvatarPath)
                    .HasColumnType("character varying")
                    .HasColumnName("AVATAR_PATH");

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

                entity.Property(e => e.Password)
                    .HasColumnType("character varying")
                    .HasColumnName("PASSWORD")
                    .HasDefaultValueSql("'user123!'::character varying");

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

            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.ToTable("USER_TOKEN");

                entity.HasComment("token managmenr for user");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.AccessToken)
                    .HasColumnType("character varying")
                    .HasColumnName("ACCESS_TOKEN");

                entity.Property(e => e.AccessTokenExpired).HasColumnName("ACCESS_TOKEN_EXPIRED");

                entity.Property(e => e.RefreshToken)
                    .HasColumnType("character varying")
                    .HasColumnName("REFRESH_TOKEN");

                entity.Property(e => e.RefreshTokenExpired).HasColumnName("REFRESH_TOKEN_EXPIRED");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("USER_TOKEN_USER_ID_fkey");
            });

            modelBuilder.HasSequence<int>("seq_schema_version", "graphql").IsCyclic();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
