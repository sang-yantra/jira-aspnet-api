using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Admin.Teams.Queries
{
    public class GetTeamByIdQuery: IRequest<TeamDto>
    {
        public Guid TeamId { get; set; }
    }

    public class GetTeamByIdHandler : IRequestHandler<GetTeamByIdQuery, TeamDto>
    {
        private readonly IJiraDbContext _context;
        public GetTeamByIdHandler(IJiraDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method to handle team by id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TeamDto> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
        {
            var team = await _context.Teams
                            .Where(team => team.Id == request.TeamId)
                            .Select(team => new TeamDto()
                            {
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
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (team == null)
                throw new Exception("Data not found");
            return team;
        }
    }
}
