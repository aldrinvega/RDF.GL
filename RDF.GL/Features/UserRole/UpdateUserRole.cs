using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace RDF.GL.Features.UserRole;

[Route("api/role"), ApiController]

public class UpdateUserRole(IMediator _mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        [FromBody] UpdateUserRoleCommand command,
        [FromRoute] int id)
    {

        if (User.Identity is ClaimsIdentity identity
                && int.TryParse(identity.FindFirst("id")?.Value, out var userId))
        {
            command.ModifiedBy = userId;
        }

        command.Id = id;

        var result = await _mediator.Send(command);
        if (result.IsFailure)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    public class UpdateUserRoleCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public List<string>? Permissions { get; set; }
        public int ModifiedBy { get; set; }
    }

    public class Handler : IRequestHandler<UpdateUserRoleCommand, Result>
    {
        private readonly ProjectGLDbContext _context;

        public Handler(ProjectGLDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            var userRoleExists = await _context.UserRoles
                .FirstOrDefaultAsync(ur => 
                    ur.UserRoleName == request.RoleName && 
                    ur.Id != request.Id,
                    cancellationToken);

            if (userRole == null)
            {
                return UserRoleError.UserRoleNotFound();
            }

            if (userRoleExists is not null)
            {
                return UserRoleError.UserRoleAlreadyExist(request.RoleName);
            }

            userRole.UserRoleName = request.RoleName;
            userRole.Permissions = request.Permissions;
            userRole.ModifiedBy = request.ModifiedBy;
            userRole.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
