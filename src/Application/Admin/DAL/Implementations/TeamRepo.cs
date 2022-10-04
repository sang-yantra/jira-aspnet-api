using Admin.DAL.Interfaces;
using Admin.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.DAL.Implementations
{
    public class TeamRepo: ITeamRepo
    {
        private readonly IJiraDbContext _context;
        public TeamRepo(IJiraDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all teams
        /// </summary>
        /// <returns></returns>
        public async Task<List<TeamDto>> GetAllTeams()
        {
            var teams = await _context.Teams.Select(team => new TeamDto
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

            }).ToListAsync();
            return teams;
        }
    }
}
