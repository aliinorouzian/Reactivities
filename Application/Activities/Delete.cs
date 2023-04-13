using Application.Core;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        // Command not return anything
        public class Command : IRequest<Result<Unit>>
        {
            // input
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            // return Unit is like void for stand return nothing.
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);
                // Deleting null give us exception so cut ir here.
                if (activity == null)
                    return null;

                _context.Remove(activity);

                var result = await _context.SaveChangesAsync() > 0;
                if (!result)
                    return Result<Unit>.Failrure("Failed to delete the activity.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}