using Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Tasks.Commands
{
    public class DeleteTaskCommand: IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteTaskCommandHandle : IRequestHandler<DeleteTaskCommand>
    {
        private readonly IJiraDbContext _context;

        public DeleteTaskCommandHandle(IJiraDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.TaskInfos.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (task == null)
            {
                throw new DataNotFoundException("Invalid TaskId" + request.Id);
            }

            _context.TaskInfos.Remove(task);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
