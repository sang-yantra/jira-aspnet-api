using Admin.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Services.Interfaces
{
    public interface ITeamService
    {
        Task<List<TeamDto>> GetAllTeamsService();
    }
}
