using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Admin.Teams.Commands.CreateTeam
{
    public class CreateTeamCommandValidator: AbstractValidator<CreateTeamCommand>
    {
        private readonly IJiraDbContext _context;
        public CreateTeamCommandValidator(IJiraDbContext context)
        {
            _context = context;
            RuleFor(c => c.Name)
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters")
                .NotEmpty().WithMessage("Name is required")
                .MustAsync(IsNameUnique).WithMessage("The Specified already exisits");

            RuleFor(c => c.CreatedBy)
                .NotEmpty();
        }

        public async Task<bool> IsNameUnique(string name, CancellationToken cancellationToken)
        {
            return await _context.Teams.AnyAsync(x => x.Name != name, cancellationToken);
        }
    }
}
