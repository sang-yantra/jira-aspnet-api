namespace Microservices.TasksManagement.Entities
{
    /// <summary>
    /// token managmenr for user
    /// </summary>
    public partial class UserToken
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string? AccessToken { get; set; }
        public long? AccessTokenExpired { get; set; }
        public string? RefreshToken { get; set; }
        public long? RefreshTokenExpired { get; set; }

        public virtual User? User { get; set; }
    }
}
