using Admin.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.DAL.Interfaces
{
    public interface ITeamRepo
    {
        Task<List<TeamDto>> GetAllTeams();
    }
}
