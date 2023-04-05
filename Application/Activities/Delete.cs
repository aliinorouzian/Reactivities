using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        // Command not return anything
        public class Command : IRequest
        {
            // input
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            // return Unit is like void for stand return nothing.
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);

                _context.Remove(activity);
                await _context.SaveChangesAsync();


                return Unit.Value;
            }
        }
    }
}