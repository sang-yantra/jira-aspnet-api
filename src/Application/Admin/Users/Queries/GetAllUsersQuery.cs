namespace Admin.Users.Queries
{
    public class GetAllUsersQuery : IRequest<List<UserDto>>
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }

    public class GetAllUsersQueryHandle : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly IJiraDbContext _context;
        public GetAllUsersQueryHandle(IJiraDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserDto>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            IQueryable<User> getUsersQueryWhere;

            if (query.UserName is null && query.Email is null)
            {
                getUsersQueryWhere = _context.Users;
            }
            else
            {
                getUsersQueryWhere = _context.Users
                            .Where(x => x.Username.Contains(query.UserName) ||
                            x.Email.Contains(query.Email));
            }


            var users = await getUsersQueryWhere
                            .Select(x => new UserDto()
                            {
                                Id = x.Id,
                                Username = x.Username,
                                Firstname = x.Firstname,
                                Middlename = x.Middlename,
                                Lastname = x.Lastname,
                                Alias = x.Alias,
                                Email = x.Email,
                                IsActive = x.IsActive,
                                IsSuperAdmin = x.IsSuperAdmin,
                                Avatar = x.AvatarPath

                            }).ToListAsync(cancellationToken);





            return users;
        }

    }

}
