using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Users.Commands
{
    public class TestBulkUpdate: IRequest<string>
    {
    }

    public class TestBulkUpdateHandler : IRequestHandler<TestBulkUpdate, string>
    {
        private readonly IJiraDbContext _context;
        public TestBulkUpdateHandler(IJiraDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(TestBulkUpdate request, CancellationToken cancellationToken)
        {
            var users = _context.Users.ToList();

            foreach (var user in users)
            {
                var fakeUser = new Faker<User>()
                    .RuleFor(x => x.AvatarPath, f => f.Internet.Avatar());

                var usergen = fakeUser.Generate();
                user.AvatarPath = usergen.AvatarPath;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return "users updated successfully";
        }
    }
}
