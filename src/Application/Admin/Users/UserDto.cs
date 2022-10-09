namespace Admin.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Lastname { get; set; }
        public string? Alias { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSuperAdmin { get; set; }
    }
}
