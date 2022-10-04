using Admin.DAL.Interfaces;
using Admin.DTO;
using Admin.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Services.Implementations
{
    public class TeamService: ITeamService
    {
        private readonly ITeamRepo _teamRepo;
        public TeamService(ITeamRepo teamRepo)
        {
            _teamRepo = teamRepo;
        }
        public async Task<List<TeamDto>> GetAllTeamsService()
        {
            return await _teamRepo.GetAllTeams();
        }
    }
}
