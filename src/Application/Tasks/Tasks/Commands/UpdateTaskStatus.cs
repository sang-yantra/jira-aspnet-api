using Common.Exceptions;

namespace Tasks.Tasks.Commands
{
    public class UpdateTaskStatus : IRequest
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }

    public class UpdateTaskStatusHandler : IRequestHandler<UpdateTaskStatus>
    {
        private readonly IJiraDbContext _context;

        public UpdateTaskStatusHandler(IJiraDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(UpdateTaskStatus request, CancellationToken cancellationToken)
        {
            var task = await _context.TaskInfos.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (task == null)
            {
                throw new DataNotFoundException("Invalid TaskId" + request.Id);
            }

            task.Status = request.Status;
            _context.TaskInfos.Update(task);
            await _context.SaveChangesAsync(cancellationToken);

            return new Unit();

        }
    }
}
