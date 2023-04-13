using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        // Command not return anything
        public class Command : IRequest<Result<Unit>>
        {
            // input
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
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
                await _context.Activities.AddAsync(request.Activity);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                    return Result<Unit>.Failrure("Failed to create activity.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}