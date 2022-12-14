
using Common.Exceptions;

namespace Admin.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public Guid UserId { get; set; }

    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IJiraDbContext _context;
        public GetUserByIdQueryHandler(IJiraDbContext jiraDbContext)
        {
            _context = jiraDbContext;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(x => x.Id == request.UserId)
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

                })
                .FirstOrDefaultAsync(cancellationToken);


            if (user == null)
                throw new DataNotFoundException("No data found for UserId = " + request.UserId);

            return user;
        }


    }
}
