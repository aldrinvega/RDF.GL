using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDF.Arcana.API.Features.Users;
using RDF.GL.Common;
using RDF.GL.Data;

namespace RDF.GL.Features.Users;

[Route("api/user")]
[ApiController]

public class ResetPassword(IMediator _mediator) : ControllerBase
{
    [HttpPatch("{id:int}/reset-password")]
    public async Task<IActionResult> Patch([FromRoute] int id)
    {
        try
        {
            var command = new ResetPasswordCommand
            {
                UserId = id
            };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
    public class ResetPasswordCommand : IRequest<Result>
    {
        public int UserId { get; set; }
    }

    public class Handler(ProjectGLDbContext _context) : IRequestHandler<ResetPasswordCommand, Result>
    {
        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                return UserErrors.NotFound();
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Username);

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
