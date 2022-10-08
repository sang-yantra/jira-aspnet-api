using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Admin.Teams.Queries
{
    public class GetTeamsQuery: IRequest<List<TeamDto>>
    {
    }

    public class GetTeamsQueryHandler : IRequestHandler<GetTeamsQuery, List<TeamDto>>
    {
        private readonly IJiraDbContext _context;
        public GetTeamsQueryHandler(IJiraDbContext jiraDbContext)
        {
            _context = jiraDbContext;
        }

        public async Task<List<TeamDto>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
        {
            return await  _context.Teams
                .Select(team => new TeamDto() {
                    Id = team.Id,
                    Name = team.Name,
                    Code = team.Code,
                    Description = team.Description,
                    CreatedDatetime = team.CreatedDatetime ?? DateTime.Now,
                    CreatedBy = team.CreatedBy,
                    UpdatedDatetime = team.UpdatedDatetime ?? DateTime.Now,
                    UpdatedBy = team.UpdatedBy,
                    IsActive = team.IsActive ?? false,
                })
                .ToListAsync(cancellationToken);
        }


    }
}
