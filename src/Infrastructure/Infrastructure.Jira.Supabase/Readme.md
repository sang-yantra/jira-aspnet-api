####

connection string

### Scaffold
### 1

Scaffold-DbContext "User Id=postgres;Password=Asupabase123!;Server=db.bhnrieohgidkilckoyyi.supabase.co;Port=5432;Database=postgres" Npgsql.EntityFrameworkCore.PostgreSQL -Context JiraDbContext -ContextDir Persistence -OutputDir Entities/ProjectManagement -Tables "TASK_INFO","PERMISSION","ROLE","ROLE_PERMISSION","ROLE_TEAM","TEAM","USER","USER_ROLE","USER_ROLE_TEAM" -f

### 2
Scaffold-DbContext "User Id=postgres;Password=Asupabase123!;Server=db.bhnrieohgidkilckoyyi.supabase.co;Port=5432;Database=postgres" Npgsql.EntityFrameworkCore.PostgreSQL -Context JiraTestDbContext -ContextDir Persistence -OutputDir Entities -f
