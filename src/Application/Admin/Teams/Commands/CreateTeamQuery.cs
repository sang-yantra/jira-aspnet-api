using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.DAL.Interfaces;
using MediatR;
using Jira.Domain.Entities.ProjectManagement;

namespace Admin.Teams.Commands
{
    public class CreateTeamQuery: IRequest<string>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string LongDescription { get; set; }
        public bool IsActive { get; set; }
    }


    public class CreateTeamQueryHandler : IRequestHandler<CreateTeamQuery, string>
    {
        private readonly IJiraDbContext _context;

        public CreateTeamQueryHandler(IJiraDbContext context)
        {
            _context =  context;
        }
        public async Task<string> Handle(CreateTeamQuery request, CancellationToken cancellationToken)
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
            return "/api/1/task/" + team.Id;
        }
    }
}
