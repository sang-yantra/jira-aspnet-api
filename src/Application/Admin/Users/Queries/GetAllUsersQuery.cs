using Admin.DAL.Interfaces;
using Jira.Domain.Entities.ProjectManagement;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Users.Queries
{
    public class GetAllUsersQuery: IRequest<List<UserDto>>
    {
        public Guid? UserId { get; set; }
        public string? UserName { get; set; } 
        public string? Email { get; set; }
    }

    public class GetAllUsersQueryHandle: IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly IJiraDbContext _context;
        public GetAllUsersQueryHandle(IJiraDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserDto>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            var users = await _context.Users
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

                            }).ToListAsync(cancellationToken);

            return users;
        }


        public bool ApplyUserFilter(GetAllUsersQuery query, User user)
        {
            //// no filters
            if(query.UserId == null && query.UserName == null && query.Email == null)
            {
                return true;
            }
            else //// with filters
            {
                if (query.UserId == user.Id) return true;
                return user.Username.Contains(query.UserName ?? "") ||
                    user.Email.Contains(query.Email);
            }

            
        }

        //public Expression<Func<User, bool>> ApplyFilters(User user, GetAllUsersQuery query)
        //{
        //    return Expression.Lambda
        //}

    }

}
