using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.UserRole;

[Route("api/role"), ApiController]
public class UpdateUserRoleStatus(IMediator _mediator) : ControllerBase
{
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> Update([FromRoute] int id)
    {
        var command = new UpdateUserRoleStatusCommand
        {
            Id = id
        };
        var result = await _mediator.Send(command);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    public class UpdateUserRoleStatusCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class Handler(ProjectGLDbContext _context) : IRequestHandler<UpdateUserRoleStatusCommand, Result>
    {
        public async Task<Result> Handle(UpdateUserRoleStatusCommand request, CancellationToken cancellationToken)
        {
            var userRole = await _context.UserRoles
                .Include(ur => ur.Users)
                .FirstOrDefaultAsync(ur => ur.Id == request.Id, cancellationToken);

            if (userRole is null)
            {
                return UserRoleError.UserRoleNotFound();
            }

            if(userRole.Users.Count > 0)
            {
                return UserRoleError.UserRoleIsInUse();
            }

            userRole.IsActive = !userRole.IsActive;

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
