using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            // return Unit is like void for stand return nothing.
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // accessor is like User.identity.name in here
                var user = await _context.Users.FirstOrDefaultAsync(u =>
                    u.UserName == _userAccessor.GetUsername());

                var attendee = new ActivityAttendee()
                {
                    AppUser = user,
                    Activity = request.Activity,
                    IsHost = true,
                };

                request.Activity.Attendees.Add(attendee);

                await _context.Activities.AddAsync(request.Activity);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                    return Result<Unit>.Failrure("Failed to create activity.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}