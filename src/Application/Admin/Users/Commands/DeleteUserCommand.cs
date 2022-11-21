using Common.Exceptions;

namespace Admin.Users.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public Guid UserId { get; set; }
    }

    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
    {
        public readonly IJiraDbContext _context;
        public DeleteUserHandler(IJiraDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userInfo = await _context.Users.Where(x => x.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (userInfo == null)
            {
                throw new DataNotFoundException("User Not Found");
            }
            _context.Users.Remove(userInfo);
            await _context.SaveChangesAsync(cancellationToken);
            return new Unit();
        }
    }

}
