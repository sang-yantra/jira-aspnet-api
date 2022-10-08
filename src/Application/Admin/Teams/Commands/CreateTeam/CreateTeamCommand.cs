using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Jira.Domain.Entities.ProjectManagement;
using Admin.Common.Interfaces;

namespace Admin.Teams.Commands.CreateTeam
{
    public class CreateTeamCommand: IRequest<Team>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string LongDescription { get; set; }
        public bool IsActive { get; set; }
    }


    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Team>
    {
        private readonly IJiraDbContext _context;

        public CreateTeamCommandHandler(IJiraDbContext context)
        {
            _context =  context;
        }
        public async Task<Team> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            Team team = new Team()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code,
                Description = request.Description,
                CreatedDatetime = DateTime.Now,
                UpdatedDatetime = DateTime.Now,
                CreatedBy = request.CreatedBy,
                UpdatedBy = request.UpdatedBy,
                LongDescription = request.LongDescription,
                IsActive = request.IsActive,
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync(cancellationToken);
            return team;
        }
    }
}
