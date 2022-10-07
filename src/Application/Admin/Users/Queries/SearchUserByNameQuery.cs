using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.DAL.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Admin.Users.Queries
{
    public class SearchUserByNameQuery: IRequest<List<UserDto>>
    {
        public string Name { get; set; }
    }

    public class SearchUserByIdQueryHandler : IRequestHandler<SearchUserByNameQuery, List<UserDto>>
    {
        private readonly IJiraDbContext _context;
        public SearchUserByIdQueryHandler(IJiraDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> Handle(SearchUserByNameQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Users
                .Where(x => x.Username.Contains(request.Name))
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
                    IsSuperAdmin = x.IsSuperAdmin
                }).ToListAsync(cancellationToken);

            return result;
        }
    }
}
