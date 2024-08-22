using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.GL.Common;
using RDF.GL.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace RDF.GL.Features.UserRole;

[Route("api/role"), ApiController]

public class AddNewUserRole : ControllerBase
{
    private readonly IMediator _mediator;

    public AddNewUserRole(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddNewUserRoleCommand request)
    {
        if (User.Identity is ClaimsIdentity identity
                && int.TryParse(identity.FindFirst("id")?.Value, out var userId))
        {
            request.AddedBy = userId;
            request.ModifiedBy = userId;
        }
        var result = await _mediator.Send(request);
        if (result.IsFailure)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    public class AddNewUserRoleCommand : IRequest<Result>
    {
        public string? RoleName { get; set; }
        public int AddedBy { get; set; }
        public int ModifiedBy { get; set; }
        public List<string>? Permission { get; set; }
    }

    public class Handler : IRequestHandler<AddNewUserRoleCommand, Result>
    {
        private readonly ProjectGLDbContext _context;

        public Handler(ProjectGLDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(AddNewUserRoleCommand request, CancellationToken cancellationToken)
        {
            var existingRole = await _context.UserRoles
                .AnyAsync(r => r.UserRoleName == request.RoleName, cancellationToken);

            if (existingRole)
            {
                return Result.Failure(UserRoleError.UserRoleAlreadyExist(request.RoleName));
            }
            else
            {
                var newRole = new Domain.UserRole
                {
                    UserRoleName = request.RoleName,
                    AddedBy = request.AddedBy,
                    ModifiedBy = request.ModifiedBy,
                    Permissions = request.Permission
                };
                await _context.UserRoles.AddAsync(newRole, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
        }
    }
}
