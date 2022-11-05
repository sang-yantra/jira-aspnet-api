namespace Microservices.TasksManagement.Entities
{
    /// <summary>
    /// USER TABLE
    /// </summary>
    public partial class User
    {
        public User()
        {
            UserTokens = new HashSet<UserToken>();
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
        public string? AvatarPath { get; set; }
        public string Password { get; set; } = null!;

        public virtual ICollection<UserToken> UserTokens { get; set; }
    }
}
