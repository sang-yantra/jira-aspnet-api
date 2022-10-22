using Bogus;


namespace Admin.Users.Commands
{
    public class CreateUserCommandQuery : IRequest<UserDto>
    {
        public string? Username { get; set; }
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Lastname { get; set; }
        public string? Alias { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSuperAdmin { get; set; }
        public string? Avatar { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandQuery, UserDto>
    {
        private readonly IJiraDbContext _context;
        public CreateUserCommandHandler(IJiraDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> Handle(CreateUserCommandQuery command, CancellationToken cancellationToken)
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                Username = command.Username,
                Firstname = command.Firstname,
                Middlename = command.Middlename,
                Lastname = command.Lastname,
                Alias = command.Alias ?? command.Username.Substring(0, 3) + DateTime.Now.Millisecond.ToString().Substring(0, 3),
                Email = command.Email,
                CreatedDatetime = DateTime.Now,
                UpdatedDatetime = DateTime.Now,
                CreatedBy = "testUser1",
                UpdatedBy = "testUser1",
                IsActive = command.IsActive,
                IsSuperAdmin = command.IsSuperAdmin,
                AvatarPath = command.Avatar
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync(cancellationToken);

            //// create a user dto response
            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                Username = user.Username,
                Firstname = user.Firstname,
                Middlename = user.Middlename,
                Lastname = user.Lastname,
                Alias = user.Alias,
                Email = user.Email,
                IsActive = user.IsActive,
                IsSuperAdmin = user.IsSuperAdmin,
                Avatar = user.AvatarPath
            };

            return userDto;
        }


        public void Create1000FakeUser(IJiraDbContext context)
        {
            for (int i = 0; i < 1000; i++)
            {
                var fakeUser = new Faker<User>()
                    .RuleFor(x => x.Id, f => Guid.NewGuid())
                    .RuleFor(x => x.Username, f => f.Person.UserName)
                    .RuleFor(x => x.Firstname, f => f.Person.FirstName)
                    .RuleFor(x => x.Lastname, f => f.Person.LastName)
                    .RuleFor(x => x.Alias, f => f.Random.AlphaNumeric(6))
                    .RuleFor(x => x.Email, f => f.Person.Email)
                    .RuleFor(x => x.IsActive, f => true)
                    .RuleFor(x => x.IsSuperAdmin, f => false)
                    .RuleFor(x => x.AvatarPath, f => f.Internet.Avatar())
                    .RuleFor(x => x.CreatedDatetime, f => DateTime.Now)
                    .RuleFor(x => x.UpdatedDatetime, f => DateTime.Now)
                    .RuleFor(x => x.CreatedBy, f => "testUser1")
                    .RuleFor(x => x.UpdatedBy, f => "testUser1");

                var user = fakeUser.Generate();
                context.Users.Add(user);

            }
        }
    }
}
