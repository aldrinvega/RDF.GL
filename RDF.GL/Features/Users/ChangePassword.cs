using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.Arcana.API.Features.Users;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.Users;

[Route("api/user")]
[ApiController]

public class ChangePassword(IMediator _mediator) : ControllerBase
{
    [HttpPatch("change-password")]
    public async Task<IActionResult> Patch([FromBody] ChangePasswordCommand request)
    {
        try
        {
            if (User.Identity is ClaimsIdentity identity
                && int.TryParse(identity.FindFirst("id")?.Value, out var userId))
            {
                request.Id = userId;
            }
            var result = await _mediator.Send(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    public class ChangePasswordCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class Handler(ProjectGLDbContext _context) : IRequestHandler<ChangePasswordCommand, Result>
    {
        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var users = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (users == null)
            {
                return UserErrors.NotFound();
            }

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, users.Password))
            {
                return UserErrors.OldPasswordIncorrect();
            }

            users.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
